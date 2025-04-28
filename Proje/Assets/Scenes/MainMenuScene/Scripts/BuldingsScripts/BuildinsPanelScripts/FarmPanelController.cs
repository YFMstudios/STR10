using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmPanelController : MonoBehaviour
{
    public TMP_Text goldText;     // Alt�n miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text woodText;     // Kereste miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text stoneText;    // Ta� miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text ironText;     // Demir miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text foodText;     // Yemek miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text buildLevelText;     // Bina seviyesini g�sterecek TMP bile�eni
    public TMP_Text maliyetText;

    public Image goldImage;        // Alt�n i�in resim
    public Image woodImage;        // Kereste i�in resim
    public Image stoneImage;       // Ta� i�in resim
    public Image ironImage;        // Demir i�in resim
    public Image foodImage;

    public Button cancelFarmButton;
    public bool isBuildCanceled = false;
    public GameObject progressBar;
    public PanelManager panelManager;

    public Button buildFarmButton;

    
    private Text buttonText;
    public void refreshFarm()
    {
        TextMeshProUGUI buttonText = buildFarmButton.GetComponentInChildren<TextMeshProUGUI>();
        if (Farm.buildLevel == 1)
        {
            buildLevelText.text = "1";
            goldText.text = "2200";
            foodText.text = "1000";
            woodText.text = "1600";
            stoneText.text = "700";
            ironText.text = "400";
            buttonText.text = "Yükselt";
        }
        if (Farm.buildLevel == 2)
        {
            buildLevelText.text = "2";
            goldText.text = "3600";
            foodText.text = "2000";
            woodText.text = "3200";
            stoneText.text = "1200";
            ironText.text = "800";
            buttonText.text = "Yükselt";
        }
        if (Farm.buildLevel == 3)
        {
            buildLevelText.text = "3";
            DestroyComponents();
        }
    }

    public void DestroyComponents()
    {
        // TMP_Text bile�enlerini yok et
        if (goldText != null) Destroy(goldText);
        if (woodText != null) Destroy(woodText);
        if (stoneText != null) Destroy(stoneText);
        if (ironText != null) Destroy(ironText);
        if (foodText != null) Destroy(foodText);
        if (maliyetText != null) Destroy(maliyetText);

        // Image bile�enlerini yok et
        if (goldImage != null) Destroy(goldImage);
        if (woodImage != null) Destroy(woodImage);
        if (stoneImage != null) Destroy(stoneImage);
        if (ironImage != null) Destroy(ironImage);
        if (foodImage != null) Destroy(foodImage);

        if (progressBar != null) Destroy(progressBar);
    }

    public void cancelFarmBuild()
    {
        isBuildCanceled = true; // �ptal i�lemini ba�lat
        panelManager.DestroyPanel("FarmBuildingProcessPanel");
        cancelFarmButton.gameObject.SetActive(false);
    }
}
