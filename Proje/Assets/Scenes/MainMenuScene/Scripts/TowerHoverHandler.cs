using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   
    public GameObject Maliyet;
    public TMP_Text goldText;     // Altýn miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text woodText;     // Kereste miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text stoneText;    // Taþ miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text ironText;     // Demir miktarýný gösterecek TMP_Text bileþeni
    public TMP_Text foodText;     // Yemek miktarýný gösterecek TMP_Text bileþeni

    void Start()
        {
            Maliyet.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(gameObject.name == "KuleBirResmi")
            {
                if (Tower.towerOneBuildLevel == 0)
                {
                    goldText.text = "2000";
                    foodText.text = "1200";
                    ironText.text = "750";
                    stoneText.text = "1000";
                    woodText.text = "1500";
                    Maliyet.SetActive(true);

                }
                else if (Tower.towerOneBuildLevel == 1)
                {
                    goldText.text = "3000";
                    foodText.text = "2000";
                    ironText.text = "1500";
                    stoneText.text = "1800";
                    woodText.text ="2200";
                    Maliyet.SetActive(true);

                }
                else if (Tower.towerOneBuildLevel == 2)
                {

                    goldText.text = "4500";
                    foodText.text = "3000";
                    ironText.text = "2000";
                    stoneText.text = "2500";
                    woodText.text = "3000";
                    Maliyet.SetActive(true);
                }
                else if(Tower.towerOneBuildLevel == 3)
                {
                DestroyResourceUIElements();
                }
            }
            else if(gameObject.name == "KuleÝkiResmi")
            {
                if (Tower.towerTwoBuildLevel == 0)
                {
                    goldText.text = "2000";
                    foodText.text = "1200";
                    ironText.text = "750";
                    stoneText.text = "1000";
                    woodText.text = "1500";
                    Maliyet.SetActive(true);

                }
                if (Tower.towerTwoBuildLevel == 1)
                {
                    goldText.text = "3000";
                    foodText.text = "2000";
                    ironText.text = "1500";
                    stoneText.text = "1800";
                    woodText.text = "2200";
                    Maliyet.SetActive(true);

                }
                else if (Tower.towerTwoBuildLevel == 2)
                {

                    goldText.text = "4500";
                    foodText.text = "3000";
                    ironText.text = "2000";
                    stoneText.text = "2500";
                    woodText.text = "3000";
                    Maliyet.SetActive(true);
                }

                else if (Tower.towerTwoBuildLevel == 3)
                {
                DestroyResourceUIElements();
                }
        }

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(Maliyet != null)
            {
            Maliyet.SetActive(false);
        }
            
        }

    public void DestroyResourceUIElements()
    {
        // Tower seviyelerinin her ikisinin de 3 olduðundan emin olun
        if (Tower.towerOneBuildLevel == 3 && Tower.towerTwoBuildLevel == 3)
        {
            // Maliyet nesnesini yok et
            if (Maliyet != null)
            {
                Destroy(Maliyet);
            }

        }
        else
        {
            Debug.Log("Tower seviyeleri 3'e ulaþmadý. Nesneler yok edilemez.");
        }
    }
}

