using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class WarController : MonoBehaviour
{
    public static WarController Instance;

    [Header("Genel")]
    public GetPlayerData getPlayerData;
    public string Attacker;   // nick / id
    public string Defender;

    [Header("Sahne Referansları")]
    public MinionSpawner      minionSpawner;       // Attacker tarafının spawner’ı
    public EnemyMinionSpawner enemyMinionSpawner;  // Defender tarafının spawner’ı
    public HealController     healController;

    [Header("Canlı Minyon Sayısı (sürekli güncellenir)")]
    public int playerkalanokçu;
    public int playerkalansavasçı;
    public int enemykalanokçu;
    public int enemykalansavasçı;

    [HideInInspector] public bool kaleyikildimi;   // Kale yıkıldığı sinyali
    [HideInInspector] public bool playerOlduMu;    // Oyuncu öldü sinyali

    // ------------------------------------------------------------
    private bool   gameEnded = false;
    private string localRole = "";    // "attacker", "defender", "spectator"

    private PhotonView pv;            // cache

    // ------------------------------------------------------------
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else                  Destroy(gameObject);

        pv = GetComponent<PhotonView>();

        // Yerel oyuncunun rolünü Photon’dan çek
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Role", out object r))
            localRole = r.ToString();
    }

    // ------------------------------------------------------------
    private void Update()
    {
        if (gameEnded) return;

        // ----- DURUM 1: Kale yıkıldı → saldıran kazandı -----
        if (kaleyikildimi)
        {
            Debug.Log($"🏰 Kale yıkıldı! Saldıran KAZANDI ✅ -> {Attacker}\nSavunan KAYBETTİ ❌ -> {Defender}");
            gameEnded = true;

            if (PhotonNetwork.IsMasterClient) FireCasualtyRPC();
            return;
        }

        // ----- DURUM 2: Oyuncu öldü + ally minyon kalmadı -----
        if (playerOlduMu && playerkalansavasçı == 0 && playerkalanokçu == 0)
        {
            Debug.Log($"☠️ Oyuncu öldü ve minyon kalmadı. Savunan KAZANDI ✅ -> {Defender}\nSaldıran KAYBETTİ ❌ -> {Attacker}");
            gameEnded = true;

            if (PhotonNetwork.IsMasterClient) FireCasualtyRPC();
            return;
        }
    }

    // ============================================================
    //  MASTER CLIENT → TÜM İSTEMCİLERE KAYIP BİLGİSİ GÖNDERİR
    // ============================================================
    private void FireCasualtyRPC()
    {
        int attackerArchers  = minionSpawner.SpawlananArcherCount;
        int attackerSoldiers = minionSpawner.SpawlananSoldierCount;
        int defenderArchers  = enemyMinionSpawner.SpawlananArcherCount;
        int defenderSoldiers = enemyMinionSpawner.SpawlananSoldierCount;

        // İsim önemli – RPC fonksiyonuyla tam aynı olmalı
        pv.RPC(nameof(SendCasualtiesToHealController),
               RpcTarget.All,
               attackerArchers, attackerSoldiers,
               defenderArchers, defenderSoldiers);
    }

    // ============================================================
    //  RPC: Her istemci kendi rolüne göre HealController’a yazar
    // ============================================================
    [PunRPC]
    public void SendCasualtiesToHealController(int attackerArcher,
                                               int attackerSoldier,
                                               int defenderArcher,
                                               int defenderSoldier)
    {
        if (healController == null) return;

        if (localRole == "attacker")
        {
           
            healController.setWoundedArcher(attackerArcher);
            healController.setWoundedSoldier(attackerSoldier);
            Debug.Log("Yaralı Savasci Sayisi : " + healController.woundedSoldier);
            Debug.Log("Yaralı Okcu Sayisi : " + healController.woundedArcher);
        }
        else if (localRole == "defender")
        {
            healController.setWoundedArcher(defenderArcher);
            healController.setWoundedSoldier(defenderSoldier);
            Debug.Log("Yaralı Savasci Sayisi : " + healController.woundedSoldier);
            Debug.Log("Yaralı Okcu Sayisi : " + healController.woundedArcher);
        }
        else
        {
            Debug.Log("[WarController] Yerel rol spectator – yaralı veri uygulanmadı.");
        }
    }
}
