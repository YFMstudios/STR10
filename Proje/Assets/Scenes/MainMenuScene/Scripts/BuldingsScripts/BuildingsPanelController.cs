using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsPanelController : MonoBehaviour
{
    public GameObject KislaPaneli;
    public GameObject TasOcagiPaneli;
    public GameObject HastanePaneli;
    public GameObject KeresteciPaneli;
    public GameObject DemirciPaneli;
    public GameObject CiftlikPaneli;
    public GameObject LaboratuvarPaneli;
    public GameObject AmbarPaneli;
    public GameObject KalePaneli;
    public GameObject KulePaneli;
    public GameObject TuzakPaneli;



    public TMP_Text goldText;     // Altın miktarını gösterecek TMP_Text bileşeni
    public TMP_Text woodText;     // Kereste miktarını gösterecek TMP_Text bileşeni
    public TMP_Text stoneText;    // Taş miktarını gösterecek TMP_Text bileşeni
    public TMP_Text ironText;     // Demir miktarını gösterecek TMP_Text bileşeni
    public TMP_Text foodText;     // Yemek miktarını gösterecek TMP_Text bileşeni
    public TMP_Text buildLevelText;     // Bina seviyesini gösterecek TMP bileşeni
    public TMP_Text productionRateText; // Üretim Miktarını gösterecek TMP bileşeni
    public TMP_Text maliyetText;

    public Image goldImage;        // Altın için resim
    public Image woodImage;        // Kereste için resim
    public Image stoneImage;       // Taş için resim
    public Image ironImage;        // Demir için resim
    public Image foodImage;

    // A��k olabilecek panellerin listesi
    private List<GameObject> allPanels;

    private void Start()
    {
        // T�m panelleri listeye ekleyelim
        allPanels = new List<GameObject> {
            KislaPaneli, TasOcagiPaneli, HastanePaneli,
            KeresteciPaneli, DemirciPaneli, CiftlikPaneli, LaboratuvarPaneli,  AmbarPaneli, KalePaneli, KulePaneli,TuzakPaneli
        };
    }

    // Belirli bir paneli a� ve di�erlerini kapat veya aktifse kapat
    public void OpenPanel(GameObject panelToToggle)
    {
        // E�er panel zaten a��ksa, t�kland���nda kapat�r
        if (panelToToggle.activeSelf)
        {
            panelToToggle.SetActive(false);
            return;
        }

        // Paneli a� ve di�er panelleri kapat
        foreach (GameObject panel in allPanels)
        {
            if (panel == panelToToggle)
            {
                panel.SetActive(true); // Paneli a�
            }
            else
            {
                panel.SetActive(false); // Di�er panelleri kapat
            }
        }
    }

    //Binalar seviye atladığında açıklamalarını güncelleyen fonksiyon.
    

    

    

   

    

    

    

    

    public void DestroyComponents()
    {
        // TMP_Text bileşenlerini yok et
        if (goldText != null) Destroy(goldText);
        if (woodText != null) Destroy(woodText);
        if (stoneText != null) Destroy(stoneText);
        if (ironText != null) Destroy(ironText);
        if (foodText != null) Destroy(foodText);
        if (maliyetText != null) Destroy(maliyetText);

        // Image bileşenlerini yok et
        if (goldImage != null) Destroy(goldImage);
        if (woodImage != null) Destroy(woodImage);
        if (stoneImage != null) Destroy(stoneImage);
        if (ironImage != null) Destroy(ironImage);
        if (foodImage != null) Destroy(foodImage);
    }
}