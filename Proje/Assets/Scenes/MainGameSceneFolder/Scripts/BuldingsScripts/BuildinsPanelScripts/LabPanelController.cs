using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabPanelController : MonoBehaviour
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

    public Button cancelLabButton;
    public bool isBuildCanceled = false;
    public GameObject progressBar;
    public PanelManager panelManager;

    public void refreshLab()
    {
        if (Lab.buildLevel == 1)
        {
            buildLevelText.text = "1";
            goldText.text = "5000";
            foodText.text = "4000";
            woodText.text = "3500";
            stoneText.text = "4500";
            ironText.text = "4000";
        }
        else if (Lab.buildLevel == 2)
        {
            buildLevelText.text = "2";
            goldText.text = "7500";
            foodText.text = "6000";
            woodText.text = "5000";
            stoneText.text = "7000";
            ironText.text = "6500";
        }
        else if (Lab.buildLevel == 3)
        {
            buildLevelText.text = "3";
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

    public void cancelLabBuild()
    {
        isBuildCanceled = true; // Ýptal iþlemini baþlat
        panelManager.DestroyPanel("LabBuildingProcessPanel");
        cancelLabButton.gameObject.SetActive(false);
    }
}
