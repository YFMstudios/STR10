using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class RegionClickHandler : MonoBehaviour, IPointerClickHandler
{
    // Bölge görselleri (harita üzerindeki çizgi görselleri vb.)
    public Image LexionLinePNGImage, AlfgardLinePNGImage, ZephrionLinePNGImage, ArianopolLinePNGImage, DhamuronLinePNGImage, AkhadzriaPNGImage;
    // Her bölgeye ait, harita üzerinde gösterilecek TMP bileşenleri
    public TMP_Text LexionTMP, AlfgardTMP, ZephrionTMP, ArianopolTMP, DhamuronTMP, AkhadzriaTMP;

    public Image FlagImage, WarIcon, ObservationImage;
    public Sprite warSprite, observationSprite;
    public TMP_Text owner, kingdom, civilization, numberOfSoldier;
    public TextMeshProUGUI playerNameText, kingdomNameText, foodAmountText, stoneAmountText, goldAmountText, woodAmountText, ironAmountText, warPowerText;

    public static string opponentName;
    private static Image lastClickedRegion;
    private static Dictionary<Image, Color> regionColors = new Dictionary<Image, Color>();

    private static string LastClickedKingdom = "Empty";
    int selectedKingdom = GetVariableFromHere.currentSpriteNum;

    // Her bölgenin harita üzerindeki Image bileşeni ile ilgili TMP metin bileşenini eşleştiren sözlük.
    private Dictionary<Image, TMP_Text> regionToTMPText = new Dictionary<Image, TMP_Text>();

    // Bölge sahipliğini tutan sözlük (bölge görseli -> krallık ismi)
    private Dictionary<Image, string> regionOwnership = new Dictionary<Image, string>();
    // Her krallığa ait detayları tutan sözlük
    private Dictionary<string, KingdomDetails> kingdomDetails = new Dictionary<string, KingdomDetails>();

    public GetPlayerData getPlayerData;

    void Start()
    {
        createDefaultPanel();
        InitializeRegionColors();
        InitializeKingdomRegions();
        getPlayerData.SetRegionHandler(this);
        // Her bölge görseliyle, üzerine yazılacak TMP bileşenini eşleştiriyoruz.
        regionToTMPText.Add(LexionLinePNGImage, LexionTMP);
        regionToTMPText.Add(AlfgardLinePNGImage, AlfgardTMP);
        regionToTMPText.Add(ZephrionLinePNGImage, ZephrionTMP);
        regionToTMPText.Add(ArianopolLinePNGImage, ArianopolTMP);
        regionToTMPText.Add(DhamuronLinePNGImage, DhamuronTMP);
        regionToTMPText.Add(AkhadzriaPNGImage, AkhadzriaTMP);

        // Başlangıçta her bölgenin TMP metinini, başlangıç sahipliği bilgisine göre ayarlıyoruz.
        foreach (var kvp in regionOwnership)
        {
            if (regionToTMPText.ContainsKey(kvp.Key))
            {
                regionToTMPText[kvp.Key].text = kvp.Value;
            }
        }
    }

    private void InitializeRegionColors()
    {
        foreach (var region in FindObjectsOfType<Image>())
        {
            if (!regionColors.ContainsKey(region))
            {
                regionColors[region] = region.color;
            }
        }
    }

    private void InitializeKingdomRegions()
    {
        kingdomDetails.Add("Lexion", new KingdomDetails("Lexion", "Elf", Color.green, new List<Image> { LexionLinePNGImage }, LexionLinePNGImage.sprite));
        kingdomDetails.Add("Alfgard", new KingdomDetails("Alfgard", "Büyücü", Color.blue, new List<Image> { AlfgardLinePNGImage }, AlfgardLinePNGImage.sprite));
        kingdomDetails.Add("Zephrion", new KingdomDetails("Zephrion", "Ölüler", Color.red, new List<Image> { ZephrionLinePNGImage }, ZephrionLinePNGImage.sprite));
        kingdomDetails.Add("Arianopol", new KingdomDetails("Arianopol", "İnsan", Color.yellow, new List<Image> { ArianopolLinePNGImage }, ArianopolLinePNGImage.sprite));
        kingdomDetails.Add("Dhamuron", new KingdomDetails("Dhamuron", "Cüceler", Color.cyan, new List<Image> { DhamuronLinePNGImage }, DhamuronLinePNGImage.sprite));
        kingdomDetails.Add("Akhadzria", new KingdomDetails("Akhadzria", "Orklar", Color.magenta, new List<Image> { AkhadzriaPNGImage }, AkhadzriaPNGImage.sprite));

        // Her krallığa ait bölgelerin başlangıç sahiplik bilgilerini ayarlıyoruz.
        foreach (var kvp in kingdomDetails)
        {
            foreach (var region in kvp.Value.RegionImages)
            {
                if (region != null && !regionOwnership.ContainsKey(region))
                {
                    regionOwnership[region] = kvp.Key; // Başlangıçta bölge, kendi krallığına aittir.
                }
            }
        }
      
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Image imageComponent = GetComponent<Image>();
        string clickedKingdomName = GetKingdomByRegion(imageComponent);

        if (LastClickedKingdom == "Empty")
        {
            LastClickedKingdom = clickedKingdomName;
            SetKingdomColor(clickedKingdomName);
        }
        else if (LastClickedKingdom != clickedKingdomName)
        {
            ResetKingdomColor(LastClickedKingdom);
            SetKingdomColor(clickedKingdomName);
            LastClickedKingdom = clickedKingdomName;
        }
        else
        {
            ResetKingdomColor(clickedKingdomName);
            LastClickedKingdom = "Empty";
        }
        UpdateRegionDetails(imageComponent);
        UpdatePhotonPlayerDetails(clickedKingdomName);
    }

    private void ResetKingdomColor(string kingdomName)
    {
        foreach (var kvp in regionOwnership)
        {
            if (kvp.Value == kingdomName)
            {
                kvp.Key.color = Color.white;
            }
        }
    }

    private void SetKingdomColor(string kingdomName)
    {
        if (kingdomDetails.ContainsKey(kingdomName))
        {
            Color kingdomColor = kingdomDetails[kingdomName].KingdomColor;
            foreach (var kvp in regionOwnership)
            {
                if (kvp.Value == kingdomName)
                {
                    kvp.Key.color = kingdomColor;
                }
            }
        }
    }

    // Fetih işlemi gerçekleştiğinde; fetheden ülke, fethedilen ülkenin tüm bölgelerini devralır.
    // Aynı zamanda, bölgeye ait TMP metni de fetheden ülkenin adını gösterecek şekilde güncellenir.
    public void ConquerKingdom(string conqueringKingdom, string conqueredKingdom)
    {
        int conqueringKingdomNumber = Kingdom.returnsKingdomNumbers(conqueringKingdom);
        int conqueredKingdomNumber = Kingdom.returnsKingdomNumbers(conqueredKingdom);

        if (kingdomDetails.ContainsKey(conqueringKingdom) && kingdomDetails.ContainsKey(conqueredKingdom))
        {
            var regionsToUpdate = new List<Image>();

            foreach (var kvp in regionOwnership)
            {
                if (kvp.Value == conqueredKingdom)
                {
                    regionsToUpdate.Add(kvp.Key);
                }
            }

            foreach (var region in regionsToUpdate)
            {
                regionOwnership[region] = conqueringKingdom;
                if (regionToTMPText.ContainsKey(region))
                {
                    regionToTMPText[region].text = conqueringKingdom;
                }
            }
            Kingdom.kingdoms[conqueringKingdomNumber].GoldAmount += Kingdom.kingdoms[conqueredKingdomNumber].GoldAmount;
            Kingdom.kingdoms[conqueredKingdomNumber].GoldAmount = 0;
            Kingdom.kingdoms[conqueringKingdomNumber].FoodAmount += Kingdom.kingdoms[conqueredKingdomNumber].FoodAmount;
            Kingdom.kingdoms[conqueredKingdomNumber].FoodAmount = 0;
            Kingdom.kingdoms[conqueringKingdomNumber].WoodAmount += Kingdom.kingdoms[conqueredKingdomNumber].WoodAmount;
            Kingdom.kingdoms[conqueredKingdomNumber].WoodAmount = 0;
            Kingdom.kingdoms[conqueringKingdomNumber].StoneAmount += Kingdom.kingdoms[conqueredKingdomNumber].StoneAmount;
            Kingdom.kingdoms[conqueredKingdomNumber].StoneAmount = 0;
            Kingdom.kingdoms[conqueringKingdomNumber].IronAmount += Kingdom.kingdoms[conqueredKingdomNumber].IronAmount;
            Kingdom.kingdoms[conqueredKingdomNumber].IronAmount = 0;
        }
        else
        {
            Debug.LogWarning($"Fetih işlemi başarısız: {conqueringKingdom} veya {conqueredKingdom} geçerli değil.");
        }

        

    }

    private string GetKingdomByRegion(Image imageComponent)
    {
        return regionOwnership.ContainsKey(imageComponent) ? regionOwnership[imageComponent] : "Unknown";
    }

    private void UpdateRegionDetails(Image imageComponent)
    {
        string kingdomName = GetKingdomByRegion(imageComponent);

        if (kingdomDetails.ContainsKey(kingdomName))
        {
            KingdomDetails details = kingdomDetails[kingdomName];
            FlagImage.sprite = Resources.Load<Sprite>($"Flags/{kingdomName}Flag");
            bool isPlayerOwned = isYourKingdoms(kingdomName);
            WarIcon.enabled = !isPlayerOwned;
            ObservationImage.enabled = !isPlayerOwned;

            if (!isPlayerOwned)
            {
                WarIcon.sprite = warSprite;
                ObservationImage.sprite = observationSprite;
            }

            owner.text = $"Sahibi: {findOwner(kingdomName)}";
            kingdom.text = $"Krallık: {kingdomName}";
            civilization.text = $"Medeniyet: {details.CivilizationName}";
            // Asker sayısı kısmı, ilgili indekse göre güncelleniyor
            // numberOfSoldier.text = $"Asker Sayısı: {Kingdom.Kingdoms[kingdomNameToKingdomID(kingdomName)].SoldierAmount}";
        }
        else
        {
            Debug.LogWarning("Bölge için eşleşme bulunamadı.");
        }
    }

    public int kingdomNameToKingdomID(string kingdomName)
    {
        if (kingdomName == "Arianopol") return 0;
        else if (kingdomName == "Alfgard") return 1;
        else if (kingdomName == "Akhadzria") return 2;
        else if (kingdomName == "Dhamuron") return 3;
        else if (kingdomName == "Lexion") return 4;
        else if (kingdomName == "Zephrion") return 5;
        else
        {
            Debug.Log(kingdomName + " Bulunamadı");
            return -1;
        }
    }

    public string findOwner(string kingdomName)
    {
        if (ScreenTransitions2.ScreenNavigator.previousScreen == "Simple" ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Mid" ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Hard")
        {
            foreach (Kingdom kingdom in Kingdom.Kingdoms)
            {
                if (kingdom.Owner == 1 && kingdom.Name == kingdomName)
                {
                    return "Player";
                }
            }
            return "Bilgisayar";
        }
        else
        {
            Player[] players = PhotonNetwork.PlayerList;
            foreach (Player player in players)
            {
                if (player.CustomProperties.ContainsKey("Kingdom") &&
                    player.CustomProperties["Kingdom"].ToString() == kingdomName)
                {
                    opponentName = player.CustomProperties.ContainsKey("PlayerName")
                                   ? player.CustomProperties["PlayerName"].ToString()
                                   : "Oyuncu Adı Yok";
                    return player.CustomProperties.ContainsKey("PlayerName")
                        ? player.CustomProperties["PlayerName"].ToString()
                        : "Oyuncu Adı Yok";
                }
            }
            return "Bilgisayar";
        }
    }

    public void createDefaultPanel()
    {
        if (selectedKingdom == 2)
        {
            FlagImage.sprite = Kingdom.Kingdoms[2].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krallık: Akhadzria";
            civilization.text = "Medeniyet: Ork";
            numberOfSoldier.text = "Asker Sayısı: " + Kingdom.Kingdoms[2].SoldierAmount.ToString();
        }
        else if (selectedKingdom == 3)
        {
            FlagImage.sprite = Kingdom.Kingdoms[1].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krallık: Alfgard";
            civilization.text = "Medeniyet: Büyücü";
            numberOfSoldier.text = "Asker Sayısı: " + Kingdom.Kingdoms[1].SoldierAmount.ToString();
        }
        else if (selectedKingdom == 4)
        {
            FlagImage.sprite = Kingdom.Kingdoms[0].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krallık: Arianopol";
            civilization.text = "Medeniyet: İnsan";
            numberOfSoldier.text = "Asker Sayısı: " + Kingdom.Kingdoms[0].SoldierAmount.ToString();
        }
        else if (selectedKingdom == 5)
        {
            FlagImage.sprite = Kingdom.Kingdoms[3].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krallık: Dhamuron";
            civilization.text = "Medeniyet: Cüceler";
            numberOfSoldier.text = "Asker Sayısı: " + Kingdom.Kingdoms[3].SoldierAmount.ToString();
        }
        else if (selectedKingdom == 6)
        {
            FlagImage.sprite = Kingdom.Kingdoms[4].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krallık: Lexion";
            civilization.text = "Medeniyet: Elf";
            numberOfSoldier.text = "Asker Sayısı: " + Kingdom.Kingdoms[4].SoldierAmount.ToString();
        }
        else
        {
            FlagImage.sprite = Kingdom.Kingdoms[5].Flag;
            WarIcon.enabled = false;
            owner.text = "Sahibi: Player";
            kingdom.text = "Krallık: Zephrion";
            civilization.text = "Medeniyet: Ölüler";
            numberOfSoldier.text = "Asker Sayısı: " + Kingdom.Kingdoms[5].SoldierAmount.ToString();
        }
    }

    private void UpdatePhotonPlayerDetails(string kingdomName)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("Kingdom", out object playerKingdom) && playerKingdom.ToString() == kingdomName)
            {
                playerNameText.text = $"Player Name: {player.CustomProperties["PlayerName"]}";
                kingdomNameText.text = $"Kingdom: {kingdomName}";
                foodAmountText.text = $"Food: {player.CustomProperties["FoodAmount"]}";
                stoneAmountText.text = $"Stone: {player.CustomProperties["StoneAmount"]}";
                goldAmountText.text = $"Gold: {player.CustomProperties["GoldAmount"]}";
                woodAmountText.text = $"Wood: {player.CustomProperties["WoodAmount"]}";
                ironAmountText.text = $"Iron: {player.CustomProperties["IronAmount"]}";
                warPowerText.text = $"War Power: {player.CustomProperties["WarPower"]}";

                /*
                if (player.CustomProperties["PlayerName"] == OyuncununAdı)//Seçili bölgedeki playername benim adım ise
                {
                    WarIcon.enabled = false;
                    ObservationImage.enabled = false;
                }
                else
                {
                    WarIcon.enabled = true;
                    ObservationImage.enabled = true;
                    WarIcon.sprite = warSprite;
                    ObservationImage.sprite = observationSprite;
                }
                */
                break;
            }
        }
    }

    private bool isYourKingdoms(string name)
    {
        foreach (Kingdom kingdom in Kingdom.Kingdoms)
        {
            if (kingdom.Owner == 1 && kingdom.Name == name)
            {
                return true;
            }
        }
        return false;
    }
}

public class KingdomDetails
{
    public string KingdomName { get; }
    public string CivilizationName { get; }
    public Color KingdomColor { get; }
    public List<Image> RegionImages { get; }
    public Sprite DefaultSprite { get; }

    public KingdomDetails(string kingdomName, string civilizationName, Color kingdomColor, List<Image> regionImages, Sprite defaultSprite)
    {
        KingdomName = kingdomName;
        CivilizationName = civilizationName;
        KingdomColor = kingdomColor;
        RegionImages = regionImages;
        DefaultSprite = defaultSprite;
    }
}
