using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomHostManager : MonoBehaviourPunCallbacks
{
    public Button startButton; // Başlat butonu referansı
    public int targetSceneIndex = 6; // Geçiş yapılacak sahnenin indexi

    private List<Player> playerList = new List<Player>(); // Oyuncuları tutacak liste

    void Awake()
    {
        // Sahne senkronizasyonunu etkinleştir
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        // Sürekli olarak oyuncu listesini güncelle
        InvokeRepeating(nameof(UpdatePlayerList), 0f, 1f); // Her saniyede bir günceller
        UpdateStartButton(); // İlk buton durumunu ayarla
    }

    private void UpdatePlayerList()
    {
        if (!PhotonNetwork.InRoom) return; // Eğer odada değilsek işlem yapma

        // Mevcut oyuncuları listeye ekle
        playerList.Clear();
        foreach (KeyValuePair<int, Player> playerEntry in PhotonNetwork.CurrentRoom.Players)
        {
            Player player = playerEntry.Value;
            if (!playerList.Contains(player))
            {
                playerList.Add(player);
            }
        }

        Debug.Log($"Player listesi güncellendi. Toplam oyuncu sayısı: {playerList.Count}");

        // Oda sahibini kontrol et ve ata
        AssignNewMaster();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} odaya katıldı.");
        UpdatePlayerList(); // Listeyi güncelle
        UpdateStartButton(); // Buton durumunu güncelle
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} odadan ayrıldı.");
        UpdatePlayerList(); // Oyuncu listesi güncelle
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"Yeni oda sahibi atandı: {newMasterClient.NickName}");
        UpdateStartButton(); // Yeni oda sahibine göre buton durumu güncelle
    }

    public void UpdateStartButton()
    {
        // Eğer oda sahibi isek buton görünür ve aktif, değilse gizli olur
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(true);
            startButton.interactable = true;
        }
        else
        {
            startButton.gameObject.SetActive(false);
        }
    }

    public void OnStartButtonPressed()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Oda sahibiysek, odadaki tüm oyuncuları aynı sahneye yönlendirelim
            PhotonNetwork.LoadLevel(targetSceneIndex);
        }
        else
        {
            Debug.LogWarning("Sadece oda sahibi oyunu başlatabilir!");
        }
    }

    // Yeni oda sahibini belirle
    private void AssignNewMaster()
    {
        if (PhotonNetwork.CurrentRoom.Players.Count > 0)
        {
            Player currentMaster = PhotonNetwork.MasterClient;

            // Eğer mevcut oda sahibi bizsek, butonu güncelle ve çık
            if (currentMaster == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("Zaten oda sahibiyim. Buton güncelleniyor.");
                UpdateStartButton();
                return;
            }

            // Yeni oda sahibini kontrol et
            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (player.IsMasterClient)
                {
                    Debug.Log($"Yeni oda sahibi otomatik atanmış: {player.NickName}");
                    UpdateStartButton(); // Buton durumunu güncelle
                    return;
                }
            }

            // Eğer otomatik atanmadıysa manuel olarak ilk oyuncuyu ata (çok nadir bir durum)
            var firstPlayer = PhotonNetwork.CurrentRoom.Players.Values.GetEnumerator();
            if (firstPlayer.MoveNext())
            {
                Player newMaster = firstPlayer.Current;
                PhotonNetwork.SetMasterClient(newMaster);
                Debug.Log($"Yeni oda sahibi manuel olarak atandı: {newMaster.NickName}");
            }
        }

        // Buton durumunu güncelle
        UpdateStartButton();
    }
}
