using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith : Building
{
    public static int ironProductionRate;
    public static int goldProductionRateBlacksmith= 1;
    public static bool canIStartProduction = false;
    public static int buildLevel = 0;
    public static bool wasBlacksmithCreated = false;

    public Blacksmith()
    {
        // Özelliklerin baþlangýç deðerlerini atama
        buildingName = "Blacksmith";
        buildingType = BuildingType.ResourceProduction;
        health = 100;
        buildGoldCost = 2000;
        buildFoodCost = 1500;
        buildIronCost = 700;
        buildStoneCost = 800;
        buildTimberCost = 1000;
        buildTime = 1f;
        ironProductionRate = 5;
    }


    public override void UpdateCosts()
    {
        // Bina seviyesine göre maliyet güncelleme
        if (buildLevel == 1)
        {
            buildGoldCost = 3500;
            buildFoodCost = 2000;
            buildIronCost = 1200;
            buildStoneCost = 1500;
            buildTimberCost = 1800;
            buildTime = 1f;
        }
        else if (buildLevel == 2)
        {
            buildGoldCost = 7000;
            buildFoodCost = 4000;
            buildIronCost = 2400;
            buildStoneCost = 3000;
            buildTimberCost = 3600;
            buildTime = 1f;
        }
    }

    public static void refreshIronProductionRate()
    {
        if (buildLevel == 1)
        {
            ironProductionRate = 5;
            goldProductionRateBlacksmith = 1;
        }
        else if (buildLevel == 2)
        {
            ironProductionRate = 7;
            goldProductionRateBlacksmith = 2;
        }
        else if (buildLevel == 3)
        {
            ironProductionRate = 10;
            goldProductionRateBlacksmith = 2;
        }
    }

}
