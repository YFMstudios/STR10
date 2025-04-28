using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HospitalPanelController : MonoBehaviour
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

    public Button cancelHospitalButton;
    public bool isBuildCanceled = false;
    public PanelManager panelManager;

                public Button buildHospitalButton;
    private Text buttonText;

    public void refreshHospital()
    {
        TextMeshProUGUI buttonText = buildHospitalButton.GetComponentInChildren<TextMeshProUGUI>();
        if (Hospital.buildLevel == 1)
        {
            buildLevelText.text = "1";
            goldText.text = "3500";
            foodText.text = "1800";
            woodText.text = "2200";
            stoneText.text = "2000";
            ironText.text = "1500";
             buttonText.text = "Yükselt";
        }
        else if (Hospital.buildLevel == 2)
        {
            buildLevelText.text = "2";
            goldText.text = "5000";
            foodText.text = "2500";
            woodText.text = "3500";
            stoneText.text = "3000";
            ironText.text = "2000";
             buttonText.text = "Yükselt";
        }
        else if (Hospital.buildLevel == 3)
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

    }

    public void cancelHospitalBuild()
    {
        isBuildCanceled = true; // �ptal i�lemini ba�lat
        cancelHospitalButton.gameObject.SetActive(false);
        panelManager.DestroyPanel("HospitalBuildingProcessPanel");
    }
}
