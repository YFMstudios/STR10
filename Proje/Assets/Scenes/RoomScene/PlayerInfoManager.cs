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
        kingdomButton.gameObject.SetActive(true);
        playerNameInputField.gameObject.SetActive(true);

        kingdomButton.onClick.AddListener(OnKingdomButtonPressed);
        enterButton.onClick.AddListener(OnEnterButtonPressed);

        LoadPlayerName();

        playerNameInputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    }

    private void OnInputFieldValueChanged(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            PlayerPrefs.SetString(PlayerNameKey, text);
            PlayerPrefs.Save();
        }
    }

    private void LoadPlayerName()
    {
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            playerNameInputField.text = PlayerPrefs.GetString(PlayerNameKey);
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

        if (string.IsNullOrWhiteSpace(playerName))
        {
            Debug.LogWarning("Kullanıcı adı boş bırakılamaz!");
            return;
        }

        string selectedKingdom = CheckScene.selectedKingdom;

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
        
        // Yeni eklenen oyuncu rol özelliği (varsayılan olarak spectator)
        customProperties["Role"] = "spectator";

        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

        inputPanel.SetActive(false);

        Debug.Log($"Photon'a gönderilen bilgiler: Kingdom = {selectedKingdom}, PlayerName = {playerName}, Role = spectator");
    }
}
