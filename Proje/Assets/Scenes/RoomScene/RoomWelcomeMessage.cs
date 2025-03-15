using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomWelcomeMessage : MonoBehaviourPunCallbacks
{
    // TextMeshPro UI bileşeni
    public TMP_Text welcomeText;

    void Start()
    {
        // Odaya giriş yapılmışsa, mesajı ayarla
        if (PhotonNetwork.InRoom)
        {
            DisplayWelcomeMessage();
        }
    }

    public override void OnJoinedRoom()
    {
        // Mevcut bir odaya katıldığında mesajı ayarla
        DisplayWelcomeMessage();
    }

    public override void OnCreatedRoom()
    {
        // Yeni bir oda kurulduğunda mesajı ayarla
        DisplayWelcomeMessage();
    }

    private void DisplayWelcomeMessage()
    {
        if (welcomeText != null)
        {
            // Oda ismini al ve mesajı oluştur
            string fullRoomName = PhotonNetwork.CurrentRoom.Name;

            // Oda ismini belirle (örneğin, '_' karakterine kadar olan kısmı al)
            string roomName = fullRoomName.Split('_')[0];

            // TextMeshPro'ya mesajı yazdır
            welcomeText.text = $"{roomName} isimli odaya hoş geldiniz!";
        }
        else
        {
            Debug.LogWarning("WelcomeText bileşeni atanmadı!");
        }
    }
}
