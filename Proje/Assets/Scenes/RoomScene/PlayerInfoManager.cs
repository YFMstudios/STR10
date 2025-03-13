using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon; // Photon'un Hashtable kullanımı için
using UnityEngine.SceneManagement;  // Sahne değiştirme için
using TMPro;

public class PlayerInfoManager : MonoBehaviourPunCallbacks
{
    // UI Bileşenleri
    public TMP_InputField playerNameInputField; // Kullanıcı ismi için InputField
    public Button kingdomButton;                // Krallık seçimi için buton
    public GameObject inputPanel;               // Panelin referansı
    public Button enterButton;                  // Enter butonunun referansı

    private const string PlayerNameKey = "PlayerName"; // PlayerPrefs'te kullanıcı adı için kullanılacak anahtar

    private void Start()
    {
        // Başlangıçta Krallık butonunu ve InputField'ı görünür yapalım
        kingdomButton.gameObject.SetActive(true);
        playerNameInputField.gameObject.SetActive(true);
        
        // Krallık butonuna tıklama olayı ekle
        kingdomButton.onClick.AddListener(OnKingdomButtonPressed);

        // Enter butonuna tıklama olayı ekle
        enterButton.onClick.AddListener(OnEnterButtonPressed);

        // InputField'a kaydedilen kullanıcı adını yükleyelim
        LoadPlayerName();

        // InputField'da bir değişiklik olduğunda kaydetmek için
        playerNameInputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    }

    // InputField'da yazı yazıldıkça kaydetmek için
    private void OnInputFieldValueChanged(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            // InputField'da bir şey yazıldıkça, yazılan değeri PlayerPrefs'e kaydediyoruz
            PlayerPrefs.SetString(PlayerNameKey, text);
            PlayerPrefs.Save();
        }
    }

    // Sahne değişimi yapıldığında kaydedilen ismi InputField'a yüklemek için
    private void LoadPlayerName()
    {
        // Kaydedilen kullanıcı adı varsa, InputField'a yükleyelim
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            playerNameInputField.text = PlayerPrefs.GetString(PlayerNameKey);  // Kaydedilen ismi InputField'a yükle
        }
    }

    public void OnKingdomButtonPressed()
    {
        // 4. ekrana geçiş için sahne değişimi yap
        SceneManager.LoadScene(4);

        // Butonu gizle
        kingdomButton.gameObject.SetActive(false);
    }

    public void OnEnterButtonPressed()
    {
        // Kullanıcı ismini al
        string playerName = playerNameInputField.text;

        // Boş isim kontrolü
        if (string.IsNullOrWhiteSpace(playerName))
        {
            Debug.LogWarning("Kullanıcı adı boş bırakılamaz!");
            return;
        }

        // CheckScene scriptinden krallık bilgisini al
        string selectedKingdom = CheckScene.selectedKingdom;

        // Kullanıcı adı ve krallık bilgisiyle birlikte Photon'a gönder
        var customProperties = new Hashtable();
        customProperties["Kingdom"] = selectedKingdom;  // Krallık bilgisini ekle
        customProperties["PlayerName"] = playerName;    // Kullanıcı adını ekle
        customProperties["FoodAmount"] = 0;    // Krallık Gıda 
        customProperties["StoneAmount"] = 0;    // Krallık Taş
        customProperties["GoldAmount"] = 0;    // Krallık Altın
        customProperties["WoodAmount"] = 0;    // Krallık Odun
        customProperties["IronAmount"] = 0;    // Krallık Demir
        customProperties["WarPower"] = 0;    // Krallık Savaş Gücü
        customProperties["Warisonline"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

        // Paneli kapat
        inputPanel.SetActive(false);

        // Debug: Photon'a gönderilen bilgileri yazdır
        Debug.Log($"Photon'a gönderilen bilgiler: Kingdom = {selectedKingdom}, PlayerName = {playerName}");
    }
}
