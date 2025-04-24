using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlacksmithPanelController : MonoBehaviour
{
    public TMP_Text goldText;     // Alt�n miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text woodText;     // Kereste miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text stoneText;    // Ta� miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text ironText;     // Demir miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text foodText;     // Yemek miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text buildLevelText;     // Bina seviyesini g�sterecek TMP bile�eni
    public TMP_Text productionRateText; // �retim Miktar�n� g�sterecek TMP bile�eni
    public TMP_Text maliyetText;

    public Image goldImage;        // Alt�n i�in resim
    public Image woodImage;        // Kereste i�in resim
    public Image stoneImage;       // Ta� i�in resim
    public Image ironImage;        // Demir i�in resim
    public Image foodImage;

    public Button cancelBlacksmithButton;
    public bool isBuildCanceled = false;
    public GameObject progressBar;
    public PanelManager panelManager;

           public Button buildBlackSmittButton;
    private Text buttonText;
    public void refreshBlacksmith()
    {
        TextMeshProUGUI buttonText = buildBlackSmittButton.GetComponentInChildren<TextMeshProUGUI>();
        if (Blacksmith.buildLevel == 1)
        {
            buildLevelText.text = "1";
            productionRateText.text = "5 b/s";
            goldText.text = "3500";
            foodText.text = "2000";
            woodText.text = "1800";
            stoneText.text = "1500";
            ironText.text = "1200";
               buttonText.text = "Yükselt";
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
               buttonText.text = "Yükselt";
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

    public void cancelBlacksmithBuild()
    {
        isBuildCanceled = true; // �ptal i�lemini ba�lat
        panelManager.DestroyPanel("BlacksmithBuildingProcessPanel");
        cancelBlacksmithButton.gameObject.SetActive(false);
    }
}
