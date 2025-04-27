using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarracksPanelController : MonoBehaviour
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

    public Button cancelBarracksButton;
    public bool isBuildCanceled = false;
    public GameObject progressBar;
    public PanelManager panelManager;
               public Button buildBarracksButton;
    private Text buttonText;
    public void refreshBarracks()
    {
        TextMeshProUGUI buttonText = buildBarracksButton.GetComponentInChildren<TextMeshProUGUI>();
        if (Barracks.buildLevel == 1)
        {
            buildLevelText.text = "1";
            productionRateText.text = "5 asker/dk";
            goldText.text = "3000";
            foodText.text = "2000";
            woodText.text = "2200";
            stoneText.text = "1800";
            ironText.text = "1500";
             buttonText.text = "Yükselt";
        }
        else if (Barracks.buildLevel == 2)
        {
            buildLevelText.text = "2";
            productionRateText.text = "10 asker/dk";
            goldText.text = "4500";
            foodText.text = "3000";
            woodText.text = "3000";
            stoneText.text = "2500";
            ironText.text = "2000";
             buttonText.text = "Yükselt";
        }
        else if (Barracks.buildLevel == 3)
        {
            buildLevelText.text = "3";
            productionRateText.text = "15 asker/dk";

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

    }

    public void cancelBarracksBuild()
    {
        isBuildCanceled = true; // �ptal i�lemini ba�lat
        panelManager.DestroyPanel("BarracksBuildingProcessPanel");
        cancelBarracksButton.gameObject.SetActive(false);
    }
}
