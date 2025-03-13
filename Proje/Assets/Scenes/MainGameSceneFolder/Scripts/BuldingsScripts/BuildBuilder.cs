using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// OPTÝMÝZASYON KISMINDA KAYNAK AZALTMA,ÝADE ETME GÝBÝ ÝÞLEMLER METHODLAÞTIRILABÝLÝR.
public class BuildBuilder : MonoBehaviour
{
   
    public Button buildStonePitButton;
    public Button buildBlacksmithButton;
    public Button buildSawmillButton;
    public Button buildBarracksButton;
    public Button buildFarmButton;
    public Button buildHospitalButton;
    public Button buildLabButton;
    public Button buildDefenseWorkshopButton;
    public Button buildWarehouseButton;
    public Button buildCastleButton;
    public Button buildTowerOneButton;
    public Button buildTowerTwoButton;
    public Button buildSiegeWorkshopButton;
    public Button buildTrapOneButton;
    public Button buildTrapTwoButton;
    public Button buildTrapThreeButton;

    private Text buttonText;

    public ProgressBarController progressBarController;
    public ResearchController researchController;
    public WareHousePanelController wareHousePanelController;
    public StonepitPanelController stonepitPanelController;
    public SawmillPanelController sawmillPanelController;
    public FarmPanelController farmPanelController;
    public BlacksmithPanelController blacksmithPanelController;
    public LabPanelController labPanelController;
    public BarracksPanelController barracksPanelController;
    public HospitalPanelController hospitalPanelController;
    public CastlePanelController castlePanelController;
    public TowerPanelController towerPanelController;
    public TrapPanelController trapPanelController;

    public static bool buildTowerOneIsActive = false;
    public static bool buildTowerTwoIsActive = false;
    public static bool isAnyTrapActive = false;

    [Header("ScriptableObject")]
    public GetPlayerData getPlayerData;

    public static bool checkResources(Building building) // Artýk Building türü kabul ediliyor
    {
        // Güncel maliyetleri kontrol edin
        building.UpdateCosts();

        if ((building.buildGoldCost > Kingdom.myKingdom.GoldAmount) ||
            (building.buildStoneCost > Kingdom.myKingdom.StoneAmount) ||
            (building.buildTimberCost > Kingdom.myKingdom.WoodAmount) ||
            (building.buildIronCost > Kingdom.myKingdom.IronAmount) ||
            (building.buildFoodCost > Kingdom.myKingdom.FoodAmount))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void BuildStonePit()
    {
        // Zaten var olan taþ ocaðý nesnesini kullanmak için kontrol edin
        StonePit stonePit = GetComponent<StonePit>();

        if (!StonePit.wasStonePitCreated)
        {
            stonePit = gameObject.AddComponent<StonePit>();
            TextMeshProUGUI buttonText = buildStonePitButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(stonePit))
            {
                // Kaynaklarý azaltýn
                Kingdom.myKingdom.GoldAmount -= stonePit.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= stonePit.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= stonePit.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= stonePit.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= stonePit.buildFoodCost;

                buildStonePitButton.enabled = false;

                StartCoroutine(progressBarController.StonePitIsFinished(stonePit, (isFinished) =>
                {
                    if (isFinished)
                    {
                        StonePit.wasStonePitCreated = true;
                        StonePit.canIStartProduction = true;
                        StonePit.buildLevel = 1;
                        StonePit.refreshStoneProductionRate();
                        stonePit.UpdateCosts(); // Maliyetleri güncelle

                        Debug.Log("Bina Seviyesi : " + StonePit.buildLevel);
                        buttonText.text = "Yükselt";
                        buildStonePitButton.enabled = true;
                        stonepitPanelController.refreshStonePit();
                    }
                    else
                    {
                        // Kaynaklarý iade et
                        Kingdom.myKingdom.GoldAmount += stonePit.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount += stonePit.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount += stonePit.buildTimberCost;
                        Kingdom.myKingdom.IronAmount += stonePit.buildIronCost;
                        Kingdom.myKingdom.FoodAmount += stonePit.buildFoodCost;
                        buildStonePitButton.enabled = true;
                    }
                }));
            }
            else
            {
                Debug.Log("Yeterli kaynak bulunmamaktadýr");
            }
        }
        else
        {

            // Zaten bir taþ ocaðý varsa, yeni bir nesne yaratmayýn
            if (StonePit.buildLevel == 1)
            {
                TextMeshProUGUI buttonText = buildStonePitButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(stonePit))
                {
                    // Kaynaklarý azaltýn
                    Kingdom.myKingdom.GoldAmount -= stonePit.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= stonePit.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= stonePit.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= stonePit.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= stonePit.buildFoodCost;

                    buildStonePitButton.enabled = false;

                    StartCoroutine(progressBarController.StonePitIsFinished(stonePit, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            // Gerekli iþlemleri yap

                            StonePit.buildLevel++;
                            StonePit.refreshStoneProductionRate(); // Üretim miktarýný güncelliyoruz.
                            stonePit.UpdateCosts(); // Maliyetleri güncelle
                            buttonText.text = "Yükselt";
                            buildStonePitButton.enabled = true;
                            stonepitPanelController.refreshStonePit();
                        }
                        else
                        {
                            // Kaynaklarý iade et
                            Kingdom.myKingdom.GoldAmount += stonePit.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += stonePit.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += stonePit.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += stonePit.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += stonePit.buildFoodCost;
                            buildStonePitButton.enabled = true;
                        }
                    }));
                }
                else
                {
                    Debug.Log("Yeterli kaynak bulunmamaktadýr");
                }
            }

            else if (StonePit.buildLevel == 2)
            {
                TextMeshProUGUI buttonText = buildStonePitButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(stonePit))
                {

                    Kingdom.myKingdom.GoldAmount -= stonePit.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= stonePit.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= stonePit.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= stonePit.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= stonePit.buildFoodCost;

                    buildStonePitButton.enabled = false;

                    StartCoroutine(progressBarController.StonePitIsFinished(stonePit, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            // Gerekli iþlemleri yap

                            StonePit.buildLevel++;
                            StonePit.refreshStoneProductionRate(); // Üretim miktarýný güncelliyoruz.                  
                            stonepitPanelController.refreshStonePit();
                            Destroy(buildStonePitButton.gameObject);
                        }
                        else
                        {
                            // Kaynaklarý iade et
                            Kingdom.myKingdom.GoldAmount += stonePit.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += stonePit.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += stonePit.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += stonePit.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += stonePit.buildFoodCost;
                            buildStonePitButton.enabled = true;
                        }
                    }));
                }

            }
            else
            {
                Debug.Log("Bir sorun var gibi duruyor 'BuildBuilder' scriptindeki buildStonePit fonksiyonunu kontrol ediniz.");
            }
        }

    }

    public void BuildBlacksmith()
    {
        // Zaten var olan demirci nesnesini kontrol et
        Blacksmith blacksmith = GetComponent<Blacksmith>();

        // Yeni bir demirci inþa ediliyorsa
        if (!Blacksmith.wasBlacksmithCreated)
        {
            blacksmith = gameObject.AddComponent<Blacksmith>();
            TextMeshProUGUI buttonText = buildBlacksmithButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(blacksmith))
            {
                // Kaynaklarý azalt
                Kingdom.myKingdom.GoldAmount -= blacksmith.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= blacksmith.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= blacksmith.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= blacksmith.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= blacksmith.buildFoodCost;

                buildBlacksmithButton.enabled = false;

                // Ýnþaat tamamlandýðýnda yapýlacak iþlemler
                StartCoroutine(progressBarController.BlacksmithIsFinished(blacksmith, (isFinished) =>
                {
                    if (isFinished)
                    {
                        Blacksmith.wasBlacksmithCreated = true;
                        Blacksmith.canIStartProduction = true;
                        Blacksmith.buildLevel = 1;
                        Blacksmith.refreshIronProductionRate();
                        blacksmith.UpdateCosts();

                        Debug.Log("Bina Seviyesi: " + Blacksmith.buildLevel);
                        buttonText.text = "Yükselt";
                        buildBlacksmithButton.enabled = true;
                        blacksmithPanelController.refreshBlacksmith();
                    }
                    else
                    {
                        // Kaynaklarý geri al
                        Kingdom.myKingdom.GoldAmount += blacksmith.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount += blacksmith.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount += blacksmith.buildTimberCost;
                        Kingdom.myKingdom.IronAmount += blacksmith.buildIronCost;
                        Kingdom.myKingdom.FoodAmount += blacksmith.buildFoodCost;
                        buildBlacksmithButton.enabled = true;
                    }
                }));
            }
            else
            {
                Debug.Log("Yeterli kaynak bulunmamaktadýr");
            }
        }
        else
        {
            // Demirci zaten var, mevcut seviyeye göre iþlem yap
            if (Blacksmith.buildLevel == 1)
            {
                TextMeshProUGUI buttonText = buildBlacksmithButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(blacksmith))
                {
                    // Kaynaklarý azalt
                    Kingdom.myKingdom.GoldAmount -= blacksmith.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= blacksmith.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= blacksmith.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= blacksmith.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= blacksmith.buildFoodCost;

                    buildBlacksmithButton.enabled = false;

                    StartCoroutine(progressBarController.BlacksmithIsFinished(blacksmith, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            Blacksmith.buildLevel++;
                            Blacksmith.refreshIronProductionRate();
                            blacksmith.UpdateCosts();
                            buttonText.text = "Yükselt";
                            buildBlacksmithButton.enabled = true;
                            blacksmithPanelController.refreshBlacksmith();
                        }
                        else
                        {
                            Kingdom.myKingdom.GoldAmount += blacksmith.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += blacksmith.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += blacksmith.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += blacksmith.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += blacksmith.buildFoodCost;
                            buildBlacksmithButton.enabled = true;
                        }
                    }));
                }
                else
                {
                    Debug.Log("Yeterli kaynak bulunmamaktadýr");
                }
            }
            else if (Blacksmith.buildLevel == 2)
            {
                TextMeshProUGUI buttonText = buildBlacksmithButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(blacksmith))
                {
                    Kingdom.myKingdom.GoldAmount -= blacksmith.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= blacksmith.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= blacksmith.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= blacksmith.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= blacksmith.buildFoodCost;

                    buildBlacksmithButton.enabled = false;

                    StartCoroutine(progressBarController.BlacksmithIsFinished(blacksmith, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            Blacksmith.buildLevel++;
                            Blacksmith.refreshIronProductionRate();
                            blacksmithPanelController.refreshBlacksmith();
                            Destroy(buildBlacksmithButton.gameObject);
                        }
                        else
                        {
                            Kingdom.myKingdom.GoldAmount += blacksmith.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += blacksmith.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += blacksmith.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += blacksmith.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += blacksmith.buildFoodCost;
                            buildBlacksmithButton.enabled = true;
                        }
                    }));
                }
            }
            else
            {
                Debug.Log("Bir sorun var gibi duruyor 'BuildBuilder' scriptindeki BuildBlacksmith fonksiyonunu kontrol ediniz.");
            }
        }
    }



    public void BuildSawmill()
    {
        // Zaten var olan kereste ocaðý nesnesini kullanmak için kontrol edin
        Sawmill sawmill = GetComponent<Sawmill>();

        if (!Sawmill.wasSawmillCreated)
        {
            sawmill = gameObject.AddComponent<Sawmill>();
            TextMeshProUGUI buttonText = buildSawmillButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(sawmill))
            {
                // Kaynaklarý azaltýn
                Kingdom.myKingdom.GoldAmount -= sawmill.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= sawmill.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= sawmill.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= sawmill.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= sawmill.buildFoodCost;

                buildSawmillButton.enabled = false;

                StartCoroutine(progressBarController.SawmillIsFinished(sawmill, (isFinished) =>
                {
                    if (isFinished)
                    {
                        Sawmill.wasSawmillCreated = true;
                        Sawmill.canIStartProduction = true;
                        Sawmill.buildLevel = 1;
                        Sawmill.refreshTimberProductionRate();
                        sawmill.UpdateCosts(); // Maliyetleri güncelle

                        Debug.Log("Bina Seviyesi : " + Sawmill.buildLevel);
                        buttonText.text = "Yükselt";
                        buildSawmillButton.enabled = true;
                        sawmillPanelController.refreshSawmill();
                    }
                    else
                    {
                        // Kaynaklarý iade et
                        Kingdom.myKingdom.GoldAmount += sawmill.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount += sawmill.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount += sawmill.buildTimberCost;
                        Kingdom.myKingdom.IronAmount += sawmill.buildIronCost;
                        Kingdom.myKingdom.FoodAmount += sawmill.buildFoodCost;
                        buildSawmillButton.enabled = true;
                    }
                }));
            }
            else
            {
                Debug.Log("Yeterli kaynak bulunmamaktadýr");
            }
        }
        else
        {
            // Zaten bir kereste ocaðý varsa, yeni bir nesne yaratmayýn
            if (Sawmill.buildLevel == 1)
            {
                TextMeshProUGUI buttonText = buildSawmillButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(sawmill))
                {
                    // Kaynaklarý azaltýn
                    Kingdom.myKingdom.GoldAmount -= sawmill.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= sawmill.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= sawmill.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= sawmill.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= sawmill.buildFoodCost;

                    buildSawmillButton.enabled = false;

                    StartCoroutine(progressBarController.SawmillIsFinished(sawmill, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            // Gerekli iþlemleri yap

                            Sawmill.buildLevel++;
                            Sawmill.refreshTimberProductionRate(); // Üretim miktarýný güncelliyoruz.
                            sawmill.UpdateCosts(); // Maliyetleri güncelle
                            buttonText.text = "Yükselt";
                            buildSawmillButton.enabled = true;
                            sawmillPanelController.refreshSawmill();
                        }
                        else
                        {
                            // Kaynaklarý iade et
                            Kingdom.myKingdom.GoldAmount += sawmill.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += sawmill.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += sawmill.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += sawmill.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += sawmill.buildFoodCost;
                            buildSawmillButton.enabled = true;
                        }
                    }));
                }
                else
                {
                    Debug.Log("Yeterli kaynak bulunmamaktadýr");
                }
            }

            else if (Sawmill.buildLevel == 2)
            {
                TextMeshProUGUI buttonText = buildSawmillButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(sawmill))
                {

                    Kingdom.myKingdom.GoldAmount -= sawmill.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= sawmill.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= sawmill.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= sawmill.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= sawmill.buildFoodCost;

                    buildSawmillButton.enabled = false;

                    StartCoroutine(progressBarController.SawmillIsFinished(sawmill, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            // Gerekli iþlemleri yap

                            Sawmill.buildLevel++;
                            Sawmill.refreshTimberProductionRate(); // Üretim miktarýný güncelliyoruz.                   
                            sawmillPanelController.refreshSawmill();
                            Destroy(buildSawmillButton.gameObject);
                        }
                        else
                        {
                            // Kaynaklarý iade et
                            Kingdom.myKingdom.GoldAmount += sawmill.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += sawmill.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += sawmill.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += sawmill.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += sawmill.buildFoodCost;
                            buildSawmillButton.enabled = true;
                        }
                    }));
                }
            }
            else
            {
                Debug.Log("Bir sorun var gibi duruyor 'BuildBuilder' scriptindeki buildSawmill fonksiyonunu kontrol ediniz.");
            }
        }
    }



    public void BuildFarm()
    {
        // Zaten var olan çiftlik nesnesini kullanmak için kontrol edin
        Farm farm = GetComponent<Farm>();

        if (!Farm.wasFarmCreated)
        {
            farm = gameObject.AddComponent<Farm>();
            TextMeshProUGUI buttonText = buildFarmButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(farm))
            {
                // Kaynaklarý azaltýn
                Kingdom.myKingdom.GoldAmount -= farm.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= farm.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= farm.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= farm.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= farm.buildFoodCost;

                buildFarmButton.enabled = false;

                StartCoroutine(progressBarController.FarmIsFinished(farm, (isFinished) =>
                {
                    if (isFinished)
                    {
                        Farm.wasFarmCreated = true;
                        Farm.canIStartProduction = true;
                        Farm.buildLevel = 1;
                        Farm.refreshFoodProductionRate();
                        farm.UpdateCosts(); // Maliyetleri güncelle

                        Debug.Log("Bina Seviyesi : " + Farm.buildLevel);
                        buttonText.text = "Yükselt";
                        buildFarmButton.enabled = true;
                        farmPanelController.refreshFarm();
                    }
                    else
                    {
                        // Kaynaklarý iade et
                        Kingdom.myKingdom.GoldAmount += farm.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount += farm.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount += farm.buildTimberCost;
                        Kingdom.myKingdom.IronAmount += farm.buildIronCost;
                        Kingdom.myKingdom.FoodAmount += farm.buildFoodCost;
                        buildFarmButton.enabled = true;
                    }
                }));
            }
            else
            {
                Debug.Log("Yeterli kaynak bulunmamaktadýr");
            }
        }
        else
        {
            // Zaten bir çiftlik varsa, yeni bir nesne yaratmayýn
            if (Farm.buildLevel == 1)
            {
                TextMeshProUGUI buttonText = buildFarmButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(farm))
                {
                    // Kaynaklarý azaltýn
                    Kingdom.myKingdom.GoldAmount -= farm.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= farm.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= farm.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= farm.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= farm.buildFoodCost;

                    buildFarmButton.enabled = false;

                    StartCoroutine(progressBarController.FarmIsFinished(farm, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            // Gerekli iþlemleri yap

                            Farm.buildLevel++;
                            Farm.refreshFoodProductionRate(); // Üretim miktarýný güncelliyoruz.
                            farm.UpdateCosts(); // Maliyetleri güncelle
                            buttonText.text = "Yükselt";
                            buildFarmButton.enabled = true;
                            farmPanelController.refreshFarm();
                        }
                        else
                        {
                            // Kaynaklarý iade et
                            Kingdom.myKingdom.GoldAmount += farm.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += farm.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += farm.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += farm.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += farm.buildFoodCost;
                            buildFarmButton.enabled = true;
                        }
                    }));
                }
                else
                {
                    Debug.Log("Yeterli kaynak bulunmamaktadýr");
                }
            }

            else if (Farm.buildLevel == 2)
            {
                TextMeshProUGUI buttonText = buildFarmButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(farm))
                {
                    Kingdom.myKingdom.GoldAmount -= farm.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= farm.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= farm.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= farm.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= farm.buildFoodCost;

                    buildFarmButton.enabled = false;

                    StartCoroutine(progressBarController.FarmIsFinished(farm, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            // Gerekli iþlemleri yap

                            Farm.buildLevel++;
                            Farm.refreshFoodProductionRate(); // Üretim miktarýný güncelliyoruz.                   
                            farmPanelController.refreshFarm();
                            Destroy(buildFarmButton.gameObject);
                        }
                        else
                        {
                            // Kaynaklarý iade et
                            Kingdom.myKingdom.GoldAmount += farm.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += farm.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += farm.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += farm.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += farm.buildFoodCost;
                            buildFarmButton.enabled = true;
                        }
                    }));
                }
            }
            else
            {
                Debug.Log("Bir sorun var gibi duruyor 'BuildBuilder' scriptindeki BuildFarm fonksiyonunu kontrol ediniz.");
            }
        }
    }



    public void BuildBarracks()
    {
        // Zaten var olan kýþla nesnesini kullanmak için kontrol edin
        Barracks barracks = GetComponent<Barracks>();

        if (!Barracks.wasBarracksCreated)
        {
            barracks = gameObject.AddComponent<Barracks>();
            TextMeshProUGUI buttonText = buildBarracksButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(barracks) /*&& Sawmill.buildLevel >= 1 && Farm.buildLevel >= 2 && Blacksmith.buildLevel >= 1*/)
            {
                //Kaynaklarý Azalt
                Kingdom.myKingdom.GoldAmount -= barracks.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= barracks.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= barracks.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= barracks.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= barracks.buildFoodCost;

                buildBarracksButton.enabled = false;


                StartCoroutine(progressBarController.BarracksIsFinished(barracks, (isFinished) =>
                {
                    if (isFinished)
                    {
                        // Gerekli iþlemleri yap


                        Barracks.wasBarracksCreated = true;
                        Barracks.buildLevel = 1;
                        barracks.UpdateCosts(); // Maliyetleri güncelle
                        Debug.Log("Bina Seviyesi : " + Barracks.buildLevel);
                        buttonText.text = "Yükselt";
                        buildBarracksButton.enabled = true;
                        barracksPanelController.refreshBarracks();
                    }
                    else
                    {
                        // Kaynaklarý iade et
                        Kingdom.myKingdom.GoldAmount += barracks.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount += barracks.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount += barracks.buildTimberCost;
                        Kingdom.myKingdom.IronAmount += barracks.buildIronCost;
                        Kingdom.myKingdom.FoodAmount += barracks.buildFoodCost;
                        buildBarracksButton.enabled = true;
                    }
                }));

            }
            else
            {
                Debug.Log("Binayý oluþturmak için gerekli gereksinimleri saðlamýyorsunuz.");
            }
        }
        else
        {
            if (Barracks.buildLevel == 1)
            {
                if (checkResources(barracks) && Sawmill.buildLevel >= 2 && Farm.buildLevel >= 3 && Blacksmith.buildLevel >= 2)
                {
                    //Eðer asker üretimi varsa buraya girme -----> Asker üretimi yaparken geliþtirilemez.
                    if (progressBarController.isUnitCreationActive)
                    {
                        Debug.Log("Asker Üretimi Sýrasýnda Bina Yükseltmesi Yapýlamaz.");
                    }
                    //yoksa gir.
                    else
                    {
                        //Kaynaklarý Azalt
                        Kingdom.myKingdom.GoldAmount -= barracks.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount -= barracks.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount -= barracks.buildTimberCost;
                        Kingdom.myKingdom.IronAmount -= barracks.buildIronCost;
                        Kingdom.myKingdom.FoodAmount -= barracks.buildFoodCost;

                        buildBarracksButton.enabled = false;


                        StartCoroutine(progressBarController.BarracksIsFinished(barracks, (isFinished) =>
                        {
                            if (isFinished)
                            {
                                // Gerekli iþlemleri yap


                                Barracks.wasBarracksCreated = true;
                                Barracks.buildLevel++;
                                barracks.UpdateCosts(); // Maliyetleri güncelle
                                Debug.Log("Bina Seviyesi : " + Barracks.buildLevel);
                                buildBarracksButton.enabled = true;
                                barracksPanelController.refreshBarracks();
                            }
                            else
                            {
                                // Kaynaklarý iade et
                                Kingdom.myKingdom.GoldAmount += barracks.buildGoldCost;
                                Kingdom.myKingdom.StoneAmount += barracks.buildStoneCost;
                                Kingdom.myKingdom.WoodAmount += barracks.buildTimberCost;
                                Kingdom.myKingdom.IronAmount += barracks.buildIronCost;
                                Kingdom.myKingdom.FoodAmount += barracks.buildFoodCost;
                                buildBarracksButton.enabled = true;
                            }
                        }));
                    }
                }
                else
                {
                    Debug.Log("Binayý oluþturmak için gerekli gereksinimleri saðlamýyorsunuz.");
                }
            }

            else if (Barracks.buildLevel == 2)
            {
                if (checkResources(barracks) && Sawmill.buildLevel >= 3 && Farm.buildLevel >= 3 && Blacksmith.buildLevel >= 3)
                {
                    //Asker üretimi varsa buraya girme.             
                    if (progressBarController.isUnitCreationActive)
                    {
                        Debug.Log("Asker Üretimi Sýrasýnda Bina Yükseltmesi Yapýlamaz.");
                    }
                    //yoksa gir.
                    else
                    {
                        //Kaynaklarý Azalt
                        Kingdom.myKingdom.GoldAmount -= barracks.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount -= barracks.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount -= barracks.buildTimberCost;
                        Kingdom.myKingdom.IronAmount -= barracks.buildIronCost;
                        Kingdom.myKingdom.FoodAmount -= barracks.buildFoodCost;

                        buildBarracksButton.enabled = false;


                        StartCoroutine(progressBarController.BarracksIsFinished(barracks, (isFinished) =>
                        {
                            if (isFinished)
                            {
                                // Gerekli iþlemleri yap


                                Barracks.buildLevel++;
                                barracks.UpdateCosts(); // Maliyetleri güncelle
                                Debug.Log("Bina Seviyesi : " + Barracks.buildLevel);
                                Destroy(buildBarracksButton.gameObject);
                                barracksPanelController.refreshBarracks();
                            }
                            else
                            {
                                // Kaynaklarý iade et
                                Kingdom.myKingdom.GoldAmount += barracks.buildGoldCost;
                                Kingdom.myKingdom.StoneAmount += barracks.buildStoneCost;
                                Kingdom.myKingdom.WoodAmount += barracks.buildTimberCost;
                                Kingdom.myKingdom.IronAmount += barracks.buildIronCost;
                                Kingdom.myKingdom.FoodAmount += barracks.buildFoodCost;
                                buildBarracksButton.enabled = true;
                            }
                        }));
                    }
                }
                else
                {
                    Debug.Log("Binayý oluþturmak için gerekli gereksinimleri saðlamýyorsunuz.");
                }
            }
            else
            {
                Debug.Log("Bir sorun var gibi duruyor 'BuildBuilder' scriptindeki buildBarracks fonksiyonunu kontrol ediniz.");
            }
        }
    }




    public void BuildHospital()
    {
        // Zaten var olan hastane nesnesini kullanmak için kontrol edin
        Hospital hospital = GetComponent<Hospital>();

        if (!Hospital.wasHospitalCreated)
        {
            hospital = gameObject.AddComponent<Hospital>();
            TextMeshProUGUI buttonText = buildHospitalButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(hospital))
            {
                buildHospitalButton.enabled = false;
                // Kaynaklarý azaltýn
                Kingdom.myKingdom.GoldAmount -= hospital.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= hospital.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= hospital.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= hospital.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= hospital.buildFoodCost;

                buildHospitalButton.enabled = false;

                StartCoroutine(progressBarController.HospitalIsFinished(hospital, (isFinished) =>
                {
                    if (isFinished)
                    {
                        // Gerekli iþlemleri yap

                        Hospital.wasHospitalCreated = true;
                        Hospital.buildLevel = 1;
                        hospital.UpdateCapasity();
                        Debug.Log("Bina Seviyesi : " + Hospital.buildLevel);
                        Debug.Log("Hastane Kapasitesi : " + Hospital.capasity);
                        hospital.UpdateCosts(); // Maliyetleri güncelle              
                        buttonText.text = "Yükselt";
                        hospitalPanelController.refreshHospital();
                        buildHospitalButton.enabled = true;
                    }
                    else
                    {
                        // Kaynaklarý iade et
                        Kingdom.myKingdom.GoldAmount += hospital.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount += hospital.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount += hospital.buildTimberCost;
                        Kingdom.myKingdom.IronAmount += hospital.buildIronCost;
                        Kingdom.myKingdom.FoodAmount += hospital.buildFoodCost;
                        buildHospitalButton.enabled = true;
                    }
                }));
            }
            else
            {
                Debug.Log("Yeterli kaynak bulunmamaktadýr");
            }
        }
        else
        {
            if (Hospital.buildLevel == 1)
            {
                if (checkResources(hospital))
                {
                    TextMeshProUGUI buttonText = buildHospitalButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (progressBarController.isHealActive)
                    {
                        Debug.Log("Ýyileþtirme esnasýnda bina yükseltmesi yapýlamaz.");
                    }
                    else
                    {
                        Kingdom.myKingdom.GoldAmount -= hospital.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount -= hospital.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount -= hospital.buildTimberCost;
                        Kingdom.myKingdom.IronAmount -= hospital.buildIronCost;
                        Kingdom.myKingdom.FoodAmount -= hospital.buildFoodCost;

                        buildHospitalButton.enabled = false;

                        StartCoroutine(progressBarController.HospitalIsFinished(hospital, (isFinished) =>
                        {
                            if (isFinished)
                            {
                                // Gerekli iþlemleri yap
                                Hospital.buildLevel++;
                                hospital.UpdateCapasity();
                                Debug.Log("Bina Seviyesi : " + Hospital.buildLevel);
                                Debug.Log("Hastane Kapasitesi : " + Hospital.capasity);
                                hospital.UpdateCosts(); // Maliyetleri güncelle
                                buttonText.text = "Yükselt";
                                hospitalPanelController.refreshHospital();
                                buildHospitalButton.enabled = true;
                            }
                            else
                            {
                                // Kaynaklarý iade et
                                Kingdom.myKingdom.GoldAmount += hospital.buildGoldCost;
                                Kingdom.myKingdom.StoneAmount += hospital.buildStoneCost;
                                Kingdom.myKingdom.WoodAmount += hospital.buildTimberCost;
                                Kingdom.myKingdom.IronAmount += hospital.buildIronCost;
                                Kingdom.myKingdom.FoodAmount += hospital.buildFoodCost;
                                buildHospitalButton.enabled = true;
                            }
                        }));
                    }
                }
                else
                {
                    Debug.Log("Yeterli kaynak bulunmamaktadýr");
                }
            }
            else if (Hospital.buildLevel == 2)
            {
                TextMeshProUGUI buttonText = buildHospitalButton.GetComponentInChildren<TextMeshProUGUI>();
                // Kaynaklarý azaltýn

                if (checkResources(hospital))
                {
                    if (progressBarController.isHealActive)
                    {
                        Debug.Log("Ýyileþtirme esnasýnda bina yükseltmesi yapýlamaz.");
                    }
                    else
                    {
                        Kingdom.myKingdom.GoldAmount -= hospital.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount -= hospital.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount -= hospital.buildTimberCost;
                        Kingdom.myKingdom.IronAmount -= hospital.buildIronCost;
                        Kingdom.myKingdom.FoodAmount -= hospital.buildFoodCost;

                        buildHospitalButton.enabled = false;

                        StartCoroutine(progressBarController.HospitalIsFinished(hospital, (isFinished) =>
                        {
                            if (isFinished)
                            {
                                // Gerekli iþlemleri yap
                                Hospital.buildLevel++;
                                hospital.UpdateCapasity();
                                Destroy(buildHospitalButton.gameObject);
                                hospitalPanelController.refreshHospital();
                            }
                            else
                            {
                                // Kaynaklarý iade et
                                Kingdom.myKingdom.GoldAmount += hospital.buildGoldCost;
                                Kingdom.myKingdom.StoneAmount += hospital.buildStoneCost;
                                Kingdom.myKingdom.WoodAmount += hospital.buildTimberCost;
                                Kingdom.myKingdom.IronAmount += hospital.buildIronCost;
                                Kingdom.myKingdom.FoodAmount += hospital.buildFoodCost;
                                buildHospitalButton.enabled = true;
                            }
                        }));
                    }
                }
                else
                {
                    Debug.Log("Yeterli kaynak bulunmamaktadýr");
                }
            }
            else
            {
                Debug.Log("Bir sorun var gibi duruyor 'BuildBuilder' scriptindeki buildHospital fonksiyonunu kontrol ediniz.");
            }
        }
    }


    public void BuildLab()
    {
        Lab lab = gameObject.GetComponent<Lab>();

        if (Lab.wasLabCreated == false) // Daha önce üretilmediyse
        {
            lab = gameObject.AddComponent<Lab>();
            TextMeshProUGUI buttonText = buildLabButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(lab) /*&& Sawmill.buildLevel >= 2*/) // Kaynaklar yeterliyse, keresteci seviye 2 ise
            {
                // Kaynaklarý azalt
                Kingdom.myKingdom.GoldAmount -= lab.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= lab.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= lab.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= lab.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= lab.buildFoodCost;

                buildLabButton.enabled = false;

                StartCoroutine(progressBarController.LabIsFinished(lab, (isFinished) =>
                {
                    if (isFinished)
                    {
                        // Gerekli iþlemleri yap
                        Lab.wasLabCreated = true;
                        Lab.buildLevel = 1;
                        // Araþtýrma hýzýný arttýr
                        researchController.OpenResearchUnit(Lab.buildLevel);
                        lab.UpdateCosts();
                        buttonText.text = "Yükselt";
                        buildLabButton.enabled = true;
                        labPanelController.refreshLab();
                    }
                    else
                    {
                        // Kaynaklarý iade et
                        Kingdom.myKingdom.GoldAmount += lab.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount += lab.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount += lab.buildTimberCost;
                        Kingdom.myKingdom.IronAmount += lab.buildIronCost;
                        Kingdom.myKingdom.FoodAmount += lab.buildFoodCost;
                        buildLabButton.enabled = true;
                    }
                }));
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr veya Keresteci 2.Seviye Deðil.");
            }
        }
        else // Daha önce üretildi ise
        {
            if (Lab.buildLevel == 1 && ResearchButtonEvents.isResearched[3] && ResearchButtonEvents.isResearched[4]) // Lab 1.seviyeyse
            {
                TextMeshProUGUI buttonText = buildLabButton.GetComponentInChildren<TextMeshProUGUI>();
                if (checkResources(lab)) // Kaynaklar yeterliyse ve 3 ve 4. araþtýrma yapýlmýþsa
                {
                    Kingdom.myKingdom.GoldAmount -= lab.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= lab.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= lab.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= lab.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= lab.buildFoodCost;

                    buildLabButton.enabled = false;

                    StartCoroutine(progressBarController.LabIsFinished(lab, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            // Gerekli iþlemleri yap
                            Lab.buildLevel++;
                            // Araþtýrma hýzýný arttýr
                            researchController.controlBuildLevelTwoResearches();
                            lab.UpdateCosts();
                            buttonText.text = "Yükselt";
                            buildLabButton.enabled = true;
                            labPanelController.refreshLab();
                        }
                        else
                        {
                            // Kaynaklarý iade et
                            Kingdom.myKingdom.GoldAmount += lab.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += lab.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += lab.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += lab.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += lab.buildFoodCost;
                            buildLabButton.enabled = true;
                        }
                    }));
                }
                else
                {
                    Debug.Log("Lütfen kaynaklarýn yeterli olduðundan veya Dört ve Beþ numaralý araþtýrmanýn tamamlandýðýndan emin olun!");
                }
            }
            else if (Lab.buildLevel == 2)
            {
                TextMeshProUGUI buttonText = buildLabButton.GetComponentInChildren<TextMeshProUGUI>();
                if (checkResources(lab) && ResearchButtonEvents.isResearched[10] && ResearchButtonEvents.isResearched[11] && ResearchButtonEvents.isResearched[12] && Sawmill.buildLevel >= 3)
                {
                    Kingdom.myKingdom.GoldAmount -= lab.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= lab.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= lab.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= lab.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= lab.buildFoodCost;

                    buildLabButton.enabled = false;

                    StartCoroutine(progressBarController.LabIsFinished(lab, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            // Gerekli iþlemleri yap
                            Lab.buildLevel++;
                            // Araþtýrma hýzýný arttýr
                            researchController.controlBuildLevelThreeResearches();
                            lab.UpdateCosts();
                            buttonText.text = "Yükselt";
                            buildLabButton.enabled = true;
                            labPanelController.refreshLab();
                            Destroy(buildLabButton.gameObject);
                        }
                        else
                        {
                            // Kaynaklarý iade et
                            Kingdom.myKingdom.GoldAmount += lab.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += lab.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += lab.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += lab.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += lab.buildFoodCost;
                            buildLabButton.enabled = true;
                        }
                    }));
                }
                else
                {
                    Debug.Log("Lütfen kaynaklarýn yeterli olduðundan, 11,12,13 numaralý araþtýrmalarý tamamladýðýnýzdan ve Kerestecinizin 3. seviye olduðundan emin olun!");
                }
            }
            else
            {
                Debug.Log("Bir sorun var gibi duruyor 'BuildBuilder' scriptindeki buildLab fonksiyonunu kontrol ediniz.");
            }
        }
    }


    public void BuildDefenseWorkshop()
    {
        // Zaten var olan savunma atölyesi nesnesini kullanmak için kontrol edin
        DefenseWorkshop defenseWorkshop = GetComponent<DefenseWorkshop>();

        if (!DefenseWorkshop.wasDefenseWorkshopCreated)
        {
            defenseWorkshop = gameObject.AddComponent<DefenseWorkshop>();
            TextMeshProUGUI buttonText = buildDefenseWorkshopButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(defenseWorkshop))
            {
                // Kaynaklarý azaltýn
                Kingdom.myKingdom.GoldAmount -= defenseWorkshop.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= defenseWorkshop.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= defenseWorkshop.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= defenseWorkshop.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= defenseWorkshop.buildFoodCost;

                DefenseWorkshop.wasDefenseWorkshopCreated = true;
                DefenseWorkshop.buildLevel = 1;
                defenseWorkshop.UpdateCosts(); // Maliyetleri güncelle

                Debug.Log("Bina Seviyesi : " + DefenseWorkshop.buildLevel);
                buttonText.text = "Yükselt";
            }
            else
            {
                Debug.Log("Yeterli kaynak bulunmamaktadýr");
            }
        }
        else
        {
            // Zaten bir savunma atölyesi varsa, yeni bir nesne yaratmayýn
            if (checkResources(defenseWorkshop))
            {
                // Kaynaklarý azaltýn
                Kingdom.myKingdom.GoldAmount -= defenseWorkshop.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= defenseWorkshop.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= defenseWorkshop.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= defenseWorkshop.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= defenseWorkshop.buildFoodCost;

                DefenseWorkshop.buildLevel++;
                Debug.Log("Bina Seviyesi : " + DefenseWorkshop.buildLevel);
                defenseWorkshop.UpdateCosts(); // Maliyetleri güncelle

                // 3. seviyeye ulaþýldýðýnda butonu yok et
                if (DefenseWorkshop.buildLevel == 3)
                {
                    Destroy(buildDefenseWorkshopButton.gameObject);
                }
            }
            else
            {
                Debug.Log("Yeterli kaynak bulunmamaktadýr");
            }
        }
    }


    public void BuildSiegeWorkshop()
    {
        // Zaten var olan kuþatma atölyesi nesnesini kullanmak için kontrol edin
        SiegeWorkshop siegeWorkshop = GetComponent<SiegeWorkshop>();

        if (!SiegeWorkshop.wasSiegeWorkshopCreated)
        {
            siegeWorkshop = gameObject.AddComponent<SiegeWorkshop>();
            TextMeshProUGUI buttonText = buildSiegeWorkshopButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(siegeWorkshop))
            {
                // Kaynaklarý azaltýn
                Kingdom.myKingdom.GoldAmount -= siegeWorkshop.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= siegeWorkshop.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= siegeWorkshop.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= siegeWorkshop.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= siegeWorkshop.buildFoodCost;

                SiegeWorkshop.wasSiegeWorkshopCreated = true;
                SiegeWorkshop.buildLevel = 1;
                siegeWorkshop.UpdateCosts(); // Maliyetleri güncelle

                Debug.Log("Bina Seviyesi : " + SiegeWorkshop.buildLevel);
                buttonText.text = "Yükselt";
            }
            else
            {
                Debug.Log("Yeterli kaynak bulunmamaktadýr");
            }
        }
        else
        {
            // Zaten bir kuþatma atölyesi varsa, yeni bir nesne yaratmayýn
            if (checkResources(siegeWorkshop))
            {
                // Kaynaklarý azaltýn
                Kingdom.myKingdom.GoldAmount -= siegeWorkshop.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= siegeWorkshop.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= siegeWorkshop.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= siegeWorkshop.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= siegeWorkshop.buildFoodCost;

                SiegeWorkshop.buildLevel++;
                Debug.Log("Bina Seviyesi : " + SiegeWorkshop.buildLevel);
                siegeWorkshop.UpdateCosts(); // Maliyetleri güncelle

                // 3. seviyeye ulaþýldýðýnda butonu yok et
                if (SiegeWorkshop.buildLevel == 3)
                {
                    Destroy(buildSiegeWorkshopButton.gameObject);
                }
            }
            else
            {
                Debug.Log("Yeterli kaynak bulunmamaktadýr");
            }
        }
    }



    public void BuildWarehouse()
    {
        // Zaten var olan kýþla nesnesini kullanmak için kontrol edin
        Warehouse warehouse = GetComponent<Warehouse>();

        if (!Warehouse.wasWarehouseCreated)
        {
            warehouse = gameObject.AddComponent<Warehouse>();
            TextMeshProUGUI buttonText = buildWarehouseButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(warehouse) /*&& Farm.buildLevel >= 1 && Sawmill.buildLevel >= 1 && StonePit.buildLevel >= 1 && Blacksmith.buildLevel >= 1*/)
            {
                // Kaynaklarý Azalt
                Kingdom.myKingdom.GoldAmount -= warehouse.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= warehouse.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= warehouse.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= warehouse.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= warehouse.buildFoodCost;

                buildWarehouseButton.enabled = false;

                StartCoroutine(progressBarController.WarehouseIsFinished(warehouse, (isFinished) =>
                {
                    if (isFinished)
                    {
                        // Gerekli iþlemleri yap
                        Warehouse.wasWarehouseCreated = true;
                        Warehouse.buildLevel = 1;
                        Warehouse.IncreaseCapacity();
                        warehouse.UpdateCosts();
                        buttonText.text = "Yükselt";
                        buildWarehouseButton.enabled = true;
                        wareHousePanelController.refreshWarehouse();
                    }
                    else
                    {
                        // Kaynaklarý iade et
                        Kingdom.myKingdom.GoldAmount += warehouse.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount += warehouse.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount += warehouse.buildTimberCost;
                        Kingdom.myKingdom.IronAmount += warehouse.buildIronCost;
                        Kingdom.myKingdom.FoodAmount += warehouse.buildFoodCost;
                        buildWarehouseButton.enabled = true;
                    }
                }));
            }
            else
            {
                Debug.Log("Yeterli kaynak bulunmamaktadýr veya Çiftlik, Demirci, TaþOcaðý, Keresteci binalarý en az birinci seviye olmalýdýr.");
            }
        }
        else
        {
            if (Warehouse.buildLevel == 1)
            {
                TextMeshProUGUI buttonText = buildWarehouseButton.GetComponentInChildren<TextMeshProUGUI>();
                if (checkResources(warehouse) && Sawmill.buildLevel >= 2 && Blacksmith.buildLevel >= 2 && Farm.buildLevel >= 2 && StonePit.buildLevel >= 2)
                {
                    Kingdom.myKingdom.GoldAmount -= warehouse.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= warehouse.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= warehouse.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= warehouse.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= warehouse.buildFoodCost;

                    buildWarehouseButton.enabled = false;

                    StartCoroutine(progressBarController.WarehouseIsFinished(warehouse, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            // Gerekli iþlemleri yap
                            Warehouse.buildLevel++;
                            Warehouse.IncreaseCapacity();
                            warehouse.UpdateCosts();
                            buttonText.text = "Yükselt";
                            buildWarehouseButton.enabled = true;
                            wareHousePanelController.refreshWarehouse();
                        }
                        else
                        {
                            // Kaynaklarý iade et
                            Kingdom.myKingdom.GoldAmount += warehouse.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += warehouse.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += warehouse.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += warehouse.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += warehouse.buildFoodCost;
                            buildWarehouseButton.enabled = true;
                        }
                    }));
                }
                else
                {
                    Debug.Log("Yeterli kaynak bulunmamaktadýr veya Çiftlik, Demirci, TaþOcaðý, Keresteci binalarý en az ikinci seviye olmalýdýr.");
                }
            }
            else if (Warehouse.buildLevel == 2 && Sawmill.buildLevel >= 2 && Blacksmith.buildLevel >= 2 && Farm.buildLevel >= 2 && StonePit.buildLevel >= 2)
            {
                TextMeshProUGUI buttonText = buildWarehouseButton.GetComponentInChildren<TextMeshProUGUI>();
                if (checkResources(warehouse))
                {
                    // ProgressBar Ekle, Zaman dolunca aþaðýdakileri yap.
                    Kingdom.myKingdom.GoldAmount -= warehouse.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= warehouse.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= warehouse.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= warehouse.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= warehouse.buildFoodCost;

                    buildWarehouseButton.enabled = false;

                    StartCoroutine(progressBarController.WarehouseIsFinished(warehouse, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            // Gerekli iþlemleri yap
                            Warehouse.buildLevel++;
                            Warehouse.IncreaseCapacity();
                            wareHousePanelController.refreshWarehouse();
                            Destroy(buildWarehouseButton.gameObject);
                        }
                        else
                        {
                            // Kaynaklarý iade et
                            Kingdom.myKingdom.GoldAmount += warehouse.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += warehouse.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += warehouse.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += warehouse.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += warehouse.buildFoodCost;
                            buildWarehouseButton.enabled = true;
                        }
                    }));
                }
            }
            else
            {
                Debug.Log("Bir sorun var gibi duruyor 'BuildBuilder' scriptindeki buildWarehouse fonksiyonunu kontrol ediniz.");
            }
        }
    }





    public void UpgradeCastle()
    {
        // Zaten var olan demirci nesnesini kullanmak için kontrol edin
        Castle castle = GetComponent<Castle>();

        if (!Castle.wasCastleCreated)
        {
            castle = gameObject.AddComponent<Castle>();

            if (checkResources(castle))
            {
                // Kaynaklarý azaltýn
                Kingdom.myKingdom.GoldAmount -= castle.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= castle.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= castle.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= castle.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= castle.buildFoodCost;

                buildCastleButton.enabled = false;

                StartCoroutine(progressBarController.CastleIsFinished(castle, (isFinished) =>
                {
                    if (isFinished)
                    {
                        Castle.wasCastleCreated = true;

                        Castle.buildLevel = 2;
                        getPlayerData.UpgradeCastleStats(Castle.buildLevel);//InGame Sahnesindeki Kalenin Özelliklerini Güncelliyoruz.(Can,SaldýrýHýzý cart curt)
                        castle.UpdateCosts(); // Maliyetleri güncelle
                        buildCastleButton.enabled = true;
                        castlePanelController.refreshCastle();
                    }
                    else
                    {
                        // Kaynaklarý iade et
                        Kingdom.myKingdom.GoldAmount += castle.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount += castle.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount += castle.buildTimberCost;
                        Kingdom.myKingdom.IronAmount += castle.buildIronCost;
                        Kingdom.myKingdom.FoodAmount += castle.buildFoodCost;
                        buildCastleButton.enabled = true;
                    }
                }));
            }
            else
            {
                Debug.Log("Yeterli kaynak bulunmamaktadýr");
            }
        }
        else
        {
            if (Castle.buildLevel == 2)
            {
                if (checkResources(castle))
                {
                    Kingdom.myKingdom.GoldAmount -= castle.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= castle.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= castle.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= castle.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= castle.buildFoodCost;

                    buildCastleButton.enabled = false;

                    StartCoroutine(progressBarController.CastleIsFinished(castle, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            // Gerekli iþlemleri yap

                            Castle.buildLevel++;
                            getPlayerData.UpgradeCastleStats(Castle.buildLevel);//InGame Sahnesindeki Kalenin Özelliklerini Güncelliyoruz.(Can,SaldýrýHýzý cart curt)
                            castlePanelController.refreshCastle();
                            Destroy(buildCastleButton.gameObject);
                        }
                        else
                        {
                            // Kaynaklarý iade et
                            Kingdom.myKingdom.GoldAmount += castle.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += castle.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += castle.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += castle.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += castle.buildFoodCost;
                            buildCastleButton.enabled = true;
                        }
                    }));
                }
            }
            else
            {
                Debug.Log("Bir sorun var gibi duruyor 'BuildBuilder' scriptindeki BuildBlacksmith fonksiyonunu kontrol ediniz.");
            }
        }
    }


    public void BuildTowerOne()
    {
        // Zaten var olan kýþla nesnesini kullanmak için kontrol edin
        if (!buildTowerTwoIsActive)
        {
            Tower towerOne = GetComponent<Tower>();

            if (!Tower.wasTowerOneCreated)
            {
                towerOne = gameObject.AddComponent<Tower>();
                TextMeshProUGUI buttonText = buildTowerOneButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(towerOne))
                {
                    //Kaynaklarý Azalt
                    Kingdom.myKingdom.GoldAmount -= towerOne.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= towerOne.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= towerOne.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= towerOne.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= towerOne.buildFoodCost;

                    buildTowerOneButton.enabled = false;

                    buildTowerOneIsActive = true;
                    StartCoroutine(progressBarController.TowerIsFinished(towerOne, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            // Gerekli iþlemleri yap
                            Tower.wasTowerOneCreated = true;
                            Tower.towerOneBuildLevel = 1;

                            //----------------InGame Scene Ýle Alaklý--------------------------//
                            getPlayerData.TowerOneIsBuilded = true;
                            getPlayerData.ActiveTowerOne();
                            getPlayerData.UpgradeTowerOneStats(Tower.towerOneBuildLevel);
                            //----------------InGame Scene Ýle Alaklý--------------------------//

                            towerOne.UpdateTowerOneCosts(towerOne);
                            buttonText.text = "Yükselt";
                            buildTowerOneButton.enabled = true;
                            towerPanelController.refreshTowerOne();
                            buildTowerOneIsActive = false;
                        }
                        else
                        {
                            // Kaynaklarý iade et
                            Kingdom.myKingdom.GoldAmount += towerOne.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += towerOne.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += towerOne.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += towerOne.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += towerOne.buildFoodCost;
                            buildTowerOneButton.enabled = true;
                            buildTowerOneIsActive = false;
                        }
                    }));
                }
                else
                {
                    Debug.Log("Yeterli kaynak bulunmamaktadýr.");
                }
            }
            else
            {
                if (Tower.towerOneBuildLevel == 1)
                {
                    TextMeshProUGUI buttonText = buildTowerOneButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (checkResources(towerOne))
                    {
                        Kingdom.myKingdom.GoldAmount -= towerOne.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount -= towerOne.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount -= towerOne.buildTimberCost;
                        Kingdom.myKingdom.IronAmount -= towerOne.buildIronCost;
                        Kingdom.myKingdom.FoodAmount -= towerOne.buildFoodCost;

                        buildTowerOneButton.enabled = false;
                        buildTowerOneIsActive = true;
                        StartCoroutine(progressBarController.TowerIsFinished(towerOne, (isFinished) =>
                        {
                            if (isFinished)
                            {
                                // Gerekli iþlemleri yap
                                Tower.towerOneBuildLevel++;//TowerOne Level = 2 Oldu
                                //----------------InGame Scene Ýle Alaklý--------------------------//                              
                                getPlayerData.UpgradeTowerOneStats(Tower.towerOneBuildLevel);
                                //----------------InGame Scene Ýle Alaklý--------------------------//

                                towerOne.UpdateTowerOneCosts(towerOne);
                                buttonText.text = "Yükselt";
                                buildTowerOneButton.enabled = true;
                                towerPanelController.refreshTowerOne();
                                buildTowerOneIsActive = false;
                            }
                            else
                            {
                                // Kaynaklarý iade et
                                Kingdom.myKingdom.GoldAmount += towerOne.buildGoldCost;
                                Kingdom.myKingdom.StoneAmount += towerOne.buildStoneCost;
                                Kingdom.myKingdom.WoodAmount += towerOne.buildTimberCost;
                                Kingdom.myKingdom.IronAmount += towerOne.buildIronCost;
                                Kingdom.myKingdom.FoodAmount += towerOne.buildFoodCost;
                                buildTowerOneButton.enabled = true;
                                buildTowerOneIsActive = false;
                            }
                        }));
                    }
                    else
                    {
                        Debug.Log("Yeterli kaynak bulunmamaktadýr.");
                    }
                }

                else if (Tower.towerOneBuildLevel == 2)
                {
                    TextMeshProUGUI buttonText = buildTowerOneButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (checkResources(towerOne))
                    {
                        //ProgressBar Ekle,Zaman dolunca aþaðýdakileri yap.
                        Kingdom.myKingdom.GoldAmount -= towerOne.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount -= towerOne.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount -= towerOne.buildTimberCost;
                        Kingdom.myKingdom.IronAmount -= towerOne.buildIronCost;
                        Kingdom.myKingdom.FoodAmount -= towerOne.buildFoodCost;

                        buildTowerOneButton.enabled = false;
                        buildTowerOneIsActive = true;
                        StartCoroutine(progressBarController.TowerIsFinished(towerOne, (isFinished) =>
                        {
                            if (isFinished)
                            {
                                // Gerekli iþlemleri yap
                                Tower.towerOneBuildLevel++;
                                //----------------InGame Scene Ýle Alaklý--------------------------//                              
                                getPlayerData.UpgradeTowerOneStats(Tower.towerOneBuildLevel);
                                //----------------InGame Scene Ýle Alaklý--------------------------//
                                towerPanelController.refreshTowerOne();
                                Destroy(buildTowerOneButton.gameObject);
                                buildTowerOneIsActive = false;
                            }
                            else
                            {
                                // Kaynaklarý iade et
                                Kingdom.myKingdom.GoldAmount += towerOne.buildGoldCost;
                                Kingdom.myKingdom.StoneAmount += towerOne.buildStoneCost;
                                Kingdom.myKingdom.WoodAmount += towerOne.buildTimberCost;
                                Kingdom.myKingdom.IronAmount += towerOne.buildIronCost;
                                Kingdom.myKingdom.FoodAmount += towerOne.buildFoodCost;
                                buildTowerOneButton.enabled = true;
                                buildTowerOneIsActive = false;
                            }
                        }));
                    }
                }
                else
                {
                    Debug.Log("Bir sorun var gibi duruyor 'BuildBuilder' scriptindeki buildTowerOne fonksiyonunu kontrol ediniz.");
                }
            }
        }
        else
        {
            Debug.Log("Halihazýrda iþlem devam ederken yeni iþlem gerçekleþtiremezsiniz.");
        }
    }



    public void BuildTowerTwo()
    {
        if (!buildTowerOneIsActive)
        {
            // Zaten var olan kýþla nesnesini kullanmak için kontrol edin
            Tower towerTwo = GetComponent<Tower>();

            if (!Tower.wasTowerTwoCreated)
            {
                towerTwo = gameObject.AddComponent<Tower>();
                TextMeshProUGUI buttonText = buildTowerTwoButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(towerTwo))
                {
                    // Kaynaklarý Azalt
                    Kingdom.myKingdom.GoldAmount -= towerTwo.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= towerTwo.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= towerTwo.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= towerTwo.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= towerTwo.buildFoodCost;

                    buildTowerTwoButton.enabled = false;
                    buildTowerTwoIsActive = true;
                    StartCoroutine(progressBarController.TowerIsFinished(towerTwo, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            // Gerekli iþlemleri yap
                            Tower.wasTowerTwoCreated = true;
                            Tower.towerTwoBuildLevel = 1;

                            //----------------InGame Scene Ýle Alaklý--------------------------//
                            getPlayerData.TowerTwoIsBuilded = true;
                            getPlayerData.ActiveTowerTwo();
                            getPlayerData.UpgradeTowerTwoStats(Tower.towerTwoBuildLevel);
                            //----------------InGame Scene Ýle Alaklý--------------------------//

                            towerTwo.UpdateTowerTwoCosts(towerTwo);
                            buttonText.text = "Yükselt";
                            buildTowerTwoButton.enabled = true;
                            towerPanelController.refreshTowerTwo();
                            buildTowerTwoIsActive = false;
                        }
                        else
                        {
                            // Kaynaklarý iade et
                            Kingdom.myKingdom.GoldAmount += towerTwo.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += towerTwo.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += towerTwo.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += towerTwo.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += towerTwo.buildFoodCost;
                            buildTowerTwoButton.enabled = true;
                            buildTowerTwoIsActive = false;
                        }
                    }));
                }
                else
                {
                    Debug.Log("Yeterli kaynak bulunmamaktadýr.");
                }
            }
            else
            {
                if (Tower.towerTwoBuildLevel == 1)
                {
                    TextMeshProUGUI buttonText = buildTowerTwoButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (checkResources(towerTwo))
                    {
                        Kingdom.myKingdom.GoldAmount -= towerTwo.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount -= towerTwo.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount -= towerTwo.buildTimberCost;
                        Kingdom.myKingdom.IronAmount -= towerTwo.buildIronCost;
                        Kingdom.myKingdom.FoodAmount -= towerTwo.buildFoodCost;

                        buildTowerTwoButton.enabled = false;
                        buildTowerTwoIsActive = true;
                        StartCoroutine(progressBarController.TowerIsFinished(towerTwo, (isFinished) =>
                        {
                            if (isFinished)
                            {
                                // Gerekli iþlemleri yap
                                Tower.towerTwoBuildLevel++;
                                //----------------InGame Scene Ýle Alaklý--------------------------//                              
                                getPlayerData.UpgradeTowerTwoStats(Tower.towerTwoBuildLevel);
                                //----------------InGame Scene Ýle Alaklý--------------------------//
                                towerTwo.UpdateTowerTwoCosts(towerTwo);
                                buttonText.text = "Yükselt";
                                buildTowerTwoButton.enabled = true;
                                towerPanelController.refreshTowerTwo();
                                buildTowerTwoIsActive = false;
                            }
                            else
                            {
                                // Kaynaklarý iade et
                                Kingdom.myKingdom.GoldAmount += towerTwo.buildGoldCost;
                                Kingdom.myKingdom.StoneAmount += towerTwo.buildStoneCost;
                                Kingdom.myKingdom.WoodAmount += towerTwo.buildTimberCost;
                                Kingdom.myKingdom.IronAmount += towerTwo.buildIronCost;
                                Kingdom.myKingdom.FoodAmount += towerTwo.buildFoodCost;
                                buildTowerTwoButton.enabled = true;
                                buildTowerTwoIsActive = false;
                            }
                        }));
                    }
                    else
                    {
                        Debug.Log("Yeterli kaynak bulunmamaktadýr.");
                    }
                }
                else if (Tower.towerTwoBuildLevel == 2)
                {
                    TextMeshProUGUI buttonText = buildTowerTwoButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (checkResources(towerTwo))
                    {
                        Kingdom.myKingdom.GoldAmount -= towerTwo.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount -= towerTwo.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount -= towerTwo.buildTimberCost;
                        Kingdom.myKingdom.IronAmount -= towerTwo.buildIronCost;
                        Kingdom.myKingdom.FoodAmount -= towerTwo.buildFoodCost;

                        buildTowerTwoButton.enabled = false;
                        buildTowerTwoIsActive = true;
                        StartCoroutine(progressBarController.TowerIsFinished(towerTwo, (isFinished) =>
                        {
                            if (isFinished)
                            {
                                // Gerekli iþlemleri yap
                                Tower.towerTwoBuildLevel++;
                                //----------------InGame Scene Ýle Alaklý--------------------------//                              
                                getPlayerData.UpgradeTowerTwoStats(Tower.towerTwoBuildLevel);
                                //----------------InGame Scene Ýle Alaklý--------------------------//
                                towerPanelController.refreshTowerTwo();
                                buildTowerTwoIsActive = false;
                                Destroy(buildTowerTwoButton.gameObject);
                            }
                            else
                            {
                                // Kaynaklarý iade et
                                Kingdom.myKingdom.GoldAmount += towerTwo.buildGoldCost;
                                Kingdom.myKingdom.StoneAmount += towerTwo.buildStoneCost;
                                Kingdom.myKingdom.WoodAmount += towerTwo.buildTimberCost;
                                Kingdom.myKingdom.IronAmount += towerTwo.buildIronCost;
                                Kingdom.myKingdom.FoodAmount += towerTwo.buildFoodCost;
                                buildTowerTwoButton.enabled = true;
                                buildTowerTwoIsActive = false;
                            }
                        }));
                    }
                }
                else
                {
                    Debug.Log("Bir sorun var gibi duruyor 'BuildBuilder' scriptindeki buildTowerTwo fonksiyonunu kontrol ediniz.");
                }
            }
        }
        else
        {
            Debug.Log("Halihazýrda iþlem devam ederken yeni iþlem gerçekleþtiremezsiniz.");
        }
    }


    public void BuildTrapOne()
    {
        if (!isAnyTrapActive)
        {
            Trap trapOne = GetComponent<Trap>();

            if (!Trap.wasTrapOneCreated)
            {
                trapOne = gameObject.AddComponent<Trap>();
                TextMeshProUGUI buttonText = buildTrapOneButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(trapOne))
                {
                    // Kaynaklarý azalt
                    Kingdom.myKingdom.GoldAmount -= trapOne.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= trapOne.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= trapOne.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= trapOne.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= trapOne.buildFoodCost;

                    buildTrapOneButton.enabled = false;
                    isAnyTrapActive = true;
                    StartCoroutine(progressBarController.TrapIsFinished(trapOne, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            Trap.wasTrapOneCreated = true;
                            Trap.trapOneBuildLevel = 1;
                            //----------------InGame Scene Ýle Alaklý--------------------------//
                            getPlayerData.TrapOneIsBuilded = true;
                            getPlayerData.ActiveTrapOne();
                            getPlayerData.UpgradeTrapOneStats(Trap.trapOneBuildLevel);
                            //----------------InGame Scene Ýle Alaklý--------------------------//
                            trapOne.UpdateTrapOneCosts(trapOne);
                            buttonText.text = "Yükselt";
                            buildTrapOneButton.enabled = true;
                            trapPanelController.refreshTrapOne();
                            isAnyTrapActive = false;
                        }
                        else
                        {
                            Kingdom.myKingdom.GoldAmount += trapOne.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += trapOne.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += trapOne.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += trapOne.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += trapOne.buildFoodCost;
                            buildTrapOneButton.enabled = true;
                            isAnyTrapActive = false;
                        }
                    }));
                }
                else
                {
                    Debug.Log("Yeterli kaynak bulunmamaktadýr.");
                }
            }
            else
            {
                if (Trap.trapOneBuildLevel == 1 || Trap.trapOneBuildLevel == 2)
                {
                    TextMeshProUGUI buttonText = buildTrapOneButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (checkResources(trapOne))
                    {
                        // Kaynaklarý azalt
                        Kingdom.myKingdom.GoldAmount -= trapOne.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount -= trapOne.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount -= trapOne.buildTimberCost;
                        Kingdom.myKingdom.IronAmount -= trapOne.buildIronCost;
                        Kingdom.myKingdom.FoodAmount -= trapOne.buildFoodCost;

                        buildTrapOneButton.enabled = false;
                        isAnyTrapActive = true;
                        StartCoroutine(progressBarController.TrapIsFinished(trapOne, (isFinished) =>
                        {
                            if (isFinished)
                            {
                                Trap.trapOneBuildLevel++;
                                //----------------InGame Scene Ýle Alaklý--------------------------//
                                getPlayerData.UpgradeTrapOneStats(Trap.trapOneBuildLevel);
                                //----------------InGame Scene Ýle Alaklý--------------------------//
                                trapOne.UpdateTrapOneCosts(trapOne);
                                buttonText.text = "Yükselt";
                                buildTrapOneButton.enabled = true;
                                trapPanelController.refreshTrapOne();
                                if (Trap.trapOneBuildLevel == 3)
                                {
                                    Destroy(buildTrapOneButton.gameObject);
                                }
                                isAnyTrapActive = false;
                            }
                            else
                            {
                                Kingdom.myKingdom.GoldAmount += trapOne.buildGoldCost;
                                Kingdom.myKingdom.StoneAmount += trapOne.buildStoneCost;
                                Kingdom.myKingdom.WoodAmount += trapOne.buildTimberCost;
                                Kingdom.myKingdom.IronAmount += trapOne.buildIronCost;
                                Kingdom.myKingdom.FoodAmount += trapOne.buildFoodCost;
                                buildTrapOneButton.enabled = true;
                                isAnyTrapActive = false;
                            }
                        }));
                    }
                    else
                    {
                        Debug.Log("Yeterli kaynak bulunmamaktadýr.");
                    }
                }
                else
                {
                    Debug.Log("Bir sorun var gibi duruyor. 'BuildTrapOne' fonksiyonunu kontrol ediniz.");
                }
            }
        }
        else
        {
            Debug.Log("Halihazýrda iþlem devam ederken yeni iþlem gerçekleþtiremezsiniz.");
        }
    }


    public void BuildTrapTwo()
    {
        if (!isAnyTrapActive)
        {
            Trap trapTwo = GetComponent<Trap>();

            if (!Trap.wasTrapTwoCreated)
            {
                trapTwo = gameObject.AddComponent<Trap>();
                TextMeshProUGUI buttonText = buildTrapTwoButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(trapTwo))
                {
                    // Kaynaklarý azalt
                    Kingdom.myKingdom.GoldAmount -= trapTwo.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= trapTwo.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= trapTwo.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= trapTwo.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= trapTwo.buildFoodCost;

                    buildTrapTwoButton.enabled = false;
                    isAnyTrapActive = true;
                    StartCoroutine(progressBarController.TrapIsFinished(trapTwo, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            Trap.wasTrapTwoCreated = true;
                            Trap.trapTwoBuildLevel = 1;
                            //----------------InGame Scene Ýle Alaklý--------------------------//
                            getPlayerData.TrapTwoIsBuilded = true;
                            getPlayerData.ActiveTrapTwo();
                            getPlayerData.UpgradeTrapTwoStats(Trap.trapTwoBuildLevel);
                            //----------------InGame Scene Ýle Alaklý--------------------------//
                            trapTwo.UpdateTrapTwoCosts(trapTwo);
                            buttonText.text = "Yükselt";
                            buildTrapTwoButton.enabled = true;
                            trapPanelController.refreshTrapTwo();
                            isAnyTrapActive = false;
                        }
                        else
                        {
                            Kingdom.myKingdom.GoldAmount += trapTwo.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += trapTwo.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += trapTwo.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += trapTwo.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += trapTwo.buildFoodCost;
                            buildTrapTwoButton.enabled = true;
                            isAnyTrapActive = false;
                        }
                    }));
                }
                else
                {
                    Debug.Log("Yeterli kaynak bulunmamaktadýr.");
                }
            }
            else
            {
                if (Trap.trapTwoBuildLevel == 1 || Trap.trapTwoBuildLevel == 2)
                {
                    TextMeshProUGUI buttonText = buildTrapTwoButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (checkResources(trapTwo))
                    {
                        // Kaynaklarý azalt
                        Kingdom.myKingdom.GoldAmount -= trapTwo.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount -= trapTwo.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount -= trapTwo.buildTimberCost;
                        Kingdom.myKingdom.IronAmount -= trapTwo.buildIronCost;
                        Kingdom.myKingdom.FoodAmount -= trapTwo.buildFoodCost;

                        buildTrapTwoButton.enabled = false;
                        isAnyTrapActive = true;
                        StartCoroutine(progressBarController.TrapIsFinished(trapTwo, (isFinished) =>
                        {
                            if (isFinished)
                            {
                                Trap.trapTwoBuildLevel++;
                                //----------------InGame Scene Ýle Alaklý--------------------------//
                                getPlayerData.UpgradeTrapTwoStats(Trap.trapTwoBuildLevel);
                                //----------------InGame Scene Ýle Alaklý--------------------------//
                                trapTwo.UpdateTrapTwoCosts(trapTwo);
                                buttonText.text = "Yükselt";
                                buildTrapTwoButton.enabled = true;
                                trapPanelController.refreshTrapTwo();
                                if (Trap.trapTwoBuildLevel == 3)
                                {
                                    Destroy(buildTrapTwoButton.gameObject);
                                }
                                isAnyTrapActive = false;
                            }
                            else
                            {
                                Kingdom.myKingdom.GoldAmount += trapTwo.buildGoldCost;
                                Kingdom.myKingdom.StoneAmount += trapTwo.buildStoneCost;
                                Kingdom.myKingdom.WoodAmount += trapTwo.buildTimberCost;
                                Kingdom.myKingdom.IronAmount += trapTwo.buildIronCost;
                                Kingdom.myKingdom.FoodAmount += trapTwo.buildFoodCost;
                                buildTrapTwoButton.enabled = true;
                                isAnyTrapActive = false;
                            }
                        }));
                    }
                    else
                    {
                        Debug.Log("Yeterli kaynak bulunmamaktadýr.");
                    }
                }
                else
                {
                    Debug.Log("Bir sorun var gibi duruyor. 'BuildTrapTwo' fonksiyonunu kontrol ediniz.");
                }
            }
        }
        else
        {
            Debug.Log("Halihazýrda iþlem devam ederken yeni iþlem gerçekleþtiremezsiniz.");
        }
    }


    public void BuildTrapThree()
    {
        if (!isAnyTrapActive)
        {
            Trap trapThree = GetComponent<Trap>();

            if (!Trap.wasTrapThreeCreated)
            {
                trapThree = gameObject.AddComponent<Trap>();
                TextMeshProUGUI buttonText = buildTrapThreeButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(trapThree))
                {
                    // Kaynaklarý azalt
                    Kingdom.myKingdom.GoldAmount -= trapThree.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= trapThree.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= trapThree.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= trapThree.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= trapThree.buildFoodCost;

                    buildTrapThreeButton.enabled = false;
                    isAnyTrapActive = true;
                    StartCoroutine(progressBarController.TrapIsFinished(trapThree, (isFinished) =>
                    {
                        if (isFinished)
                        {
                            Trap.wasTrapThreeCreated = true;
                            Trap.trapThreeBuildLevel = 1;
                            //----------------InGame Scene Ýle Alaklý--------------------------//
                            getPlayerData.TrapThreeIsBuilded = true;
                            getPlayerData.ActiveTrapThree();
                            getPlayerData.UpgradeTrapThreeStats(Trap.trapThreeBuildLevel);
                            //----------------InGame Scene Ýle Alaklý--------------------------//
                            trapThree.UpdateTrapThreeCosts(trapThree);
                            buttonText.text = "Yükselt";
                            buildTrapThreeButton.enabled = true;
                            trapPanelController.refreshTrapThree();
                            isAnyTrapActive = false;
                        }
                        else
                        {
                            Kingdom.myKingdom.GoldAmount += trapThree.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += trapThree.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += trapThree.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += trapThree.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += trapThree.buildFoodCost;
                            buildTrapThreeButton.enabled = true;
                            isAnyTrapActive = false;
                        }
                    }));
                }
                else
                {
                    Debug.Log("Yeterli kaynak bulunmamaktadýr.");
                }
            }
            else
            {
                if (Trap.trapThreeBuildLevel == 1 || Trap.trapThreeBuildLevel == 2)
                {
                    TextMeshProUGUI buttonText = buildTrapThreeButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (checkResources(trapThree))
                    {
                        // Kaynaklarý azalt
                        Kingdom.myKingdom.GoldAmount -= trapThree.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount -= trapThree.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount -= trapThree.buildTimberCost;
                        Kingdom.myKingdom.IronAmount -= trapThree.buildIronCost;
                        Kingdom.myKingdom.FoodAmount -= trapThree.buildFoodCost;

                        buildTrapThreeButton.enabled = false;
                        isAnyTrapActive = true;
                        StartCoroutine(progressBarController.TrapIsFinished(trapThree, (isFinished) =>
                        {
                            if (isFinished)
                            {
                                Trap.trapThreeBuildLevel++;
                                //----------------InGame Scene Ýle Alaklý--------------------------//
                                getPlayerData.UpgradeTrapThreeStats(Trap.trapThreeBuildLevel);
                                //----------------InGame Scene Ýle Alaklý--------------------------//
                                trapThree.UpdateTrapThreeCosts(trapThree);
                                buttonText.text = "Yükselt";
                                buildTrapThreeButton.enabled = true;
                                trapPanelController.refreshTrapThree();
                                if (Trap.trapThreeBuildLevel == 3)
                                {
                                    Destroy(buildTrapThreeButton.gameObject);
                                }
                                isAnyTrapActive = false;
                            }
                            else
                            {
                                Kingdom.myKingdom.GoldAmount += trapThree.buildGoldCost;
                                Kingdom.myKingdom.StoneAmount += trapThree.buildStoneCost;
                                Kingdom.myKingdom.WoodAmount += trapThree.buildTimberCost;
                                Kingdom.myKingdom.IronAmount += trapThree.buildIronCost;
                                Kingdom.myKingdom.FoodAmount += trapThree.buildFoodCost;
                                buildTrapThreeButton.enabled = true;
                                isAnyTrapActive = false;
                            }
                        }));
                    }
                    else
                    {
                        Debug.Log("Yeterli kaynak bulunmamaktadýr.");
                    }
                }
                else
                {
                    Debug.Log("Bir sorun var gibi duruyor. 'BuildTrapThree' fonksiyonunu kontrol ediniz.");
                }
            }
        }
        else
        {
            Debug.Log("Halihazýrda iþlem devam ederken yeni iþlem gerçekleþtiremezsiniz.");
        }
    }



}

