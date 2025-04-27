using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastlePanelController : MonoBehaviour
{
    public TMP_Text goldText;     // Alt�n miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text woodText;     // Kereste miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text stoneText;    // Ta� miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text ironText;     // Demir miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text foodText;     // Yemek miktar�n� g�sterecek TMP_Text bile�eni
    public TMP_Text buildLevelText;     // Bina seviyesini g�sterecek TMP bile�eni
    public TMP_Text productionRateText; // �retim Miktar�n� g�sterecek TMP bile�eni
    public TMP_Text maliyetText;
    public TMP_Text menzilText;
    public TMP_Text canText;
    public TMP_Text saldiriHiziText;


    public Image goldImage;        // Alt�n i�in resim
    public Image woodImage;        // Kereste i�in resim
    public Image stoneImage;       // Ta� i�in resim
    public Image ironImage;        // Demir i�in resim
    public Image foodImage;

    public Button cancelUpgradeCastleButton;
    public bool isBuildCanceled = false;
    public PanelManager panelManager;
    public GameObject progressBar;

                  public Button buildCastleButton;
    private Text buttonText;

    public void refreshCastle()
    {
        TextMeshProUGUI buttonText = buildCastleButton.GetComponentInChildren<TextMeshProUGUI>();
        if (Castle.buildLevel == 2)
        {
            buildLevelText.text = "2";
            goldText.text = "5000";
            foodText.text = "2500";
            woodText.text = "3500";
            stoneText.text = "3000";
            ironText.text = "2000";
            menzilText.text = "15 br";
            canText.text = "1500";
            saldiriHiziText.text = "7.5 h/s";
              buttonText.text = "Yükselt";
        }
        else if (Castle.buildLevel == 3)
        {
            buildLevelText.text = "3";
            menzilText.text = "20 br";
            canText.text = "2500";
            saldiriHiziText.text = "10 h/s";


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

    public void cancelCastleBuild()
    {
        isBuildCanceled = true; // �ptal i�lemini ba�lat
        panelManager.DestroyPanel("CastleUpgradeProcessPanel");
        cancelUpgradeCastleButton.gameObject.SetActive(false);
    }
}
