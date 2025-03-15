using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Building
{
    public static int buildLevel = 1;
    public static bool wasCastleCreated = false;

    public Castle()
    {
        // Özelliklerin baþlangýç deðerlerini atama
        buildingName = "Castle";
        buildingType = BuildingType.Defense;
        health = 100;
        buildGoldCost = 2000;
        buildFoodCost = 1200;
        buildIronCost = 750;
        buildStoneCost = 1000;
        buildTimberCost = 1500;
        buildTime = 1f;
    }

    public override void UpdateCosts()
    {
        // Bina seviyesine göre maliyet güncelleme
        if (buildLevel == 1)
        {
            buildGoldCost = 3000;
            buildFoodCost = 2000;
            buildIronCost = 1500;
            buildStoneCost = 1800;
            buildTimberCost = 2200;
            buildTime = 1f;
        }
        else if (buildLevel == 2)
        {
            buildGoldCost = 4500;
            buildFoodCost = 3000;
            buildIronCost = 2000;
            buildStoneCost = 2500;
            buildTimberCost = 3000;
            buildTime = 1f;
        }
    }


}
