using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class WarController : MonoBehaviour
{
    public static WarController Instance;

    [Header("Genel")]
    public GetPlayerData getPlayerData;
    public string Attacker;   // nick / id
    public string Defender;
    public string AttackerKingdom; // Saldıran oyuncunun krallığı
    public string DefenderKingdom; // Savunan oyuncunun krallığı

    [Header("Sahne Referansları")]
    public MinionSpawner minionSpawner;       // Attacker tarafının spawner'ı
    public EnemyMinionSpawner enemyMinionSpawner;  // Defender tarafının spawner'ı
    public HealController healController;
    public WarResultPanelController warResultPanel; // Savaş sonuç paneli
    public GameObject defenderCastle;              // Kale nesnesi referansı

    [Header("Canlı Minyon Sayısı (sürekli güncellenir)")]
    public int playerkalanokçu;
    public int playerkalansavasçı;
    public int enemykalanokçu;
    public int enemykalansavasçı;

    [HideInInspector] public bool kaleyikildimi;   // Kale yıkıldığı sinyali
    [HideInInspector] public bool playerOlduMu;    // Oyuncu öldü sinyali

    // ------------------------------------------------------------
    private bool gameEnded = false;
    private string localRole = "";    // "attacker", "defender", "spectator"

    private PhotonView pv;            // cache
    private float checkCastleInterval = 1f; // Kale kontrol aralığı

    // ------------------------------------------------------------
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        pv = GetComponent<PhotonView>();

        // Yerel oyuncunun rolünü Photon'dan çek
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Role", out object r))
            localRole = r.ToString();
    }

    private void Start()
    {
        // Kale kontrolünü düzenli aralıklarla yap
        StartCoroutine(CheckDefenderCastle());

        // Savaşan oyuncuların krallık bilgilerini al
        GetKingdomInfo();
    }

    // Savaşan oyuncuların krallık bilgilerini al
    private void GetKingdomInfo()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("Role", out object role))
            {
                if (role.ToString() == "attacker" && player.CustomProperties.TryGetValue("Kingdom", out object attackerKingdom))
                {
                    AttackerKingdom = attackerKingdom.ToString();
                    Debug.Log($"<color=green>[WarController] Saldıranın krallığı: {AttackerKingdom}</color>");
                }
                else if (role.ToString() == "defender" && player.CustomProperties.TryGetValue("Kingdom", out object defenderKingdom))
                {
                    DefenderKingdom = defenderKingdom.ToString();
                    Debug.Log($"<color=green>[WarController] Savunanın krallığı: {DefenderKingdom}</color>");
                }
            }
        }
    }

    // Kaleyi düzenli aralıklarla kontrol etmek için Coroutine
    private IEnumerator CheckDefenderCastle()
    {
        while (!gameEnded)
        {
            // Kale null olduysa (destroy edildiyse) kaleyikildimi'yi true yap
            if (defenderCastle == null || !defenderCastle.activeInHierarchy)
            {
                kaleyikildimi = true;
                Debug.Log("<color=orange>[WarController] Kale yıkıldı tespit edildi!</color>");
            }

            yield return new WaitForSeconds(checkCastleInterval);
        }
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

            if (PhotonNetwork.IsMasterClient)
            {

                ConquestManager.Conquer(AttackerKingdom, DefenderKingdom);
                // Savaş kayıplarını gönder 
                FireCasualtyRPC();

                // Savaş sonuç panelini göster - Attacker kazandı
                ShowWarResult(AttackerKingdom);
            }
            return;
        }

        // ----- DURUM 2: Oyuncu öldü + ally minyon kalmadı -----
        if (playerOlduMu && IsAttackerOutOfMinions())
        {
            Debug.Log($"☠️ Oyuncu öldü ve minyon kalmadı. Savunan KAZANDI ✅ -> {Defender}\nSaldıran KAYBETTİ ❌ -> {Attacker}");
            gameEnded = true;

            if (PhotonNetwork.IsMasterClient)
            {
                // Savunan oyuncu uygulamadan savaş sonucu bilgisiyle birlikte çıkacak, fetih işlemi yok
                FireCasualtyRPC();

                // Savaş sonuç panelini göster - Defender kazandı
                ShowWarResult(DefenderKingdom);
            }
            return;
        }
    }

   

    // YENİ: Kalenin yıkıldığını tüm oyunculara bildiren RPC
    [PunRPC]
    public void RPC_CastleDestroyed()
    {
        kaleyikildimi = true;
        Debug.Log("<color=orange>[WarController][RPC] Kale yıkıldı bildirimi alındı!</color>");
    }

    // YENİ: Attacker'ın minyonlarının tükenip tükenmediğini kontrol et
    private bool IsAttackerOutOfMinions()
    {
        // minionSpawner.AreAllMinionsDead değerini kullan
        return minionSpawner != null && minionSpawner.AreAllMinionsDead;
    }

    // YENİ: Savaş sonuç panelini göster
    private void ShowWarResult(string kazananKrallik)
    {
        if (warResultPanel != null)
        {
            Debug.Log($"<color=green>[WarController] Savaş sonuç paneli gösteriliyor: {kazananKrallik} KAZANDI</color>");
            warResultPanel.ShowWarResultPanel(kazananKrallik);
        }
        else
        {
            Debug.LogError("[WarController] warResultPanel referansı atanmamış!");

            // Alternatif plan - doğrudan sahne yükleme
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogWarning("[WarController] Panel bulunamadı, 10 saniye sonra doğrudan sahne yüklenecek...");
                StartCoroutine(DelayedSceneLoad(10f));
            }
        }
    }

    // YENİ: Gecikme sonrası sahne yükleme (alternatif plan)
    private System.Collections.IEnumerator DelayedSceneLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        pv.RPC("RPC_LoadScene6", RpcTarget.AllBufferedViaServer);
    }

    // YENİ: Sahne yükleme RPC (alternatif plan)
    [PunRPC]
    private void RPC_LoadScene6()
    {
        PhotonNetwork.LoadLevel(6);
    }

    // ============================================================
    //  MASTER CLIENT → TÜM İSTEMCİLERE KAYIP BİLGİSİ GÖNDERİR
    // ============================================================
    private void FireCasualtyRPC()
    {
        int attackerArchers = minionSpawner.SpawlananArcherCount;
        int attackerSoldiers = minionSpawner.SpawlananSoldierCount;
        int defenderArchers = enemyMinionSpawner.SpawlananArcherCount;
        int defenderSoldiers = enemyMinionSpawner.SpawlananSoldierCount;

        // İsim önemli – RPC fonksiyonuyla tam aynı olmalı
        pv.RPC(nameof(SendCasualtiesToHealController),
               RpcTarget.All,
               attackerArchers, attackerSoldiers,
               defenderArchers, defenderSoldiers);
    }

    // ============================================================
    //  RPC: Her istemci kendi rolüne göre HealController'a yazar
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