using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildsDataResetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StonePit.wasStonePitCreated = false;
        StonePit.buildLevel = 0;
        StonePit.canIStartProduction = false;
        StonePit.stoneProductionRate = 3;
        StonePit.goldProductionRateStonePit = 1;

        Blacksmith.wasBlacksmithCreated = false;
        Blacksmith.buildLevel = 0;
        Blacksmith.canIStartProduction = false;
        Blacksmith.ironProductionRate = 5;
        Blacksmith.goldProductionRateBlacksmith = 1;

        Castle.buildLevel = 1;
        Castle.wasCastleCreated = false;

        Farm.wasFarmCreated = false;
        Farm.buildLevel = 0;
        Farm.canIStartProduction = false;
        Farm.foodProductionRate = 15;
        Farm.goldProductionRateFarm = 1;

        Hospital.buildLevel = 0;
        Hospital.wasHospitalCreated = false;
        Hospital.capasity = 0;

        Lab.wasLabCreated = false;
        Lab.buildLevel = 0;

        Sawmill.wasSawmillCreated = false;
        Sawmill.buildLevel = 0;
        Sawmill.canIStartProduction = false;
        Sawmill.timberProductionRate = 5;
        Sawmill.goldProductionRateSawmill = 1;

        Tower.towerOneBuildLevel = 0;
        Tower.wasTowerOneCreated = false;
        Tower.towerTwoBuildLevel = 0;
        Tower.wasTowerTwoCreated = false;

        Trap.trapOneBuildLevel = 0;
        Trap.wasTrapOneCreated = false;
        Trap.trapTwoBuildLevel = 0;
        Trap.wasTrapTwoCreated = false;
        Trap.trapThreeBuildLevel = 0;
        Trap.wasTrapThreeCreated = false;

        Warehouse.buildLevel = 0;
        Warehouse.wasWarehouseCreated = false;
        Warehouse.foodCapacity = 120000;
        Warehouse.ironCapacity = 120000;
        Warehouse.timberCapacity = 120000;
        Warehouse.stoneCapacity = 120000;

        Barracks.buildLevel = 0;
        Barracks.wasBarracksCreated = false;

    }
}
