using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab : Building
{

    public static bool wasLabCreated = false;
    public static int buildLevel = 0;
    public Lab()
    {
        // Özelliklerin baþlangýç deðerlerini atama
        buildingName = "Lab";
        buildingType = BuildingType.Research;
        health = 100;
        buildGoldCost = 3000;
        buildFoodCost = 2500;
        buildIronCost = 2500;
        buildStoneCost = 3000;
        buildTimberCost = 2000;
        buildTime = 1f;
    }



    public override void UpdateCosts()
    {
        if (buildLevel == 2)
        {
            buildGoldCost = 5000;    // 2. Seviye maliyet
            buildFoodCost = 4000;
            buildIronCost = 4000;
            buildStoneCost = 4500;
            buildTimberCost = 3500;
            buildTime = 1f;
        }
        else if (buildLevel == 3)
        {
            buildGoldCost = 7500;    // 3. Seviye maliyet
            buildFoodCost = 6000;
            buildIronCost = 6500;
            buildStoneCost = 7000;
            buildTimberCost = 5000;
            buildTime = 1f;
        }
    }
}
