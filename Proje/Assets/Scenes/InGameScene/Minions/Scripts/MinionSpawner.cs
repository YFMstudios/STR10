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

    // Ağdan gelen kesin değerler
    private int attackerSoldierCnt, attackerArcherCnt;
    private int defenderSoldierCnt, defenderArcherCnt;

    // ============================================================
    //  Start – yalnızca MasterClient çalıştırır
    // ============================================================
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

            Debug.Log($"[MinionSpawner][WAIT] t={waitTime:F1}s  attackerReady:{attackerReady}  defenderReady:{defenderReady}");

            if (attackerReady && defenderReady)
            {
                attackerSoldierCnt = (int)attacker.CustomProperties["SoldierCount"];
                attackerArcherCnt  = (int)attacker.CustomProperties["ArcherCount"];

                defenderSoldierCnt = (int)defender.CustomProperties["SoldierCount"];
                defenderArcherCnt  = (int)defender.CustomProperties["ArcherCount"];
                Debug.Log("[MinionSpawner] İki taraf da hazır, döngüden çıkılıyor.");
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
        Debug.Log("[MinionSpawner] SpawnMinions başladı.");
        int meleeLeft  = meleeUnitsToSpawn;
        int rangedLeft = rangedUnitsToSpawn;

        int unitsPerWave = 10;
        int waves = Mathf.CeilToInt((float)(meleeLeft + rangedLeft) / unitsPerWave);
        Debug.Log($"[MinionSpawner] Toplam {waves} dalga.");

        for (int wave = 0; wave < waves; wave++)
        {
            Debug.Log($"[MinionSpawner] === Dalga {wave + 1}/{waves} ===");
            int meleeThisWave  = Mathf.Min(5, meleeLeft);
            int rangedThisWave = Mathf.Min(5, rangedLeft);

            for (int i = 0; i < meleeThisWave; i++)
            {
                GameObject m = SpawnMinionForAll(true, meleeMinionMoveSpeed);
                AttachDeathLogic(m, true);
                meleeLeft--;
                kalanSavasci = meleeLeft;
                Debug.Log($"[MinionSpawner] Melee spawn – kalan:{meleeLeft}");
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            for (int i = 0; i < rangedThisWave; i++)
            {
                GameObject m = SpawnMinionForAll(false, rangedMinionMoveSpeed);
                AttachDeathLogic(m, false);
                rangedLeft--;
                kalanOkcu = rangedLeft;
                Debug.Log($"[MinionSpawner] Ranged spawn – kalan:{rangedLeft}");
                yield return new WaitForSeconds(delayBetweenMinions);
            }

            if (wave < waves - 1)
            {
                float wait = spawnInterval - delayBetweenMinions * (meleeThisWave + rangedThisWave);
                Debug.Log($"[MinionSpawner] Dalga arası {wait:F1}s bekleniyor.");
                yield return new WaitForSeconds(wait);
            }
        }
        Debug.Log("[MinionSpawner] Tüm dalgalar bitti.");
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

        Debug.Log($"[MinionSpawner] DecreaseMinionCount – Melee:{meleeRemaining}  Ranged:{rangedRemaining}");

        if (WarController.Instance != null)
        {
            WarController.Instance.playerkalansavasçı = meleeRemaining;
            WarController.Instance.playerkalanokçu    = rangedRemaining;
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
    public void Init(MinionSpawner s, bool melee) { spawner = s; isMelee = melee; }
    private void OnDestroy()
    {
        Debug.Log("[MinionDeathTracker] Minyon öldü.");
        if (spawner) spawner.DecreaseMinionCount(isMelee);
    }
}
