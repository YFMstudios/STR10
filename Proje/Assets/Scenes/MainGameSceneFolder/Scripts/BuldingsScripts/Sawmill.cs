using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawmill : Building
{
    // Ekstra özellikler
    public static int timberProductionRate;
    public static int goldProductionRateSawmill = 1;
    public static bool canIStartProduction = false;

    public static int buildLevel = 0;
    public static bool wasSawmillCreated = false;
    // Kurucu yöntem
    public Sawmill()
    {
        // Özelliklerin baþlangýç deðerlerini atama
        buildingName = "Sawmill";
        buildingType = BuildingType.ResourceProduction;
        health = 100;
        buildGoldCost = 1200;
        buildFoodCost = 900;
        buildIronCost = 500;
        buildStoneCost = 600;
        buildTimberCost = 800;
        buildTime = 15f;
        timberProductionRate = 5;
    }

    public override void UpdateCosts()
    {
        // Bina seviyesine göre maliyet güncelleme
        if (buildLevel == 1)
        {
            buildGoldCost = 2000;
            buildFoodCost = 1600;
            buildIronCost = 1000;
            buildStoneCost = 1300;
            buildTimberCost = 1600;
            buildTime = 2f;
        }
        else if (buildLevel == 2)
        {
            buildGoldCost = 4000;
            buildFoodCost = 3200;
            buildIronCost = 2000;
            buildStoneCost = 2600;
            buildTimberCost = 3200;
            buildTime = 3f;
        }
    }

    public static void refreshTimberProductionRate()
    {
        if (buildLevel == 1)
        {
            timberProductionRate = 20;
            goldProductionRateSawmill = 1;
        }
        else if (buildLevel == 2)
        {
            timberProductionRate = 25;
            goldProductionRateSawmill = 2;
        }
        else if (buildLevel == 3)
        {
            timberProductionRate = 30;
            goldProductionRateSawmill = 3;
        }
    }

}
