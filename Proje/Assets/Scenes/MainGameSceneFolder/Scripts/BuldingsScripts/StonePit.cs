using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// StonePit sýnýfý, Building sýnýfýndan kalýtým alýr
public class StonePit : Building
{
    // Ekstra özellikler

    public static int stoneProductionRate;
    public static int goldProductionRateStonePit = 1;
    public static bool canIStartProduction = false;

    public static int buildLevel = 0;
    public static bool wasStonePitCreated = false;

    // Kurucu yöntem
    public StonePit()
    {
        // Özelliklerin baþlangýç deðerlerini atama
        buildingName = "Stonepit";
        buildingType = BuildingType.ResourceProduction;
        health = 100;
        buildGoldCost = 1800;
        buildFoodCost = 1200;
        buildIronCost = 500;
        buildStoneCost = 600;
        buildTimberCost = 900;
        buildTime = 15f;
        stoneProductionRate = 3;
        
    }



    public static void refreshStoneProductionRate()
    {
        if (buildLevel == 1)
        {
            stoneProductionRate = 3;
            goldProductionRateStonePit = 1;
        }
        else if (buildLevel == 2)
        {
            stoneProductionRate = 5;
            goldProductionRateStonePit = 2;
        }
        else if (buildLevel == 3)
        {
            stoneProductionRate = 12;
            goldProductionRateStonePit = 3;
        }
    }

    public override void UpdateCosts()
    {
        // Bina seviyesine göre maliyet güncelleme
        if (buildLevel == 1)
        {
            buildGoldCost = 3200;
            buildFoodCost = 1800;
            buildIronCost = 1000;
            buildStoneCost = 1300;
            buildTimberCost = 1600;
            buildTime = 5f;
        }
        else if (buildLevel == 2)
        {
            buildGoldCost = 6500;
            buildFoodCost = 3600;
            buildIronCost = 2000;
            buildStoneCost = 2600;
            buildTimberCost = 3200;
            buildTime = 7f;
        }
    }
}