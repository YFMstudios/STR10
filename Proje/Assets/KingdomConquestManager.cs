// KingdomConquestManager.cs
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class KingdomConquestManager : MonoBehaviourPunCallbacks
{
    [Header("Krallýk Image Referanslarý")]
    // Tüm krallýk Image referanslarý
    public Image lexionImage;
    public Image alfgardImage;
    public Image zephyrionImage;
    public Image arianopolImage;
    public Image dhamuronImage;
    public Image akhadzriaImage;

    // PhotonView referansý
    private PhotonView pv;

    private void Awake()
    {
        // PhotonView bileþenini al
        pv = GetComponent<PhotonView>();

        // Eðer yoksa uyarý ver
        if (pv == null)
        {
            Debug.LogError("[KingdomConquestManager] PhotonView bulunamadý! Lütfen PhotonView bileþenini ekleyin.");
        }
    }

    private void Start()
    {
        // Savaþ verilerini kontrol et ve uygula
        StartCoroutine(CheckAndApplyConquest());
    }

    // Savaþ sonrasý fethi uygula
    private IEnumerator CheckAndApplyConquest()
    {
        // Oyunun tüm nesnelerinin yüklenmesi için kýsa bir bekleme
        yield return new WaitForSeconds(2f);

        // Toprak deðiþimi gerekli mi?
        if (PlayerPrefs.GetInt("IsTerritoryChangeNeeded", 0) == 1)
        {
            string conqueror = PlayerPrefs.GetString("ConqueringKingdom", "");
            string conquered = PlayerPrefs.GetString("ConqueredKingdom", "");

            Debug.Log($"<color=green>[KingdomConquestManager] Toprak deðiþimi uygulanacak: {conqueror} -> {conquered}</color>");

            // Fetih iþlemini uygula
            ApplyConquest(conqueror, conquered);

            // Ýþlem tamamlandý olarak iþaretle
            PlayerPrefs.SetInt("TerritoryChangeCompleted", 1);
            PlayerPrefs.Save();

            // Master Client ise tüm oyunculara bildir
            if (PhotonNetwork.IsMasterClient && pv != null && pv.ViewID != 0)
            {
                // Güvenlik kontrolü ekleyerek RPC çaðrýsý yap
                try
                {
                    pv.RPC("RPC_NotifyConquestComplete", RpcTarget.Others);
                    Debug.Log("[KingdomConquestManager] RPC çaðrýsý baþarýlý.");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[KingdomConquestManager] RPC çaðrýsý baþarýsýz: {e.Message}");
                    // Alternatif yöntem - RPC olmadan
                    Debug.Log("[KingdomConquestManager] Alternatif yöntem kullanýlýyor - RPC olmadan.");
                }
            }
            else
            {
                Debug.LogWarning("[KingdomConquestManager] RPC çaðrýsý yapýlamadý. MasterClient: " + PhotonNetwork.IsMasterClient
                                + ", PhotonView: " + (pv != null ? "Var" : "Yok")
                                + ", ViewID: " + (pv != null ? pv.ViewID.ToString() : "N/A"));
            }
        }
    }

    // Fetih iþlemini uygula
    public void ApplyConquest(string conqueror, string conquered)
    {
        // Tüm Image bileþenlerini kontrol et ve RegionClickHandler'larýný bul
        List<Image> imageList = new List<Image>
        {
            lexionImage, alfgardImage, zephyrionImage,
            arianopolImage, dhamuronImage, akhadzriaImage
        };

        // Tüm geçerli Image'lerin RegionClickHandler'larýný topla
        HashSet<RegionClickHandler> handlers = new HashSet<RegionClickHandler>();

        foreach (Image img in imageList)
        {
            if (img != null)
            {
                RegionClickHandler handler = img.GetComponent<RegionClickHandler>();
                if (handler != null)
                {
                    handlers.Add(handler);
                    Debug.Log($"[KingdomConquestManager] {img.name} için RegionClickHandler bulundu");
                }
            }
        }

        // Her bir RegionClickHandler'a fetih iþlemini uygula
        foreach (RegionClickHandler handler in handlers)
        {
            handler.ConquerKingdom(conqueror, conquered);
            Debug.Log($"[KingdomConquestManager] Fetih bilgisi bir handler'a iletildi");
        }

        Debug.Log($"<color=green>[KingdomConquestManager] Fetih iþlemi tamamlandý! Toplam {handlers.Count} handler'a bilgi iletildi.</color>");
    }

    // Diðer oyunculara bildirim için RPC
    [PunRPC]
    private void RPC_NotifyConquestComplete()
    {
        Debug.Log("<color=green>[KingdomConquestManager] Fetih iþlemi tüm oyuncularda tamamlandý!</color>");
    }
}