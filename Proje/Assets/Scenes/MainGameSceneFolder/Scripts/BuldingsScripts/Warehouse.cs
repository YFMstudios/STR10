using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Warehouse : Building
{

    public static int buildLevel = 0;
    public static bool wasWarehouseCreated = false;

    public static int foodCapacity = 101000;
    public static int ironCapacity = 101000;
    public static int timberCapacity = 101000;
    public static int stoneCapacity = 100100;



    public Warehouse()
    {
        // Özelliklerin baþlangýç deðerlerini atama
        buildingName = "Warehouse";
        buildingType = BuildingType.ResourceProduction;
        health = 100;
        buildGoldCost = 2000;
        buildFoodCost = 1500;
        buildIronCost = 1500;
        buildStoneCost = 2000;
        buildTimberCost = 2500;
        buildTime = 5f;
    }

    public static void IncreaseCapacity()
    {
        if (buildLevel == 1)
        {
            Warehouse.foodCapacity += 10;
            Warehouse.stoneCapacity += 10;
            Warehouse.timberCapacity += 10;
            Warehouse.ironCapacity += 10;
        }
        if (buildLevel == 2)
        {
            Warehouse.foodCapacity += 10;
            Warehouse.stoneCapacity += 10;
            Warehouse.timberCapacity += 10;
            Warehouse.ironCapacity += 10;
        }
        if (buildLevel == 3)
        {
            Warehouse.foodCapacity += 10;
            Warehouse.stoneCapacity += 10;
            Warehouse.timberCapacity += 10;
            Warehouse.ironCapacity += 10;
        }
    }


    public override void UpdateCosts()
    {
        // Bina seviyesine göre maliyet güncelleme
        if (buildLevel == 1)
        {
            buildGoldCost = 4000;
            buildFoodCost = 3000;
            buildIronCost = 3000;
            buildStoneCost = 3500;
            buildTimberCost = 4000;
            buildTime = 10f;
        }
        else if (buildLevel == 2)
        {
            buildGoldCost = 6000;
            buildFoodCost = 4500;
            buildIronCost = 4000;
            buildStoneCost = 5000;
            buildTimberCost = 5500;
            buildTime = 15f;
        }
    }
}
