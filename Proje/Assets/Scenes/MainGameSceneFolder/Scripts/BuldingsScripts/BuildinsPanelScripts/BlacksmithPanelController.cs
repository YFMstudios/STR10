using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithPanelController : MonoBehaviour
{
    public TMP_Text goldText;     // Altýn miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text woodText;     // Kereste miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text stoneText;    // Taþ miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text ironText;     // Demir miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text foodText;     // Yemek miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text buildLevelText;     // Bina seviyesini gösterecek TMP bileþeni
    public TMP_Text productionRateText; // Üretim Miktarýný gösterecek TMP bileþeni
    public TMP_Text maliyetText;

    public Image goldImage;        // Altýn için resim
    public Image woodImage;        // Kereste için resim
    public Image stoneImage;       // Taþ için resim
    public Image ironImage;        // Demir için resim
    public Image foodImage;

    public Button cancelBlacksmithButton;
    public bool isBuildCanceled = false;
    public GameObject progressBar;
    public PanelManager panelManager;
    public void refreshBlacksmith()
    {

        if (Blacksmith.buildLevel == 1)
        {
            buildLevelText.text = "1";
            productionRateText.text = "5 b/s";
            goldText.text = "3500";
            foodText.text = "2000";
            woodText.text = "1800";
            stoneText.text = "1500";
            ironText.text = "1200";
        }
        if (Blacksmith.buildLevel == 2)
        {
            buildLevelText.text = "2";
            productionRateText.text = "10 b/s";
            goldText.text = "7000";
            foodText.text = "4000";
            woodText.text = "3600";
            stoneText.text = "3000";
            ironText.text = "2400";
        }
        if (Blacksmith.buildLevel == 3)
        {
            buildLevelText.text = "3";
            productionRateText.text = "15 b/s";
            DestroyComponents();
        }
    }

    public void DestroyComponents()
    {
        // TMP_Text bileþenlerini yok et
        if (goldText != null) Destroy(goldText);
        if (woodText != null) Destroy(woodText);
        if (stoneText != null) Destroy(stoneText);
        if (ironText != null) Destroy(ironText);
        if (foodText != null) Destroy(foodText);
        if (maliyetText != null) Destroy(maliyetText);

        // Image bileþenlerini yok et
        if (goldImage != null) Destroy(goldImage);
        if (woodImage != null) Destroy(woodImage);
        if (stoneImage != null) Destroy(stoneImage);
        if (ironImage != null) Destroy(ironImage);
        if (foodImage != null) Destroy(foodImage);

        if (progressBar != null) Destroy(progressBar);
    }

    public void cancelBlacksmithBuild()
    {
        isBuildCanceled = true; // Ýptal iþlemini baþlat
        panelManager.DestroyPanel("BlacksmithBuildingProcessPanel");
        cancelBlacksmithButton.gameObject.SetActive(false);
    }
}
