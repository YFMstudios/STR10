using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

public class MinionSpawner : MonoBehaviourPunCallbacks
{
    public float meleeMinionMoveSpeed;
    public float rangedMinionMoveSpeed;

    private const string MELEE_MINION_PREFAB = "Minions/MeleeMinion";
    private const string RANGED_MINION_PREFAB = "Minions/RangedMinion";

    public Transform[] spawnPoints;
    public float spawnInterval = 20.0f;
    public float delayBetweenMinions;

    [Header("ScriptableObject")]
    public GetPlayerData getPlayerData;

    private int meleeUnitsToSpawn;
    private int rangedUnitsToSpawn;
    private int meleeRemaining;
    private int rangedRemaining;

    public int kalanOkcu;
    public int kalanSavasci;

        public SoldierController soldierManager;




    private void Start()
    {

        Debug.Log("[MinionSpawner] Start()");

        meleeUnitsToSpawn = getPlayerData.currentSoldierAmount;
        rangedUnitsToSpawn = getPlayerData.currentArcherAmount;

        Debug.Log($"[MinionSpawner] Toplam Melee: {meleeUnitsToSpawn}, Ranged: {rangedUnitsToSpawn}");

        meleeRemaining = meleeUnitsToSpawn;
        rangedRemaining = rangedUnitsToSpawn;

        if (WarController.Instance != null)
{
    WarController.Instance.playerkalansavasçı = meleeRemaining;
    WarController.Instance.playerkalanokçu = rangedRemaining;
}


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("[MinionSpawner] MasterClient olduğum için spawn başlatılıyor");
            StartCoroutine(SpawnMinions());
        }
    }

    private IEnumerator SpawnMinions()
    {
        int totalUnits = meleeUnitsToSpawn + rangedUnitsToSpawn;
        int meleeLeft = meleeUnitsToSpawn;
        int rangedLeft = rangedUnitsToSpawn;

        int unitsPerWave = 10;
        int waves = Mathf.CeilToInt((float)totalUnits / unitsPerWave);

        Debug.Log($"[MinionSpawner] Toplam {waves} dalga oluşacak.");

        for (int wave = 0; wave < waves; wave++)
        {
            Debug.Log($"[MinionSpawner] Dalga {wave + 1}/{waves} başlıyor.");

            int meleeThisWave = Mathf.Min(5, meleeLeft);
            int rangedThisWave = Mathf.Min(5, rangedLeft);

            for (int i = 0; i < meleeThisWave; i++)
            {
                GameObject minion = SpawnMinionForAll(true, meleeMinionMoveSpeed);
                AttachDeathLogic(minion, true);
                meleeLeft--;
                Debug.Log($"[MinionSpawner] Melee minion spawn edildi. Kalan: {meleeLeft}");
                kalanSavasci = meleeLeft;
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            for (int i = 0; i < rangedThisWave; i++)
            {
                GameObject minion = SpawnMinionForAll(false, rangedMinionMoveSpeed);
                AttachDeathLogic(minion, false);
                rangedLeft--;
                Debug.Log($"[MinionSpawner] Ranged minion spawn edildi. Kalan: {rangedLeft}");
                kalanOkcu = rangedLeft;
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            if (wave < waves - 1)
            {
                float wait = spawnInterval - delayBetweenMinions * (meleeThisWave + rangedThisWave);
                Debug.Log($"[MinionSpawner] Dalga arası bekleniyor: {wait} saniye");
                yield return new WaitForSeconds(wait);
            }
        }

        Debug.Log("[MinionSpawner] Tüm dalgalar tamamlandı.");
    }

    private GameObject SpawnMinionForAll(bool isMelee, float moveSpeed)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform chosenPoint = spawnPoints[spawnIndex];

        string prefabName = isMelee ? MELEE_MINION_PREFAB : RANGED_MINION_PREFAB;

        if (!PhotonNetwork.IsMasterClient) return null;

        GameObject minion = PhotonNetwork.Instantiate(
            prefabName,
            chosenPoint.position,
            chosenPoint.rotation
        );

        Debug.Log($"[MinionSpawner] {(isMelee ? "Melee" : "Ranged")} minyon instantiate edildi: {prefabName}");

        var agent = minion.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
            Debug.Log($"[MinionSpawner] NavMeshAgent hızı ayarlandı: {moveSpeed}");
        }

        return minion;
    }

    private void AttachDeathLogic(GameObject minion, bool isMelee)
    {
        Debug.Log("[MinionSpawner] Death tracker eklendi.");
        MinionDeathTracker tracker = minion.AddComponent<MinionDeathTracker>();
        tracker.Init(this, isMelee);
    }

    public void DecreaseMinionCount(bool isMelee)
    {
        if (isMelee)
        {
            meleeRemaining--;
            Debug.Log($"[MinionSpawner] Melee kalan: {meleeRemaining}");
        }
        else
        {
            rangedRemaining--;
            Debug.Log($"[MinionSpawner] Ranged kalan: {rangedRemaining}");
        }

        // WarController'a bildir
        if (WarController.Instance != null)
{
    WarController.Instance.playerkalansavasçı = meleeRemaining;
    WarController.Instance.playerkalanokçu = rangedRemaining;
}

    }
}

public class MinionDeathTracker : MonoBehaviour
{
    private MinionSpawner spawner;
    private bool isMelee;

    public void Init(MinionSpawner spawnerRef, bool isMeleeType)
    {
        spawner = spawnerRef;
        isMelee = isMeleeType;
        Debug.Log("[MinionDeathTracker] Tracker başlatıldı.");
    }

    private void OnDestroy()
    {
        Debug.Log("[MinionDeathTracker] Minyon öldü, spawner bilgilendiriliyor.");
        if (spawner != null)
        {
            spawner.DecreaseMinionCount(isMelee);
        }
    }
}
