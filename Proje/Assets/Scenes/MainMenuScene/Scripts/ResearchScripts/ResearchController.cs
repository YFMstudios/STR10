using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ResearchController : MonoBehaviour
{
    
    public KaynakYoneticisi kaynakYoneticisi;
    public Image[] lockItems = new Image[18];
    public ProgressBarController progressBarController;

    public ResearchActions researchActions;

    public void Start()
    {
        ResearchButtonEvents.isAnyResearchActive=false;
        researchActions.MinionHareketHiziArttirma();

      
    }
    public void OpenResearchUnit()
{
    if (Lab.buildLevel == 1)
    {
        // İlk seviyenin kilidini aç
        if (lockItems[0] != null) Destroy(lockItems[0]);
    }
}

public void OpenTwoAndThreeLevels()
{
    if (ResearchButtonEvents.isResearched[0] && ResearchResetter.isResearched[0])
    {
        if (lockItems[1] != null) Destroy(lockItems[1]);
        if (lockItems[2] != null) Destroy(lockItems[2]);
    }
}

public void OpenFourLevel()
{
    if (ResearchButtonEvents.isResearched[1])
    {
        if (lockItems[3] != null) Destroy(lockItems[3]);
    }
}

public void OpenFiveLevel()
{
    if (ResearchButtonEvents.isResearched[2])
    {
        if (lockItems[4] != null) Destroy(lockItems[4]);
    }
}

public void controlBuildLevelTwoResearches()
{
    if (ResearchButtonEvents.isResearched[3] && Lab.buildLevel >= 2)
    {
        if (lockItems[5] != null) Destroy(lockItems[5]);
    }
    if (ResearchButtonEvents.isResearched[4] && Lab.buildLevel >= 2)
    {
        if (lockItems[7] != null) Destroy(lockItems[7]);
    }
    if (ResearchButtonEvents.isResearched[3] && ResearchButtonEvents.isResearched[4] && Lab.buildLevel >= 2)
    {
        if (lockItems[6] != null) Destroy(lockItems[6]);
    }
}

public void control9And10Levels()
{
    if (ResearchButtonEvents.isResearched[5])
    {
        Debug.Log("Level6 Araştırıldı");
    }
    if (ResearchButtonEvents.isResearched[6])
    {
        Debug.Log("Level7 Araştırıldı");
    }
    if (ResearchButtonEvents.isResearched[5] && ResearchButtonEvents.isResearched[6])
    {
        if (lockItems[8] != null) Destroy(lockItems[8]);
        Debug.Log("Seviye9 Açıldı");
    }
    if (ResearchButtonEvents.isResearched[6] && ResearchButtonEvents.isResearched[7])
    {
        if (lockItems[9] != null) Destroy(lockItems[9]);
    }
}

public void control11And12And13Levels()
{
    if (ResearchButtonEvents.isResearched[8] && Lab.buildLevel >= 2)
    {
        if (lockItems[10] != null) Destroy(lockItems[10]);
    }
    if (ResearchButtonEvents.isResearched[9] && Lab.buildLevel >= 2)
    {
        if (lockItems[12] != null) Destroy(lockItems[12]);
    }
    if (ResearchButtonEvents.isResearched[8] && ResearchButtonEvents.isResearched[9] && Lab.buildLevel >= 2)
    {
        if (lockItems[11] != null) Destroy(lockItems[11]);
    }
}

public void controlBuildLevelThreeResearches()
{
    if (ResearchButtonEvents.isResearched[10] && ResearchButtonEvents.isResearched[11] && Lab.buildLevel >= 3)
    {
        if (lockItems[13] != null) Destroy(lockItems[13]);
    }
    if (ResearchButtonEvents.isResearched[11] && ResearchButtonEvents.isResearched[12] && Lab.buildLevel >= 3)
    {
        if (lockItems[14] != null) Destroy(lockItems[14]);
    }
}

public void control16And17Levels()
{
    if (ResearchButtonEvents.isResearched[13] && Lab.buildLevel >= 3)
    {
        if (lockItems[15] != null) Destroy(lockItems[15]);
    }
    if (ResearchButtonEvents.isResearched[14] && Lab.buildLevel >= 3)
    {
        if (lockItems[16] != null) Destroy(lockItems[16]);
    }
}

public void level18Control()
{
    if (ResearchButtonEvents.isResearched[15] && ResearchButtonEvents.isResearched[16] && Lab.buildLevel >= 3)
    {
        if (lockItems[17] != null) Destroy(lockItems[17]);
    }
}


    public void UpgradeResearchedItems(int researchedLevel)
    {
        if (researchedLevel == 0)
        {
            kaynakYoneticisi.WarPowerArttirma(50);
            Debug.Log("Eski �retim Oran� : " + Farm.foodProductionRate);
            Debug.Log("Eski Alt�n �retme Oran� : " + Farm.goldProductionRateFarm);
            Farm.foodProductionRate += (Farm.foodProductionRate * 25) / 100;
            Farm.goldProductionRateFarm += (Farm.goldProductionRateFarm * 100) / 100;
            Debug.Log("Yeni �retim Oran� : " + Farm.foodProductionRate);
            Debug.Log("Yeni Alt�n �retme Oran� : " + Farm.goldProductionRateFarm);
        }
        else if (researchedLevel == 1)
        {
             kaynakYoneticisi.WarPowerArttirma(100);
            Debug.Log("Eski �retim Oran� : " + Sawmill.timberProductionRate);
            Debug.Log("Eski Alt�n �retme Oran� : " + Sawmill.goldProductionRateSawmill);
            Sawmill.timberProductionRate += (Sawmill.timberProductionRate * 25) / 100;
            Sawmill.goldProductionRateSawmill += (Sawmill.goldProductionRateSawmill * 100) / 100;
            Debug.Log("Yeni �retim Oran� : " + Sawmill.timberProductionRate);
            Debug.Log("Yeni Alt�n �retme Oran� : " + Sawmill.goldProductionRateSawmill);
        }
        else if (researchedLevel == 2)
        {
             kaynakYoneticisi.WarPowerArttirma(150);
            Debug.Log("Eski �retim Oran� : " + StonePit.stoneProductionRate);
            Debug.Log("Eski Alt�n �retme Oran� : " + StonePit.goldProductionRateStonePit);
            StonePit.stoneProductionRate += (StonePit.stoneProductionRate * 25) / 100;
            StonePit.goldProductionRateStonePit += (StonePit.goldProductionRateStonePit * 100) / 100;
            Debug.Log("Yeni �retim Oran� : " + StonePit.stoneProductionRate);
            Debug.Log("Yeni Alt�n �retme Oran� : " + StonePit.goldProductionRateStonePit);
        }
        else if (researchedLevel == 3)
        {
             kaynakYoneticisi.WarPowerArttirma(200);
            Debug.Log("Eski �retim Oran� : " + Blacksmith.ironProductionRate);
            Debug.Log("Eski Alt�n �retme Oran� : " + Blacksmith.goldProductionRateBlacksmith);
            Blacksmith.ironProductionRate += (Blacksmith.ironProductionRate * 25) / 100;
            Blacksmith.goldProductionRateBlacksmith += (Blacksmith.goldProductionRateBlacksmith * 100) / 100;
            Debug.Log("Yeni �retim Oran� : " + Blacksmith.ironProductionRate);
            Debug.Log("Yeni Alt�n �retme Oran� : " + Blacksmith.goldProductionRateBlacksmith);
        }
        else if (researchedLevel == 4)
        {
             kaynakYoneticisi.WarPowerArttirma(300);
            //Ara�t�rmalar� h�zland�r.
        }
        else if (researchedLevel == 5)
        {
             kaynakYoneticisi.WarPowerArttirma(400);
            Debug.Log("Eski Sava��� Heal Time : " + progressBarController.savasciHealTime);
            Debug.Log("Eski Ok�u Heal Time : " + progressBarController.okcuHealTime);
            progressBarController.savasciHealTime = 1.25f;
            progressBarController.okcuHealTime = 2.15f;
            Debug.Log("Yeni Sava��� Heal Time : " + progressBarController.savasciHealTime);
            Debug.Log("Yeni Ok�u Heal Time : " + progressBarController.okcuHealTime);
        }
        else if (researchedLevel == 6)
        {
             kaynakYoneticisi.WarPowerArttirma(450);
              researchActions.KarakterCanVeHasarArttirma();
        }
        else if ((researchedLevel == 7))
        {
             kaynakYoneticisi.WarPowerArttirma(500);
            progressBarController.savasciCreationTime -= 0.25f;
            progressBarController.okcuCreationTime -= 0.25f;

        }
        else if ((researchedLevel == 8))
        {
             kaynakYoneticisi.WarPowerArttirma(550);
            Farm.foodProductionRate += (Farm.foodProductionRate * 40) / 100;
            Farm.goldProductionRateFarm += (Farm.goldProductionRateFarm * 100) / 100;
        }
        else if (researchedLevel == 9)
        {
             kaynakYoneticisi.WarPowerArttirma(600);
            researchActions.TrapHasarAttirma();
        }
        else if (researchedLevel == 10)
        {
             kaynakYoneticisi.WarPowerArttirma(650);
            StonePit.stoneProductionRate += (StonePit.stoneProductionRate * 40) / 100;
            StonePit.goldProductionRateStonePit += (StonePit.goldProductionRateStonePit * 100) / 100;
        }
        else if (researchedLevel == 11)
        {
             kaynakYoneticisi.WarPowerArttirma(700);
            Blacksmith.ironProductionRate += (Blacksmith.ironProductionRate * 40) / 100;
            Blacksmith.goldProductionRateBlacksmith += (Blacksmith.goldProductionRateBlacksmith * 100) / 100;
        }
        else if (researchedLevel == 12)
        {
             kaynakYoneticisi.WarPowerArttirma(750);
            Warehouse.foodCapacity += (Warehouse.foodCapacity * 10) / 100;
            Warehouse.ironCapacity += (Warehouse.ironCapacity * 10) / 100;
            Warehouse.timberCapacity += (Warehouse.timberCapacity * 10) / 100;
            Warehouse.stoneCapacity += (Warehouse.stoneCapacity * 10) / 100;
        }
        else if (researchedLevel == 13)
        {
             kaynakYoneticisi.WarPowerArttirma(800);
            researchActions.KarakterCanVeHasarArttirma2();
        }
        else if (researchedLevel == 14)
        {
             kaynakYoneticisi.WarPowerArttirma(850);
            researchActions.MinionHareketHiziArttirma();
        }
        else if (researchedLevel == 15)
        {
             kaynakYoneticisi.WarPowerArttirma(900);
            progressBarController.savasciCreationTime -= 0.25f;
            progressBarController.okcuCreationTime -= 0.25f;
        }
        else if (researchedLevel == 16)
        {
             kaynakYoneticisi.WarPowerArttirma(950);
            researchActions.KaleKuleCanArttirma();
        }
        else if (researchedLevel == 17)
        {
             kaynakYoneticisi.WarPowerArttirma(1000);
            researchActions.MinionHasarArttirma();
            
        }

    }

}
