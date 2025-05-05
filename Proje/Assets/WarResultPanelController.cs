using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using ExitGames.Client.Photon;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;

public class WarResultPanelController : MonoBehaviourPunCallbacks
{
    [SerializeField] public TextMeshProUGUI saldiranKrallikTMP;
    [SerializeField] public TextMeshProUGUI savunanKrallikTMP;
    [SerializeField] public TextMeshProUGUI saldiranKullaniciTMP;
    [SerializeField] public TextMeshProUGUI savunanKullaniciTMP;
    [SerializeField] public TextMeshProUGUI warResultTMP;

    [SerializeField] public Image saldiranKrallikFlama;
    [SerializeField] public Image savunanKrallikFlama;
    [SerializeField] public GameObject warResultPanel;

    private const string ROLE_KEY = "Role";
    private const string PLAYER_KEY = "PlayerName";
    private const string KINGDOM_KEY = "Kingdom";

    private PhotonView photonView;

    void Awake()
    {
        // PhotonView bileþenini al, eðer yoksa ekle
        photonView = GetComponent<PhotonView>();
        if (photonView == null)
        {
            photonView = gameObject.AddComponent<PhotonView>();
        }

        // Deðiþkenlerin null olup olmadýðýný kontrol et
        CheckSerializedFields();
    }
    private void Start()
    {
        ShowWarResultPanel("Akhadzria");
    }

    void CheckSerializedFields()
    {
        if (saldiranKrallikTMP == null) Debug.LogWarning("saldiranKrallikTMP is not assigned!");
        if (savunanKrallikTMP == null) Debug.LogWarning("savunanKrallikTMP is not assigned!");
        if (saldiranKullaniciTMP == null) Debug.LogWarning("saldiranKullaniciTMP is not assigned!");
        if (savunanKullaniciTMP == null) Debug.LogWarning("savunanKullaniciTMP is not assigned!");
        if (warResultTMP == null) Debug.LogWarning("warResultTMP is not assigned!");
        if (saldiranKrallikFlama == null) Debug.LogWarning("saldiranKrallikFlama is not assigned!");
        if (savunanKrallikFlama == null) Debug.LogWarning("savunanKrallikFlama is not assigned!");
        if (warResultPanel == null) Debug.LogWarning("warResultPanel is not assigned!");
    }
    public void ShowWarResultPanel(string kazananKrallik)
    {
        // Photon baðlantýsý kontrolü
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Photon baðlantýsý yok!");
            return;
        }

        // PhotonView null kontrolü
        if (photonView == null)
        {
            Debug.LogError("PhotonView bulunamadý!");
            return;
        }

        // RPC çaðrýsý
        photonView.RPC(nameof(RPC_CreateWarResultPanel), RpcTarget.All, kazananKrallik);
    }

    [PunRPC]
    private void RPC_CreateWarResultPanel(string kazananKrallik)
    {
        // Null kontrolleri
        if (warResultPanel == null)
        {
            Debug.LogError("warResultPanel null!");
            return;
        }

        // Saldýran ve savunan oyuncularý bul
        Player saldiranOyuncu = null, savunanOyuncu = null;

        foreach (var oyuncu in PhotonNetwork.PlayerList)
        {
            string rol = oyuncu.CustomProperties.TryGetValue(ROLE_KEY, out object rolObj)
                         ? rolObj.ToString()
                         : "izleyici";

            if (rol == "attacker" && saldiranOyuncu == null)
                saldiranOyuncu = oyuncu;
            else if (rol == "defender" && savunanOyuncu == null)
                savunanOyuncu = oyuncu;
        }

        // Oyuncu bilgilerini al
        if (saldiranOyuncu != null && savunanOyuncu != null)
        {
            // Krallýk adlarýný al
            string saldiranKrallik = saldiranOyuncu.CustomProperties.TryGetValue(KINGDOM_KEY, out object saldiranKrallikObj)
                ? saldiranKrallikObj.ToString()
                : "Bilinmeyen Krallýk";

            string savunanKrallik = savunanOyuncu.CustomProperties.TryGetValue(KINGDOM_KEY, out object savunanKrallikObj)
                ? savunanKrallikObj.ToString()
                : "Bilinmeyen Krallýk";

            // Kullanýcý adlarýný al
            string saldiranKullanici = saldiranOyuncu.CustomProperties.TryGetValue(PLAYER_KEY, out object saldiranKullaniciObj)
                ? saldiranKullaniciObj.ToString()
                : $"Oyuncu {saldiranOyuncu.ActorNumber}";

            string savunanKullanici = savunanOyuncu.CustomProperties.TryGetValue(PLAYER_KEY, out object savunanKullaniciObj)
                ? savunanKullaniciObj.ToString()
                : $"Oyuncu {savunanOyuncu.ActorNumber}";

            // Null kontrolleri
            if (saldiranKrallikTMP != null) saldiranKrallikTMP.text = Capitalize(saldiranKrallik);
            if (savunanKrallikTMP != null) savunanKrallikTMP.text = Capitalize(savunanKrallik);
            if (saldiranKullaniciTMP != null) saldiranKullaniciTMP.text = saldiranKullanici;
            if (savunanKullaniciTMP != null) savunanKullaniciTMP.text = savunanKullanici;

            // Bayraklarý yükle
            if (saldiranKrallikFlama != null)
            {
                Sprite saldiranBayrak = Resources.Load<Sprite>($"Flamas/{saldiranKrallik}WithFrame");
                if (saldiranBayrak != null)
                    saldiranKrallikFlama.sprite = saldiranBayrak;
                else
                    Debug.LogWarning($"Bayrak bulunamadý: Flamas/{saldiranKrallik}WithFrame");
            }

            if (savunanKrallikFlama != null)
            {
                Sprite savunanBayrak = Resources.Load<Sprite>($"Flamas/{savunanKrallik}WithFrame");
                if (savunanBayrak != null)
                    savunanKrallikFlama.sprite = savunanBayrak;
                else
                    Debug.LogWarning($"Bayrak bulunamadý: Flamas/{savunanKrallik}WithFrame");
            }

            // Savaþ sonuç metnini ayarla
            if (warResultTMP != null)
                warResultTMP.text = $"{Capitalize(kazananKrallik)} Kazandý.\nSavaþtan Çýkýlýyor.";

            // Paneli göster ve geri sayýmý baþlat
            warResultPanel.SetActive(true);
            StartCoroutine(CountdownAndLoadScene(kazananKrallik));
        }
    }

    private IEnumerator CountdownAndLoadScene(string kazananKrallik)
    {
        // Oyunu durdur
        Time.timeScale = 0f;

        float countdown = 10f;
        while (countdown > 0)
        {
            // Zamaný güncelle ve metni ayarla
            if (warResultTMP != null)
                warResultTMP.text = $"{Capitalize(kazananKrallik)} Kazandý.\nSavaþtan Çýkýlýyor({(int)countdown})";

            // Bir saniye bekle
            yield return new WaitForSecondsRealtime(1f);

            countdown -= 1f;
        }

        // Oyunu normale çevir
        Time.timeScale = 1f;

        // Tüm oyuncularý 6. sahneye yönlendir
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(RPC_LoadScene6), RpcTarget.AllBufferedViaServer);
        }
    }

    [PunRPC]
    private void RPC_LoadScene6()
    {
        // 6 numaralý sahneyi yükle
        PhotonNetwork.LoadLevel(6);
    }

    // Krallýk adýnýn ilk harfini büyük harf yap
    private string Capitalize(string s) =>
        string.IsNullOrEmpty(s)
            ? s
            : char.ToUpper(s[0]) + s.Substring(1).ToLower();
}