using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// OPT�M�ZASYON KISMINDA KAYNAK AZALTMA,�ADE ETME G�B� ��LEMLER METHODLA�TIRILAB�L�R.
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

    public static bool checkResources(Building building) // Art�k Building t�r� kabul ediliyor
    {
        if(building == null){
            Debug.Log("NULLLLLLLLLLLLLLLLLLLLLL");
        }
        // G�ncel maliyetleri kontrol edin
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
        Debug.Log("BuildStonePit çağrıldı. Mevcut StonePit.wasStonePitCreated: " + StonePit.wasStonePitCreated +
                  ", buildLevel: " + StonePit.buildLevel);

        // Zaten var olan taş ocağı nesnesini kullanmak için kontrol edin
        StonePit stonePit = GetComponent<StonePit>();

        if (!StonePit.wasStonePitCreated)
        {
            Debug.Log("Yeni StonePit oluşturuluyor (İlk inşaat)");
            stonePit = gameObject.AddComponent<StonePit>();
            TextMeshProUGUI buttonText = buildStonePitButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(stonePit))
            {
                Debug.Log("Kaynak kontrolü başarılı. Şu maliyetlerle inşaat başlatılıyor: " +
                         "Altın: " + stonePit.buildGoldCost +
                         ", Taş: " + stonePit.buildStoneCost +
                         ", Odun: " + stonePit.buildTimberCost +
                         ", Demir: " + stonePit.buildIronCost +
                         ", Yiyecek: " + stonePit.buildFoodCost);

                // Kaynakları azaltın
                Kingdom.myKingdom.GoldAmount -= stonePit.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= stonePit.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= stonePit.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= stonePit.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= stonePit.buildFoodCost;

                buildStonePitButton.enabled = false;
                Debug.Log("İnşaat başlatılıyor - Buton devre dışı bırakıldı");

                StartCoroutine(progressBarController.StonePitIsFinished(stonePit, (isFinished) =>
                {
                    Debug.Log("StonePitIsFinished callback alındı, isFinished: " + isFinished);

                    if (isFinished)
                    {
                        Debug.Log("StonePit inşaatı tamamlandı. Durum güncelleniyor...");
                        StonePit.wasStonePitCreated = true;
                        StonePit.canIStartProduction = true;
                        StonePit.buildLevel = 1;
                        StonePit.refreshStoneProductionRate();
                        stonePit.UpdateCosts(); // Maliyetleri güncelle

                        Debug.Log("Bina Seviyesi : " + StonePit.buildLevel);
                        buttonText.text = "Yükselt";
                        buildStonePitButton.enabled = true;
                        stonepitPanelController.refreshStonePit();
                        Debug.Log("İnşaat başarıyla tamamlandı ve durum güncellendi");
                    }
                    else
                    {
                        Debug.Log("StonePit inşaatı tamamlanamadı. Kaynaklar iade ediliyor.");
                        // Kaynakları iade et
                        Kingdom.myKingdom.GoldAmount += stonePit.buildGoldCost;
                        Kingdom.myKingdom.StoneAmount += stonePit.buildStoneCost;
                        Kingdom.myKingdom.WoodAmount += stonePit.buildTimberCost;
                        Kingdom.myKingdom.IronAmount += stonePit.buildIronCost;
                        Kingdom.myKingdom.FoodAmount += stonePit.buildFoodCost;
                        buildStonePitButton.enabled = true;
                        Debug.Log("Kaynaklar iade edildi ve buton tekrar aktifleştirildi");
                    }
                }));
            }
            else
            {
                Debug.Log("Yeterli kaynak bulunmamaktadır - İnşaat başlatılamadı");
            }
        }
        else
        {
            Debug.Log("Mevcut StonePit yükseltiliyor, seviye: " + StonePit.buildLevel);
            // Zaten bir taş ocağı varsa, yeni bir nesne yaratmayın
            if (StonePit.buildLevel == 1)
            {
                Debug.Log("StonePit seviye 1'den seviye 2'ye yükseltiliyor");
                TextMeshProUGUI buttonText = buildStonePitButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(stonePit))
                {
                    Debug.Log("Seviye 2 için kaynak kontrolü başarılı. Şu maliyetlerle yükseltme başlatılıyor: " +
                             "Altın: " + stonePit.buildGoldCost +
                             ", Taş: " + stonePit.buildStoneCost +
                             ", Odun: " + stonePit.buildTimberCost +
                             ", Demir: " + stonePit.buildIronCost +
                             ", Yiyecek: " + stonePit.buildFoodCost);

                    // Kaynakları azaltın
                    Kingdom.myKingdom.GoldAmount -= stonePit.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= stonePit.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= stonePit.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= stonePit.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= stonePit.buildFoodCost;

                    buildStonePitButton.enabled = false;
                    Debug.Log("Seviye 2 yükseltmesi başlatılıyor - Buton devre dışı bırakıldı");

                    StartCoroutine(progressBarController.StonePitIsFinished(stonePit, (isFinished) =>
                    {
                        Debug.Log("Seviye 2 için StonePitIsFinished callback alındı, isFinished: " + isFinished);

                        if (isFinished)
                        {
                            Debug.Log("Seviye 2 yükseltmesi tamamlandı. Durum güncelleniyor...");
                            // Gerekli işlemleri yap

                            StonePit.buildLevel++;
                            StonePit.refreshStoneProductionRate(); // Üretim miktarını güncelliyoruz.
                            stonePit.UpdateCosts(); // Maliyetleri güncelle
                            buttonText.text = "Yükselt";
                            buildStonePitButton.enabled = true;
                            stonepitPanelController.refreshStonePit();
                            Debug.Log("Seviye 2 yükseltmesi başarıyla tamamlandı. Yeni seviye: " + StonePit.buildLevel);
                        }
                        else
                        {
                            Debug.Log("Seviye 2 yükseltmesi tamamlanamadı. Kaynaklar iade ediliyor.");
                            // Kaynakları iade et
                            Kingdom.myKingdom.GoldAmount += stonePit.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += stonePit.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += stonePit.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += stonePit.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += stonePit.buildFoodCost;
                            buildStonePitButton.enabled = true;
                            Debug.Log("Seviye 2 için kaynaklar iade edildi ve buton tekrar aktifleştirildi");
                        }
                    }));
                }
                else
                {
                    Debug.Log("Seviye 2 için yeterli kaynak bulunmamaktadır");
                }
            }

            else if (StonePit.buildLevel == 2)
            {
                Debug.Log("StonePit seviye 2'den seviye 3'e yükseltiliyor");
                TextMeshProUGUI buttonText = buildStonePitButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(stonePit))
                {
                    Debug.Log("Seviye 3 için kaynak kontrolü başarılı. Şu maliyetlerle yükseltme başlatılıyor: " +
                             "Altın: " + stonePit.buildGoldCost +
                             ", Taş: " + stonePit.buildStoneCost +
                             ", Odun: " + stonePit.buildTimberCost +
                             ", Demir: " + stonePit.buildIronCost +
                             ", Yiyecek: " + stonePit.buildFoodCost);

                    Kingdom.myKingdom.GoldAmount -= stonePit.buildGoldCost;
                    Kingdom.myKingdom.StoneAmount -= stonePit.buildStoneCost;
                    Kingdom.myKingdom.WoodAmount -= stonePit.buildTimberCost;
                    Kingdom.myKingdom.IronAmount -= stonePit.buildIronCost;
                    Kingdom.myKingdom.FoodAmount -= stonePit.buildFoodCost;

                    buildStonePitButton.enabled = false;
                    Debug.Log("Seviye 3 yükseltmesi başlatılıyor - Buton devre dışı bırakıldı");

                    StartCoroutine(progressBarController.StonePitIsFinished(stonePit, (isFinished) =>
                    {
                        Debug.Log("Seviye 3 için StonePitIsFinished callback alındı, isFinished: " + isFinished);

                        if (isFinished)
                        {
                            Debug.Log("Seviye 3 yükseltmesi tamamlandı. Durum güncelleniyor...");
                            // Gerekli işlemleri yap

                            StonePit.buildLevel++;
                            StonePit.refreshStoneProductionRate(); // Üretim miktarını güncelliyoruz.                  
                            stonepitPanelController.refreshStonePit();
                            Destroy(buildStonePitButton.gameObject);
                            Debug.Log("Seviye 3 yükseltmesi başarıyla tamamlandı. Yükseltme butonu kaldırıldı.");
                        }
                        else
                        {
                            Debug.Log("Seviye 3 yükseltmesi tamamlanamadı. Kaynaklar iade ediliyor.");
                            // Kaynakları iade et
                            Kingdom.myKingdom.GoldAmount += stonePit.buildGoldCost;
                            Kingdom.myKingdom.StoneAmount += stonePit.buildStoneCost;
                            Kingdom.myKingdom.WoodAmount += stonePit.buildTimberCost;
                            Kingdom.myKingdom.IronAmount += stonePit.buildIronCost;
                            Kingdom.myKingdom.FoodAmount += stonePit.buildFoodCost;
                            buildStonePitButton.enabled = true;
                            Debug.Log("Seviye 3 için kaynaklar iade edildi ve buton tekrar aktifleştirildi");
                        }
                    }));
                }
                else
                {
                    Debug.Log("Seviye 3 için yeterli kaynak bulunmamaktadır");
                }
            }
            else
            {
                Debug.Log("Bir sorun var gibi duruyor 'BuildBuilder' scriptindeki buildStonePit fonksiyonunu kontrol ediniz. Beklenmeyen buildLevel: " + StonePit.buildLevel);
            }
        }
    }

    public void BuildBlacksmith()
    {
        // Zaten var olan demirci nesnesini kontrol et
        Blacksmith blacksmith = GetComponent<Blacksmith>();

        // Eğer bileşen null ise ve daha önce demirci inşa edilmişse, yeniden oluştur
        if (blacksmith == null && Blacksmith.wasBlacksmithCreated)
        {
            blacksmith = gameObject.AddComponent<Blacksmith>();
            Debug.Log("Blacksmith bileşeni eksikti, yeniden oluşturuldu. Seviye: " + Blacksmith.buildLevel);
            blacksmith.UpdateCosts(); // Maliyetleri güncelle
            blacksmith.buildTime = 1f; // veya uygun bir değer
        }

        // Yeni bir demirci inşa ediliyorsa
        if (!Blacksmith.wasBlacksmithCreated)
        {
            blacksmith = gameObject.AddComponent<Blacksmith>();
            TextMeshProUGUI buttonText = buildBlacksmithButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(blacksmith))
            {
                // Kaynakları azalt
                Kingdom.myKingdom.GoldAmount -= blacksmith.buildGoldCost;
                Kingdom.myKingdom.StoneAmount -= blacksmith.buildStoneCost;
                Kingdom.myKingdom.WoodAmount -= blacksmith.buildTimberCost;
                Kingdom.myKingdom.IronAmount -= blacksmith.buildIronCost;
                Kingdom.myKingdom.FoodAmount -= blacksmith.buildFoodCost;

                buildBlacksmithButton.enabled = false;

                // İnşaat tamamlandığında yapılacak işlemler
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
                        // Kaynakları geri al
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
                Debug.Log("Yeterli kaynak bulunmamaktadır");
            }
        }
        else
        {
            Debug.Log("Demirci zaten var.");
            if (Blacksmith.buildLevel == 1)
            {
                
                TextMeshProUGUI buttonText = buildBlacksmithButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(blacksmith))
                {
                    // Kaynakları azalt
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
                    Debug.Log("Yeterli kaynak bulunmamaktadır");
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
        // Zaten var olan kereste oca�� nesnesini kullanmak i�in kontrol edin
        Sawmill sawmill = GetComponent<Sawmill>();

        if (!Sawmill.wasSawmillCreated)
        {
            sawmill = gameObject.AddComponent<Sawmill>();
            TextMeshProUGUI buttonText = buildSawmillButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(sawmill))
            {
                // Kaynaklar� azalt�n
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
                        sawmill.UpdateCosts(); // Maliyetleri g�ncelle

                        Debug.Log("Bina Seviyesi : " + Sawmill.buildLevel);
                        buttonText.text = "Y�kselt";
                        buildSawmillButton.enabled = true;
                        sawmillPanelController.refreshSawmill();
                    }
                    else
                    {
                        // Kaynaklar� iade et
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
                Debug.Log("Yeterli kaynak bulunmamaktad�r");
            }
        }
        else
        {
            // Zaten bir kereste oca�� varsa, yeni bir nesne yaratmay�n
            if (Sawmill.buildLevel == 1)
            {
                TextMeshProUGUI buttonText = buildSawmillButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(sawmill))
                {
                    // Kaynaklar� azalt�n
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
                            // Gerekli i�lemleri yap

                            Sawmill.buildLevel++;
                            Sawmill.refreshTimberProductionRate(); // �retim miktar�n� g�ncelliyoruz.
                            sawmill.UpdateCosts(); // Maliyetleri g�ncelle
                            buttonText.text = "Y�kselt";
                            buildSawmillButton.enabled = true;
                            sawmillPanelController.refreshSawmill();
                        }
                        else
                        {
                            // Kaynaklar� iade et
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
                    Debug.Log("Yeterli kaynak bulunmamaktad�r");
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
                            // Gerekli i�lemleri yap

                            Sawmill.buildLevel++;
                            Sawmill.refreshTimberProductionRate(); // �retim miktar�n� g�ncelliyoruz.                   
                            sawmillPanelController.refreshSawmill();
                            Destroy(buildSawmillButton.gameObject);
                        }
                        else
                        {
                            // Kaynaklar� iade et
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
        // Zaten var olan �iftlik nesnesini kullanmak i�in kontrol edin
        Farm farm = GetComponent<Farm>();

        if (!Farm.wasFarmCreated)
        {
            farm = gameObject.AddComponent<Farm>();
            TextMeshProUGUI buttonText = buildFarmButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(farm))
            {
                // Kaynaklar� azalt�n
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
                        farm.UpdateCosts(); // Maliyetleri g�ncelle

                        Debug.Log("Bina Seviyesi : " + Farm.buildLevel);
                        buttonText.text = "Y�kselt";
                        buildFarmButton.enabled = true;
                        farmPanelController.refreshFarm();
                    }
                    else
                    {
                        // Kaynaklar� iade et
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
                Debug.Log("Yeterli kaynak bulunmamaktad�r");
            }
        }
        else
        {
            // Zaten bir �iftlik varsa, yeni bir nesne yaratmay�n
            if (Farm.buildLevel == 1)
            {
                TextMeshProUGUI buttonText = buildFarmButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(farm))
                {
                    // Kaynaklar� azalt�n
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
                            // Gerekli i�lemleri yap

                            Farm.buildLevel++;
                            Farm.refreshFoodProductionRate(); // �retim miktar�n� g�ncelliyoruz.
                            farm.UpdateCosts(); // Maliyetleri g�ncelle
                            buttonText.text = "Y�kselt";
                            buildFarmButton.enabled = true;
                            farmPanelController.refreshFarm();
                        }
                        else
                        {
                            // Kaynaklar� iade et
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
                    Debug.Log("Yeterli kaynak bulunmamaktad�r");
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
                            // Gerekli i�lemleri yap

                            Farm.buildLevel++;
                            Farm.refreshFoodProductionRate(); // �retim miktar�n� g�ncelliyoruz.                   
                            farmPanelController.refreshFarm();
                            Destroy(buildFarmButton.gameObject);
                        }
                        else
                        {
                            // Kaynaklar� iade et
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
        // Zaten var olan k��la nesnesini kullanmak i�in kontrol edin
        Barracks barracks = GetComponent<Barracks>();

        if (!Barracks.wasBarracksCreated)
        {
            barracks = gameObject.AddComponent<Barracks>();
            TextMeshProUGUI buttonText = buildBarracksButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(barracks) /*&& Sawmill.buildLevel >= 1 && Farm.buildLevel >= 2 && Blacksmith.buildLevel >= 1*/)
            {
                //Kaynaklar� Azalt
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
                        // Gerekli i�lemleri yap


                        Barracks.wasBarracksCreated = true;
                        Barracks.buildLevel = 1;
                        barracks.UpdateCosts(); // Maliyetleri g�ncelle
                        Debug.Log("Bina Seviyesi : " + Barracks.buildLevel);
                        buttonText.text = "Y�kselt";
                        buildBarracksButton.enabled = true;
                        barracksPanelController.refreshBarracks();
                    }
                    else
                    {
                        // Kaynaklar� iade et
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
                Debug.Log("Binay� olu�turmak i�in gerekli gereksinimleri sa�lam�yorsunuz.");
            }
        }
        else
        {
            if (Barracks.buildLevel == 1)
            {
                if (checkResources(barracks) && Sawmill.buildLevel >= 2 && Farm.buildLevel >= 3 && Blacksmith.buildLevel >= 2)
                {
                    //E�er asker �retimi varsa buraya girme -----> Asker �retimi yaparken geli�tirilemez.
                    if (progressBarController.isUnitCreationActive)
                    {
                        Debug.Log("Asker �retimi S�ras�nda Bina Y�kseltmesi Yap�lamaz.");
                    }
                    //yoksa gir.
                    else
                    {
                        //Kaynaklar� Azalt
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
                                // Gerekli i�lemleri yap


                                Barracks.wasBarracksCreated = true;
                                Barracks.buildLevel++;
                                barracks.UpdateCosts(); // Maliyetleri g�ncelle
                                Debug.Log("Bina Seviyesi : " + Barracks.buildLevel);
                                buildBarracksButton.enabled = true;
                                barracksPanelController.refreshBarracks();
                            }
                            else
                            {
                                // Kaynaklar� iade et
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
                    Debug.Log("Binay� olu�turmak i�in gerekli gereksinimleri sa�lam�yorsunuz.");
                }
            }

            else if (Barracks.buildLevel == 2)
            {
                if (checkResources(barracks) && Sawmill.buildLevel >= 3 && Farm.buildLevel >= 3 && Blacksmith.buildLevel >= 3)
                {
                    //Asker �retimi varsa buraya girme.             
                    if (progressBarController.isUnitCreationActive)
                    {
                        Debug.Log("Asker �retimi S�ras�nda Bina Y�kseltmesi Yap�lamaz.");
                    }
                    //yoksa gir.
                    else
                    {
                        //Kaynaklar� Azalt
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
                                // Gerekli i�lemleri yap


                                Barracks.buildLevel++;
                                barracks.UpdateCosts(); // Maliyetleri g�ncelle
                                Debug.Log("Bina Seviyesi : " + Barracks.buildLevel);
                                Destroy(buildBarracksButton.gameObject);
                                barracksPanelController.refreshBarracks();
                            }
                            else
                            {
                                // Kaynaklar� iade et
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
                    Debug.Log("Binay� olu�turmak i�in gerekli gereksinimleri sa�lam�yorsunuz.");
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
        // Zaten var olan hastane nesnesini kullanmak i�in kontrol edin
        Hospital hospital = GetComponent<Hospital>();

        if (!Hospital.wasHospitalCreated)
        {
            hospital = gameObject.AddComponent<Hospital>();
            TextMeshProUGUI buttonText = buildHospitalButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(hospital))
            {
                buildHospitalButton.enabled = false;
                // Kaynaklar� azalt�n
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
                        // Gerekli i�lemleri yap

                        Hospital.wasHospitalCreated = true;
                        Hospital.buildLevel = 1;
                        hospital.UpdateCapasity();
                        Debug.Log("Bina Seviyesi : " + Hospital.buildLevel);
                        Debug.Log("Hastane Kapasitesi : " + Hospital.capasity);
                        hospital.UpdateCosts(); // Maliyetleri g�ncelle              
                        buttonText.text = "Y�kselt";
                        hospitalPanelController.refreshHospital();
                        buildHospitalButton.enabled = true;
                    }
                    else
                    {
                        // Kaynaklar� iade et
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
                Debug.Log("Yeterli kaynak bulunmamaktad�r");
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
                        Debug.Log("�yile�tirme esnas�nda bina y�kseltmesi yap�lamaz.");
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
                                // Gerekli i�lemleri yap
                                Hospital.buildLevel++;
                                hospital.UpdateCapasity();
                                Debug.Log("Bina Seviyesi : " + Hospital.buildLevel);
                                Debug.Log("Hastane Kapasitesi : " + Hospital.capasity);
                                hospital.UpdateCosts(); // Maliyetleri g�ncelle
                                buttonText.text = "Y�kselt";
                                hospitalPanelController.refreshHospital();
                                buildHospitalButton.enabled = true;
                            }
                            else
                            {
                                // Kaynaklar� iade et
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
                    Debug.Log("Yeterli kaynak bulunmamaktad�r");
                }
            }
            else if (Hospital.buildLevel == 2)
            {
                TextMeshProUGUI buttonText = buildHospitalButton.GetComponentInChildren<TextMeshProUGUI>();
                // Kaynaklar� azalt�n

                if (checkResources(hospital))
                {
                    if (progressBarController.isHealActive)
                    {
                        Debug.Log("�yile�tirme esnas�nda bina y�kseltmesi yap�lamaz.");
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
                                // Gerekli i�lemleri yap
                                Hospital.buildLevel++;
                                hospital.UpdateCapasity();
                                Destroy(buildHospitalButton.gameObject);
                                hospitalPanelController.refreshHospital();
                            }
                            else
                            {
                                // Kaynaklar� iade et
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
                    Debug.Log("Yeterli kaynak bulunmamaktad�r");
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

        if (Lab.wasLabCreated == false) // Daha �nce �retilmediyse
        {
            lab = gameObject.AddComponent<Lab>();
            TextMeshProUGUI buttonText = buildLabButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(lab) /*&& Sawmill.buildLevel >= 2*/) // Kaynaklar yeterliyse, keresteci seviye 2 ise
            {
                // Kaynaklar� azalt
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
                        // Gerekli i�lemleri yap
                        Lab.wasLabCreated = true;
                        Lab.buildLevel = 1;
                        // Ara�t�rma h�z�n� artt�r
                        researchController.OpenResearchUnit(Lab.buildLevel);
                        lab.UpdateCosts();
                        buttonText.text = "Y�kselt";
                        buildLabButton.enabled = true;
                        labPanelController.refreshLab();
                    }
                    else
                    {
                        // Kaynaklar� iade et
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
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r veya Keresteci 2.Seviye De�il.");
            }
        }
        else // Daha �nce �retildi ise
        {
            if (Lab.buildLevel == 1 && ResearchButtonEvents.isResearched[3] && ResearchButtonEvents.isResearched[4]) // Lab 1.seviyeyse
            {
                TextMeshProUGUI buttonText = buildLabButton.GetComponentInChildren<TextMeshProUGUI>();
                if (checkResources(lab)) // Kaynaklar yeterliyse ve 3 ve 4. ara�t�rma yap�lm��sa
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
                            // Gerekli i�lemleri yap
                            Lab.buildLevel++;
                            // Ara�t�rma h�z�n� artt�r
                            researchController.controlBuildLevelTwoResearches();
                            lab.UpdateCosts();
                            buttonText.text = "Y�kselt";
                            buildLabButton.enabled = true;
                            labPanelController.refreshLab();
                        }
                        else
                        {
                            // Kaynaklar� iade et
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
                    Debug.Log("L�tfen kaynaklar�n yeterli oldu�undan veya D�rt ve Be� numaral� ara�t�rman�n tamamland���ndan emin olun!");
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
                            // Gerekli i�lemleri yap
                            Lab.buildLevel++;
                            // Ara�t�rma h�z�n� artt�r
                            researchController.controlBuildLevelThreeResearches();
                            lab.UpdateCosts();
                            buttonText.text = "Y�kselt";
                            buildLabButton.enabled = true;
                            labPanelController.refreshLab();
                            Destroy(buildLabButton.gameObject);
                        }
                        else
                        {
                            // Kaynaklar� iade et
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
                    Debug.Log("L�tfen kaynaklar�n yeterli oldu�undan, 11,12,13 numaral� ara�t�rmalar� tamamlad���n�zdan ve Kerestecinizin 3. seviye oldu�undan emin olun!");
                }
            }
            else
            {
                Debug.Log("Bir sorun var gibi duruyor 'BuildBuilder' scriptindeki buildLab fonksiyonunu kontrol ediniz.");
            }
        }
    }


    


    



    public void BuildWarehouse()
    {
        // Zaten var olan k��la nesnesini kullanmak i�in kontrol edin
        Warehouse warehouse = GetComponent<Warehouse>();

        if (!Warehouse.wasWarehouseCreated)
        {
            warehouse = gameObject.AddComponent<Warehouse>();
            TextMeshProUGUI buttonText = buildWarehouseButton.GetComponentInChildren<TextMeshProUGUI>();

            if (checkResources(warehouse) /*&& Farm.buildLevel >= 1 && Sawmill.buildLevel >= 1 && StonePit.buildLevel >= 1 && Blacksmith.buildLevel >= 1*/)
            {
                // Kaynaklar� Azalt
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
                        // Gerekli i�lemleri yap
                        Warehouse.wasWarehouseCreated = true;
                        Warehouse.buildLevel = 1;
                        Warehouse.IncreaseCapacity();
                        warehouse.UpdateCosts();
                        buttonText.text = "Y�kselt";
                        buildWarehouseButton.enabled = true;
                        wareHousePanelController.refreshWarehouse();
                    }
                    else
                    {
                        // Kaynaklar� iade et
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
                Debug.Log("Yeterli kaynak bulunmamaktad�r veya �iftlik, Demirci, Ta�Oca��, Keresteci binalar� en az birinci seviye olmal�d�r.");
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
                            // Gerekli i�lemleri yap
                            Warehouse.buildLevel++;
                            Warehouse.IncreaseCapacity();
                            warehouse.UpdateCosts();
                            buttonText.text = "Y�kselt";
                            buildWarehouseButton.enabled = true;
                            wareHousePanelController.refreshWarehouse();
                        }
                        else
                        {
                            // Kaynaklar� iade et
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
                    Debug.Log("Yeterli kaynak bulunmamaktad�r veya �iftlik, Demirci, Ta�Oca��, Keresteci binalar� en az ikinci seviye olmal�d�r.");
                }
            }
            else if (Warehouse.buildLevel == 2 && Sawmill.buildLevel >= 2 && Blacksmith.buildLevel >= 2 && Farm.buildLevel >= 2 && StonePit.buildLevel >= 2)
            {
                TextMeshProUGUI buttonText = buildWarehouseButton.GetComponentInChildren<TextMeshProUGUI>();
                if (checkResources(warehouse))
                {
                    // ProgressBar Ekle, Zaman dolunca a�a��dakileri yap.
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
                            // Gerekli i�lemleri yap
                            Warehouse.buildLevel++;
                            Warehouse.IncreaseCapacity();
                            wareHousePanelController.refreshWarehouse();
                            Destroy(buildWarehouseButton.gameObject);
                        }
                        else
                        {
                            // Kaynaklar� iade et
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
        // Zaten var olan demirci nesnesini kullanmak i�in kontrol edin
        Castle castle = GetComponent<Castle>();

        if (!Castle.wasCastleCreated)
        {
            castle = new Castle();
            castle = gameObject.AddComponent<Castle>();

            if (checkResources(castle))
            {
                // Kaynaklar� azalt�n
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
                        getPlayerData.UpgradeCastleStats(Castle.buildLevel);//InGame Sahnesindeki Kalenin �zelliklerini G�ncelliyoruz.(Can,Sald�r�H�z� cart curt)
                        castle.UpdateCosts(); // Maliyetleri g�ncelle
                        buildCastleButton.enabled = true;
                        castlePanelController.refreshCastle();
                    }
                    else
                    {
                        // Kaynaklar� iade et
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
                Debug.Log("Yeterli kaynak bulunmamaktad�r");
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
                            // Gerekli i�lemleri yap

                            Castle.buildLevel++;
                            getPlayerData.UpgradeCastleStats(Castle.buildLevel);//InGame Sahnesindeki Kalenin �zelliklerini G�ncelliyoruz.(Can,Sald�r�H�z� cart curt)
                            castlePanelController.refreshCastle();
                            Destroy(buildCastleButton.gameObject);
                        }
                        else
                        {
                            // Kaynaklar� iade et
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
        // Zaten var olan k��la nesnesini kullanmak i�in kontrol edin
        if (!buildTowerTwoIsActive)
        {
            Tower towerOne = GetComponent<Tower>();

            if (!Tower.wasTowerOneCreated)
            {
                towerOne = gameObject.AddComponent<Tower>();
                TextMeshProUGUI buttonText = buildTowerOneButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(towerOne))
                {
                    //Kaynaklar� Azalt
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
                            // Gerekli i�lemleri yap
                            Tower.wasTowerOneCreated = true;
                            Tower.towerOneBuildLevel = 1;

                            //----------------InGame Scene �le Alakl�--------------------------//
                            getPlayerData.TowerOneIsBuilded = true;
                            getPlayerData.ActiveTowerOne();
                            getPlayerData.UpgradeTowerOneStats(Tower.towerOneBuildLevel);
                            //----------------InGame Scene �le Alakl�--------------------------//

                            towerOne.UpdateTowerOneCosts(towerOne);
                            buttonText.text = "Y�kselt";
                            buildTowerOneButton.enabled = true;
                            towerPanelController.refreshTowerOne();
                            buildTowerOneIsActive = false;
                        }
                        else
                        {
                            // Kaynaklar� iade et
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
                    Debug.Log("Yeterli kaynak bulunmamaktad�r.");
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
                                // Gerekli i�lemleri yap
                                Tower.towerOneBuildLevel++;//TowerOne Level = 2 Oldu
                                //----------------InGame Scene �le Alakl�--------------------------//                              
                                getPlayerData.UpgradeTowerOneStats(Tower.towerOneBuildLevel);
                                //----------------InGame Scene �le Alakl�--------------------------//

                                towerOne.UpdateTowerOneCosts(towerOne);
                                buttonText.text = "Y�kselt";
                                buildTowerOneButton.enabled = true;
                                towerPanelController.refreshTowerOne();
                                buildTowerOneIsActive = false;
                            }
                            else
                            {
                                // Kaynaklar� iade et
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
                        Debug.Log("Yeterli kaynak bulunmamaktad�r.");
                    }
                }

                else if (Tower.towerOneBuildLevel == 2)
                {
                    TextMeshProUGUI buttonText = buildTowerOneButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (checkResources(towerOne))
                    {
                        //ProgressBar Ekle,Zaman dolunca a�a��dakileri yap.
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
                                // Gerekli i�lemleri yap
                                Tower.towerOneBuildLevel++;
                                //----------------InGame Scene �le Alakl�--------------------------//                              
                                getPlayerData.UpgradeTowerOneStats(Tower.towerOneBuildLevel);
                                //----------------InGame Scene �le Alakl�--------------------------//
                                towerPanelController.refreshTowerOne();
                                Destroy(buildTowerOneButton.gameObject);
                                buildTowerOneIsActive = false;
                            }
                            else
                            {
                                // Kaynaklar� iade et
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
            Debug.Log("Halihaz�rda i�lem devam ederken yeni i�lem ger�ekle�tiremezsiniz.");
        }
    }



    public void BuildTowerTwo()
    {
        if (!buildTowerOneIsActive)
        {
            // Zaten var olan k��la nesnesini kullanmak i�in kontrol edin
            Tower towerTwo = GetComponent<Tower>();

            if (!Tower.wasTowerTwoCreated)
            {
                towerTwo = gameObject.AddComponent<Tower>();
                TextMeshProUGUI buttonText = buildTowerTwoButton.GetComponentInChildren<TextMeshProUGUI>();

                if (checkResources(towerTwo))
                {
                    // Kaynaklar� Azalt
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
                            // Gerekli i�lemleri yap
                            Tower.wasTowerTwoCreated = true;
                            Tower.towerTwoBuildLevel = 1;

                            //----------------InGame Scene �le Alakl�--------------------------//
                            getPlayerData.TowerTwoIsBuilded = true;
                            getPlayerData.ActiveTowerTwo();
                            getPlayerData.UpgradeTowerTwoStats(Tower.towerTwoBuildLevel);
                            //----------------InGame Scene �le Alakl�--------------------------//

                            towerTwo.UpdateTowerTwoCosts(towerTwo);
                            buttonText.text = "Y�kselt";
                            buildTowerTwoButton.enabled = true;
                            towerPanelController.refreshTowerTwo();
                            buildTowerTwoIsActive = false;
                        }
                        else
                        {
                            // Kaynaklar� iade et
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
                    Debug.Log("Yeterli kaynak bulunmamaktad�r.");
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
                                // Gerekli i�lemleri yap
                                Tower.towerTwoBuildLevel++;
                                //----------------InGame Scene �le Alakl�--------------------------//                              
                                getPlayerData.UpgradeTowerTwoStats(Tower.towerTwoBuildLevel);
                                //----------------InGame Scene �le Alakl�--------------------------//
                                towerTwo.UpdateTowerTwoCosts(towerTwo);
                                buttonText.text = "Y�kselt";
                                buildTowerTwoButton.enabled = true;
                                towerPanelController.refreshTowerTwo();
                                buildTowerTwoIsActive = false;
                            }
                            else
                            {
                                // Kaynaklar� iade et
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
                        Debug.Log("Yeterli kaynak bulunmamaktad�r.");
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
                                // Gerekli i�lemleri yap
                                Tower.towerTwoBuildLevel++;
                                //----------------InGame Scene �le Alakl�--------------------------//                              
                                getPlayerData.UpgradeTowerTwoStats(Tower.towerTwoBuildLevel);
                                //----------------InGame Scene �le Alakl�--------------------------//
                                towerPanelController.refreshTowerTwo();
                                buildTowerTwoIsActive = false;
                                Destroy(buildTowerTwoButton.gameObject);
                            }
                            else
                            {
                                // Kaynaklar� iade et
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
            Debug.Log("Halihaz�rda i�lem devam ederken yeni i�lem ger�ekle�tiremezsiniz.");
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
                    // Kaynaklar� azalt
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
                            //----------------InGame Scene �le Alakl�--------------------------//
                            getPlayerData.TrapOneIsBuilded = true;
                            getPlayerData.ActiveTrapOne();
                            getPlayerData.UpgradeTrapOneStats(Trap.trapOneBuildLevel);
                            //----------------InGame Scene �le Alakl�--------------------------//
                            trapOne.UpdateTrapOneCosts(trapOne);
                            buttonText.text = "Y�kselt";
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
                    Debug.Log("Yeterli kaynak bulunmamaktad�r.");
                }
            }
            else
            {
                if (Trap.trapOneBuildLevel == 1 || Trap.trapOneBuildLevel == 2)
                {
                    TextMeshProUGUI buttonText = buildTrapOneButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (checkResources(trapOne))
                    {
                        // Kaynaklar� azalt
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
                                //----------------InGame Scene �le Alakl�--------------------------//
                                getPlayerData.UpgradeTrapOneStats(Trap.trapOneBuildLevel);
                                //----------------InGame Scene �le Alakl�--------------------------//
                                trapOne.UpdateTrapOneCosts(trapOne);
                                buttonText.text = "Y�kselt";
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
                        Debug.Log("Yeterli kaynak bulunmamaktad�r.");
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
            Debug.Log("Halihaz�rda i�lem devam ederken yeni i�lem ger�ekle�tiremezsiniz.");
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
                    // Kaynaklar� azalt
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
                            //----------------InGame Scene �le Alakl�--------------------------//
                            getPlayerData.TrapTwoIsBuilded = true;
                            getPlayerData.ActiveTrapTwo();
                            getPlayerData.UpgradeTrapTwoStats(Trap.trapTwoBuildLevel);
                            //----------------InGame Scene �le Alakl�--------------------------//
                            trapTwo.UpdateTrapTwoCosts(trapTwo);
                            buttonText.text = "Y�kselt";
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
                    Debug.Log("Yeterli kaynak bulunmamaktad�r.");
                }
            }
            else
            {
                if (Trap.trapTwoBuildLevel == 1 || Trap.trapTwoBuildLevel == 2)
                {
                    TextMeshProUGUI buttonText = buildTrapTwoButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (checkResources(trapTwo))
                    {
                        // Kaynaklar� azalt
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
                                //----------------InGame Scene �le Alakl�--------------------------//
                                getPlayerData.UpgradeTrapTwoStats(Trap.trapTwoBuildLevel);
                                //----------------InGame Scene �le Alakl�--------------------------//
                                trapTwo.UpdateTrapTwoCosts(trapTwo);
                                buttonText.text = "Y�kselt";
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
                        Debug.Log("Yeterli kaynak bulunmamaktad�r.");
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
            Debug.Log("Halihaz�rda i�lem devam ederken yeni i�lem ger�ekle�tiremezsiniz.");
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
                    // Kaynaklar� azalt
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
                            //----------------InGame Scene �le Alakl�--------------------------//
                            getPlayerData.TrapThreeIsBuilded = true;
                            getPlayerData.ActiveTrapThree();
                            getPlayerData.UpgradeTrapThreeStats(Trap.trapThreeBuildLevel);
                            //----------------InGame Scene �le Alakl�--------------------------//
                            trapThree.UpdateTrapThreeCosts(trapThree);
                            buttonText.text = "Y�kselt";
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
                    Debug.Log("Yeterli kaynak bulunmamaktad�r.");
                }
            }
            else
            {
                if (Trap.trapThreeBuildLevel == 1 || Trap.trapThreeBuildLevel == 2)
                {
                    TextMeshProUGUI buttonText = buildTrapThreeButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (checkResources(trapThree))
                    {
                        // Kaynaklar� azalt
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
                                //----------------InGame Scene �le Alakl�--------------------------//
                                getPlayerData.UpgradeTrapThreeStats(Trap.trapThreeBuildLevel);
                                //----------------InGame Scene �le Alakl�--------------------------//
                                trapThree.UpdateTrapThreeCosts(trapThree);
                                buttonText.text = "Y�kselt";
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
                        Debug.Log("Yeterli kaynak bulunmamaktad�r.");
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
            Debug.Log("Halihaz�rda i�lem devam ederken yeni i�lem ger�ekle�tiremezsiniz.");
        }
    }



}

