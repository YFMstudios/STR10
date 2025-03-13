using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class ProductionController : MonoBehaviour
{


    void Start()
    {
        StartCoroutine(IncrementStoneAmount());
        
        StartCoroutine(IncrementIronAmount());
        StartCoroutine(IncrementFoodAmount());
        StartCoroutine(IncrementTimberAmount());
    }

    IEnumerator IncrementStoneAmount()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // 2 saniye bekle

            if (StonePit.canIStartProduction) // StonePit isClickedButton true ise
            {

                if ((Kingdom.myKingdom.StoneAmount + StonePit.stoneProductionRate) >= Warehouse.stoneCapacity)
                {
                    StonePit.canIStartProduction = false;
                }
                else
                {

                    Kingdom.myKingdom.StoneAmount += StonePit.stoneProductionRate;
                    Kingdom.myKingdom.GoldAmount += StonePit.goldProductionRateStonePit;
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

                if ((Kingdom.myKingdom.IronAmount + Blacksmith.ironProductionRate) >= Warehouse.ironCapacity)
                {
                    Blacksmith.canIStartProduction = false;
                }
                else
                {

                    Kingdom.myKingdom.IronAmount += Blacksmith.ironProductionRate; // myKingdom.StoneAmount deðerini 5 artýr
                    Kingdom.myKingdom.GoldAmount += Blacksmith.goldProductionRateBlacksmith;
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

                if ((Kingdom.myKingdom.FoodAmount + Farm.foodProductionRate) >= Warehouse.foodCapacity)
                {
                    Farm.canIStartProduction = false;
                }
                else
                {

                    Kingdom.myKingdom.FoodAmount += Farm.foodProductionRate; // myKingdom.StoneAmount deðerini 5 artýr
                    Kingdom.myKingdom.GoldAmount += Farm.goldProductionRateFarm;
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

                if ((Kingdom.myKingdom.WoodAmount + Sawmill.timberProductionRate) >= Warehouse.timberCapacity)
                {
                    Sawmill.canIStartProduction = false;
                }
                else
                {

                    Kingdom.myKingdom.WoodAmount += Sawmill.timberProductionRate; // myKingdom.StoneAmount deðerini 5 artýr
                    Kingdom.myKingdom.GoldAmount += Sawmill.goldProductionRateSawmill;
                }

            }
        }

    }

}