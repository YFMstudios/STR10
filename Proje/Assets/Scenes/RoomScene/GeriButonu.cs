using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GeriButonu : MonoBehaviourPunCallbacks
{
    public void GeriButonunaBasildi()
    {
        if (PhotonNetwork.InRoom)
        {
            // Oyuncu odadaysa, odadan çık
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            // Odada değilse doğrudan lobiye dön
            SceneManager.LoadScene(8); // veya sahne numarası ile 8
        }
    }

    public override void OnLeftRoom()
    {
        // Odadan çıkıldıktan sonra Photon'a tekrar bağlanıp lobiye gir
        PhotonNetwork.ConnectUsingSettings();  // Photon'a tekrar bağlan
    }

    public override void OnConnectedToMaster()
    {
        // Bağlantı tamamlandıktan sonra lobiye katıl
        PhotonNetwork.JoinLobby(); // Lobiye katıl
    }

    public override void OnJoinedLobby()
    {
        // Lobiye başarıyla katıldıktan sonra lobi sahnesini yükle
        SceneManager.LoadScene(8); // veya sahne numarası ile 8
    }
}
