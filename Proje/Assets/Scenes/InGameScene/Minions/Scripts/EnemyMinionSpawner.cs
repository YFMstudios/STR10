// ------------------- EnemyMinionSpawner.cs -------------------
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

public class EnemyMinionSpawner : MonoBehaviourPunCallbacks
{
    public float meleeMinionMoveSpeed;
    public float rangedMinionMoveSpeed;

    private const string ENEMY_MELEE_MINION_PREFAB = "Minions/EnemyMeleeMinion";
    private const string ENEMY_RANGED_MINION_PREFAB = "Minions/EnemyRangedMinion";

    public Transform[] spawnPoints;
    public float spawnInterval = 20.0f;
    public float delayBetweenMinions;

    [Header("ScriptableObject")]
    public GetPlayerData getPlayerData;

    private int meleeUnitsToSpawn;
    private int rangedUnitsToSpawn;
    private int meleeRemaining;
    private int rangedRemaining;

    private void Start()
    {
        Debug.Log("[EnemySpawner] Start()");

        meleeUnitsToSpawn = getPlayerData.currentSoldierAmount;
        rangedUnitsToSpawn = getPlayerData.currentArcherAmount;

        Debug.Log($"[EnemySpawner] Toplam Melee: {meleeUnitsToSpawn}, Ranged: {rangedUnitsToSpawn}");

        meleeRemaining = meleeUnitsToSpawn;
        rangedRemaining = rangedUnitsToSpawn;

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("[EnemySpawner] MasterClient olarak minyon spawn başlatılıyor");
            StartCoroutine(SpawnMinions());
        }
    }

    private IEnumerator SpawnMinions()
    {
        int totalUnits = meleeUnitsToSpawn + rangedUnitsToSpawn;
        int meleeLeft = meleeUnitsToSpawn;
        int rangedLeft = rangedUnitsToSpawn;

        int unitsPerWave = 10; // 5 melee + 5 ranged max
        int waves = Mathf.CeilToInt((float)totalUnits / unitsPerWave);

        Debug.Log($"[EnemySpawner] Toplam {waves} dalga oluşturulacak.");

        for (int wave = 0; wave < waves; wave++)
        {
            Debug.Log($"[EnemySpawner] Dalga {wave + 1}/{waves} başlatılıyor.");

            int meleeThisWave = Mathf.Min(5, meleeLeft);
            int rangedThisWave = Mathf.Min(5, rangedLeft);

            for (int i = 0; i < meleeThisWave; i++)
            {
                GameObject minion = SpawnMinionForAll(true, meleeMinionMoveSpeed);
                AttachDeathLogic(minion, true);
                meleeLeft--;
                Debug.Log($"[EnemySpawner] Melee minion spawn edildi. Kalan: {meleeLeft}");
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            for (int i = 0; i < rangedThisWave; i++)
            {
                GameObject minion = SpawnMinionForAll(false, rangedMinionMoveSpeed);
                AttachDeathLogic(minion, false);
                rangedLeft--;
                Debug.Log($"[EnemySpawner] Ranged minion spawn edildi. Kalan: {rangedLeft}");
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            if (wave < waves - 1)
            {
                float wait = spawnInterval - delayBetweenMinions * (meleeThisWave + rangedThisWave);
                Debug.Log($"[EnemySpawner] Dalga arası bekleniyor: {wait} saniye");
                yield return new WaitForSeconds(wait);
            }
        }

        Debug.Log("[EnemySpawner] Tüm dalgalar tamamlandı.");
    }

    private GameObject SpawnMinionForAll(bool isMelee, float moveSpeed)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        string prefabName = isMelee ? ENEMY_MELEE_MINION_PREFAB : ENEMY_RANGED_MINION_PREFAB;

        if (!PhotonNetwork.IsMasterClient) return null;

        GameObject minion = PhotonNetwork.Instantiate(
            prefabName,
            spawnPoint.position,
            spawnPoint.rotation
        );

        Debug.Log($"[EnemySpawner] {(isMelee ? "Melee" : "Ranged")} minyon instantiate edildi: {prefabName}");

        var agent = minion.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
            Debug.Log($"[EnemySpawner] NavMeshAgent hızı ayarlandı: {moveSpeed}");
        }

        return minion;
    }

    private void AttachDeathLogic(GameObject minion, bool isMelee)
    {
        Debug.Log("[EnemySpawner] Death tracker eklendi.");
        EnemyMinionDeathTracker tracker = minion.AddComponent<EnemyMinionDeathTracker>();
        tracker.Init(this, isMelee);
    }

    public void DecreaseMinionCount(bool isMelee)
    {
        if (isMelee)
        {
            meleeRemaining--;
            Debug.Log($"[EnemySpawner] Melee kalan: {meleeRemaining}");
        }
        else
        {
            rangedRemaining--;
            Debug.Log($"[EnemySpawner] Ranged kalan: {rangedRemaining}");
        }
    }
}

public class EnemyMinionDeathTracker : MonoBehaviour
{
    private EnemyMinionSpawner spawner;
    private bool isMelee;

    public void Init(EnemyMinionSpawner spawnerRef, bool isMeleeType)
    {
        spawner = spawnerRef;
        isMelee = isMeleeType;
        Debug.Log("[EnemyMinionDeathTracker] Tracker başlatıldı.");
    }

    private void OnDestroy()
    {
        Debug.Log("[EnemyMinionDeathTracker] Minyon öldü, spawner bilgilendiriliyor.");
        if (spawner != null)
        {
            spawner.DecreaseMinionCount(isMelee);
        }
    }
}