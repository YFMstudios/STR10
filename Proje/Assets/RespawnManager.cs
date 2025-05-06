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

    public void OnAllMinionsDead(string side)
    {
        Debug.Log($"<color=yellow>[RespawnMgr] {side} tarafı için tüm minyonlar öldü bildirimi alındı!</color>");

        // Bekleyen respawn işlerini iptal et
        for (int i = respawnJobs.Count - 1; i >= 0; i--)
        {
            if (respawnJobs[i].Role == side)
            {
                Debug.LogWarning($"<color=red>[RespawnMgr] {side} için bekleyen respawn işi iptal edildi!</color>");
                respawnJobs.RemoveAt(i);
            }
        }
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
    for (int i = respawnJobs.Count - 1; i >= 0; i--)
    {
        respawnJobs[i].RespawnTime -= Time.deltaTime;
        if (respawnJobs[i].RespawnTime > 0) continue;

        string role = respawnJobs[i].Role;
        int    act  = respawnJobs[i].OwnerActorNumber;

        bool minionsDone = false;
        if (role == "attacker")
            minionsDone = spawner.playerMinionSpawner != null &&
                          spawner.playerMinionSpawner.AreAllMinionsDead;
        else
            minionsDone = spawner.enemyMinionSpawner  != null &&
                          spawner.enemyMinionSpawner.AreAllMinionsDead;

        if (minionsDone)
        {
            Debug.LogWarning($"[RespawnMgr] ⛔ Job çatladı: {role} minyonları bitmiş, tetiklenmedi");
            respawnJobs.RemoveAt(i);
            continue;
        }

        Debug.Log($"[RespawnMgr] ►► job tetikleniyor  role={role}  owner={act}");
        if (spawner) spawner.ForceRespawnCharacter(role, act);
        else Debug.LogError("[RespawnMgr] Spawner hâlâ null!");

        respawnJobs.RemoveAt(i);
    }
}


    // Bu metod BattleScenePlayerSpawner'dan çağrılacak
    public void ScheduleRespawn(string role, float delay, int ownerActorNumber)
{
    if (spawner == null)          // güvenlik
    {
        Debug.LogWarning("[RespawnMgr] Spawner yok → job eklenmedi");
        return;
    }

    /* ——— ÖN ŞART ——— */
    bool minionsDone = false;
    if (role == "attacker")
        minionsDone = spawner.playerMinionSpawner != null &&
                      spawner.playerMinionSpawner.AreAllMinionsDead;
    else            // defender
        minionsDone = spawner.enemyMinionSpawner  != null &&
                      spawner.enemyMinionSpawner.AreAllMinionsDead;

    if (minionsDone)
    {
        Debug.LogWarning($"[RespawnMgr] ⛔ {role} minyonları yok → ScheduleRespawn iptal");
        return;                     // <‑‑ job eklemeyi PAS geç
    }
    /* ———————————— */

    Debug.Log($"[RespawnMgr] Job eklendi  role={role}  delay={delay}s");

    // listede varsa güncelle
    foreach (var j in respawnJobs)
        if (j.Role == role) { j.RespawnTime = delay; j.OwnerActorNumber = ownerActorNumber; return; }

    respawnJobs.Add(new RespawnJob {
        Role = role,
        RespawnTime = delay,
        OwnerActorNumber = ownerActorNumber
    });
}

}