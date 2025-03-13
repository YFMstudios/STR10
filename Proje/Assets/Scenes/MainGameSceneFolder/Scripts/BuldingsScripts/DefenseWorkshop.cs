using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseWorkshop : Building
{

    public static int buildLevel = 0;
    public static bool wasDefenseWorkshopCreated = false;
    public DefenseWorkshop()
    {
        // Özelliklerin baþlangýç deðerlerini atama
        buildingName = "Defense Workshop";
        buildingType = BuildingType.Defense;
        health = 100;
        buildGoldCost = 1;
        buildFoodCost = 1;
        buildIronCost = 1;
        buildStoneCost = 1;
        buildTimberCost = 1;
        buildTime = 10f;
    }



    public override void UpdateCosts()
    {
        // Bina seviyesine göre maliyet güncelleme
        if (buildLevel == 1)
        {
            buildGoldCost = 1000;
        }
        else if (buildLevel == 2)
        {
            buildGoldCost = 2500;
        }
    }
}
