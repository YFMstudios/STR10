using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WareHousePanelController : MonoBehaviour
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

    public GameObject progressBar;
    public Button cancelWarehouseButton;
    public bool isBuildCanceled = false;
    public PanelManager panelManager;
    public void refreshWarehouse()
    {
        if (Warehouse.buildLevel == 1)
        {
            buildLevelText.text = "1";
            goldText.text = "4000";
            foodText.text = "3000";
            woodText.text = "4000";
            stoneText.text = "3500";
            ironText.text = "3000";
        }
        else if (Warehouse.buildLevel == 2)
        {
            buildLevelText.text = "2";
            goldText.text = "6000";
            foodText.text = "4500";
            woodText.text = "5000";
            stoneText.text = "5000";
            ironText.text = "5500";
        }
        else if (Warehouse.buildLevel == 3)
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
    public void cancelWareHouseBuild()
    {
        isBuildCanceled = true; // Ýptal iþlemini baþlat
        panelManager.DestroyPanel("WareHouseBuildingProcessPanel");
        cancelWarehouseButton.gameObject.SetActive(false);
    }
}
