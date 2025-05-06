using Photon.Pun;
using UnityEngine;
using Photon.Pun;
using System.Collections;

public class MainGameSceneManager : MonoBehaviourPunCallbacks
{
    // RegionClickHandler referansý
    public RegionClickHandler regionClickHandler;

    private void Start()
    {
        // WarDataManager'den verileri yükle
        WarDataManager.LoadDataFromPlayerPrefs();

        // Toprak deðiþimi gerekiyorsa ve henüz tamamlanmadýysa iþlemi yap
        CheckAndApplyTerritoryChanges();
    }

    private void CheckAndApplyTerritoryChanges()
    {
        // Eðer toprak deðiþimi gerekliyse ve henüz tamamlanmadýysa
        if (WarDataManager.IsTerritoryChangeNeeded && !WarDataManager.TerritoryChangeCompleted)
        {
            Debug.Log("<color=green>[MainGameSceneManager] Toprak deðiþimi gerekli, uygulama baþlatýlýyor...</color>");

            // RegionClickHandler var mý kontrol et
            if (regionClickHandler == null)
            {
                regionClickHandler = FindObjectOfType<RegionClickHandler>();

                if (regionClickHandler == null)
                {
                    Debug.LogError("[MainGameSceneManager] RegionClickHandler bulunamadý! Toprak deðiþimi BAÞARISIZ.");
                    return;
                }
            }

            string conqueror = WarDataManager.ConqueringKingdom;
            string conquered = WarDataManager.ConqueredKingdom;

            // Fotish iþlemini uygula
            if (!string.IsNullOrEmpty(conqueror) && !string.IsNullOrEmpty(conquered))
            {
                Debug.Log($"<color=green>[MainGameSceneManager] Toprak deðiþimi uygulanýyor: {conqueror} -> {conquered}</color>");

                // RegionClickHandler üzerinden fetih iþlemini çaðýr
                regionClickHandler.ConquerKingdom(conqueror, conquered);

                // Ýþlemi tamamlandý olarak iþaretle
                WarDataManager.MarkTerritoryChangeCompleted();

                // Master Client ise RPC ile diðer oyunculara da bildir
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("RPC_TerritoryChangeComplete", RpcTarget.Others, conqueror, conquered);
                }
            }
            else
            {
                Debug.LogWarning("[MainGameSceneManager] Toprak deðiþimi bilgileri eksik veya hatalý!");
            }
        }
    }

    // Diðer oyunculara bilgi vermek için RPC
    [PunRPC]
    private void RPC_TerritoryChangeComplete(string conqueror, string conquered)
    {
        Debug.Log($"<color=green>[MainGameSceneManager][RPC] Toprak deðiþimi tamamlandý: {conqueror} -> {conquered}</color>");
        WarDataManager.MarkTerritoryChangeCompleted();
    }
}