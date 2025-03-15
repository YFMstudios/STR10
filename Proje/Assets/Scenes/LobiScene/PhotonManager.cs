using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;
using System.Collections;


public class PhotonManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNameInputField;
    public GameObject roomCreationPanel; // Oda oluşturma paneli

    // Prefablar ve UI için değişkenler
    public GameObject roomNumberPrefab;     // Oda numarası prefabı
    public GameObject roomNamePrefab;       // Oda ismi prefabı
    public GameObject playerCountPrefab;    // Oyuncu sayısı prefabı
    public GameObject roomJoinButtonPrefab; // Odaya katılma butonu prefabı

    public Transform contentParent;         // Prefabların yerleştirileceği parent
    public float yOffset = 50f;             // Her satır için dikey boşluk

    private bool isCreatingRoom = false;
    private string roomNameToCreate = null;
    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private bool shouldRefreshRoomList = false; // Refresh işlemi için bayrak

    private List<GameObject> roomEntries = new List<GameObject>(); // Oluşturulan prefabları takip etmek için

    private string roomNameToJoin = null;
    private bool isJoiningRoom = false;

    private Coroutine refreshCoroutine;
    void Start()
{
    // PhotonNetwork'e bağlanmaya çalışıyoruz
    if (!PhotonNetwork.IsConnected)
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "1.0"; // Versiyon kontrolü
        Debug.Log("Photon'a bağlanılıyor...");
    }
    else if (PhotonNetwork.IsConnected && !PhotonNetwork.InLobby)
    {
        // Eğer bağlıysak ama lobiye katılmadıysak, lobiye katıl
        PhotonNetwork.JoinLobby();
    }
    else
    {
        // Eğer zaten lobiye katılınmışsa, oda listesini yenile
        RefreshRoomList();
    }
}

public override void OnConnectedToMaster()
{
    // Bağlantı tamamlandı ve lobiye katılmaya hazır
    if (PhotonNetwork.IsConnectedAndReady)
    {
        Debug.Log("Lobiye başarıyla katıldınız!");

        // Oda oluşturma işlemi
        if (isCreatingRoom && !string.IsNullOrEmpty(roomNameToCreate))
        {
            CreateRoomInternal(roomNameToCreate);
            isCreatingRoom = false;
            roomNameToCreate = null;
        }

        // Oda listesini güncelleme veya yenileme işlemi
        if (shouldRefreshRoomList)
        {
            shouldRefreshRoomList = false;
            RefreshCachedRoomList();
        }
        else
        {
            RefreshRoomList();
        }
    }
    else
    {
        Debug.Log("Bağlantı henüz tamamlanmadı. Lobiye katılma işlemi başlatılmadı.");
    }
}



    private IEnumerator AutoRefreshRoomList()
{
    while (true)
    {
        RefreshRoomList(); // Oda listesini yenile
        yield return new WaitForSeconds(5f); // 5 saniyede bir çalıştır
    }
}

public new void OnEnable()
{
    // Üst sınıfın metodunu çağırmak isterseniz:
    base.OnEnable();
    // Kendi kodunuz
    if (refreshCoroutine == null)
    {
        refreshCoroutine = StartCoroutine(AutoRefreshRoomList());
    }
}

public new void OnDisable()
{
    // Üst sınıfın metodunu çağırmak isterseniz:
    base.OnDisable();
    // Kendi kodunuza
    if (refreshCoroutine != null)
    {
        StopCoroutine(refreshCoroutine);
        refreshCoroutine = null;
    }
}

    // Oda oluşturma
    public void CreateRoom()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("Sunucuya bağlı değilsiniz!");
            return;
        }

        if (PhotonNetwork.InRoom)
        {
            Debug.LogWarning("Zaten bir odadasınız, önce odadan çıkılıyor.");
            isCreatingRoom = true;
            roomNameToCreate = roomNameInputField.text;
            if (string.IsNullOrEmpty(roomNameToCreate))
            {
                Debug.LogWarning("Lütfen bir oda adı girin!");
                isCreatingRoom = false;
                return;
            }
            PhotonNetwork.LeaveRoom();
            return;
        }

        if (!PhotonNetwork.InLobby)
        {
            Debug.LogWarning("Lobiye katılınıyor...");
            PhotonNetwork.JoinLobby();
            isCreatingRoom = true;
            roomNameToCreate = roomNameInputField.text;
            return;
        }

        // Oda oluşturma işlemine devam et
        CreateRoomInternal(roomNameInputField.text);
    }

    private void CreateRoomInternal(string roomName)
{
    if (string.IsNullOrEmpty(roomName))
    {
        Debug.LogWarning("Lütfen bir oda adı girin!");
        return;
    }

    // 6 basamaklı rastgele bir sayı oluştur
    string roomNumber = Random.Range(100000, 999999).ToString();

    // Oda ismi ve rastgele sayıyı birleştir
    string fullRoomName = $"{roomName}_{roomNumber}";

    RoomOptions roomOptions = new RoomOptions
{
    MaxPlayers = 6,           // Maksimum 6 oyuncu
    IsVisible = true,         // Oda herkes tarafından görülebilir
    IsOpen = true,            // Oda yeni oyunculara açık
    PlayerTtl = 0,            // Oyuncu odadan çıkar çıkmaz bilgileri sıfırlanır
    EmptyRoomTtl = 300000,    // Oda boş kaldıktan sonra 5 dakika açık kalır
    CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
    {
        { "war", null }       // 'war' bilgisi oda özelliklerine ekleniyor (başlangıçta null olarak)
    },
    CustomRoomPropertiesForLobby = new string[] { "war" } // lobby'de 'war' bilgisini görmek için
};


    PhotonNetwork.CreateRoom(fullRoomName, roomOptions);
    Debug.Log($"Oda oluşturma isteği gönderildi: {fullRoomName}");
}





    public override void OnCreatedRoom()
    {
        Debug.Log($"Oda başarıyla oluşturuldu: {PhotonNetwork.CurrentRoom.Name}");

        // Oda oluşturma panelini kapat
        if (roomCreationPanel != null)
        {
            roomCreationPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("roomCreationPanel atanmamış!");
        }

        // Oda bilgilerini yazdır
        Debug.Log($"Oda Bilgileri:\nOda İsmi: {PhotonNetwork.CurrentRoom.Name}\n" +
                  $"Maksimum Oyuncu Sayısı: {PhotonNetwork.CurrentRoom.MaxPlayers}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Oda oluşturma başarısız: {message}");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Odadan çıkıldı.");

        if (isJoiningRoom && !string.IsNullOrEmpty(roomNameToJoin))
        {
            PhotonNetwork.JoinRoom(roomNameToJoin);
            isJoiningRoom = false;
            roomNameToJoin = null;
        }
        else if (shouldRefreshRoomList)
        {
            // Odadan çıkıldıktan sonra lobiye katıl
            PhotonNetwork.JoinLobby();
        }
        else if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    // Odaları listeleme ve UI ile gösterme
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Oda listesi güncellendi.");

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                // Oda listeden kaldırıldıysa, cachedRoomList'ten çıkar
                int index = cachedRoomList.FindIndex(r => r.Name == roomInfo.Name);
                if (index != -1)
                {
                    cachedRoomList.RemoveAt(index);
                }
            }
            else
            {
                // Oda yeni eklenmiş veya güncellenmişse, cachedRoomList'e ekle veya güncelle
                int index = cachedRoomList.FindIndex(r => r.Name == roomInfo.Name);
                if (index == -1)
                {
                    // Oda listede yok, ekle
                    cachedRoomList.Add(roomInfo);
                }
                else
                {
                    // Oda listede var, güncelle
                    cachedRoomList[index] = roomInfo;
                }
            }
        }

        RefreshCachedRoomList();
    }

    // Refresh butonuna basıldığında çağrılacak metot
    public void RefreshRoomList()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("Odadan çıkılıyor...");
                shouldRefreshRoomList = true;
                PhotonNetwork.LeaveRoom();
            }
            else if (!PhotonNetwork.InLobby)
            {
                Debug.Log("Lobiye katılınıyor...");
                shouldRefreshRoomList = true;
                PhotonNetwork.JoinLobby();
            }
            else
            {
                Debug.Log("Oda listesi yenileniyor...");
                // Oda listesini güncelle ve yazdır
                RefreshCachedRoomList();
            }
        }
        else
        {
            Debug.LogWarning("Sunucuya bağlı değilsiniz!");
        }
    }

    public void RefreshCachedRoomList()
    {
        // Mevcut prefabları temizle
        foreach (GameObject entry in roomEntries)
        {
            Destroy(entry);
        }
        roomEntries.Clear();

        if (cachedRoomList != null && cachedRoomList.Count > 0)
        {
            float yPosition = 0f;

            // Odaları sıralamak için (yeni odalar en alta eklensin)
          // Odaları sıralamak için (yeni odalar en alta eklensin)
for (int i = 0; i < cachedRoomList.Count; i++)
{
    RoomInfo room = cachedRoomList[i];

    // Oda numarası prefabını oluştur
    GameObject roomNumberObj = Instantiate(roomNumberPrefab, contentParent);
    roomNumberObj.GetComponent<TMP_Text>().text = room.Name.Split('_')[1]; // Oda numarası
    roomNumberObj.transform.position = roomNumberPrefab.transform.position + new Vector3(0, -yPosition, 0);

    // Oda ismi prefabını oluştur
    GameObject roomNameObj = Instantiate(roomNamePrefab, contentParent);
    roomNameObj.GetComponent<TMP_Text>().text = room.Name.Split('_')[0]; // Oda ismi
    roomNameObj.transform.position = roomNamePrefab.transform.position + new Vector3(0, -yPosition, 0);

    // Oyuncu sayısı prefabını oluştur
    GameObject playerCountObj = Instantiate(playerCountPrefab, contentParent);
    playerCountObj.GetComponent<TMP_Text>().text = $"{room.PlayerCount}/{room.MaxPlayers}";
    playerCountObj.transform.position = playerCountPrefab.transform.position + new Vector3(0, -yPosition, 0);

    // Odaya katılma butonu prefabını oluştur
    GameObject joinButtonObj = Instantiate(roomJoinButtonPrefab, contentParent);
    joinButtonObj.transform.position = roomJoinButtonPrefab.transform.position + new Vector3(0, -yPosition, 0);

    // Butonu aktif hale getirin
    joinButtonObj.SetActive(true);

    // Butonun tıklama olayını ayarla
    Button joinButton = joinButtonObj.GetComponent<Button>();
    if (joinButton != null)
    {
        // Oda ismini doğru bir şekilde yakala
        string roomNameCopy = room.Name; // Oda ismini tamamen alıyoruz
        Debug.Log($"Join button oluşturuluyor. Oda ismi: {roomNameCopy}"); // Oda ismini logla

        // Oda adı boşsa buton tıklama olayını ekleme
        joinButton.onClick.AddListener(() =>
        {
            // Eğer oda adı boşsa giriş yapma işlemini iptal et
            if (string.IsNullOrEmpty(roomNameCopy))
            {
                Debug.LogError("Oda adı boş olamaz! Giriş yapılmadı.");
                return; // Oda adı boşsa işlem sonlandırılır
            }

            // Odaya katılma işlemi
            if (PhotonNetwork.JoinRoom(roomNameCopy))
            {
                Debug.Log("Odaya katılındı: " + roomNameCopy);
            }
            else
            {
                Debug.LogError("Odaya katılmak başarısız oldu.");
            }
        });
    }
    else
    {
        Debug.LogWarning("Join Button bir Button bileşenine sahip değil.");
    }

    roomEntries.Add(roomNumberObj);
    roomEntries.Add(roomNameObj);
    roomEntries.Add(playerCountObj);
    roomEntries.Add(joinButtonObj);

    yPosition += yOffset; // Her satır için yOffset kadar aşağı kaydır
}
        }
        else
        {
            Debug.Log("Oda listesi boş.");
        }
    }


    public override void OnJoinedRoom()
    {
        Debug.Log($"Odaya katıldı: {PhotonNetwork.CurrentRoom.Name}");
        SceneManager.LoadScene(9);
    }

    public void CloseRoomCreationPanel()
{
    if (roomCreationPanel != null)
    {
        roomCreationPanel.SetActive(false);
        Debug.Log("Oda oluşturma paneli kapatıldı.");
    }
    else
    {
        Debug.LogWarning("roomCreationPanel atanmamış!");
    }
}

// Buton ile çağrılacak, bağlantıyı yeniden başlatıp oda listesini güncelleyen fonksiyon
public void ReconnectAndRefreshRoomList()
{
    StartCoroutine(ReconnectAndRefresh());
}

private IEnumerator ReconnectAndRefresh()
{
    // Kullanıcıyı önce bağlantıdan çıkar
    if (PhotonNetwork.IsConnected)
    {
        Debug.Log("Photon bağlantısı kesiliyor...");
        PhotonNetwork.Disconnect();
    }

    // Bağlantının tamamen kesilmesini bekle
    while (PhotonNetwork.IsConnected)
    {
        yield return null;
    }

    Debug.Log("Photon bağlantısı yeniden başlatılıyor...");
    // Photon'a yeniden bağlan
    PhotonNetwork.ConnectUsingSettings();
    PhotonNetwork.GameVersion = "1.0";

    // Bağlantının yeniden kurulmasını bekle
    while (!PhotonNetwork.IsConnectedAndReady)
    {
        yield return null;
    }

    Debug.Log("Photon'a yeniden bağlanıldı ve lobiye katılınıyor...");
    PhotonNetwork.JoinLobby();

    // Lobiye katılmayı bekle
    while (!PhotonNetwork.InLobby)
    {
        yield return null;
    }

    Debug.Log("Oda listesi yenileniyor...");
    // Oda listesini yenile
    RefreshRoomList();
}


}
