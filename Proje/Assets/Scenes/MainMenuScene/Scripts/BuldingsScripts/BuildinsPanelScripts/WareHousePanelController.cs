using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WareHousePanelController : MonoBehaviour
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

    public GameObject progressBar;
    public Button cancelWarehouseButton;
    public bool isBuildCanceled = false;
    public PanelManager panelManager;

    public Button buildWareHouseButton;

    
    private Text buttonText;
    public void refreshWarehouse()
    {
        TextMeshProUGUI buttonText = buildWareHouseButton.GetComponentInChildren<TextMeshProUGUI>();
        if (Warehouse.buildLevel == 1)
        {
            buildLevelText.text = "1";
            goldText.text = "4000";
            foodText.text = "3000";
            woodText.text = "4000";
            stoneText.text = "3500";
            ironText.text = "3000";
            buttonText.text = "Yükselt";
        }
        else if (Warehouse.buildLevel == 2)
        {
            buildLevelText.text = "2";
            goldText.text = "6000";
            foodText.text = "4500";
            woodText.text = "5000";
            stoneText.text = "5000";
            ironText.text = "5500";
            buttonText.text = "Yükselt";
        }
        else if (Warehouse.buildLevel == 3)
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
    public void cancelWareHouseBuild()
    {
        isBuildCanceled = true; // �ptal i�lemini ba�lat
        panelManager.DestroyPanel("WareHouseBuildingProcessPanel");
        cancelWarehouseButton.gameObject.SetActive(false);
    }
}
