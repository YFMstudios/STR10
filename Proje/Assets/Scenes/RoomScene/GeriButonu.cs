using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using ExitGames.Client.Photon;

public class GeriButonu : MonoBehaviourPunCallbacks
{
    public void GeriButonunaBasildi()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene(8);
        }
    }

    public override void OnLeftRoom()
    {
        ExitGames.Client.Photon.Hashtable resetProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "Kingdom", "" },
            { "PlayerName", "" },
            { "FoodAmount", 0 },
            { "StoneAmount", 0 },
            { "GoldAmount", 0 },
            { "WoodAmount", 0 },
            { "IronAmount", 0 },
            { "WarPower", 0 },
            { "Warisonline", false },
            { "SoldierCount", 0 },
            { "ArcherCount", 0 },
            { "Role", "spectator" },
            { "HasConfirmed", false }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(resetProperties);

        CheckScene.selectedKingdom = "";

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Master Server'a bağlanıldı. Lobiye giriliyor...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobiye girildi. Ana menü sahnesine geçiliyor...");
        SceneManager.LoadScene(8);  // <<< Önce sahneyi yükle
    }

    private void Update()
    {
        // Eğer sahne 8'e geçildiyse, Reset işlemi yap
        if (SceneManager.GetActiveScene().buildIndex == 8)
        {
            if (FindObjectOfType<PlayerInfoManager>() != null)
            {
                FindObjectOfType<PlayerInfoManager>().ResetPlayerPrefs();
                Debug.Log("Sahne yüklenince InputField temizlendi.");
            }

            // Kendini devre dışı bırak bir kere çalışsın
            this.enabled = false;
        }
    }
}
