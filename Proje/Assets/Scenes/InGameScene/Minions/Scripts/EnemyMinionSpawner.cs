using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

public class EnemyMinionSpawner : MonoBehaviourPunCallbacks
{
    // ------------------------------------------------------------
    // Inspector değişkenleri
    // ------------------------------------------------------------
    public float meleeMinionMoveSpeed;
    public float rangedMinionMoveSpeed;

    private const string ENEMY_MELEE_MINION_PREFAB  = "Minions/EnemyMeleeMinion";
    private const string ENEMY_RANGED_MINION_PREFAB = "Minions/EnemyRangedMinion";

    public Transform[] spawnPoints;
    public float spawnInterval     = 20.0f;
    public float delayBetweenMinions;

    [Header("ScriptableObject (artık fallback değil)")]
    public GetPlayerData getPlayerData;
    public KaynakYoneticisi kaynakYoneticisi;//(+)

    // ------------------------------------------------------------
    // İç değişkenler
    // ------------------------------------------------------------
    private int meleeUnitsToSpawn;
    private int rangedUnitsToSpawn;
    private int meleeRemaining;
    private int rangedRemaining;

    public  int kalanOkcu;
    public  int kalanSavasci;

    public SoldierController soldierManager;

    // Ağdan gelen kesin değerler
    private int attackerSoldierCnt, attackerArcherCnt;
    private int defenderSoldierCnt, defenderArcherCnt;

        public int SpawlananArcherCount , SpawlananSoldierCount ;

        public HealController healController ;

    // ============================================================
    //  Start – yalnızca MasterClient çalıştırır
    // ============================================================
       public void Awake()
    {
        SpawlananArcherCount=0;
        SpawlananSoldierCount=0;
    }
    private IEnumerator Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("[EnemySpawner] Master değilim, çıkıyorum.");
            yield break;
        }

        Debug.Log("[EnemySpawner] Başlıyor...");

        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(WaitForBothSidesCounts());

        // *** Defender tarafının minyonları ***
        meleeUnitsToSpawn  = defenderSoldierCnt;
        rangedUnitsToSpawn = defenderArcherCnt;

        meleeRemaining  = meleeUnitsToSpawn;
        rangedRemaining = rangedUnitsToSpawn;

        Debug.Log($"[EnemySpawner] Sayılar alındı  Soldier:{meleeUnitsToSpawn}  Archer:{rangedUnitsToSpawn}");

        if (WarController.Instance != null)
        {
            WarController.Instance.enemykalansavasçı = meleeRemaining;
            WarController.Instance.enemykalanokçu    = rangedRemaining;
        }

        StartCoroutine(SpawnMinions());
    }

    // ============================================================
    //  WaitForBothSidesCounts
    // ============================================================
    private IEnumerator WaitForBothSidesCounts()
    {
        float waitTime = 0f;
        while (true)
        {
            Player attacker = FindPlayerByRole("attacker");
            Player defender = FindPlayerByRole("defender");

            bool attackerReady = attacker != null &&
                                  attacker.CustomProperties.ContainsKey("SoldierCount") &&
                                  attacker.CustomProperties.ContainsKey("ArcherCount");

            bool defenderReady = defender != null &&
                                  defender.CustomProperties.ContainsKey("SoldierCount") &&
                                  defender.CustomProperties.ContainsKey("ArcherCount");

            Debug.Log($"[EnemySpawner][WAIT] t={waitTime:F1}s  attackerReady:{attackerReady}  defenderReady:{defenderReady}");

            if (attackerReady && defenderReady)
            {
                attackerSoldierCnt = (int)attacker.CustomProperties["SoldierCount"];
                attackerArcherCnt  = (int)attacker.CustomProperties["ArcherCount"];

                defenderSoldierCnt = (int)defender.CustomProperties["SoldierCount"];
                defenderArcherCnt  = (int)defender.CustomProperties["ArcherCount"];
                Debug.Log("[EnemySpawner] İki taraf da hazır, döngüden çıkılıyor.");
                break;
            }
            waitTime += Time.deltaTime;
            yield return null;
        }
    }

    // ============================================================
    //  SpawnMinions – dalga dalga üretim
    // ============================================================
    private IEnumerator SpawnMinions()
    {
        Debug.Log("[EnemySpawner] SpawnMinions başladı.");
        int meleeLeft  = meleeUnitsToSpawn;
        int rangedLeft = rangedUnitsToSpawn;

        int unitsPerWave = 10;
        int waves = Mathf.CeilToInt((float)(meleeLeft + rangedLeft) / unitsPerWave);
        Debug.Log($"[EnemySpawner] Toplam {waves} dalga.");

        for (int wave = 0; wave < waves; wave++)
        {
            Debug.Log($"[EnemySpawner] === Dalga {wave + 1}/{waves} ===");
            int meleeThisWave  = Mathf.Min(5, meleeLeft);
            int rangedThisWave = Mathf.Min(5, rangedLeft);

            for (int i = 0; i < meleeThisWave; i++)
            {
                GameObject m = SpawnMinionForAll(true, meleeMinionMoveSpeed);
                AttachDeathLogic(m, true);
                meleeLeft--;
                SpawlananSoldierCount++;
                kaynakYoneticisi.WarPowerArttirma(-50);//(+)
                getPlayerData.savasciAzalt();//(+)
                kalanSavasci = meleeLeft;
                Debug.Log($"[EnemySpawner] Melee spawn – kalan:{meleeLeft}");
                Debug.Log($"[EnemySpawner] Spawlanan Asker Sayısı  – Üretilen:{SpawlananSoldierCount}");
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            for (int i = 0; i < rangedThisWave; i++)
            {
                GameObject m = SpawnMinionForAll(false, rangedMinionMoveSpeed);
                AttachDeathLogic(m, false);
                rangedLeft--;
                SpawlananArcherCount++;
                kaynakYoneticisi.WarPowerArttirma(-25);//(+)
                getPlayerData.okcuAzalt();//(+)
                kalanOkcu = rangedLeft;
                Debug.Log($"[EnemySpawner] Ranged spawn – kalan:{rangedLeft}");
                Debug.Log($"[EnemySpawner] Spawlanan Archer Sayısı  – Üretilen:{SpawlananArcherCount}");
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            if (wave < waves - 1)
            {
                float wait = spawnInterval - delayBetweenMinions * (meleeThisWave + rangedThisWave);
                Debug.Log($"[EnemySpawner] Dalga arası {wait:F1}s bekleniyor.");
                yield return new WaitForSeconds(wait);
            }
        }
        Debug.Log("[EnemySpawner] Tüm dalgalar bitti.");
    }

    // ============================================================
    //  Yardımcı fonksiyonlar
    // ============================================================
    private GameObject SpawnMinionForAll(bool isMelee, float moveSpeed)
    {
        if (!PhotonNetwork.IsMasterClient) return null;

        int idx = Random.Range(0, spawnPoints.Length);
        Transform p = spawnPoints[idx];
        string prefab = isMelee ? ENEMY_MELEE_MINION_PREFAB : ENEMY_RANGED_MINION_PREFAB;

        Debug.Log($"[EnemySpawner] Instantiate prefab:{prefab}  pos:{p.position}");

        GameObject m = PhotonNetwork.Instantiate(prefab, p.position, p.rotation);

        var agent = m.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent) agent.speed = moveSpeed;

        return m;
    }

    private void AttachDeathLogic(GameObject minion, bool isMelee)
    {
        EnemyMinionDeathTracker t = minion.AddComponent<EnemyMinionDeathTracker>();
        t.Init(this, isMelee);
    }

    public void DecreaseMinionCount(bool isMelee)
    {
        if (isMelee)  meleeRemaining--;
        else          rangedRemaining--;

        Debug.Log($"[EnemySpawner] DecreaseMinionCount – Melee:{meleeRemaining}  Ranged:{rangedRemaining}");

        if (WarController.Instance != null)
        {
            WarController.Instance.enemykalansavasçı = meleeRemaining;
            WarController.Instance.enemykalanokçu    = rangedRemaining;
        }
    }

    private Player FindPlayerByRole(string role)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
            if (p.CustomProperties.TryGetValue("Role", out object r) && r.ToString() == role)
                return p;
        return null;
    }
}

public class EnemyMinionDeathTracker : MonoBehaviour
{
    private EnemyMinionSpawner spawner;
    private bool isMelee;
    public void Init(EnemyMinionSpawner s, bool melee) { spawner = s; isMelee = melee; }
    private void OnDestroy()
    {
        Debug.Log("[EnemyMinionDeathTracker] Minyon öldü.");
        if (spawner) spawner.DecreaseMinionCount(isMelee);
    }
}
