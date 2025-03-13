using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastlePanelController : MonoBehaviour
{
    public TMP_Text goldText;     // Altýn miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text woodText;     // Kereste miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text stoneText;    // Taþ miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text ironText;     // Demir miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text foodText;     // Yemek miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text buildLevelText;     // Bina seviyesini gösterecek TMP bileþeni
    public TMP_Text productionRateText; // Üretim Miktarýný gösterecek TMP bileþeni
    public TMP_Text maliyetText;
    public TMP_Text menzilText;
    public TMP_Text canText;
    public TMP_Text saldiriHiziText;


    public Image goldImage;        // Altýn için resim
    public Image woodImage;        // Kereste için resim
    public Image stoneImage;       // Taþ için resim
    public Image ironImage;        // Demir için resim
    public Image foodImage;

    public Button cancelUpgradeCastleButton;
    public bool isBuildCanceled = false;
    public PanelManager panelManager;
    public GameObject progressBar;

    public void refreshCastle()
    {

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

    public void cancelCastleBuild()
    {
        isBuildCanceled = true; // Ýptal iþlemini baþlat
        panelManager.DestroyPanel("CastleUpgradeProcessPanel");
        cancelUpgradeCastleButton.gameObject.SetActive(false);
    }
}
