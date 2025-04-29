using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerInfoManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField playerNameInputField;
    public Button kingdomButton;
    public GameObject inputPanel;
    public Button enterButton;

    private const string PlayerNameKey = "PlayerName";
private void Start()
{
    // CheckScene.selectedKingdom = "";  <<< BUNU SİL!

    playerNameInputField.text = "";
    playerNameInputField.interactable = !string.IsNullOrEmpty(CheckScene.selectedKingdom);  // <<< KRALLIK SEÇİLİYSE AKTİF

    kingdomButton.gameObject.SetActive(true);
    playerNameInputField.gameObject.SetActive(true);

    kingdomButton.onClick.AddListener(OnKingdomButtonPressed);
    enterButton.onClick.AddListener(OnEnterButtonPressed);

    playerNameInputField.onValueChanged.AddListener(OnInputFieldValueChanged);

    enterButton.interactable = false;
}





private void Update()
{
    string selectedKingdom = CheckScene.selectedKingdom;
    string playerName = playerNameInputField.text;

    playerNameInputField.interactable = !string.IsNullOrEmpty(selectedKingdom);
    enterButton.interactable = (!string.IsNullOrWhiteSpace(playerName) && !string.IsNullOrEmpty(selectedKingdom));
}



    private void OnInputFieldValueChanged(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            PlayerPrefs.SetString(PlayerNameKey, text);
            PlayerPrefs.Save();
        }
    }

    public void OnKingdomButtonPressed()
    
    {// Oyunun başlangıcında bir kere çalıştır (örn: Main Menu'de)
PhotonNetwork.AutomaticallySyncScene = false;

        SceneManager.LoadScene(4);
        kingdomButton.gameObject.SetActive(false);
    }

   public void OnEnterButtonPressed()
{
    string playerName = playerNameInputField.text;
    string selectedKingdom = CheckScene.selectedKingdom;

    if (string.IsNullOrWhiteSpace(playerName))
    {
        Debug.LogWarning("Kullanıcı adı boş bırakılamaz!");
        return;
    }

    if (string.IsNullOrEmpty(selectedKingdom))
    {
        Debug.LogWarning("Lütfen bir krallık seçiniz!");
        return;
    }

    var customProperties = new Hashtable();
    customProperties["Kingdom"] = selectedKingdom;
    customProperties["PlayerName"] = playerName;
    customProperties["FoodAmount"] = 0;
    customProperties["StoneAmount"] = 0;
    customProperties["GoldAmount"] = 0;
    customProperties["WoodAmount"] = 0;
    customProperties["IronAmount"] = 0;
    customProperties["WarPower"] = 0;
    customProperties["Warisonline"] = false;
    customProperties["SoldierCount"] = 0;
    customProperties["ArcherCount"] = 0;
    customProperties["Role"] = "spectator";
customProperties["HasConfirmed"] = true;  // <<< EKLENDİ

    PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

    inputPanel.SetActive(false);

    Debug.Log($"Photon'a gönderilen bilgiler: Kingdom = {selectedKingdom}, PlayerName = {playerName}, Role = spectator");
}

// private void ResetPlayerPrefs()  <<< ESKİ (yanlış)
public void ResetPlayerPrefs()
{
    if (PlayerPrefs.HasKey(PlayerNameKey))
    {
        PlayerPrefs.DeleteKey(PlayerNameKey);
        Debug.Log("PlayerPrefs'teki isim verisi temizlendi.");
    }

    // >>> BURAYI EKLE <<<
    if (playerNameInputField != null)
    {
        playerNameInputField.text = "";   // Ekrandaki input field'ı da temizle
    }
}



}
