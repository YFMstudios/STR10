using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

public class MinionSpawner : MonoBehaviourPunCallbacks
{
    // ------------------------------------------------------------
    // Inspector değişkenleri
    // ------------------------------------------------------------
    public float meleeMinionMoveSpeed;
    public float rangedMinionMoveSpeed;

    

    private const string MELEE_MINION_PREFAB  = "Minions/MeleeMinion";
    private const string RANGED_MINION_PREFAB = "Minions/RangedMinion";

    public Transform[] spawnPoints;
    public float spawnInterval     = 20.0f;
    public float delayBetweenMinions;

    [Header("ScriptableObject (artık fallback değil)")]
    public GetPlayerData getPlayerData;

    // ------------------------------------------------------------
    // İç değişkenler
    // ------------------------------------------------------------
    private int meleeUnitsToSpawn;
    private int rangedUnitsToSpawn;
    private int meleeRemaining;
    private int rangedRemaining;

    public int kalanOkcu;
    public int kalanSavasci;

    public SoldierController soldierManager;

    /*  ──────────────────────────────────────────────────────────  */
/*  ► TÜM minyonlar öldü mü?                                   */
private bool allMinionsDead = false;

// class içinde, mevcut değişkenlerin hemen altına ekle
private int  aliveMinionCount = 0;   // sahnede canlı minyon sayısı
private bool wavesFinished    = false;

public  bool AreAllMinionsDead => wavesFinished && aliveMinionCount <= 0;


    public HealController healController ;

    // Ağdan gelen kesin değerler
    private int attackerSoldierCnt, attackerArcherCnt;
    private int defenderSoldierCnt, defenderArcherCnt;

    public int SpawlananArcherCount , SpawlananSoldierCount ;
    public KaynakYoneticisi kaynakYoneticisi;//(+)

    // ============================================================
    //  Start – yalnızca MasterClient çalıştırır
    // ============================================================

    public void Awake()
    {
        SpawlananArcherCount=0;
        allMinionsDead = false;   // sahne başında her zaman false

        SpawlananSoldierCount=0;
    }
    private IEnumerator Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("[MinionSpawner] Master değilim, çıkıyorum.");
            yield break;
        }

        Debug.Log("[MinionSpawner] Başlıyor...");

        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(WaitForBothSidesCounts());

        // *** Attacker tarafının minyonları ***
        meleeUnitsToSpawn  = attackerSoldierCnt;
        rangedUnitsToSpawn = attackerArcherCnt;

        meleeRemaining  = meleeUnitsToSpawn;
        rangedRemaining = rangedUnitsToSpawn;

        Debug.Log($"[MinionSpawner] Sayılar alındı  Soldier:{meleeUnitsToSpawn}  Archer:{rangedUnitsToSpawn}");

        if (WarController.Instance != null)
        {
            WarController.Instance.playerkalansavasçı = meleeRemaining;
            WarController.Instance.playerkalanokçu    = rangedRemaining;
        }

        StartCoroutine(SpawnMinions());
    }

    // ============================================================
    //  WaitForBothSidesCounts – her iki oyuncu sayılarını yollayana kadar bekler
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
        string prefab = isMelee ? MELEE_MINION_PREFAB : RANGED_MINION_PREFAB;

        Debug.Log($"[MinionSpawner] Instantiate prefab:{prefab}  pos:{p.position}");

        GameObject m = PhotonNetwork.Instantiate(prefab, p.position, p.rotation);

        var agent = m.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent) agent.speed = moveSpeed;

        return m;
    }

    private void AttachDeathLogic(GameObject minion, bool isMelee)
    {
        MinionDeathTracker t = minion.AddComponent<MinionDeathTracker>();
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

public class MinionDeathTracker : MonoBehaviour
{
    private MinionSpawner spawner;
    private bool isMelee;
    private bool notified = false;

    public void Init(MinionSpawner s, bool melee)
    {
        spawner = s;
        isMelee = melee;
    }

    /*  çağrıların tümü tek yere yönleniyor */
    private void NotifyDeath()
    {
        if (notified) return;
        notified = true;

        if (spawner != null)
            spawner.DecreaseMinionCount(isMelee);
    }

    private void OnDestroy()  { NotifyDeath(); }

    private void OnDisable()
    {
        if (gameObject.scene.IsValid())   // pooling’de sahne null olabilir
            NotifyDeath();
    }
}