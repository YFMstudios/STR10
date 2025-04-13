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

    public float createdSoldierAmount = 6f;
    public float createdArcherAmount = 6f;

    private float totalAltin = 0, totalYemek = 0, totalDemir = 0, totalTas = 0, totalKereste = 0;

    [Header("ScriptableObject")]
    public GetPlayerData getPlayerData;

    void Start()
    {
        // Ba�lang��ta zaman s�f�rlanabilir.
        time = 0;
        
        buttonText = createUnitButton.GetComponentInChildren<TextMeshProUGUI>();
        healButtonText = healButton.GetComponentInChildren<TextMeshProUGUI>();

        getPlayerData.UpdateSoldierAmount(createdSoldierAmount, createdArcherAmount);//Default olarak 5'er adet askerler ba�lat�yoruz.(5 Piyade, 5 Ok�u)

        
    }

    public void CreateUnits()
    {
        //Progressbar� kontrol et 0'dan farkl�ysa ---> Bina Y�kseltmesi s�ras�nda asker �retemezsiniz.
        //De�ilse asker �retebilirsin.
        if (!isBarracksBuildActive)
        {
            if (Barracks.wasBarracksCreated == true)
            {
                // �retim s�relerini toplamak i�in de�i�kenler
                float totalTime = 0;
                totalUnitAmount = 0;
                // Sava��� slider'�n�n de�eri varsa
                if (slider.savasciSlider.value > 0)
                {
                    float savasciTime = slider.savasciSlider.value * savasciCreationTime;
                    totalTime += savasciTime;
                    totalUnitAmount += slider.savasciSlider.value;
                   
                }

                // Ok�u slider'�n�n de�eri varsa
                if (slider.okcuSlider.value > 0)
                {
                    float okcuTime = slider.okcuSlider.value * okcuCreationTime;
                    totalTime += okcuTime;
                    totalUnitAmount += slider.okcuSlider.value;
                    
                }


                // T�m birimlerin toplam �retim s�resi s�f�rdan b�y�kse progress bar'� g�ncelle
                if (totalTime > 0)
                {
                    // E�er progress bar doluyorsa ve aktifse
                    if (isUnitCreationActive)
                    {
                        // Mevcut animasyonu durdur
                        buttonText.text = "E�it";

                        // Mevcut asker say�s�n� de�i�tirmiyoruz
                        // Sadece slider de�erlerini s�f�rl�yoruz
                        slider.okcuSlider.value = 0f;
                        slider.savasciSlider.value = 0f;

                        // Kaynaklar� geri ver
                        giveCostBack(slider.savasciSlider.value, slider.okcuSlider.value);

                        Debug.Log("Sava�c� Sayisi :" + createdSoldierAmount); // Burada mevcut asker say�s� de�i�meden kal�r
                        Debug.Log("Okcu Sayisi : " + createdArcherAmount);

                        LeanTween.cancel(progressBar);
                        panelManager.DestroyPanel("SoldierCreation");
                        isUnitCreationActive = false;

                        // Progress bar'� s�f�rla
                        ResetProgressBar(progressBar);

                        // Toplam birim miktar�n� s�f�rla
                        totalUnitAmount = 0;
                    }

                    else
                    {
                        // Progress bar'� ba�lat
                        isUnitCreationActive = true; // Progress bar aktif
                        buttonText.text = "�ptal Et";
                        reduceCost(slider.savasciSlider.value, slider.okcuSlider.value);
                        LeanTween.scaleX(progressBar, 1, totalTime)
                            .setOnComplete(() =>
                            {
                                // Progress bar doldu�unda yap�lacak i�lemler
                                buttonText.text = "�ret";
                                OnProgressComplete();
                                createdArcherAmount += slider.okcuSlider.value;
                                createdSoldierAmount += slider.savasciSlider.value;

                                //--------------InGameAskerSay�s�G�ncelleme-----------------
                                getPlayerData.UpdateSoldierAmount(createdSoldierAmount, createdArcherAmount); 
                                //-----------------------------------------------------------
                                
                                slider.okcuSlider.value = 0f;
                                slider.savasciSlider.value = 0f;
                                Debug.Log("Sava�c� Sayisi :" + createdSoldierAmount);
                                Debug.Log("Okcu Sayisi : " + createdArcherAmount);
                               
                                ResetProgressBar(progressBar); // Progress bar'� s�f�rlamak i�in �a��r
                               isUnitCreationActive = false;
                            });
                        panelManager.CreatePanel("SoldierCreation", totalUnitAmount.ToString(), totalTime, "SoldierCreation");
                    }
                }
            }
            else
            {
                Debug.Log("�ncelikle bir k��la �retmelisiniz.");
            }
        }
        else
        {
            Debug.Log("Bina Y�kseltmesi S�ras�nda Asker E�itemezsin");
        }


        
        
    }

    void reduceCost(float savasciCount, float okcuCount) // Maliyetleri kaynaklardan d��en fonksiyon.
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
        // �sterseniz progress bar'� yeniden kullanmak i�in ba�ka i�lemler de yapabilirsiniz
    }
    void OnProgressComplete()
    {
        // Burada progress bar doldu�unda yap�lacak i�lemleri tan�mla
        Debug.Log("Progress Bar doldu, i�lem ger�ekle�tiriliyor!");
        Kingdom.myKingdom.SoldierAmount += totalUnitAmount;
        totalUnitAmount = 0;
        Debug.Log("Krall���n�z�n asker say�s�:" + Kingdom.myKingdom.SoldierAmount);
    }


    public void HealUnits()
    {
        if(!isHospitalBuildActive)
        {
            if (Hospital.wasHospitalCreated == true)
            {
                float totalHealTime = 0; // Toplam iyile�tirme s�resi
                int totalHealedUnitaAmount = 0;
                // HastaneSlider de�erlerini kontrol et
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


                // Toplam iyile�tirme s�resi s�f�rdan b�y�kse progress bar'� g�ncelle
                if (totalHealTime > 0)
                {


                    Debug.Log("Toplam iyile�tirme s�resi: " + totalHealTime);

                    // E�er progress bar doluyorsa ve aktifse
                    if (isHealActive)
                    {
                        // Mevcut animasyonu durdur
                        healButtonText.text = "�yile�tir";
                        giveCostBack(hastaneSlider.savasciSlider.value, hastaneSlider.okcuSlider.value);
                        LeanTween.cancel(healProgressBar);
                        panelManager.DestroyPanel("HealSoldier");
                        // Progress bar'� s�f�rla
                        isHealActive = false;
                        ResetProgressBar(healProgressBar);
                        totalHealTime = 0;
                    }
                    else
                    {
                        // Progress bar'� ba�lat
                        isHealActive = true; // Progress bar aktif
                        healButtonText.text = "�ptal Et";
                        reduceCost(hastaneSlider.savasciSlider.value, hastaneSlider.okcuSlider.value);
                        LeanTween.scaleX(healProgressBar, 1, totalHealTime)
                            .setOnComplete(() =>
                            {
                                // Progress bar doldu�unda yap�lacak i�lemler
                                healButtonText.text = "�yile�tir";
                                OnProgressComplete();
                                ResetProgressBar(healProgressBar); // Progress bar'� s�f�rlamak i�in �a��r
                                isHealActive = false;

                            });
                        panelManager.CreatePanel("HealSoldier",totalHealedUnitaAmount.ToString(), totalHealTime, "HealSoldier");
                    }
                }
            }
            else
            {
                Debug.Log("�ncelikle hastane in�a etmelisiniz.");
            }
        }
        else
        {
            Debug.Log("�n�a s�ras�nda birlik e�itemezsin");
        }
        
        
    }


    public IEnumerator WarehouseIsFinished(Warehouse warehouse, System.Action<bool> onCompletion)
    {
        // Yeni in�aata izin verilip verilmedi�ini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 in�aat ayn� anda aktif olabilir.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }

        wareHousePanelController.cancelWarehouseButton.gameObject.SetActive(true);
        wareHousePanelController.isBuildCanceled = false; // �ptal durumu s�f�rla
        string panelName = "WareHouseBuildingProcessPanel";
        // LeanTween animasyonu ba�lat
        LeanTween.scaleX(buildWareHouseBar, 1, warehouse.buildTime).setOnComplete(() => ResetProgressBar(buildWareHouseBar));
        panelManager.CreatePanel(panelName, warehouse.buildingName, warehouse.buildTime, "Building");

        isWareHouseBuildingActive = true;
        float elapsedTime = 0f; // Ge�en zaman� takip et

        while (elapsedTime < warehouse.buildTime)
        {
            if (wareHousePanelController.isBuildCanceled) // E�er iptal edilirse
            {
                LeanTween.cancel(buildWareHouseBar); // Animasyonu iptal et
                ResetProgressBar(buildWareHouseBar); // ProgressBar'� s�f�rla
                onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
               
                isWareHouseBuildingActive = false;
                yield break; // Coroutine sonland�r
            }

            elapsedTime += Time.deltaTime; // Ge�en s�reyi art�r
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // �ptal edilmeden tamamland�ysa
        wareHousePanelController.cancelWarehouseButton.gameObject.SetActive(false);
        isWareHouseBuildingActive = false;
        panelManager.DestroyPanel("WarehouseBuildingProcessPanel");


        onCompletion(true); // Tamamland���nda ba�ar�l� olarak bildir
    }


    public IEnumerator StonePitIsFinished(StonePit stonepit, System.Action<bool> onCompletion)
    {
        // Yeni in�aata izin verilip verilmedi�ini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 in�aat ayn� anda aktif olabilir.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }
        string panelName = "StonepitBuildingProcessPanel";
        stonepitPanelController.cancelStonepitButton.gameObject.SetActive(true);
        stonepitPanelController.isBuildCanceled = false; // �ptal durumu s�f�rla

        // LeanTween animasyonu ba�lat
        LeanTween.scaleX(buildStonepitBar, 1, stonepit.buildTime).setOnComplete(() => ResetProgressBar(buildStonepitBar));
        panelManager.CreatePanel(panelName,stonepit.buildingName,stonepit.buildTime, "Building");
        isStonePitBuildingActive = true;
        float elapsedTime = 0f; // Ge�en zaman� takip et

        while (elapsedTime < stonepit.buildTime)
        {
            if (stonepitPanelController.isBuildCanceled) // E�er iptal edilirse
            {
                LeanTween.cancel(buildStonepitBar); // Animasyonu iptal et
                ResetProgressBar(buildStonepitBar); // ProgressBar'� s�f�rla
                onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
                isStonePitBuildingActive = false;
                yield break; // Coroutine sonland�r
            }

            elapsedTime += Time.deltaTime; // Ge�en s�reyi art�r
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // �ptal edilmeden tamamland�ysa
        stonepitPanelController.cancelStonepitButton.gameObject.SetActive(false);
        isStonePitBuildingActive = false;
        panelManager.DestroyPanel("StonepitBuildingProcessPanel");
        onCompletion(true); // Tamamland���nda ba�ar�l� olarak bildir
    }

    
    public IEnumerator SawmillIsFinished(Sawmill sawmill, System.Action<bool> onCompletion)
    {
        // Yeni in�aata izin verilip verilmedi�ini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 in�aat ayn� anda aktif olabilir.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }

        sawmillPanelController.cancelSawmillButton.gameObject.SetActive(true);
        sawmillPanelController.isBuildCanceled = false; // �ptal durumu s�f�rla

        // LeanTween animasyonu ba�lat
        LeanTween.scaleX(buildSawmillBar, 1, sawmill.buildTime).setOnComplete(() => ResetProgressBar(buildSawmillBar));
        string panelName = "SawmillBuildingProcessPanel";
        panelManager.CreatePanel(panelName, sawmill.buildingName, sawmill.buildTime, "Building");
        isSawmillBuildingActive = true;

        float elapsedTime = 0f; // Ge�en zaman� takip et

        while (elapsedTime < sawmill.buildTime)
        {
          
            if (sawmillPanelController.isBuildCanceled) // E�er iptal edilirse
            {
                LeanTween.cancel(buildSawmillBar); // Animasyonu iptal et
                ResetProgressBar(buildSawmillBar); // ProgressBar'� s�f�rla
                onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
                //DestroyProcess();
                isSawmillBuildingActive = false;
                Debug.Log(" Coroutine Sonland�");
                
                yield break; // Coroutine sonland�r
            }
            
            elapsedTime += Time.deltaTime; // Ge�en s�reyi art�r
          
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // �ptal edilmeden tamamland�ysa
        Debug.Log("Tamamland�");
        sawmillPanelController.cancelSawmillButton.gameObject.SetActive(false);
        isSawmillBuildingActive = false;
        panelManager.DestroyPanel("SawmillBuildingProcessPanel");
        sawmillPanelController.refreshSawmill();
        //DestroyProcess();
        onCompletion(true); // Tamamland���nda ba�ar�l� olarak bildir
    }
    
   

    public IEnumerator FarmIsFinished(Farm farm, System.Action<bool> onCompletion)
    {
        // Yeni in�aata izin verilip verilmedi�ini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 in�aat ayn� anda aktif olabilir.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }

        farmPanelController.cancelFarmButton.gameObject.SetActive(true);
        farmPanelController.isBuildCanceled = false; // �ptal durumu s�f�rla

        // LeanTween animasyonu ba�lat
        LeanTween.scaleX(buildFarmBar, 1, farm.buildTime).setOnComplete(() => ResetProgressBar(buildFarmBar));
        isFarmBuildActive = true;

        string panelName = "FarmBuildingProcessPanel";
        panelManager.CreatePanel(panelName, farm.buildingName, farm.buildTime, "Building");

        float elapsedTime = 0f; // Ge�en zaman� takip et

        while (elapsedTime < farm.buildTime)
        {
            Debug.Log("SawmillIsFinished adl� IEnumarator'un i�indeki while d�ng�s�ndeyim.");
            if (farmPanelController.isBuildCanceled) // E�er iptal edilirse
            {
                LeanTween.cancel(buildFarmBar); // Animasyonu iptal et
                ResetProgressBar(buildFarmBar); // ProgressBar'� s�f�rla
                onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
                isFarmBuildActive = false;
                yield break; // Coroutine sonland�r
            }

            elapsedTime += Time.deltaTime; // Ge�en s�reyi art�r
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // �ptal edilmeden tamamland�ysa
        Debug.Log("Coroutine Bitti");
        farmPanelController.cancelFarmButton.gameObject.SetActive(false);
        isFarmBuildActive = false;
        panelManager.DestroyPanel("FarmBuildingProcessPanel");
        farmPanelController.refreshFarm();
        onCompletion(true); // Tamamland���nda ba�ar�l� olarak bildir
    }



    public IEnumerator BlacksmithIsFinished(Blacksmith blacksmith, System.Action<bool> onCompletion)
    {
        // Yeni in�aata izin verilip verilmedi�ini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 in�aat ayn� anda aktif olabilir.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }

        blacksmithPanelController.cancelBlacksmithButton.gameObject.SetActive(true);
        blacksmithPanelController.isBuildCanceled = false; // �ptal durumu s�f�rla

        // LeanTween animasyonu ba�lat
        LeanTween.scaleX(buildBlacksmithBar, 1, blacksmith.buildTime).setOnComplete(() => ResetProgressBar(buildBlacksmithBar));
        isBlacksmithBuildingActive = true;

        string panelName = "BlacksmithBuildingProcessPanel";
        panelManager.CreatePanel(panelName, blacksmith.buildingName, blacksmith.buildTime, "Building");

        float elapsedTime = 0f; // Ge�en zaman� takip et

        while (elapsedTime < blacksmith.buildTime)
        {
            if (blacksmithPanelController.isBuildCanceled) // E�er iptal edilirse
            {
                LeanTween.cancel(buildBlacksmithBar); // Animasyonu iptal et
                ResetProgressBar(buildBlacksmithBar); // ProgressBar'� s�f�rla
                onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
                isBlacksmithBuildingActive = false;
                yield break; // Coroutine sonland�r
            }

            elapsedTime += Time.deltaTime; // Ge�en s�reyi art�r
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // �ptal edilmeden tamamland�ysa
        blacksmithPanelController.cancelBlacksmithButton.gameObject.SetActive(false);
        isBlacksmithBuildingActive = false;
        panelManager.DestroyPanel("BlacksmithBuildingProcessPanel");

        onCompletion(true); // Tamamland���nda ba�ar�l� olarak bildir
    }


    public IEnumerator LabIsFinished(Lab lab, System.Action<bool> onCompletion)
    {
        // Ara�t�rma s�ras�nda bina y�kseltmesine izin verilmez
        if (ResearchButtonEvents.isAnyResearchActive)
        {
            Debug.Log("Ara�t�rma s�ras�nda bina y�kseltmesi yapamazs�n�z.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }

        // Yeni in�aata izin verilip verilmedi�ini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 in�aat ayn� anda aktif olabilir.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }

        labPanelController.cancelLabButton.gameObject.SetActive(true);
        labPanelController.isBuildCanceled = false; // �ptal durumu s�f�rla

        // LeanTween animasyonu ba�lat
        LeanTween.scaleX(buildLabBar, 1, lab.buildTime).setOnComplete(() => ResetProgressBar(buildLabBar));
        isLabBuildActive = true;

        string panelName = "LabBuildingProcessPanel";
        panelManager.CreatePanel(panelName, lab.buildingName, lab.buildTime, "Building");

        float elapsedTime = 0f; // Ge�en zaman� takip et

        while (elapsedTime < lab.buildTime)
        {
            if (labPanelController.isBuildCanceled) // E�er iptal edilirse
            {
                LeanTween.cancel(buildLabBar); // Animasyonu iptal et
                isLabBuildActive = false;
                ResetProgressBar(buildLabBar); // ProgressBar'� s�f�rla
                onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
                yield break; // Coroutine sonland�r
            }

            elapsedTime += Time.deltaTime; // Ge�en s�reyi art�r
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // �ptal edilmeden tamamland�ysa
        isLabBuildActive = false;
        panelManager.DestroyPanel("LabBuildingProcessPanel");
        labPanelController.cancelLabButton.gameObject.SetActive(false);
        onCompletion(true); // Tamamland���nda ba�ar�l� olarak bildir
    }


    public IEnumerator BarracksIsFinished(Barracks barracks, System.Action<bool> onCompletion)
    {
        // Asker �retimi kontrol�
        if (isUnitCreationActive)
        {
            Debug.Log("Asker �retimi yaparken bina y�kseltemezsiniz.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }

        // Yeni in�aata izin verilip verilmedi�ini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 in�aat ayn� anda aktif olabilir.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }

        barracksPanelController.cancelBarracksButton.gameObject.SetActive(true);
        barracksPanelController.isBuildCanceled = false; // �ptal durumu s�f�rla

        // LeanTween animasyonu ba�lat
        LeanTween.scaleX(buildBarracksBar, 1, barracks.buildTime).setOnComplete(() => ResetProgressBar(buildBarracksBar));
        isBarracksBuildActive = true;

        string panelName = "BarracksBuildingProcessPanel";
        panelManager.CreatePanel(panelName, barracks.buildingName, barracks.buildTime, "Building");

        float elapsedTime = 0f; // Ge�en zaman� takip et

        while (elapsedTime < barracks.buildTime)
        {
            if (barracksPanelController.isBuildCanceled) // E�er iptal edilirse
            {
                LeanTween.cancel(buildBarracksBar); // Animasyonu iptal et
                ResetProgressBar(buildBarracksBar); // ProgressBar'� s�f�rla
                isBarracksBuildActive = false;
                onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
                yield break; // Coroutine sonland�r
            }

            elapsedTime += Time.deltaTime; // Ge�en s�reyi art�r
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // �ptal edilmeden tamamland�ysa
        isBarracksBuildActive = false;
        panelManager.DestroyPanel("BarracksBuildingProcessPanel");
        barracksPanelController.cancelBarracksButton.gameObject.SetActive(false);
        onCompletion(true); // Tamamland���nda ba�ar�l� olarak bildir
    }


    public IEnumerator HospitalIsFinished(Hospital hospital, System.Action<bool> onCompletion)
    {
        // �yile�tirme aktifse bina y�kseltme yap�lmas�n
        if (isHealActive)
        {
            Debug.Log("�yile�tirme s�ras�nda bina y�kseltemezsiniz, iyile�tirmeyi iptal edip yeniden deneyin.");
            onCompletion(false); // Ba�ar�s�zl�k durumu bildir
            yield break; // Coroutine sonland�r
        }

        // Yeni in�aata izin verilip verilmedi�ini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 in�aat ayn� anda aktif olabilir.");
            onCompletion(false); // Ba�ar�s�zl�k durumu bildir
            yield break; // Coroutine sonland�r
        }

        hospitalPanelController.cancelHospitalButton.gameObject.SetActive(true);
        hospitalPanelController.isBuildCanceled = false; // �ptal durumu s�f�rla

        // LeanTween animasyonu ba�lat
        LeanTween.scaleX(buildHospitalBar, 1, hospital.buildTime).setOnComplete(() => ResetProgressBar(buildHospitalBar));
        isHospitalBuildActive = true;

        string panelName = "HospitalBuildingProcessPanel";
        panelManager.CreatePanel(panelName, hospital.buildingName, hospital.buildTime, "Building");

        float elapsedTime = 0f; // Ge�en zaman� takip et

        while (elapsedTime < hospital.buildTime)
        {
            if (hospitalPanelController.isBuildCanceled) // E�er iptal edilirse
            {
                LeanTween.cancel(buildHospitalBar); // Animasyonu iptal et
                ResetProgressBar(buildHospitalBar); // ProgressBar'� s�f�rla
                isHospitalBuildActive = false;
                onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
                yield break; // Coroutine sonland�r
            }

            elapsedTime += Time.deltaTime; // Ge�en s�reyi art�r
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // �ptal edilmeden tamamland�ysa
        isHospitalBuildActive = false;
        panelManager.DestroyPanel("HospitalBuildingProcessPanel");
        hospitalPanelController.cancelHospitalButton.gameObject.SetActive(false);
        onCompletion(true); // Tamamland���nda ba�ar�l� olarak bildir
    }


    public IEnumerator CastleIsFinished(Castle castle, System.Action<bool> onCompletion)
    {
        // Yeni in�aata izin verilip verilmedi�ini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 in�aat ayn� anda aktif olabilir.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }

        castlePanelController.cancelUpgradeCastleButton.gameObject.SetActive(true);
        castlePanelController.isBuildCanceled = false; // �ptal durumu s�f�rla

        // LeanTween animasyonu ba�lat
        LeanTween.scaleX(upgradeCastleBar, 1, castle.buildTime).setOnComplete(() => ResetProgressBar(upgradeCastleBar));
        isCastleBuildingActive = true;


        string panelName = "CastleUpgradeProcessPanel";
        panelManager.CreatePanel(panelName, castle.buildingName, castle.buildTime, "Building");

        float elapsedTime = 0f; // Ge�en zaman� takip et

        while (elapsedTime < castle.buildTime)
        {
            if (castlePanelController.isBuildCanceled) // E�er iptal edilirse
            {
                LeanTween.cancel(upgradeCastleBar); // Animasyonu iptal et
                ResetProgressBar(upgradeCastleBar); // ProgressBar'� s�f�rla
                isCastleBuildingActive = false;
                onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
                yield break; // Coroutine sonland�r
            }

            elapsedTime += Time.deltaTime; // Ge�en s�reyi art�r
            yield return null; // Bir sonraki kareye kadar bekle
        }

        // �ptal edilmeden tamamland�ysa
        castlePanelController.cancelUpgradeCastleButton.gameObject.SetActive(false);
        isCastleBuildingActive = false;
        panelManager.DestroyPanel("CastleUpgradeProcessPanel");
        onCompletion(true); // Tamamland���nda ba�ar�l� olarak bildir
    }


    public IEnumerator TowerIsFinished(Tower tower, System.Action<bool> onCompletion)
    {

        if (isTowerBuildingActive)
        {
            Debug.Log("Halihaz�rda bir i�lem devam ederken yeni i�lem ger�ekle�tiremezsiniz.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 in�aat ayn� anda aktif olabilir.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }
      
            towerPanelController.cancelTowerButton.gameObject.SetActive(true);
            towerPanelController.isBuildCanceled = false; // �ptal durumu s�f�rla

            // LeanTween animasyonu ba�lat
            LeanTween.scaleX(buildTowerBar, 1, tower.buildTime).setOnComplete(() => ResetProgressBar(buildTowerBar));
            isTowerBuildingActive = true;

            string panelName = "TowerBuildingProcessPanel";  
            panelManager.CreatePanel(panelName, tower.buildingName, tower.buildTime, "Building");

            float elapsedTime = 0f; // Ge�en zaman� takip et

            while (elapsedTime < tower.buildTime)
            {
                if (towerPanelController.isBuildCanceled) // E�er iptal edilirse
                {
                    LeanTween.cancel(buildTowerBar); // Animasyonu iptal et
                    ResetProgressBar(buildTowerBar); // ProgressBar'� s�f�rla
                    isTowerBuildingActive = false;
                    onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
                    yield break; // Coroutine sonland�r
                }

                elapsedTime += Time.deltaTime; // Ge�en s�reyi art�r
                yield return null; // Bir sonraki kareye kadar bekle
            }

            // �ptal edilmeden tamamland�ysa
            isTowerBuildingActive = false;
        panelManager.DestroyPanel("TowerBuildingProcessPanel");
        towerPanelController.cancelTowerButton.gameObject.SetActive(false);
            onCompletion(true); // Tamamland���nda ba�ar�l� olarak bildir
        
    }

    public IEnumerator TrapIsFinished(Trap trap, System.Action<bool> onCompletion)
    {
        if (isAnyTrapActive)
        {
            Debug.Log("Halihaz�rda bir i�lem devam ederken yeni i�lem ger�ekle�tiremezsiniz.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }
        
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 in�aat ayn� anda aktif olabilir.");
            onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
            yield break; // Coroutine sonland�r
        }
       
            trapPanelController.cancelTrapButton.gameObject.SetActive(true);
            trapPanelController.isBuildCanceled = false; // �ptal durumu s�f�rla

            // LeanTween animasyonu ba�lat
            LeanTween.scaleX(buildTrapBar, 1, trap.buildTime).setOnComplete(() => ResetProgressBar(buildTrapBar));
            isAnyTrapActive = true;

            string panelName = "TrapBuildingProcessPanel";
            panelManager.CreatePanel(panelName, trap.buildingName, trap.buildTime, "Building");

        float elapsedTime = 0f; // Ge�en zaman� takip et

            while (elapsedTime < trap.buildTime)
            {
                if (trapPanelController.isBuildCanceled) // E�er iptal edilirse
                {
                    LeanTween.cancel(buildTrapBar); // Animasyonu iptal et
                    ResetProgressBar(buildTrapBar); // ProgressBar'� s�f�rla
                    isAnyTrapActive = false;
                    onCompletion(false); // Ba�ar�s�zl�k durumunu bildir
                    yield break; // Coroutine sonland�r
                }

                elapsedTime += Time.deltaTime; // Ge�en s�reyi art�r
                yield return null; // Bir sonraki kareye kadar bekle
            }

            // �ptal edilmeden tamamland�ysa
            isAnyTrapActive = false;
            panelManager.DestroyPanel("TrapBuildingProcessPanel");
            trapPanelController.cancelTrapButton.gameObject.SetActive(false);
            onCompletion(true); // Tamamland���nda ba�ar�l� olarak bildir
        
    }


}
