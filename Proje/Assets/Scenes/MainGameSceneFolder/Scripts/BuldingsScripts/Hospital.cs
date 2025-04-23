using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hospital : Building
{

    public static int buildLevel = 0;
    public static bool wasHospitalCreated = false;
    public static int capasity = 0;
    public Hospital()
    {
        // Özelliklerin baþlangýç deðerlerini atama
        buildingName = "Hospital";
        buildingType = BuildingType.Medical;
        health = 100;
        buildGoldCost = 2000;    // 1. Seviye baþlangýç maliyeti
        buildFoodCost = 1000;
        buildIronCost = 1000;
        buildStoneCost = 1200;
        buildTimberCost = 1500;
        buildTime = 1f;
    }



    public override void UpdateCosts()
    {
        if (buildLevel == 2)
        {
            buildGoldCost = 3500;    // 2. Seviye maliyet
            buildFoodCost = 1800;
            buildIronCost = 1500;
            buildStoneCost = 2000;
            buildTimberCost = 2200;
            buildTime = 2f;
        }
        else if (buildLevel == 3)
        {
            buildGoldCost = 5000;    // 3. Seviye maliyet
            buildFoodCost = 2500;
            buildIronCost = 2000;
            buildStoneCost = 3000;
            buildTimberCost = 3500;
            buildTime = 3f;
        }
    }

    public void UpdateCapasity()
    {
        if (buildLevel == 1)
        {
            capasity = 1000;
        }
        else if (buildLevel == 2)
        {
            capasity = 2500;
        }
        else if(buildLevel == 3)
        {
            capasity = 3000;
        }
    }
}
