using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

// Bu script, GameController'a eklenmelidir ve oyun süresince aktif kalmalıdır
public class RespawnManager : MonoBehaviourPunCallbacks
{
    private static RespawnManager _instance;
    public static RespawnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RespawnManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("RespawnManager");
                    _instance = go.AddComponent<RespawnManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    private BattleScenePlayerSpawner spawner;

    // Zamanlanmış respawn için yapı
    private class RespawnJob
    {
        public string Role;
        public float RespawnTime;
        public int OwnerActorNumber;
    }

    private List<RespawnJob> respawnJobs = new List<RespawnJob>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("[RespawnManager] Initialized!");
    }

    void Start()
    {
        // Spawner'a referans al - geç başlat
        StartCoroutine(FindSpawnerDelayed());
    }

    private IEnumerator FindSpawnerDelayed()
    {
        yield return new WaitForSeconds(1f);
        FindSpawner();
    }

    private void FindSpawner()
    {
        spawner = FindObjectOfType<BattleScenePlayerSpawner>();
        if (spawner != null)
        {
            Debug.Log("[RespawnManager] BattleScenePlayerSpawner bulundu!");
        }
        else
        {
            Debug.LogWarning("[RespawnManager] BattleScenePlayerSpawner bulunamadı! 3 saniye sonra tekrar deneyecek.");
            StartCoroutine(RetryFindSpawner());
        }
    }

    private IEnumerator RetryFindSpawner()
    {
        yield return new WaitForSeconds(3f);
        FindSpawner();
    }

    void Update()
    {
        // Respawn zamanı gelen işleri kontrol et
        for (int i = respawnJobs.Count - 1; i >= 0; i--)
        {
            respawnJobs[i].RespawnTime -= Time.deltaTime;

            if (respawnJobs[i].RespawnTime <= 0)
            {
                string role = respawnJobs[i].Role;
                int ownerActorNumber = respawnJobs[i].OwnerActorNumber;

                Debug.Log($"[RespawnManager] Zamanı gelen respawn işi: {role}, OwnerActor={ownerActorNumber}");

                if (spawner != null)
                {
                    // Force respawn - spawner üzerindeki RPC_RespawnCharacter metodunu doğrudan çağır
                    spawner.ForceRespawnCharacter(role, ownerActorNumber);
                }
                else
                {
                    Debug.LogError("[RespawnManager] Spawner null, respawn yapılamıyor!");
                    FindSpawner();
                }

                // İşi listeden kaldır
                respawnJobs.RemoveAt(i);
            }
        }
    }

    // Bu metod BattleScenePlayerSpawner'dan çağrılacak
    public void ScheduleRespawn(string role, float delay, int ownerActorNumber)
    {
        Debug.Log($"[RespawnManager] Respawn zamanlandı: {role}, Süre={delay}sn, OwnerActor={ownerActorNumber}");

        // Zaten aynı role için bekleyen bir iş var mı?
        for (int i = 0; i < respawnJobs.Count; i++)
        {
            if (respawnJobs[i].Role == role)
            {
                Debug.Log($"[RespawnManager] {role} için zaten bir respawn zamanlaması var, güncelleniyor.");
                respawnJobs[i].RespawnTime = delay;
                respawnJobs[i].OwnerActorNumber = ownerActorNumber;
                return;
            }
        }

        // Yeni iş ekle
        RespawnJob job = new RespawnJob
        {
            Role = role,
            RespawnTime = delay,
            OwnerActorNumber = ownerActorNumber
        };

        respawnJobs.Add(job);
        Debug.Log($"[RespawnManager] Yeni respawn işi eklendi. Toplam iş sayısı: {respawnJobs.Count}");
    }
}