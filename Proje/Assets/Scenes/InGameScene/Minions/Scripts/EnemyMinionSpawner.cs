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

    /*  ──────────────────────────────────────────────────────────  */
/*  ► TÜM minyonlar öldü mü?                                   */
private bool allMinionsDead = false;

// class içinde, mevcut değişkenlerin hemen altına ekle
private int  aliveMinionCount = 0;   // sahnede canlı minyon sayısı
private bool wavesFinished    = false;

public  bool AreAllMinionsDead => wavesFinished && aliveMinionCount <= 0;



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
        allMinionsDead = false;   // sahne başında her zaman false

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
    float elapsed = 0f;
    const float TIMEOUT = 5f;   // 5 saniyede gelmezse devam et

    while (elapsed < TIMEOUT)
    {
        Player attacker = FindPlayerByRole("attacker");
        Player defender = FindPlayerByRole("defender");

        bool aReady = attacker != null
            && attacker.CustomProperties.ContainsKey("SoldierCount")
            && attacker.CustomProperties.ContainsKey("ArcherCount");
        bool dReady = defender != null
            && defender.CustomProperties.ContainsKey("SoldierCount")
            && defender.CustomProperties.ContainsKey("ArcherCount");

        if (aReady && dReady)
        {
            attackerSoldierCnt = (int)attacker.CustomProperties["SoldierCount"];
            attackerArcherCnt  = (int)attacker.CustomProperties["ArcherCount"];
            defenderSoldierCnt = (int)defender.CustomProperties["SoldierCount"];
            defenderArcherCnt  = (int)defender.CustomProperties["ArcherCount"];
            Debug.Log("[MinionSpawner] Veriler alındı, devam ediliyor.");
            yield break;
        }

        elapsed += Time.deltaTime;
        yield return null;
    }

    // Timeout aşıldıysa fallback yap (örneğin eşit pay, sabit değer veya görünen değer)
    attackerSoldierCnt = defenderSoldierCnt = getPlayerData.currentSoldierAmount;
    attackerArcherCnt  = defenderArcherCnt  = getPlayerData.currentArcherAmount;
    Debug.LogWarning("[MinionSpawner] Sayı verisi geç gelmedi, fallback ile devam ediliyor.");
}


    // ============================================================
    //  SpawnMinions – dalga dalga üretim
    // ============================================================
   private IEnumerator SpawnMinions()
{
    Debug.Log("[Spawner] SpawnMinions başladı.");

    int meleeLeft  = meleeUnitsToSpawn;
    int rangedLeft = rangedUnitsToSpawn;

    const int unitsPerWave = 10;
    int waves = Mathf.CeilToInt((float)(meleeLeft + rangedLeft) / unitsPerWave);
    Debug.Log($"[Spawner] Toplam {waves} dalga.");

    for (int wave = 0; wave < waves; wave++)
    {
        Debug.Log($"[Spawner] === Dalga {wave + 1}/{waves} ===");

        int meleeThisWave  = Mathf.Min(5, meleeLeft);
        int rangedThisWave = Mathf.Min(5, rangedLeft);

        /* ---------- MELEE ---------- */
        for (int i = 0; i < meleeThisWave; i++)
        {
            GameObject m = SpawnMinionForAll(true, meleeMinionMoveSpeed);

            AttachDeathLogic(m, true);
            meleeLeft--;

            /*  ↓↓↓  SAHNEDEKİ CANLI SAYACI  ↓↓↓  */
            aliveMinionCount++;
            Debug.Log($"<color=#00FFFF>[Spawn] {m.name}  →  alive={aliveMinionCount}</color>");

            yield return new WaitForSeconds(delayBetweenMinions);
        }

        /* ---------- RANGED ---------- */
        for (int i = 0; i < rangedThisWave; i++)
        {
            GameObject m = SpawnMinionForAll(false, rangedMinionMoveSpeed);

            AttachDeathLogic(m, false);
            rangedLeft--;

            aliveMinionCount++;
            Debug.Log($"<color=#00FFFF>[Spawn] {m.name}  →  alive={aliveMinionCount}</color>");

            yield return new WaitForSeconds(delayBetweenMinions);
        }

        /* Dalga arası bekleme */
        if (wave < waves - 1)
        {
            float wait = spawnInterval - delayBetweenMinions * (meleeThisWave + rangedThisWave);
            Debug.Log($"[Spawner] Dalga arası {wait:F1}s bekleniyor.");
            yield return new WaitForSeconds(wait);
        }
    }

    /* -------------- TÜM DALGALAR BİTTİ -------------- */
    wavesFinished = true;
    Debug.Log("<color=#00FFFF>[Spawn] ►► BÜTÜN DALGALAR BİTTİ ◀◀</color>");
    CheckAllDead();
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

    aliveMinionCount--;
    Debug.Log($"<color=orange>[Death] isMelee={isMelee}  →  alive={aliveMinionCount}</color>");

    CheckAllDead();
}


private void CheckAllDead()
{
    if (!allMinionsDead && wavesFinished && aliveMinionCount <= 0)
    {
        allMinionsDead = true;
        Debug.Log("<color=lime>[Spawner] *** SAHNEDE HİÇ MİNYON KALMADI → allMinionsDead=TRUE ***</color>");
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
    private EnemyMinionSpawner spawner;   // ← TİP DÜZELTİLDİ
    private bool  isMelee;
    private bool  notified = false;

    public void Init(EnemyMinionSpawner s, bool melee)
    {
        spawner  = s;
        isMelee  = melee;
    }

    /* -------- Ölüm bildirimi tek noktada -------- */
    private void NotifyDeath()
    {
        if (notified) return;   // aynı objeyi iki kez sayma
        notified = true;

        if (spawner != null)
            spawner.DecreaseMinionCount(isMelee);
    }

    private void OnDestroy()  { NotifyDeath(); }

    private void OnDisable()
    {
        // Pooling kullanıyorsanız sahne null gelebilir
        if (gameObject.scene.IsValid())
            NotifyDeath();
    }
}