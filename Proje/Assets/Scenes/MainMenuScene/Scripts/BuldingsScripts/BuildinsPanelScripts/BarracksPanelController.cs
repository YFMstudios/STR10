using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarracksPanelController : MonoBehaviour
{
    public TMP_Text goldText;     
    public TMP_Text woodText;     
    public TMP_Text stoneText;    
    public TMP_Text ironText;     
    public TMP_Text foodText;    
    public TMP_Text buildLevelText;     
    public TMP_Text maliyetText;

    //Savasci Ozellikleri
    public TMP_Text savasciOzellikleri;    
    //Okcu Ozellikleri
    public TMP_Text okcuOzellikleri;    


    public Image goldImage;        
    public Image woodImage;        
    public Image stoneImage;       
    public Image ironImage;        
    public Image foodImage;

    public Button cancelBarracksButton;
    public bool isBuildCanceled = false;
    public GameObject progressBar;
    public PanelManager panelManager;
    public Button buildBarracksButton;
    private Text buttonText;

    public ResearchActions researchActions;
    public GetPlayerData getPlayerData;
    public void refreshBarracks()
    {
        
        TextMeshProUGUI buttonText = buildBarracksButton.GetComponentInChildren<TextMeshProUGUI>();
        if (Barracks.buildLevel == 0)
        {
            refreshSavasciOzellikleri();
            refreshOkcuOzellikleri();
        }
        else if (Barracks.buildLevel == 1)
        {
            buildLevelText.text = "1";
            goldText.text = "3000";
            foodText.text = "2000";
            woodText.text = "2200";
            stoneText.text = "1800";
            ironText.text = "1500";
             buttonText.text = "Yükselt";
            refreshSavasciOzellikleri();
            refreshOkcuOzellikleri();
        }
        else if (Barracks.buildLevel == 2)
        {
            buildLevelText.text = "2";
            goldText.text = "4500";
            foodText.text = "3000";
            woodText.text = "3000";
            stoneText.text = "2500";
            ironText.text = "2000";
             buttonText.text = "Yükselt";
            refreshSavasciOzellikleri();
            refreshOkcuOzellikleri();
        }
        else if (Barracks.buildLevel == 3)
        {
            buildLevelText.text = "3";
            DestroyComponents();
            refreshSavasciOzellikleri();
            refreshOkcuOzellikleri();
        }
    }

    public void refreshSavasciOzellikleri()
    {
        // Savaşçı özelliklerini al
        float can = researchActions.GetMeleeMinionCan();
        float hasar = researchActions.GetMeleeMinionHasar();
        float hiz = researchActions.GetMeleeMinionHiz();
        float saldiriHizi = researchActions.GetMeleeMinionSaldiriHizi();
        int savasci_Sayisi = getPlayerData.currentSoldierAmount; // Bu değeri dinamik olarak güncellemek isterseniz, kendi sisteminize bağlı olarak burayı değiştirin

        // Ekranda göster
        savasciOzellikleri.text = 
                             "Can Miktarı : " + can.ToString() + "\n" +
                             "Hasar Miktarı : " + hasar.ToString() + "\n" +
                             "Hız : " + hiz.ToString() + "\n" +
                             "Saldırı Hızı : " + saldiriHizi.ToString() + "\n" +
                             "Savaşçı Sayısı : " + savasci_Sayisi.ToString();
    }

    public void refreshOkcuOzellikleri()
    {
        // Okçu özelliklerini al
        float can = researchActions.GetRangedMinionCan();
        float hasar = researchActions.GetRangedMinionHasar();
        float hiz = researchActions.GetRangedMinionHiz();
        float saldiriHizi = researchActions.GetRangedMinionSaldiriHizi();
        int okcu_Sayisi = getPlayerData.currentArcherAmount; // Bu değeri dinamik olarak güncellemek isterseniz, kendi sisteminize bağlı olarak burayı değiştirin

        okcuOzellikleri.text = 
                          "Can Miktarı : " + can.ToString() + "\n" +
                          "Hasar Miktarı : " + hasar.ToString() + "\n" +
                          "Hız : " + hiz.ToString() + "\n" +
                          "Saldırı Hızı : " + saldiriHizi.ToString() + "\n" +
                          "Okçu Sayısı : " + okcu_Sayisi.ToString();
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
