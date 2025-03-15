using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public GameObject progressBar;
    public GameObject healProgressBar;
    public GameObject buildWareHouseBar;
    public GameObject buildStonepitBar;
    public GameObject buildSawmillBar;
    public GameObject buildFarmBar;
    public GameObject buildBlacksmithBar;
    public GameObject buildLabBar;
    public GameObject buildBarracksBar;
    public GameObject buildHospitalBar;
    public GameObject upgradeCastleBar;
    public GameObject buildTowerBar;
    public GameObject buildTrapBar;


    public SliderController slider;
    public HastaneSliderController hastaneSlider;
    public float savasciCreationTime = 1.5f;
    public float okcuCreationTime = 2.5f;
    private float totalUnitAmount;
    public float savasciHealTime = 1.5f;
    public float okcuHealTime = 2.5f;

    public bool isBarracksBuildActive = false;
    public bool isUnitCreationActive = false;
    public bool isHealActive = false;
    public bool isHospitalBuildActive = false;
    public bool isTowerBuildingActive = false;
    public bool isFarmBuildActive = false;
    public bool isAnyTrapActive = false;
    public bool isWareHouseBuildingActive = false;
    public bool isStonePitBuildingActive = false;
    public bool isSawmillBuildingActive = false;
    public bool isBlacksmithBuildingActive = false;
    public bool isCastleBuildingActive = false;


    public Button createUnitButton;
    public Button healButton;

    public bool isLabBuildActive = false;
   
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
    public ConstructionController constructionController;
    public PanelManager panelManager;


    private TextMeshProUGUI buttonText;
    private TextMeshProUGUI healButtonText;


    public float time;
    public TextMeshProUGUI kalanZaman;

    public float createdSoldierAmount = 5f;
    public float createdArcherAmount = 5f;

    private float totalAltin = 0, totalYemek = 0, totalDemir = 0, totalTas = 0, totalKereste = 0;

    [Header("ScriptableObject")]
    public GetPlayerData getPlayerData;

    void Start()
    {
        // Baþlangýçta zaman sýfýrlanabilir.
        time = 0;
        
        buttonText = createUnitButton.GetComponentInChildren<TextMeshProUGUI>();
        healButtonText = healButton.GetComponentInChildren<TextMeshProUGUI>();

        getPlayerData.UpdateSoldierAmount(createdSoldierAmount, createdArcherAmount);//Default olarak 5'er adet askerler baþlatýyoruz.(5 Piyade, 5 Okçu)

        
    }

    public void CreateUnits()
    {
        //Progressbarý kontrol et 0'dan farklýysa ---> Bina Yükseltmesi sýrasýnda asker üretemezsiniz.
        //Deðilse asker üretebilirsin.
        if (!isBarracksBuildActive)
        {
            if (Barracks.wasBarracksCreated == true)
            {
                // Üretim sürelerini toplamak için deðiþkenler
                float totalTime = 0;
                totalUnitAmount = 0;
                // Savaþçý slider'ýnýn deðeri varsa
                if (slider.savasciSlider.value > 0)
                {
                    float savasciTime = slider.savasciSlider.value * savasciCreationTime;
                    totalTime += savasciTime;
                    totalUnitAmount += slider.savasciSlider.value;
                   
                }

                // Okçu slider'ýnýn deðeri varsa
                if (slider.okcuSlider.value > 0)
                {
                    float okcuTime = slider.okcuSlider.value * okcuCreationTime;
                    totalTime += okcuTime;
                    totalUnitAmount += slider.okcuSlider.value;
                    
                }


                // Tüm birimlerin toplam üretim süresi sýfýrdan büyükse progress bar'ý güncelle
                if (totalTime > 0)
                {
                    // Eðer progress bar doluyorsa ve aktifse
                    if (isUnitCreationActive)
                    {
                        // Mevcut animasyonu durdur
                        buttonText.text = "Eðit";

                        // Mevcut asker sayýsýný deðiþtirmiyoruz
                        // Sadece slider deðerlerini sýfýrlýyoruz
                        slider.okcuSlider.value = 0f;
                        slider.savasciSlider.value = 0f;

                        // Kaynaklarý geri ver
                        giveCostBack(slider.savasciSlider.value, slider.okcuSlider.value);

                        Debug.Log("Savaþcý Sayisi :" + createdSoldierAmount); // Burada mevcut asker sayýsý deðiþmeden kalýr
                        Debug.Log("Okcu Sayisi : " + createdArcherAmount);

                        LeanTween.cancel(progressBar);
                        panelManager.DestroyPanel("SoldierCreation");
                        isUnitCreationActive = false;

                        // Progress bar'ý sýfýrla
                        ResetProgressBar(progressBar);

                        // Toplam birim miktarýný sýfýrla
                        totalUnitAmount = 0;
                    }

                    else
                    {
                        // Progress bar'ý baþlat
                        isUnitCreationActive = true; // Progress bar aktif
                        buttonText.text = "Ýptal Et";
                        reduceCost(slider.savasciSlider.value, slider.okcuSlider.value);
                        LeanTween.scaleX(progressBar, 1, totalTime)
                            .setOnComplete(() =>
                            {
                                // Progress bar dolduðunda yapýlacak iþlemler
                                buttonText.text = "Üret";
                                OnProgressComplete();
                                createdArcherAmount += slider.okcuSlider.value;
                                createdSoldierAmount += slider.savasciSlider.value;

                                //--------------InGameAskerSayýsýGüncelleme-----------------
                                getPlayerData.UpdateSoldierAmount(createdSoldierAmount, createdArcherAmount); 
                                //-----------------------------------------------------------
                                
                                slider.okcuSlider.value = 0f;
                                slider.savasciSlider.value = 0f;
                                Debug.Log("Savaþcý Sayisi :" + createdSoldierAmount);
                                Debug.Log("Okcu Sayisi : " + createdArcherAmount);
                               
                                ResetProgressBar(progressBar); // Progress bar'ý sýfýrlamak için çaðýr
                               isUnitCreationActive = false;
                            });
                        panelManager.CreatePanel("SoldierCreation", totalUnitAmount.ToString(), totalTime, "SoldierCreation");
                    }
                }
            }
            else
            {
                Debug.Log("Öncelikle bir kýþla üretmelisiniz.");
            }
        }
        else
        {
            Debug.Log("Bina Yükseltmesi Sýrasýnda Asker Eðitemezsin");
        }


        
        
    }

    void reduceCost(float savasciCount, float okcuCount) // Maliyetleri kaynaklardan düþen fonksiyon.
    {

        totalAltin = ((int)savasciCount * 5) + ((int)okcuCount * 7) ;
        totalYemek = ((int)savasciCount * 5) + ((int)okcuCount * 6) ;
        totalDemir = ((int)savasciCount * 5) + ((int)okcuCount * 3) ;
        totalTas = ((int)savasciCount * 5) + ((int)okcuCount * 2) ;
        totalKereste = ((int)savasciCount * 5) + ((int)okcuCount * 10);

        Kingdom.myKingdom.GoldAmount -= (int)totalAltin;
        Kingdom.myKingdom.FoodAmount -= (int)totalYemek;
        Kingdom.myKingdom.IronAmount -= (int)totalDemir;
        Kingdom.myKingdom.StoneAmount -= (int)totalTas;
        Kingdom.myKingdom.WoodAmount -= (int)totalKereste;
    }

    void giveCostBack(float savasciCount, float okcuCount)
    {

        totalAltin = ((int)savasciCount * 5) + ((int)okcuCount * 7) ;
        totalYemek = ((int)savasciCount * 5) + ((int)okcuCount * 6);
        totalDemir = ((int)savasciCount * 5) + ((int)okcuCount * 3);
        totalTas = ((int)savasciCount * 5) + ((int)okcuCount * 2) ;
        totalKereste = ((int)savasciCount * 5) + ((int)okcuCount * 10) ;

        Kingdom.myKingdom.GoldAmount += (int)totalAltin;
        Kingdom.myKingdom.FoodAmount += (int)totalYemek;
        Kingdom.myKingdom.IronAmount += (int)totalDemir;
        Kingdom.myKingdom.StoneAmount += (int)totalTas;
        Kingdom.myKingdom.WoodAmount += (int)totalKereste;
    }
    void ResetProgressBar(GameObject gameObject)
    {
        gameObject.transform.localScale = new Vector3(0, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        // Ýsterseniz progress bar'ý yeniden kullanmak için baþka iþlemler de yapabilirsiniz
    }
    void OnProgressComplete()
    {
        // Burada progress bar dolduðunda yapýlacak iþlemleri tanýmla
        Debug.Log("Progress Bar doldu, iþlem gerçekleþtiriliyor!");
        Kingdom.myKingdom.SoldierAmount += totalUnitAmount;
        totalUnitAmount = 0;
        Debug.Log("Krallýðýnýzýn asker sayýsý:" + Kingdom.myKingdom.SoldierAmount);
    }


    public void HealUnits()
    {
        if(!isHospitalBuildActive)
        {
            if (Hospital.wasHospitalCreated == true)
            {
                float totalHealTime = 0; // Toplam iyileþtirme süresi
                int totalHealedUnitaAmount = 0;
                // HastaneSlider deðerlerini kontrol et
                if (hastaneSlider.savasciSlider.value > 0)
                {
                    totalHealTime += hastaneSlider.savasciSlider.value * savasciHealTime;
                    totalHealedUnitaAmount += (int)hastaneSlider.savasciSlider.value;
                }

                if (hastaneSlider.okcuSlider.value > 0)
                {
                    totalHealTime += hastaneSlider.okcuSlider.value * okcuHealTime;
                    totalHealedUnitaAmount += (int)hastaneSlider.okcuSlider.value;
                }


                // Toplam iyileþtirme süresi sýfýrdan büyükse progress bar'ý güncelle
                if (totalHealTime > 0)
                {


                    Debug.Log("Toplam iyileþtirme süresi: " + totalHealTime);

                    // Eðer progress bar doluyorsa ve aktifse
                    if (isHealActive)
                    {
                        // Mevcut animasyonu durdur
                        healButtonText.text = "Ýyileþtir";
                        giveCostBack(hastaneSlider.savasciSlider.value, hastaneSlider.okcuSlider.value);
                        LeanTween.cancel(healProgressBar);
                        panelManager.DestroyPanel("HealSoldier");
                        // Progress bar'ý sýfýrla
                        isHealActive = false;
                        ResetProgressBar(healProgressBar);
                        totalHealTime = 0;
                    }
                    else
                    {
                        // Progress bar'ý baþlat
                        isHealActive = true; // Progress bar aktif
                        healButtonText.text = "Ýptal Et";
                        reduceCost(hastaneSlider.savasciSlider.value, hastaneSlider.okcuSlider.value);
                        LeanTween.scaleX(healProgressBar, 1, totalHealTime)
                            .setOnComplete(() =>
                            {
                                // Progress bar dolduðunda yapýlacak iþlemler
                                healButtonText.text = "Ýyileþtir";
                                OnProgressComplete();
                                ResetProgressBar(healProgressBar); // Progress bar'ý sýfýrlamak için çaðýr
                                isHealActive = false;

                            });
                        panelManager.CreatePanel("HealSoldier",totalHealedUnitaAmount.ToString(), totalHealTime, "HealSoldier");
                    }
                }
            }
            else
            {
                Debug.Log("Öncelikle hastane inþa etmelisiniz.");
            }
        }
        else
        {
            Debug.Log("Ýnþa sýrasýnda birlik eðitemezsin");
        }
        
        
    }


    public IEnumerator WarehouseIsFinished(Warehouse warehouse, System.Action<bool> onCompletion)
    {
        // Yeni inþaata izin verilip verilmediðini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inþaat ayný anda aktif olabilir.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }

        wareHousePanelController.cancelWarehouseButton.gameObject.SetActive(true);
        wareHousePanelController.isBuildCanceled = false; // Ýptal durumu sýfýrla
        string panelName = "WareHouseBuildingProcessPanel";
        // LeanTween animasyonu baþlat
        LeanTween.scaleX(buildWareHouseBar, 1, warehouse.buildTime).setOnComplete(() => ResetProgressBar(buildWareHouseBar));
        panelManager.CreatePanel(panelName, warehouse.buildingName, warehouse.buildTime, "Building");

        isWareHouseBuildingActive = true;
        float elapsedTime = 0f; // Geçen zamaný takip et

        while (elapsedTime < warehouse.buildTime)
        {
            if (wareHousePanelController.isBuildCanceled) // Eðer iptal edilirse
            {
                LeanTween.cancel(buildWareHouseBar); // Animasyonu iptal et
                ResetProgressBar(buildWareHouseBar); // ProgressBar'ý sýfýrla
                onCompletion(false); // Baþarýsýzlýk durumunu bildir
               
                isWareHouseBuildingActive = false;
                yield break; // Coroutine sonlandýr
            }

            elapsedTime += Time.deltaTime; // Geçen süreyi artýr
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // Ýptal edilmeden tamamlandýysa
        wareHousePanelController.cancelWarehouseButton.gameObject.SetActive(false);
        isWareHouseBuildingActive = false;
        panelManager.DestroyPanel("WarehouseBuildingProcessPanel");


        onCompletion(true); // Tamamlandýðýnda baþarýlý olarak bildir
    }


    public IEnumerator StonePitIsFinished(StonePit stonepit, System.Action<bool> onCompletion)
    {
        // Yeni inþaata izin verilip verilmediðini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inþaat ayný anda aktif olabilir.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }
        string panelName = "StonepitBuildingProcessPanel";
        stonepitPanelController.cancelStonepitButton.gameObject.SetActive(true);
        stonepitPanelController.isBuildCanceled = false; // Ýptal durumu sýfýrla

        // LeanTween animasyonu baþlat
        LeanTween.scaleX(buildStonepitBar, 1, stonepit.buildTime).setOnComplete(() => ResetProgressBar(buildStonepitBar));
        panelManager.CreatePanel(panelName,stonepit.buildingName,stonepit.buildTime, "Building");
        isStonePitBuildingActive = true;
        float elapsedTime = 0f; // Geçen zamaný takip et

        while (elapsedTime < stonepit.buildTime)
        {
            if (stonepitPanelController.isBuildCanceled) // Eðer iptal edilirse
            {
                LeanTween.cancel(buildStonepitBar); // Animasyonu iptal et
                ResetProgressBar(buildStonepitBar); // ProgressBar'ý sýfýrla
                onCompletion(false); // Baþarýsýzlýk durumunu bildir
                isStonePitBuildingActive = false;
                yield break; // Coroutine sonlandýr
            }

            elapsedTime += Time.deltaTime; // Geçen süreyi artýr
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // Ýptal edilmeden tamamlandýysa
        stonepitPanelController.cancelStonepitButton.gameObject.SetActive(false);
        isStonePitBuildingActive = false;
        panelManager.DestroyPanel("StonepitBuildingProcessPanel");
        onCompletion(true); // Tamamlandýðýnda baþarýlý olarak bildir
    }

    
    public IEnumerator SawmillIsFinished(Sawmill sawmill, System.Action<bool> onCompletion)
    {
        // Yeni inþaata izin verilip verilmediðini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inþaat ayný anda aktif olabilir.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }

        sawmillPanelController.cancelSawmillButton.gameObject.SetActive(true);
        sawmillPanelController.isBuildCanceled = false; // Ýptal durumu sýfýrla

        // LeanTween animasyonu baþlat
        LeanTween.scaleX(buildSawmillBar, 1, sawmill.buildTime).setOnComplete(() => ResetProgressBar(buildSawmillBar));
        string panelName = "SawmillBuildingProcessPanel";
        panelManager.CreatePanel(panelName, sawmill.buildingName, sawmill.buildTime, "Building");
        isSawmillBuildingActive = true;

        float elapsedTime = 0f; // Geçen zamaný takip et

        while (elapsedTime < sawmill.buildTime)
        {
          
            if (sawmillPanelController.isBuildCanceled) // Eðer iptal edilirse
            {
                LeanTween.cancel(buildSawmillBar); // Animasyonu iptal et
                ResetProgressBar(buildSawmillBar); // ProgressBar'ý sýfýrla
                onCompletion(false); // Baþarýsýzlýk durumunu bildir
                //DestroyProcess();
                isSawmillBuildingActive = false;
                Debug.Log(" Coroutine Sonlandý");
                
                yield break; // Coroutine sonlandýr
            }
            
            elapsedTime += Time.deltaTime; // Geçen süreyi artýr
          
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // Ýptal edilmeden tamamlandýysa
        Debug.Log("Tamamlandý");
        sawmillPanelController.cancelSawmillButton.gameObject.SetActive(false);
        isSawmillBuildingActive = false;
        panelManager.DestroyPanel("SawmillBuildingProcessPanel");
        sawmillPanelController.refreshSawmill();
        //DestroyProcess();
        onCompletion(true); // Tamamlandýðýnda baþarýlý olarak bildir
    }
    
   

    public IEnumerator FarmIsFinished(Farm farm, System.Action<bool> onCompletion)
    {
        // Yeni inþaata izin verilip verilmediðini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inþaat ayný anda aktif olabilir.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }

        farmPanelController.cancelFarmButton.gameObject.SetActive(true);
        farmPanelController.isBuildCanceled = false; // Ýptal durumu sýfýrla

        // LeanTween animasyonu baþlat
        LeanTween.scaleX(buildFarmBar, 1, farm.buildTime).setOnComplete(() => ResetProgressBar(buildFarmBar));
        isFarmBuildActive = true;

        string panelName = "FarmBuildingProcessPanel";
        panelManager.CreatePanel(panelName, farm.buildingName, farm.buildTime, "Building");

        float elapsedTime = 0f; // Geçen zamaný takip et

        while (elapsedTime < farm.buildTime)
        {
            Debug.Log("SawmillIsFinished adlý IEnumarator'un içindeki while döngüsündeyim.");
            if (farmPanelController.isBuildCanceled) // Eðer iptal edilirse
            {
                LeanTween.cancel(buildFarmBar); // Animasyonu iptal et
                ResetProgressBar(buildFarmBar); // ProgressBar'ý sýfýrla
                onCompletion(false); // Baþarýsýzlýk durumunu bildir
                isFarmBuildActive = false;
                yield break; // Coroutine sonlandýr
            }

            elapsedTime += Time.deltaTime; // Geçen süreyi artýr
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // Ýptal edilmeden tamamlandýysa
        Debug.Log("Coroutine Bitti");
        farmPanelController.cancelFarmButton.gameObject.SetActive(false);
        isFarmBuildActive = false;
        panelManager.DestroyPanel("FarmBuildingProcessPanel");
        farmPanelController.refreshFarm();
        onCompletion(true); // Tamamlandýðýnda baþarýlý olarak bildir
    }



    public IEnumerator BlacksmithIsFinished(Blacksmith blacksmith, System.Action<bool> onCompletion)
    {
        // Yeni inþaata izin verilip verilmediðini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inþaat ayný anda aktif olabilir.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }

        blacksmithPanelController.cancelBlacksmithButton.gameObject.SetActive(true);
        blacksmithPanelController.isBuildCanceled = false; // Ýptal durumu sýfýrla

        // LeanTween animasyonu baþlat
        LeanTween.scaleX(buildBlacksmithBar, 1, blacksmith.buildTime).setOnComplete(() => ResetProgressBar(buildBlacksmithBar));
        isBlacksmithBuildingActive = true;

        string panelName = "BlacksmithBuildingProcessPanel";
        panelManager.CreatePanel(panelName, blacksmith.buildingName, blacksmith.buildTime, "Building");

        float elapsedTime = 0f; // Geçen zamaný takip et

        while (elapsedTime < blacksmith.buildTime)
        {
            if (blacksmithPanelController.isBuildCanceled) // Eðer iptal edilirse
            {
                LeanTween.cancel(buildBlacksmithBar); // Animasyonu iptal et
                ResetProgressBar(buildBlacksmithBar); // ProgressBar'ý sýfýrla
                onCompletion(false); // Baþarýsýzlýk durumunu bildir
                isBlacksmithBuildingActive = false;
                yield break; // Coroutine sonlandýr
            }

            elapsedTime += Time.deltaTime; // Geçen süreyi artýr
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // Ýptal edilmeden tamamlandýysa
        blacksmithPanelController.cancelBlacksmithButton.gameObject.SetActive(false);
        isBlacksmithBuildingActive = false;
        panelManager.DestroyPanel("BlacksmithBuildingProcessPanel");

        onCompletion(true); // Tamamlandýðýnda baþarýlý olarak bildir
    }


    public IEnumerator LabIsFinished(Lab lab, System.Action<bool> onCompletion)
    {
        // Araþtýrma sýrasýnda bina yükseltmesine izin verilmez
        if (ResearchButtonEvents.isAnyResearchActive)
        {
            Debug.Log("Araþtýrma sýrasýnda bina yükseltmesi yapamazsýnýz.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }

        // Yeni inþaata izin verilip verilmediðini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inþaat ayný anda aktif olabilir.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }

        labPanelController.cancelLabButton.gameObject.SetActive(true);
        labPanelController.isBuildCanceled = false; // Ýptal durumu sýfýrla

        // LeanTween animasyonu baþlat
        LeanTween.scaleX(buildLabBar, 1, lab.buildTime).setOnComplete(() => ResetProgressBar(buildLabBar));
        isLabBuildActive = true;

        string panelName = "LabBuildingProcessPanel";
        panelManager.CreatePanel(panelName, lab.buildingName, lab.buildTime, "Building");

        float elapsedTime = 0f; // Geçen zamaný takip et

        while (elapsedTime < lab.buildTime)
        {
            if (labPanelController.isBuildCanceled) // Eðer iptal edilirse
            {
                LeanTween.cancel(buildLabBar); // Animasyonu iptal et
                isLabBuildActive = false;
                ResetProgressBar(buildLabBar); // ProgressBar'ý sýfýrla
                onCompletion(false); // Baþarýsýzlýk durumunu bildir
                yield break; // Coroutine sonlandýr
            }

            elapsedTime += Time.deltaTime; // Geçen süreyi artýr
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // Ýptal edilmeden tamamlandýysa
        isLabBuildActive = false;
        panelManager.DestroyPanel("LabBuildingProcessPanel");
        labPanelController.cancelLabButton.gameObject.SetActive(false);
        onCompletion(true); // Tamamlandýðýnda baþarýlý olarak bildir
    }


    public IEnumerator BarracksIsFinished(Barracks barracks, System.Action<bool> onCompletion)
    {
        // Asker üretimi kontrolü
        if (isUnitCreationActive)
        {
            Debug.Log("Asker üretimi yaparken bina yükseltemezsiniz.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }

        // Yeni inþaata izin verilip verilmediðini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inþaat ayný anda aktif olabilir.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }

        barracksPanelController.cancelBarracksButton.gameObject.SetActive(true);
        barracksPanelController.isBuildCanceled = false; // Ýptal durumu sýfýrla

        // LeanTween animasyonu baþlat
        LeanTween.scaleX(buildBarracksBar, 1, barracks.buildTime).setOnComplete(() => ResetProgressBar(buildBarracksBar));
        isBarracksBuildActive = true;

        string panelName = "BarracksBuildingProcessPanel";
        panelManager.CreatePanel(panelName, barracks.buildingName, barracks.buildTime, "Building");

        float elapsedTime = 0f; // Geçen zamaný takip et

        while (elapsedTime < barracks.buildTime)
        {
            if (barracksPanelController.isBuildCanceled) // Eðer iptal edilirse
            {
                LeanTween.cancel(buildBarracksBar); // Animasyonu iptal et
                ResetProgressBar(buildBarracksBar); // ProgressBar'ý sýfýrla
                isBarracksBuildActive = false;
                onCompletion(false); // Baþarýsýzlýk durumunu bildir
                yield break; // Coroutine sonlandýr
            }

            elapsedTime += Time.deltaTime; // Geçen süreyi artýr
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // Ýptal edilmeden tamamlandýysa
        isBarracksBuildActive = false;
        panelManager.DestroyPanel("BarracksBuildingProcessPanel");
        barracksPanelController.cancelBarracksButton.gameObject.SetActive(false);
        onCompletion(true); // Tamamlandýðýnda baþarýlý olarak bildir
    }


    public IEnumerator HospitalIsFinished(Hospital hospital, System.Action<bool> onCompletion)
    {
        // Ýyileþtirme aktifse bina yükseltme yapýlmasýn
        if (isHealActive)
        {
            Debug.Log("Ýyileþtirme sýrasýnda bina yükseltemezsiniz, iyileþtirmeyi iptal edip yeniden deneyin.");
            onCompletion(false); // Baþarýsýzlýk durumu bildir
            yield break; // Coroutine sonlandýr
        }

        // Yeni inþaata izin verilip verilmediðini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inþaat ayný anda aktif olabilir.");
            onCompletion(false); // Baþarýsýzlýk durumu bildir
            yield break; // Coroutine sonlandýr
        }

        hospitalPanelController.cancelHospitalButton.gameObject.SetActive(true);
        hospitalPanelController.isBuildCanceled = false; // Ýptal durumu sýfýrla

        // LeanTween animasyonu baþlat
        LeanTween.scaleX(buildHospitalBar, 1, hospital.buildTime).setOnComplete(() => ResetProgressBar(buildHospitalBar));
        isHospitalBuildActive = true;

        string panelName = "HospitalBuildingProcessPanel";
        panelManager.CreatePanel(panelName, hospital.buildingName, hospital.buildTime, "Building");

        float elapsedTime = 0f; // Geçen zamaný takip et

        while (elapsedTime < hospital.buildTime)
        {
            if (hospitalPanelController.isBuildCanceled) // Eðer iptal edilirse
            {
                LeanTween.cancel(buildHospitalBar); // Animasyonu iptal et
                ResetProgressBar(buildHospitalBar); // ProgressBar'ý sýfýrla
                isHospitalBuildActive = false;
                onCompletion(false); // Baþarýsýzlýk durumunu bildir
                yield break; // Coroutine sonlandýr
            }

            elapsedTime += Time.deltaTime; // Geçen süreyi artýr
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // Ýptal edilmeden tamamlandýysa
        isHospitalBuildActive = false;
        panelManager.DestroyPanel("HospitalBuildingProcessPanel");
        hospitalPanelController.cancelHospitalButton.gameObject.SetActive(false);
        onCompletion(true); // Tamamlandýðýnda baþarýlý olarak bildir
    }


    public IEnumerator CastleIsFinished(Castle castle, System.Action<bool> onCompletion)
    {
        // Yeni inþaata izin verilip verilmediðini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inþaat ayný anda aktif olabilir.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }

        castlePanelController.cancelUpgradeCastleButton.gameObject.SetActive(true);
        castlePanelController.isBuildCanceled = false; // Ýptal durumu sýfýrla

        // LeanTween animasyonu baþlat
        LeanTween.scaleX(upgradeCastleBar, 1, castle.buildTime).setOnComplete(() => ResetProgressBar(upgradeCastleBar));
        isCastleBuildingActive = true;


        string panelName = "CastleUpgradeProcessPanel";
        panelManager.CreatePanel(panelName, castle.buildingName, castle.buildTime, "Building");

        float elapsedTime = 0f; // Geçen zamaný takip et

        while (elapsedTime < castle.buildTime)
        {
            if (castlePanelController.isBuildCanceled) // Eðer iptal edilirse
            {
                LeanTween.cancel(upgradeCastleBar); // Animasyonu iptal et
                ResetProgressBar(upgradeCastleBar); // ProgressBar'ý sýfýrla
                isCastleBuildingActive = false;
                onCompletion(false); // Baþarýsýzlýk durumunu bildir
                yield break; // Coroutine sonlandýr
            }

            elapsedTime += Time.deltaTime; // Geçen süreyi artýr
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // Ýptal edilmeden tamamlandýysa
        castlePanelController.cancelUpgradeCastleButton.gameObject.SetActive(false);
        isCastleBuildingActive = false;
        panelManager.DestroyPanel("CastleUpgradeProcessPanel");
        onCompletion(true); // Tamamlandýðýnda baþarýlý olarak bildir
    }


    public IEnumerator TowerIsFinished(Tower tower, System.Action<bool> onCompletion)
    {

        if (isTowerBuildingActive)
        {
            Debug.Log("Halihazýrda bir iþlem devam ederken yeni iþlem gerçekleþtiremezsiniz.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inþaat ayný anda aktif olabilir.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }
      
            towerPanelController.cancelTowerButton.gameObject.SetActive(true);
            towerPanelController.isBuildCanceled = false; // Ýptal durumu sýfýrla

            // LeanTween animasyonu baþlat
            LeanTween.scaleX(buildTowerBar, 1, tower.buildTime).setOnComplete(() => ResetProgressBar(buildTowerBar));
            isTowerBuildingActive = true;

            string panelName = "TowerBuildingProcessPanel";  
            panelManager.CreatePanel(panelName, tower.buildingName, tower.buildTime, "Building");

            float elapsedTime = 0f; // Geçen zamaný takip et

            while (elapsedTime < tower.buildTime)
            {
                if (towerPanelController.isBuildCanceled) // Eðer iptal edilirse
                {
                    LeanTween.cancel(buildTowerBar); // Animasyonu iptal et
                    ResetProgressBar(buildTowerBar); // ProgressBar'ý sýfýrla
                    isTowerBuildingActive = false;
                    onCompletion(false); // Baþarýsýzlýk durumunu bildir
                    yield break; // Coroutine sonlandýr
                }

                elapsedTime += Time.deltaTime; // Geçen süreyi artýr
                yield return null; // Bir sonraki kareye kadar bekle
            }

            // Ýptal edilmeden tamamlandýysa
            isTowerBuildingActive = false;
        panelManager.DestroyPanel("TowerBuildingProcessPanel");
        towerPanelController.cancelTowerButton.gameObject.SetActive(false);
            onCompletion(true); // Tamamlandýðýnda baþarýlý olarak bildir
        
    }

    public IEnumerator TrapIsFinished(Trap trap, System.Action<bool> onCompletion)
    {
        if (isAnyTrapActive)
        {
            Debug.Log("Halihazýrda bir iþlem devam ederken yeni iþlem gerçekleþtiremezsiniz.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }
        
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inþaat ayný anda aktif olabilir.");
            onCompletion(false); // Baþarýsýzlýk durumunu bildir
            yield break; // Coroutine sonlandýr
        }
       
            trapPanelController.cancelTrapButton.gameObject.SetActive(true);
            trapPanelController.isBuildCanceled = false; // Ýptal durumu sýfýrla

            // LeanTween animasyonu baþlat
            LeanTween.scaleX(buildTrapBar, 1, trap.buildTime).setOnComplete(() => ResetProgressBar(buildTrapBar));
            isAnyTrapActive = true;

            string panelName = "TrapBuildingProcessPanel";
            panelManager.CreatePanel(panelName, trap.buildingName, trap.buildTime, "Building");

        float elapsedTime = 0f; // Geçen zamaný takip et

            while (elapsedTime < trap.buildTime)
            {
                if (trapPanelController.isBuildCanceled) // Eðer iptal edilirse
                {
                    LeanTween.cancel(buildTrapBar); // Animasyonu iptal et
                    ResetProgressBar(buildTrapBar); // ProgressBar'ý sýfýrla
                    isAnyTrapActive = false;
                    onCompletion(false); // Baþarýsýzlýk durumunu bildir
                    yield break; // Coroutine sonlandýr
                }

                elapsedTime += Time.deltaTime; // Geçen süreyi artýr
                yield return null; // Bir sonraki kareye kadar bekle
            }

            // Ýptal edilmeden tamamlandýysa
            isAnyTrapActive = false;
            panelManager.DestroyPanel("TrapBuildingProcessPanel");
            trapPanelController.cancelTrapButton.gameObject.SetActive(false);
            onCompletion(true); // Tamamlandýðýnda baþarýlý olarak bildir
        
    }


}
