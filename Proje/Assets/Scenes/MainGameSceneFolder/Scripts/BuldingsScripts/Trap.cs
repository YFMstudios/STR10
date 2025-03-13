using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Building
{
    // Tuzak seviyeleri ve oluþturulma durumlarý
    public static int trapOneBuildLevel = 0;
    public static bool wasTrapOneCreated = false;

    public static int trapTwoBuildLevel = 0;
    public static bool wasTrapTwoCreated = false;

    public static int trapThreeBuildLevel = 0;
    public static bool wasTrapThreeCreated = false;



    public Trap()
    {
        // Özelliklerin baþlangýç deðerlerini atama
        buildingName = "Trap";
        buildingType = BuildingType.Defense;
        health = 50;
        buildGoldCost = 1000;
        buildFoodCost = 500;
        buildIronCost = 300;
        buildStoneCost = 400;
        buildTimberCost = 600;
        buildTime = 5f;
    }

    // Tuzak 1 maliyet güncelleyici
    public void UpdateTrapOneCosts(Trap trapOne)
    {
        if (trapOneBuildLevel == 1)
        {
            trapOne.buildGoldCost = 1500;
            trapOne.buildFoodCost = 1000;
            trapOne.buildIronCost = 500;
            trapOne.buildStoneCost = 700;
            trapOne.buildTimberCost = 900;
            trapOne.buildTime = 7f;
        }
        else if (trapOneBuildLevel == 2)
        {
            trapOne.buildGoldCost = 2000;
            trapOne.buildFoodCost = 1500;
            trapOne.buildIronCost = 750;
            trapOne.buildStoneCost = 1000;
            trapOne.buildTimberCost = 1200;
            trapOne.buildTime = 10f;
        }
    }

    // Tuzak 2 maliyet güncelleyici
    public void UpdateTrapTwoCosts(Trap trapTwo)
    {
        if (trapTwoBuildLevel == 1)
        {
            trapTwo.buildGoldCost = 1500;
            trapTwo.buildFoodCost = 1000;
            trapTwo.buildIronCost = 500;
            trapTwo.buildStoneCost = 700;
            trapTwo.buildTimberCost = 900;
            trapTwo.buildTime = 7f;
        }
        else if (trapTwoBuildLevel == 2)
        {
            trapTwo.buildGoldCost = 2000;
            trapTwo.buildFoodCost = 1500;
            trapTwo.buildIronCost = 750;
            trapTwo.buildStoneCost = 1000;
            trapTwo.buildTimberCost = 1200;
            trapTwo.buildTime = 10f;
        }
    }

    // Tuzak 3 maliyet güncelleyici
    public void UpdateTrapThreeCosts(Trap trapThree)
    {
        if (trapThreeBuildLevel == 1)
        {
            trapThree.buildGoldCost = 1500;
            trapThree.buildFoodCost = 1000;
            trapThree.buildIronCost = 500;
            trapThree.buildStoneCost = 700;
            trapThree.buildTimberCost = 900;
            trapThree.buildTime = 7f;
        }
        else if (trapThreeBuildLevel == 2)
        {
            trapThree.buildGoldCost = 2000;
            trapThree.buildFoodCost = 1500;
            trapThree.buildIronCost = 750;
            trapThree.buildStoneCost = 1000;
            trapThree.buildTimberCost = 1200;
            trapThree.buildTime = 10f;
        }
    }

    // Tuzak 1 geliþtirme iþlemleri
    public void upgradeTrapOneStats()
    {
        Debug.Log("Tuzak 1'in sahnedeki özelliklerini geliþtir.");
    }

    // Tuzak 2 geliþtirme iþlemleri
    public void upgradeTrapTwoStats()
    {
        Debug.Log("Tuzak 2'nin sahnedeki özelliklerini geliþtir.");
    }

    // Tuzak 3 geliþtirme iþlemleri
    public void upgradeTrapThreeStats()
    {
        Debug.Log("Tuzak 3'ün sahnedeki özelliklerini geliþtir.");
    }
}

