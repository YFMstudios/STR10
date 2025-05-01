// WaitingRoomUIManager.cs
// 6 flama + 12 TMP_Text doldurur, ready olunca 10 sn sonra 7. sahneye geçer.

using System.Collections;
using System.Collections.Generic;
using System.Linq;                             // LINQ için
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.SceneManagement;
using System.Globalization;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;   // Alias

public class WaitingRoomUIManager : MonoBehaviourPunCallbacks
{
    [Header("Sıralama: 0-Attacker | 1-Defender | 2-5 Spectator-1…4")]
    public Image[] flagImages = new Image[6];
    public TMP_Text[] kingdomTexts = new TMP_Text[6];
    public TMP_Text[] playerNameTexts = new TMP_Text[6];

    private readonly string[] defaultKingdomOrder =
        { "akhadzria", "alfgard", "arianopol", "dhamuron", "lexion", "zeprion" };

    private const string ROLE_KEY = "Role";
    private const string PLAYER_KEY = "PlayerName";
    private const string KINGDOM_KEY = "Kingdom";

    private Coroutine startRoutine;

     //ORJİNALİ BÖYLE
    void Start()
    {
        RefreshUI();//Vardı
        TryStartGame();//Vard
    }
    



    private void FakePlayersForTesting()
    {
        // Sahte oyuncu listesi oluştur
        var testPlayers = new List<(string role, string kingdom, string name)>
    {
        ("attacker", "akhadzria", "PlayerA"),
        ("defender", "alfgard", "PlayerB"),
        ("spectator", "arianopol", "Spec1"),
        ("spectator", "dhamuron", "Spec2"),
        ("spectator", "lexion", "Spec3"),
        ("spectator", "zeprion", "Spec4")
    };

        for (int i = 0; i < testPlayers.Count && i < 6; i++)
        {
            flagImages[i].gameObject.SetActive(true);
            kingdomTexts[i].gameObject.SetActive(true);
            playerNameTexts[i].gameObject.SetActive(true);

            var (role, kingdom, name) = testPlayers[i];
            kingdomTexts[i].text = Capitalize(kingdom);
            playerNameTexts[i].text = name;

            Sprite flag = Resources.Load<Sprite>($"Flamas/{kingdom}Flama[1]");
            if (flag != null)
                flagImages[i].sprite = flag;
            else
                Debug.LogWarning($"[TEST] Flama bulunamadı: Flamas/{kingdom}Flama[1]");
        }
    }


    // =====================  UI  =====================
    private void RefreshUI()
    {
        // 1) Her slotı gizle
        for (int i = 0; i < 6; i++)
        {
            flagImages[i].gameObject.SetActive(false);
            kingdomTexts[i].gameObject.SetActive(false);
            playerNameTexts[i].gameObject.SetActive(false);
        }

        // 2) Oyuncuları rollere ayır
        //    Eğer role yoksa => spectator
        Player attacker = null, defender = null;
        var spectators = new List<Player>();

        foreach (var p in PhotonNetwork.PlayerList)
        {
            string role = p.CustomProperties.TryGetValue(ROLE_KEY, out object rObj)
                          ? rObj.ToString()
                          : "spectator";

            if (role == "attacker" && attacker == null)
                attacker = p;
            else if (role == "defender" && defender == null)
                defender = p;
            else
                spectators.Add(p);
        }

        // 3) Slotlara sırayla yerleştir
        if (attacker != null) FillSlot(0, attacker);
        if (defender != null) FillSlot(1, defender);

        for (int i = 0; i < spectators.Count && i < 4; i++)
            FillSlot(2 + i, spectators[i]);
    }

    private void FillSlot(int index, Player p)
    {
        string pname = p.CustomProperties.TryGetValue(PLAYER_KEY, out object n)
                       ? n.ToString()
                       : $"Oyuncu {p.ActorNumber}";

        string kname = p.CustomProperties.TryGetValue(KINGDOM_KEY, out object k)
                       ? k.ToString()
                       : defaultKingdomOrder[index];

        // Görünür yap
        flagImages[index].gameObject.SetActive(true);
        kingdomTexts[index].gameObject.SetActive(true);
        playerNameTexts[index].gameObject.SetActive(true);

        // İçeriği doldur
        kingdomTexts[index].text = Capitalize(kname);
        playerNameTexts[index].text = pname;

        Sprite flag = Resources.Load<Sprite>($"Flamas/{kname}Flama[1]");
        if (flag != null)
            flagImages[index].sprite = flag;
        else
            Debug.LogWarning($"Flama bulunamadı: Flamas/{kname}Flama[1]");
    }

    private string Capitalize(string s) =>
        string.IsNullOrEmpty(s)
            ? s
            : char.ToUpper(s[0]) + s.Substring(1).ToLower();

    // =================  Oyun Başlatma  =================
    private void TryStartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        bool attackerReady = PhotonNetwork.PlayerList
            .Any(p => p.CustomProperties.TryGetValue(ROLE_KEY, out object r) && r.ToString() == "attacker");
        bool defenderReady = PhotonNetwork.PlayerList
            .Any(p => p.CustomProperties.TryGetValue(ROLE_KEY, out object r) && r.ToString() == "defender");

        if (attackerReady && defenderReady)
        {
            if (startRoutine == null)
                startRoutine = StartCoroutine(StartGameAfterDelay(10f));
        }
        else if (startRoutine != null)
        {
            StopCoroutine(startRoutine);
            startRoutine = null;
        }
    }

    private IEnumerator StartGameAfterDelay(float sec)
    {
        float t = 0f;
        while (t < sec)
        {
            yield return null;
            t += Time.unscaledDeltaTime;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("[WaitingRoom] Savaş sahnesi başlatılıyor, tüm oyunculara RPC gönderiliyor.");
            photonView.RPC(nameof(RPC_StartGame), RpcTarget.AllBufferedViaServer);
        }

    }

    [PunRPC]
    private void RPC_StartGame()
    {
        Debug.Log("[WaitingRoom] RPC_StartGame çağrıldı → Sahne 7'ye geçiliyor.");
        PhotonNetwork.LoadLevel(7);
    }


    // ===============  Photon Callback’leri  ===============
    public override void OnPlayerEnteredRoom(Player _) =>
        InvokeRefreshAndStart();

    public override void OnPlayerLeftRoom(Player _) =>
        InvokeRefreshAndStart();

    public override void OnPlayerPropertiesUpdate(Player _, PhotonHashtable changed)
    {
        if (changed.ContainsKey(ROLE_KEY) ||
            changed.ContainsKey(KINGDOM_KEY) ||
            changed.ContainsKey(PLAYER_KEY))
        {
            InvokeRefreshAndStart();
        }
    }

    private void InvokeRefreshAndStart()
    {
        RefreshUI();
        TryStartGame();
    }
}