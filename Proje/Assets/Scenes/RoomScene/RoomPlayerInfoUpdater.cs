 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerInfoPrefab; // Prefab'inizi buraya atayın.
    public RectTransform Panel; // Prefab'lerin yerleşeceği panel (UI RectTransform).
    private List<int> playerOrder = new List<int>(); // Oyuncu sırasını tutan liste.
    private Dictionary<int, GameObject> playerObjects = new Dictionary<int, GameObject>();

    private float yOffset = 140f; // Her prefab arasındaki mesafe.
    private float topPadding = 150f; // En üst prefab için başlangıç mesafesi (panelin üstünden aşağı doğru).

    private void Start()
    {
        StartCoroutine(UpdatePlayerData());
    }

private IEnumerator UpdatePlayerData()
{
    while (true)
    {
        if (PhotonNetwork.InRoom)
        {
            foreach (KeyValuePair<int, Player> playerEntry in PhotonNetwork.CurrentRoom.Players)
            {
                Player player = playerEntry.Value;

                // Oyuncunun bilgilerini kontrol et
                if (player.CustomProperties == null || player.CustomProperties.Count == 0)
                {
                    Debug.Log($"Oyuncunun bilgileri eksik, prefab oluşturulmadı: {player.NickName}");
                    continue;
                }

                // CustomProperties'den oyuncu adı çekiliyor
                string playerName = player.CustomProperties.ContainsKey("PlayerName")
                    ? player.CustomProperties["PlayerName"].ToString()
                    : "Unnamed Player";

                // Krallık bilgisi alınıyor
                string kingdom = player.CustomProperties.ContainsKey("Kingdom")
                    ? player.CustomProperties["Kingdom"].ToString()
                    : "Unknown";

                // Debug log ile kontrol ediliyor
                Debug.Log($"Oyuncu Adı: {playerName}, Krallık: {kingdom}");


                // Debug log ekleyerek kontrol et
                Debug.Log($"Oyuncu Adı: {playerName}, Krallık: {kingdom}");

                // Bilgiler eksikse prefab oluşturmayı atla
                if (kingdom == "Unknown")
                {
                    Debug.Log($"Krallık bilgisi eksik, prefab oluşturulmadı: {playerName}");
                    continue;
                }

                // Eğer prefab zaten varsa güncelle
                if (playerObjects.ContainsKey(player.ActorNumber))
                {
                    var playerDisplay = playerObjects[player.ActorNumber].GetComponent<PlayerInfoDisplay>();
                    playerDisplay.UpdateInfo(playerName, kingdom);
                }
                else
                {
                    // Yeni prefab oluştur
                    GameObject playerObject = Instantiate(PlayerInfoPrefab, Panel);
                    playerObject.transform.localScale = new Vector3(0.8f, 0.5f, 0.5f); // Prefab'ı %80 küçült
                    playerObjects[player.ActorNumber] = playerObject;
                    playerOrder.Add(player.ActorNumber);
                    UpdatePrefabPositions();

                    // Prefab'ın ilk verilerini ayarla
                    var playerDisplay = playerObject.GetComponent<PlayerInfoDisplay>();
                    playerDisplay.UpdateInfo(playerName, kingdom);
                }
            }
        }

        yield return new WaitForSeconds(1f); // 1 saniyede bir güncelle
    }
}




    private void UpdatePrefabPositions()
{
    // Panelin üst kısmını başlangıç pozisyonu olarak al ve biraz daha yukarı kaydır
    float startY = Panel.rect.height / 2 - (topPadding - 50f); // Ekstra 50f yukarı kaydırma

    for (int i = 0; i < playerOrder.Count; i++)
    {
        int actorNumber = playerOrder[i];
        if (playerObjects.ContainsKey(actorNumber))
        {
            // Prefab'in pozisyonunu güncelle
            GameObject playerObject = playerObjects[actorNumber];
            RectTransform rectTransform = playerObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, startY - (i * yOffset));
        }
    }
}


    public override void OnPlayerLeftRoom(Player otherPlayer)
{
    // Prefab ve oyuncu sırasını kaldır
    if (playerObjects.ContainsKey(otherPlayer.ActorNumber))
    {
        Destroy(playerObjects[otherPlayer.ActorNumber]);
        playerObjects.Remove(otherPlayer.ActorNumber);
        playerOrder.Remove(otherPlayer.ActorNumber);
        UpdatePrefabPositions();
    }

    // Oyuncunun bilgilerini temizle
    if (otherPlayer.CustomProperties != null)
    {
        otherPlayer.CustomProperties.Clear(); // Oyuncunun özel bilgilerini temizle
        Debug.Log($"Oyuncunun bilgileri temizlendi: {otherPlayer.NickName}");
    }
}

private void ClearLocalPlayerData()
{
    // Yerel oyuncunun özel bilgilerini temizle
    if (PhotonNetwork.LocalPlayer.CustomProperties != null)
    {
        ExitGames.Client.Photon.Hashtable emptyProps = new ExitGames.Client.Photon.Hashtable();
        PhotonNetwork.LocalPlayer.SetCustomProperties(emptyProps); // Photon sunucusunda da temizle
        Debug.Log("Yerel oyuncunun bilgileri temizlendi.");
    }
}


public override void OnLeftRoom()
{
    ClearLocalPlayerData();

    // Prefabları temizle
    foreach (var playerObject in playerObjects.Values)
    {
        Destroy(playerObject);
    }

    playerObjects.Clear();
    playerOrder.Clear();

    Debug.Log("Odadan çıkıldı ve prefablar temizlendi.");
}


} 