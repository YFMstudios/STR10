using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Building
{
    public static int foodProductionRate;
    public static int goldProductionRateFarm = 1;
    public static bool canIStartProduction = false;
    public static int buildLevel = 0;
    public static bool wasFarmCreated = false;
    public Farm()
    {
        // Özelliklerin baþlangýç deðerlerini atama
        buildingName = "Farm";
        buildingType = BuildingType.ResourceProduction;
        health = 100;
        buildGoldCost = 1500;
        buildFoodCost = 600;
        buildIronCost = 250;
        buildStoneCost = 400;
        buildTimberCost = 800;
        buildTime = 25f;
        foodProductionRate = 15;
    }

    public override void UpdateCosts()
    {
        // Bina seviyesine göre maliyet güncelleme
        if (buildLevel == 1)
        {
            buildGoldCost = 2200;
            buildFoodCost = 1000;
            buildIronCost = 400;
            buildStoneCost = 700;
            buildTimberCost = 1600;
            buildTime = 1f;
        }
        else if (buildLevel == 2)
        {
            buildGoldCost = 3600;
            buildFoodCost = 2000;
            buildIronCost = 800;
            buildStoneCost = 1200;
            buildTimberCost = 3200;
            buildTime = 1f;
        }
    }

    public static void refreshFoodProductionRate()
    {
        if(buildLevel == 1)
        {
            foodProductionRate = 20;
            goldProductionRateFarm = 1;
        }
        else if(buildLevel == 2)
        {
            foodProductionRate = 25;
            goldProductionRateFarm = 2;
        }
        else if(buildLevel == 3) 
        {
            foodProductionRate = 30;
            goldProductionRateFarm = 3;
        }
    }
}
