using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrapHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        if (gameObject.name == "Tuzak1Resmi")
        {
            if (Trap.trapOneBuildLevel == 0)
            {
                goldText.text = "2000";
                foodText.text = "1200";
                ironText.text = "750";
                stoneText.text = "1000";
                woodText.text = "1500";
                Maliyet.SetActive(true);
            }
            else if (Trap.trapOneBuildLevel == 1)
            {
                goldText.text = "3000";
                foodText.text = "2000";
                ironText.text = "1500";
                stoneText.text = "1800";
                woodText.text = "2200";
                Maliyet.SetActive(true);
            }
            else if (Trap.trapOneBuildLevel == 2)
            {
                goldText.text = "4500";
                foodText.text = "3000";
                ironText.text = "2000";
                stoneText.text = "2500";
                woodText.text = "3000";
                Maliyet.SetActive(true);
            }
            else if (Trap.trapOneBuildLevel == 3)
            {
                DestroyResourceUIElements();
            }
        }
        else if (gameObject.name == "Tuzak2Resmi")
        {
            if (Trap.trapTwoBuildLevel == 0)
            {
                goldText.text = "2000";
                foodText.text = "1200";
                ironText.text = "750";
                stoneText.text = "1000";
                woodText.text = "1500";
                Maliyet.SetActive(true);
            }
            else if (Trap.trapTwoBuildLevel == 1)
            {
                goldText.text = "3000";
                foodText.text = "2000";
                ironText.text = "1500";
                stoneText.text = "1800";
                woodText.text = "2200";
                Maliyet.SetActive(true);
            }
            else if (Trap.trapTwoBuildLevel == 2)
            {
                goldText.text = "4500";
                foodText.text = "3000";
                ironText.text = "2000";
                stoneText.text = "2500";
                woodText.text = "3000";
                Maliyet.SetActive(true);
            }
            else if (Trap.trapTwoBuildLevel == 3)
            {
                DestroyResourceUIElements();
            }
        }
        else if (gameObject.name == "Tuzak3Resmi")
        {
            if (Trap.trapThreeBuildLevel == 0)
            {
                goldText.text = "2000";
                foodText.text = "1200";
                ironText.text = "750";
                stoneText.text = "1000";
                woodText.text = "1500";
                Maliyet.SetActive(true);
            }
            else if (Trap.trapThreeBuildLevel == 1)
            {
                goldText.text = "3000";
                foodText.text = "2000";
                ironText.text = "1500";
                stoneText.text = "1800";
                woodText.text = "2200";
                Maliyet.SetActive(true);
            }
            else if (Trap.trapThreeBuildLevel == 2)
            {
                goldText.text = "4500";
                foodText.text = "3000";
                ironText.text = "2000";
                stoneText.text = "2500";
                woodText.text = "3000";
                Maliyet.SetActive(true);
            }
            else if (Trap.trapThreeBuildLevel == 3)
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
        // Trap seviyelerinin her üçünün de 3 olduðundan emin olun
        if (Trap.trapOneBuildLevel == 3 && Trap.trapTwoBuildLevel == 3 && Trap.trapThreeBuildLevel == 3)
        {
            // Maliyet nesnesini yok et
            if (Maliyet != null)
            {
                Destroy(Maliyet);
            }
        }
        else
        {
            Debug.Log("Tuzak seviyeleri 3'e ulaþmadý. Nesneler yok edilemez.");
        }
    }
}
