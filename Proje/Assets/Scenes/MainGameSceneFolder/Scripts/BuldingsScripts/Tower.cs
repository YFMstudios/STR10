using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    public static int towerOneBuildLevel = 0;
    public static bool wasTowerOneCreated = false;
    public static int towerTwoBuildLevel = 0;
    public static bool wasTowerTwoCreated = false;

    public Tower()
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
        buildTime = 10f;
        //Bunlarý deðiþtirirsen TowerHoverHandler'ý da deðiþtir. !!!!!!
    }

    public void UpdateTowerOneCosts(Tower towerOne)
    {
        // Bina seviyesine göre maliyet güncelleme
        if (towerOneBuildLevel == 1)
        {
            towerOne.buildGoldCost = 3000;
            towerOne.buildFoodCost = 2000;
            towerOne.buildIronCost = 1500;
            towerOne.buildStoneCost = 1800;
            towerOne.buildTimberCost = 2200;
            towerOne.buildTime = 15f;
        }
        else if (towerOneBuildLevel == 2)
        {
            towerOne.buildGoldCost = 4500;
            towerOne.buildFoodCost = 3000;
            towerOne.buildIronCost = 2000;
            towerOne.buildStoneCost = 2500;
            towerOne.buildTimberCost = 3000;
            towerOne.buildTime = 20f;
        }
    }

    public void UpdateTowerTwoCosts(Tower towerTwo)
    {
        // Bina seviyesine göre maliyet güncelleme
        if (towerTwoBuildLevel == 1)
        {
            towerTwo.buildGoldCost = 3000;
            towerTwo.buildFoodCost = 2000;
            towerTwo.buildIronCost = 1500;
            towerTwo.buildStoneCost = 1800;
            towerTwo.buildTimberCost = 2200;
            towerTwo.buildTime = 15f;
        }
        else if (towerTwoBuildLevel == 2)
        {
            towerTwo.buildGoldCost = 4500;
            towerTwo.buildFoodCost = 3000;
            towerTwo.buildIronCost = 2000;
            towerTwo.buildStoneCost = 2500;
            towerTwo.buildTimberCost = 3000;
            towerTwo.buildTime = 20f;
        }
    }

    public void upgradeTowerOneStats()
    {
        //Furkanýn Sahnesindeki özellikleri arttýr.
        Debug.Log("Furkanýn sahnedeki deðiþkenleri burada deðiþtir.");
    }

    public void upgradeTowerTwoStats()
    {
        //Furkanýn Sahnesindeki özellikleri arttýr.
        Debug.Log("Furkanýn sahnedeki deðiþkenleri burada deðiþtir.");
    }
}
