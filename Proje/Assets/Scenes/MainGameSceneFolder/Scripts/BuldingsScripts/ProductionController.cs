using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class ProductionController : MonoBehaviour
{


    void Start()
    {
        Time.timeScale=1;
        Debug.Log("Coroutinler baslatıldı.");
        StartCoroutine(IncrementStoneAmount());
        
        StartCoroutine(IncrementIronAmount());
        StartCoroutine(IncrementFoodAmount());
        StartCoroutine(IncrementTimberAmount());
    }

    IEnumerator IncrementStoneAmount()
    {
        Debug.Log("Fonksiyona Girdim.");
        while (true)
        {
             Debug.Log("Whilea Girdim.");
            yield return new WaitForSeconds(2f); // 2 saniye bekle
                Debug.Log("Stone pit üretim durumu:"+StonePit.canIStartProduction);
            if (StonePit.canIStartProduction) // StonePit isClickedButton true ise
            {
                     Debug.Log("Stone pit üretim durumu:"+StonePit.canIStartProduction);
                if ((KaynakYoneticisi.StoneAmount + StonePit.stoneProductionRate) >= Warehouse.stoneCapacity)
                {
                    StonePit.canIStartProduction = false;
                     Debug.Log("Stone pit üretim durumu:"+StonePit.canIStartProduction);
                }
                else
                {

                    KaynakYoneticisi.StoneAmount += StonePit.stoneProductionRate;
                    KaynakYoneticisi.GoldAmount += StonePit.goldProductionRateStonePit;
                }
            }

        }
    }

    IEnumerator IncrementIronAmount()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // 2 saniye bekle

            if (Blacksmith.canIStartProduction) // StonePit isClickedButton true ise
            {

                if ((KaynakYoneticisi.IronAmount + Blacksmith.ironProductionRate) >= Warehouse.ironCapacity)
                {
                    Blacksmith.canIStartProduction = false;
                }
                else
                {

                    KaynakYoneticisi.IronAmount += Blacksmith.ironProductionRate; // myKingdom.StoneAmount de�erini 5 art�r
                    KaynakYoneticisi.GoldAmount += Blacksmith.goldProductionRateBlacksmith;
                }
            }

        }
    }

    IEnumerator IncrementFoodAmount()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 2 saniye bekle

            if (Farm.canIStartProduction) // StonePit isClickedButton true ise
            {

                if ((KaynakYoneticisi.FoodAmount + Farm.foodProductionRate) >= Warehouse.foodCapacity)
                {
                    Farm.canIStartProduction = false;
                }
                else
                {

                    KaynakYoneticisi.FoodAmount += Farm.foodProductionRate; // myKingdom.StoneAmount de�erini 5 art�r
                    KaynakYoneticisi.GoldAmount += Farm.goldProductionRateFarm;
                }
            }


        }
    }

    IEnumerator IncrementTimberAmount()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 2 saniye bekle

            if (Sawmill.canIStartProduction) // StonePit isClickedButton true ise
            {

                if ((KaynakYoneticisi.WoodAmount + Sawmill.timberProductionRate) >= Warehouse.timberCapacity)
                {
                    Sawmill.canIStartProduction = false;
                }
                else
                {

                    KaynakYoneticisi.WoodAmount += Sawmill.timberProductionRate; // myKingdom.StoneAmount de�erini 5 art�r
                    KaynakYoneticisi.GoldAmount += Sawmill.goldProductionRateSawmill;
                }

            }
        }

    }

}