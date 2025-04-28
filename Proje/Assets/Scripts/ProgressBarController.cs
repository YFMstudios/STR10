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
    private int healedArcher;
    private int healedSoldeir;

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

    public float createdSoldierAmount = 0f;
    public float createdArcherAmount = 0f;

    private float totalAltin = 0, totalYemek = 0, totalDemir = 0, totalTas = 0, totalKereste = 0;

    [Header("ScriptableObject")]
    public GetPlayerData getPlayerData;
    public HealController healController;
    public KaynakYoneticisi kaynakYoneticisi;

    // Awake fonksiyonu (ProgressBarController sınıfına)
    private void Awake()
    {
        // Oyun başlatıldığında çağrılır
        Debug.Log("ProgressBarController Awake çağrıldı");

        // İnşaat durumunu sıfırla (oyun her açıldığında sıfırlamak için)
        isStonePitBuildingActive = false;
        isSawmillBuildingActive = false;
        isFarmBuildActive = false;
        isAnyTrapActive = false;
        isBlacksmithBuildingActive = false;
        isLabBuildActive = false;
        isBarracksBuildActive = false;
        isHospitalBuildActive = false;
        isCastleBuildingActive = false;
        isTowerBuildingActive = false;

        // 1 saniye sonra ilerleme çubuklarını bulmayı dene
        Invoke("FindProgressBars", 1.0f);
    }

    private void FindProgressBars()
    {
        // Stonepit
        buildStonepitBar = GameObject.Find("BuildStonepitBar");
        if (buildStonepitBar != null)
        {
            Debug.Log("Stonepit ilerleme çubuğu bulundu: " + buildStonepitBar.name);
            buildStonepitBar.transform.localScale = new Vector3(0, buildStonepitBar.transform.localScale.y, buildStonepitBar.transform.localScale.z);
        }

        // Sawmill
        buildSawmillBar = GameObject.Find("BuildSawmillBar");
        if (buildSawmillBar != null)
        {
            Debug.Log("Sawmill ilerleme çubuğu bulundu: " + buildSawmillBar.name);
            buildSawmillBar.transform.localScale = new Vector3(0, buildSawmillBar.transform.localScale.y, buildSawmillBar.transform.localScale.z);
        }

        // Warehouse
        buildWareHouseBar = GameObject.Find("BuildWareHouseBar");
        if (buildWareHouseBar != null)
        {
            Debug.Log("Warehouse ilerleme çubuğu bulundu: " + buildWareHouseBar.name);
            buildWareHouseBar.transform.localScale = new Vector3(0, buildWareHouseBar.transform.localScale.y, buildWareHouseBar.transform.localScale.z);
        }

        // Farm
        buildFarmBar = GameObject.Find("BuildFarmBar");
        if (buildFarmBar != null)
        {
            Debug.Log("Farm ilerleme çubuğu bulundu: " + buildFarmBar.name);
            buildFarmBar.transform.localScale = new Vector3(0, buildFarmBar.transform.localScale.y, buildFarmBar.transform.localScale.z);
        }
        buildBlacksmithBar = GameObject.Find("BuildBlacksmithBar");
        if (buildBlacksmithBar != null)
        {
            Debug.Log("Blacksmith ilerleme çubuğu bulundu: " + buildBlacksmithBar.name);
            buildBlacksmithBar.transform.localScale = new Vector3(0, buildBlacksmithBar.transform.localScale.y, buildBlacksmithBar.transform.localScale.z);
        }
        buildLabBar = GameObject.Find("BuildLabBar");
        if (buildLabBar != null)
        {
            Debug.Log("Lab ilerleme çubuğu bulundu: " + buildLabBar.name);
            buildLabBar.transform.localScale = new Vector3(0, buildBlacksmithBar.transform.localScale.y, buildBlacksmithBar.transform.localScale.z);
        }
        buildBarracksBar = GameObject.Find("BuildBarracksBar");
        if (buildBarracksBar != null)
        {
            Debug.Log("Barracks ilerleme çubuğu bulundu: " + buildBarracksBar.name);
            buildBarracksBar.transform.localScale = new Vector3(0, buildBarracksBar.transform.localScale.y, buildBarracksBar.transform.localScale.z);
        }
        buildHospitalBar = GameObject.Find("BuildHospitalBar");
        if (buildHospitalBar != null)
        {
            Debug.Log("Hospital ilerleme çubuğu bulundu: " + buildHospitalBar.name);
            buildHospitalBar.transform.localScale = new Vector3(0, buildHospitalBar.transform.localScale.y, buildHospitalBar.transform.localScale.z);
        }
        upgradeCastleBar = GameObject.Find("UpgradeCastleBar");
        if (upgradeCastleBar != null)
        {
            Debug.Log("Castle ilerleme çubuğu bulundu: " + upgradeCastleBar.name);
            upgradeCastleBar.transform.localScale = new Vector3(0, upgradeCastleBar.transform.localScale.y, upgradeCastleBar.transform.localScale.z);
        }
        buildTowerBar = GameObject.Find("BuildTowerBar");
        if (buildTowerBar != null)
        {
            Debug.Log("Tower ilerleme çubuğu bulundu: " + buildTowerBar.name);
            buildTowerBar.transform.localScale = new Vector3(0, buildTowerBar.transform.localScale.y, buildTowerBar.transform.localScale.z);
        }

        buildTrapBar = GameObject.Find("BuildTrapBar");
        if (buildTrapBar != null)
        {
            Debug.Log("Trap ilerleme çubuğu bulundu: " + buildTrapBar.name);
            buildTrapBar.transform.localScale = new Vector3(0, buildTrapBar.transform.localScale.y, buildTrapBar.transform.localScale.z);
        }
    }




    void Start()
    {
        // Ba lang  ta zaman s f rlanabilir.
        time = 0;

        buttonText = createUnitButton.GetComponentInChildren<TextMeshProUGUI>();
        healButtonText = healButton.GetComponentInChildren<TextMeshProUGUI>();



    }

    // Update fonksiyonu ekleyelim (ProgressBarController sınıfına)
    private void Update()
    {
        if (isStonePitBuildingActive && buildStonepitBar != null)
        {
            if (!buildStonepitBar.activeInHierarchy)
            {
                Debug.LogWarning("StonePit ilerleme çubuğu aktif değil! Aktif hale getiriliyor.");
                buildStonepitBar.SetActive(true);
            }
        }

        if (isSawmillBuildingActive && buildSawmillBar != null)
        {
            if (!buildSawmillBar.activeInHierarchy)
            {
                Debug.LogWarning("Sawmill ilerleme çubuğu aktif değil! Aktif hale getiriliyor.");
                buildSawmillBar.SetActive(true);
            }
        }

        if (isWareHouseBuildingActive && buildWareHouseBar != null)
        {
            if (!buildWareHouseBar.activeInHierarchy)
            {
                Debug.LogWarning("Warehouse ilerleme çubuğu aktif değil! Aktif hale getiriliyor.");
                buildWareHouseBar.SetActive(true);
            }
        }

        if (isFarmBuildActive && buildFarmBar != null)
        {
            if (!buildFarmBar.activeInHierarchy)
            {
                Debug.LogWarning("Farm ilerleme çubuğu aktif değil! Aktif hale getiriliyor.");
                buildFarmBar.SetActive(true);
            }
        }
        if (isBlacksmithBuildingActive && buildBlacksmithBar != null)
        {
            if (!buildBlacksmithBar.activeInHierarchy)
            {
                Debug.LogWarning("Blacksmith ilerleme çubuğu aktif değil! Aktif hale getiriliyor.");
                buildBlacksmithBar.SetActive(true);
            }
        }
        if (isLabBuildActive && buildLabBar != null)
        {
            if (!buildLabBar.activeInHierarchy)
            {
                Debug.LogWarning("Lab ilerleme çubuğu aktif değil! Aktif hale getiriliyor.");
                buildLabBar.SetActive(true);
            }
        }
        if (isBarracksBuildActive && buildBarracksBar != null)
        {
            if (!buildBarracksBar.activeInHierarchy)
            {
                Debug.LogWarning("Barracks ilerleme çubuğu aktif değil! Aktif hale getiriliyor.");
                buildBarracksBar.SetActive(true);
            }
        }
        if (isHospitalBuildActive && buildHospitalBar != null)
        {
            if (!buildHospitalBar.activeInHierarchy)
            {
                Debug.LogWarning("Hospital ilerleme çubuğu aktif değil! Aktif hale getiriliyor.");
                buildHospitalBar.SetActive(true);
            }
        }
        if (isCastleBuildingActive && upgradeCastleBar != null)
        {
            if (!upgradeCastleBar.activeInHierarchy)
            {
                Debug.LogWarning("Castle ilerleme çubuğu aktif değil! Aktif hale getiriliyor.");
                upgradeCastleBar.SetActive(true);
            }
        }
        if (isTowerBuildingActive && buildTowerBar != null)
        {
            if (!buildTowerBar.activeInHierarchy)
            {
                Debug.LogWarning("Tower ilerleme çubuğu aktif değil! Aktif hale getiriliyor.");
                buildTowerBar.SetActive(true);
            }
        }
        if (isAnyTrapActive && buildTrapBar != null)
        {
            if (!buildTrapBar.activeInHierarchy)
            {
                Debug.LogWarning("Trap ilerleme çubuğu aktif değil! Aktif hale getiriliyor.");
                buildTrapBar.SetActive(true);
            }
        }
    }



    public void CreateUnits()
    {
        if (!isBarracksBuildActive)
        {
            if (Barracks.wasBarracksCreated == true)
            {
                float totalTime = 0;
                totalUnitAmount = 0;

                if (slider.savasciSlider.value > 0)
                {
                    float savasciTime = slider.savasciSlider.value * savasciCreationTime;
                    totalTime += savasciTime;
                    totalUnitAmount += slider.savasciSlider.value;
                }

                if (slider.okcuSlider.value > 0)
                {
                    float okcuTime = slider.okcuSlider.value * okcuCreationTime;
                    totalTime += okcuTime;
                    totalUnitAmount += slider.okcuSlider.value;
                }

                if (totalTime > 0)
                {
                    if (isUnitCreationActive)
                    {
                        buttonText.text = "Eğit";
                        slider.okcuSlider.value = 0f;
                        slider.savasciSlider.value = 0f;

                        giveCostBack(slider.savasciSlider.value, slider.okcuSlider.value);

                        Debug.Log("Savaşçı Sayısı :" + createdSoldierAmount);
                        Debug.Log("Okçu Sayısı : " + createdArcherAmount);

                        StopCoroutine("CreateUnitsCoroutine");
                        panelManager.DestroyPanel("SoldierCreation");
                        isUnitCreationActive = false;

                        ResetProgressBar(progressBar);
                        totalUnitAmount = 0;
                    }
                    else
                    {
                        isUnitCreationActive = true;
                        buttonText.text = "İptal Et";
                        reduceCost(slider.savasciSlider.value, slider.okcuSlider.value);
                        StartCoroutine(CreateUnitsCoroutine(totalTime));
                        panelManager.CreatePanel("SoldierCreation", totalUnitAmount.ToString(), totalTime, "SoldierCreation");
                    }
                }
            }
            else
            {
                Debug.Log("Öncelikle bir kışla üretmelisiniz.");
            }
        }
        else
        {
            Debug.Log("Bina Yükseltmesi Sırasında Asker Eğitemezsin");
        }
    }

    private IEnumerator CreateUnitsCoroutine(float totalTime)
    {
        float elapsedTime = 0f;
        float nextLogTime = 0.5f;

        progressBar.transform.localScale = new Vector3(0f, progressBar.transform.localScale.y, progressBar.transform.localScale.z);

        while (elapsedTime < totalTime)
        {
            if (!isUnitCreationActive)
            {
                progressBar.transform.localScale = new Vector3(0f, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
                yield break;
            }

            elapsedTime += Time.deltaTime;

            if (progressBar != null)
            {
                float progress = elapsedTime / totalTime;
                progressBar.transform.localScale = new Vector3(progress, progressBar.transform.localScale.y, progressBar.transform.localScale.z);

                if (elapsedTime > nextLogTime)
                {
                    Debug.Log("Asker üretim ilerleme: %" + (progress * 100f).ToString("F1"));
                    nextLogTime += 0.5f;
                }
            }

            yield return null;
        }

        // Tamamlandığında
        buttonText.text = "Üret";
        OnProgressComplete();
        createdArcherAmount += slider.okcuSlider.value;
        createdSoldierAmount += slider.savasciSlider.value;
        
        getPlayerData.UpdateSoldierAmount(createdSoldierAmount, createdArcherAmount);
        kaynakYoneticisi.WarPowerArttirma((int)((createdSoldierAmount * 50) + (createdArcherAmount * 25)));
        barracksPanelController.refreshBarracks();

        slider.okcuSlider.value = 0f;
        slider.savasciSlider.value = 0f;

        Debug.Log("Savaşçı Sayısı :" + createdSoldierAmount);
        Debug.Log("Okçu Sayısı : " + createdArcherAmount);
        createdSoldierAmount = 0;
        createdArcherAmount = 0;
        ResetProgressBar(progressBar);
        isUnitCreationActive = false;
        panelManager.DestroyPanel("SoldierCreation");

        yield return new WaitForSeconds(0.2f);
        if (progressBar != null)
        {
            progressBar.transform.localScale = new Vector3(0f, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
        }
    }


    void reduceCost(float savasciCount, float okcuCount) // Maliyetleri kaynaklardan d  en fonksiyon.
    {

        totalAltin = ((int)savasciCount * 5) + ((int)okcuCount * 7);
        totalYemek = ((int)savasciCount * 5) + ((int)okcuCount * 6);
        totalDemir = ((int)savasciCount * 5) + ((int)okcuCount * 3);
        totalTas = ((int)savasciCount * 5) + ((int)okcuCount * 2);
        totalKereste = ((int)savasciCount * 5) + ((int)okcuCount * 10);

        KaynakYoneticisi.GoldAmount -= (int)totalAltin;
        KaynakYoneticisi.FoodAmount -= (int)totalYemek;
        KaynakYoneticisi.IronAmount -= (int)totalDemir;
        KaynakYoneticisi.StoneAmount -= (int)totalTas;
        KaynakYoneticisi.WoodAmount -= (int)totalKereste;
    }

    void giveCostBack(float savasciCount, float okcuCount)
    {

        totalAltin = ((int)savasciCount * 5) + ((int)okcuCount * 7);
        totalYemek = ((int)savasciCount * 5) + ((int)okcuCount * 6);
        totalDemir = ((int)savasciCount * 5) + ((int)okcuCount * 3);
        totalTas = ((int)savasciCount * 5) + ((int)okcuCount * 2);
        totalKereste = ((int)savasciCount * 5) + ((int)okcuCount * 10);

        KaynakYoneticisi.GoldAmount += (int)totalAltin;
        KaynakYoneticisi.FoodAmount += (int)totalYemek;
        KaynakYoneticisi.IronAmount += (int)totalDemir;
        KaynakYoneticisi.StoneAmount += (int)totalTas;
        KaynakYoneticisi.WoodAmount += (int)totalKereste;
    }
    void ResetProgressBar(GameObject gameObject)
    {
        Debug.Log("ResetProgressBar çağrıldı: " + gameObject.name + " için");
        Debug.Log("Sıfırlamadan önce - Ölçek: " + gameObject.transform.localScale);

        gameObject.transform.localScale = new Vector3(0, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

        Debug.Log("Sıfırlamadan sonra - Ölçek: " + gameObject.transform.localScale);
    }
    void OnProgressComplete()
    {
        // Burada progress bar doldu unda yap lacak i lemleri tan mla
        Debug.Log("Progress Bar doldu, islem gerceklestiriliyor!");
        Kingdom.myKingdom.SoldierAmount += totalUnitAmount;
        totalUnitAmount = 0;
    }


    public void HealUnits()
    {
        if (!isHospitalBuildActive)
        {
            if (Hospital.wasHospitalCreated == true)
            {
                // Input field değerlerini doğrudan çek
                int inputSavasciCount = 0;
                int inputOkcuCount = 0;

                // Input field değerlerini int'e çevir
                if (!string.IsNullOrEmpty(hastaneSlider.savasciInputField.text))
                    int.TryParse(hastaneSlider.savasciInputField.text, out inputSavasciCount);

                if (!string.IsNullOrEmpty(hastaneSlider.okcuInputField.text))
                    int.TryParse(hastaneSlider.okcuInputField.text, out inputOkcuCount);

                // Yaralı asker ve okçu sayılarını kontrol et
                bool validInput = true;
                healedArcher = inputOkcuCount;
                healedSoldeir = inputSavasciCount;

                if (inputSavasciCount > healController.woundedSoldier)
                {
                    Debug.Log("Hata: Yaralı asker sayısından fazla değer girdiniz!");
                    validInput = false;
                }

                if (inputOkcuCount > healController.woundedArcher)
                {
                    Debug.Log("Hata: Yaralı okçu sayısından fazla değer girdiniz!");
                    validInput = false;
                }

                // Sadece geçerli değerler girilmişse devam et
                if (validInput)
                {
                    float totalHealTime = 0;
                    int totalHealedUnitAmount = 0;

                    if (inputSavasciCount > 0)
                    {
                        totalHealTime += inputSavasciCount * savasciHealTime;
                        totalHealedUnitAmount += inputSavasciCount;
                    }

                    if (inputOkcuCount > 0)
                    {
                        totalHealTime += inputOkcuCount * okcuHealTime;
                        totalHealedUnitAmount += inputOkcuCount;
                    }

                    if (totalHealTime > 0)
                    {
                        Debug.Log("Toplam iyileştirme süresi: " + totalHealTime);
                        if (isHealActive)
                        {
                            healButtonText.text = "İyileştir";
                            giveCostBack(inputSavasciCount, inputOkcuCount);
                            StopCoroutine("HealUnitsCoroutine");
                            panelManager.DestroyPanel("HealSoldier");
                            isHealActive = false;
                            ResetProgressBar(healProgressBar);
                        }
                        else
                        {
                            isHealActive = true;
                            healButtonText.text = "İptal Et";
                            reduceCost(inputSavasciCount, inputOkcuCount);
                            StartCoroutine(HealUnitsCoroutine(totalHealTime));
                            panelManager.CreatePanel("HealSoldier", totalHealedUnitAmount.ToString(), totalHealTime, "HealSoldier");
                        }
                    }
                }
                else
                {
                    // Geçersiz değer girildiyse genel bir uyarı mesajı
                    Debug.Log("Lütfen yaralı asker ve okçu sayısını aşmayacak değerler giriniz.");
                }
            }
            else
            {
                Debug.Log("Öncelikle hastane inşa etmelisiniz.");
            }
        }
        else
        {
            Debug.Log("İnşa sırasında birlik iyileştiremezsin");
        }
    }

    private IEnumerator HealUnitsCoroutine(float totalTime)
    {
        float elapsedTime = 0f;
        float nextLogTime = 0.5f;

        if (healProgressBar != null)
        {
            healProgressBar.transform.localScale = new Vector3(0f, healProgressBar.transform.localScale.y, healProgressBar.transform.localScale.z);
        }

        while (elapsedTime < totalTime)
        {
            if (!isHealActive)
            {
                if (healProgressBar != null)
                {
                    healProgressBar.transform.localScale = new Vector3(0f, healProgressBar.transform.localScale.y, healProgressBar.transform.localScale.z);
                }
                yield break;
            }

            elapsedTime += Time.deltaTime;

            if (healProgressBar != null)
            {
                float progress = elapsedTime / totalTime;
                healProgressBar.transform.localScale = new Vector3(progress, healProgressBar.transform.localScale.y, healProgressBar.transform.localScale.z);

                if (elapsedTime > nextLogTime)
                {
                    Debug.Log("İyileştirme ilerleme: %" + (progress * 100f).ToString("F1"));
                    nextLogTime += 0.5f;
                }
            }

            yield return null;
        }

        healButtonText.text = "İyileştir";
        OnProgressComplete();
        ResetProgressBar(healProgressBar);
        isHealActive = false;
        panelManager.DestroyPanel("HealSoldier");
        getPlayerData.UpdateSoldierAmount(healedSoldeir, healedArcher);
        kaynakYoneticisi.WarPowerArttirma((int)((healedSoldeir * 50) + (healedArcher * 25)));
        barracksPanelController.refreshBarracks();
        yield return new WaitForSeconds(0.2f);
        if (healProgressBar != null)
        {
            healProgressBar.transform.localScale = new Vector3(0f, healProgressBar.transform.localScale.y, healProgressBar.transform.localScale.z);
        }
    }



    public IEnumerator WarehouseIsFinished(Warehouse warehouse, System.Action<bool> onCompletion)
    {
        Debug.Log("WarehouseIsFinished başlatıldı. İnşa süresi: " + warehouse.buildTime);

        // İlerleme çubuğunu sahnede bul (eğer daha önce atanmadıysa)
        if (buildWareHouseBar == null)
        {
            buildWareHouseBar = GameObject.Find("BuildWareHouseBar");
            if (buildWareHouseBar != null)
            {
                Debug.Log("buildWareHouseBar bulundu: " + buildWareHouseBar.name);
            }
            else
            {
                Debug.LogError("BuildWareHouseBar objesi bulunamadı!");
            }
        }

        // Yeni inşaata izin verilip verilmediğini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inşaat aynı anda aktif olabilir.");
            onCompletion(false);
            yield break;
        }

        string panelName = "WareHouseBuildingProcessPanel";
        wareHousePanelController.cancelWarehouseButton.gameObject.SetActive(true);
        wareHousePanelController.isBuildCanceled = false;

        // İlerleme çubuğunu sıfırla
        if (buildWareHouseBar != null)
        {
            buildWareHouseBar.transform.localScale = new Vector3(0, buildWareHouseBar.transform.localScale.y, buildWareHouseBar.transform.localScale.z);
        }

        panelManager.CreatePanel(panelName, warehouse.buildingName, warehouse.buildTime, "Building");
        Debug.Log("Panel oluşturuldu: " + panelName);

        isWareHouseBuildingActive = true;
        float elapsedTime = 0f;
        float nextLogTime = 0.5f;

        while (elapsedTime < warehouse.buildTime)
        {
            if (wareHousePanelController.isBuildCanceled)
            {
                Debug.Log("Depo inşaatı iptal edildi.");
                if (buildWareHouseBar != null)
                {
                    buildWareHouseBar.transform.localScale = new Vector3(0, buildWareHouseBar.transform.localScale.y, buildWareHouseBar.transform.localScale.z);
                }

                onCompletion(false);
                isWareHouseBuildingActive = false;
                yield break;
            }

            elapsedTime += Time.deltaTime;

            // İlerleme çubuğunu güncelle
            if (buildWareHouseBar != null)
            {
                float progress = elapsedTime / warehouse.buildTime;
                buildWareHouseBar.transform.localScale = new Vector3(progress, buildWareHouseBar.transform.localScale.y, buildWareHouseBar.transform.localScale.z);

                if (elapsedTime > nextLogTime)
                {
                    Debug.Log("Warehouse progress: %" + (progress * 100f).ToString("F1"));
                    nextLogTime += 0.5f;
                }
            }

            yield return null;
        }

        // Tamamlandığında
        Debug.Log("Depo inşaatı başarıyla tamamlandı.");

        if (buildWareHouseBar != null)
        {
            buildWareHouseBar.transform.localScale = new Vector3(1f, buildWareHouseBar.transform.localScale.y, buildWareHouseBar.transform.localScale.z);
        }

        wareHousePanelController.cancelWarehouseButton.gameObject.SetActive(false);
        isWareHouseBuildingActive = false;
        panelManager.DestroyPanel(panelName);

        onCompletion(true);

        // Kısa bir gecikmeden sonra sıfırla
        yield return new WaitForSeconds(0.2f);
        if (buildWareHouseBar != null)
        {
            buildWareHouseBar.transform.localScale = new Vector3(0f, buildWareHouseBar.transform.localScale.y, buildWareHouseBar.transform.localScale.z);
            Debug.Log("Depo inşaatı tamamlandıktan sonra çubuk sıfırlandı.");
        }
    }



    // ProgressBarController.cs dosyasında yapılacak değişiklikler
    // Sadece ProgressBarController.cs dosyasında StonePitIsFinished fonksiyonunda değişiklik yapacağız
    // Diğer kodlara dokunmuyoruz

    public IEnumerator StonePitIsFinished(StonePit stonepit, System.Action<bool> onCompletion)
    {
        Debug.Log("StonePitIsFinished başlatıldı. İnşa süresi: " + stonepit.buildTime);

        // İlerleme çubuğunu yeniden bul (referansı güncelle)
        buildStonepitBar = GameObject.Find("BuildStonepitBar"); // İlerleme çubuğunuzun tam adını buraya yazın

        if (buildStonepitBar == null)
        {
            Debug.LogError("BuildStonepitBar objesi bulunamadı! İlerleme çubuğu çalışmayacak.");

            // Alternatif olarak canvas'ı tarayarak bulmayı deneyelim
            Canvas[] canvaslar = FindObjectsOfType<Canvas>();
            foreach (Canvas c in canvaslar)
            {
                Transform bar = c.transform.Find("BuildStonepitBar"); // Doğru hiyerarşi yolunu yazın
                if (bar != null)
                {
                    buildStonepitBar = bar.gameObject;
                    Debug.Log("BuildStonepitBar bulundu: " + buildStonepitBar.name);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("BuildStonepitBar başarıyla bulundu: " + buildStonepitBar.name);
        }

        // Yeni inşaata izin verilip verilmediğini kontrol et
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inşaat aynı anda aktif olabilir.");
            onCompletion(false); // Başarısızlık durumunu bildir
            yield break; // Coroutine sonlandır
        }

        Debug.Log("İnşaata izin verildi - StonePit inşaatına devam ediliyor");

        string panelName = "StonepitBuildingProcessPanel";
        stonepitPanelController.cancelStonepitButton.gameObject.SetActive(true);
        stonepitPanelController.isBuildCanceled = false; // İptal durumu sıfırla

        // Önce ilerleme çubuğunu sıfırla
        if (buildStonepitBar != null)
        {
            buildStonepitBar.transform.localScale = new Vector3(0, buildStonepitBar.transform.localScale.y, buildStonepitBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu sıfırlandı: " + buildStonepitBar.transform.localScale);
        }

        panelManager.CreatePanel(panelName, stonepit.buildingName, stonepit.buildTime, "Building");
        Debug.Log("Panel oluşturuldu: " + panelName);

        isStonePitBuildingActive = true;
        float elapsedTime = 0f; // Geçen zamanı takip et

        // Her karedeki ilerleme çubuğu durumunu loglayalım
        float nextLogTime = 0.5f;

        while (elapsedTime < stonepit.buildTime)
        {
            if (stonepitPanelController.isBuildCanceled) // Eğer iptal edilirse
            {
                Debug.Log("İnşaat kullanıcı tarafından iptal edildi.");
                // İlerleme çubuğunu sıfırla
                if (buildStonepitBar != null)
                {
                    buildStonepitBar.transform.localScale = new Vector3(0, buildStonepitBar.transform.localScale.y, buildStonepitBar.transform.localScale.z);
                }
                onCompletion(false); // Başarısızlık durumunu bildir
                isStonePitBuildingActive = false;
                yield break; // Coroutine sonlandır
            }

            elapsedTime += Time.deltaTime; // Geçen süreyi artır

            // İlerleme çubuğunu manuel olarak güncelle
            if (buildStonepitBar != null)
            {
                float progress = elapsedTime / stonepit.buildTime;
                // X ekseni boyunca scale'i güncelle, diğer eksenler aynı kalsın
                buildStonepitBar.transform.localScale = new Vector3(
                    progress,
                    buildStonepitBar.transform.localScale.y,
                    buildStonepitBar.transform.localScale.z
                );

                // Progress bar'ın mevcut durumunu belirli aralıklarla logla
                if (elapsedTime > nextLogTime)
                {
                    Debug.Log("İlerleme çubuğu durumu: Scale=" + buildStonepitBar.transform.localScale +
                             ", Aktif=" + buildStonepitBar.activeInHierarchy +
                             ", İlerleme=%" + (progress * 100).ToString("F1"));

                    nextLogTime += 0.5f; // Her 0.5 saniyede bir logla
                }
            }

            yield return null; // Bir sonraki kareye kadar bekle
        }

        // İnşaat tamamlandığında
        Debug.Log("İnşaat başarıyla tamamlandı");

        // İlerleme çubuğunun tam dolu olduğundan emin ol
        if (buildStonepitBar != null)
        {
            buildStonepitBar.transform.localScale = new Vector3(1f, buildStonepitBar.transform.localScale.y, buildStonepitBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu tam dolu duruma getirildi: " + buildStonepitBar.transform.localScale);
        }

        stonepitPanelController.cancelStonepitButton.gameObject.SetActive(false);
        isStonePitBuildingActive = false;

        // Panel'i yok et
        panelManager.DestroyPanel("StonepitBuildingProcessPanel");
        Debug.Log("Panel yok edildi ve durum sıfırlandı");

        // Callback'i çağırarak inşaatın tamamlandığını bildir
        onCompletion(true);

        // İnşaat tamamlandıktan sonra yarım saniye bekle ve ilerleme çubuğunu sıfırla


        // İnşaat tamamlandıktan sonra ilerleme çubuğunu sıfırla
        if (buildStonepitBar != null)
        {
            buildStonepitBar.transform.localScale = new Vector3(0f, buildStonepitBar.transform.localScale.y, buildStonepitBar.transform.localScale.z);
            Debug.Log("İnşaat tamamlandıktan sonra ilerleme çubuğu sıfırlandı");
        }
    }




    public IEnumerator SawmillIsFinished(Sawmill sawmill, System.Action<bool> onCompletion)
    {
        Debug.Log("SawmillIsFinished başlatıldı. İnşa süresi: " + sawmill.buildTime);

        // İlerleme çubuğunu bul
        buildSawmillBar = GameObject.Find("BuildSawmillBar");

        if (buildSawmillBar == null)
        {
            Debug.LogError("BuildSawmillBar objesi bulunamadı! İlerleme çubuğu çalışmayacak.");

            // Alternatif olarak canvas'ları tara
            Canvas[] canvaslar = FindObjectsOfType<Canvas>();
            foreach (Canvas c in canvaslar)
            {
                Transform bar = c.transform.Find("BuildSawmillBar");
                if (bar != null)
                {
                    buildSawmillBar = bar.gameObject;
                    Debug.Log("BuildSawmillBar bulundu: " + buildSawmillBar.name);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("BuildSawmillBar başarıyla bulundu: " + buildSawmillBar.name);
        }

        // Yeni inşaata izin kontrolü
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inşaat aynı anda aktif olabilir.");
            onCompletion(false);
            yield break;
        }

        Debug.Log("İnşaata izin verildi - Sawmill inşaatına devam ediliyor");

        string panelName = "SawmillBuildingProcessPanel";
        sawmillPanelController.cancelSawmillButton.gameObject.SetActive(true);
        sawmillPanelController.isBuildCanceled = false;

        // Progress bar sıfırla
        if (buildSawmillBar != null)
        {
            buildSawmillBar.transform.localScale = new Vector3(0, buildSawmillBar.transform.localScale.y, buildSawmillBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu sıfırlandı: " + buildSawmillBar.transform.localScale);
        }

        panelManager.CreatePanel(panelName, sawmill.buildingName, sawmill.buildTime, "Building");

        isSawmillBuildingActive = true;
        float elapsedTime = 0f;
        float nextLogTime = 0.5f;

        while (elapsedTime < sawmill.buildTime)
        {
            if (sawmillPanelController.isBuildCanceled)
            {
                Debug.Log("İnşaat kullanıcı tarafından iptal edildi.");
                if (buildSawmillBar != null)
                {
                    buildSawmillBar.transform.localScale = new Vector3(0, buildSawmillBar.transform.localScale.y, buildSawmillBar.transform.localScale.z);
                }
                onCompletion(false);
                isSawmillBuildingActive = false;
                yield break;
            }

            elapsedTime += Time.deltaTime;

            if (buildSawmillBar != null)
            {
                float progress = elapsedTime / sawmill.buildTime;
                buildSawmillBar.transform.localScale = new Vector3(
                    progress,
                    buildSawmillBar.transform.localScale.y,
                    buildSawmillBar.transform.localScale.z
                );

                if (elapsedTime > nextLogTime)
                {
                    Debug.Log("İlerleme çubuğu durumu: Scale=" + buildSawmillBar.transform.localScale +
                             ", Aktif=" + buildSawmillBar.activeInHierarchy +
                             ", İlerleme=%" + (progress * 100).ToString("F1"));
                    nextLogTime += 0.5f;
                }
            }

            yield return null;
        }

        Debug.Log("İnşaat başarıyla tamamlandı");

        if (buildSawmillBar != null)
        {
            buildSawmillBar.transform.localScale = new Vector3(1f, buildSawmillBar.transform.localScale.y, buildSawmillBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu tam dolu duruma getirildi: " + buildSawmillBar.transform.localScale);
        }

        sawmillPanelController.cancelSawmillButton.gameObject.SetActive(false);
        isSawmillBuildingActive = false;

        panelManager.DestroyPanel("SawmillBuildingProcessPanel");
        sawmillPanelController.refreshSawmill();
        onCompletion(true);

        if (buildSawmillBar != null)
        {
            buildSawmillBar.transform.localScale = new Vector3(0f, buildSawmillBar.transform.localScale.y, buildSawmillBar.transform.localScale.z);
            Debug.Log("İnşaat tamamlandıktan sonra ilerleme çubuğu sıfırlandı");
        }
    }




    public IEnumerator FarmIsFinished(Farm farm, System.Action<bool> onCompletion)
    {
        Debug.Log("FarmIsFinished başlatıldı. İnşa süresi: " + farm.buildTime);

        // İlerleme çubuğunu sahnede bul
        if (buildFarmBar == null)
        {
            buildFarmBar = GameObject.Find("BuildFarmBar");
            if (buildFarmBar != null)
            {
                Debug.Log("buildFarmBar bulundu: " + buildFarmBar.name);
            }
            else
            {
                Debug.LogError("BuildFarmBar objesi bulunamadı!");
            }
        }

        // Yeni inşaata izin kontrolü
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inşaat aynı anda aktif olabilir.");
            onCompletion(false);
            yield break;
        }

        string panelName = "FarmBuildingProcessPanel";
        farmPanelController.cancelFarmButton.gameObject.SetActive(true);
        farmPanelController.isBuildCanceled = false;

        // ProgressBar’ı sıfırla
        if (buildFarmBar != null)
        {
            buildFarmBar.transform.localScale = new Vector3(0, buildFarmBar.transform.localScale.y, buildFarmBar.transform.localScale.z);
        }

        panelManager.CreatePanel(panelName, farm.buildingName, farm.buildTime, "Building");
        isFarmBuildActive = true;

        float elapsedTime = 0f;
        float nextLogTime = 0.5f;

        while (elapsedTime < farm.buildTime)
        {
            if (farmPanelController.isBuildCanceled)
            {
                Debug.Log("Farm inşaatı kullanıcı tarafından iptal edildi.");
                if (buildFarmBar != null)
                {
                    buildFarmBar.transform.localScale = new Vector3(0, buildFarmBar.transform.localScale.y, buildFarmBar.transform.localScale.z);
                }

                onCompletion(false);
                isFarmBuildActive = false;
                yield break;
            }

            elapsedTime += Time.deltaTime;

            // İlerleme çubuğunu güncelle
            if (buildFarmBar != null)
            {
                float progress = elapsedTime / farm.buildTime;
                buildFarmBar.transform.localScale = new Vector3(progress, buildFarmBar.transform.localScale.y, buildFarmBar.transform.localScale.z);

                if (elapsedTime > nextLogTime)
                {
                    Debug.Log("Farm ilerleme: %" + (progress * 100f).ToString("F1"));
                    nextLogTime += 0.5f;
                }
            }

            yield return null;
        }

        // Tamamlandığında
        Debug.Log("Farm inşaatı başarıyla tamamlandı.");

        if (buildFarmBar != null)
        {
            buildFarmBar.transform.localScale = new Vector3(1f, buildFarmBar.transform.localScale.y, buildFarmBar.transform.localScale.z);
        }

        farmPanelController.cancelFarmButton.gameObject.SetActive(false);
        isFarmBuildActive = false;
        panelManager.DestroyPanel(panelName);
        farmPanelController.refreshFarm();

        onCompletion(true);

        // Scale sıfırla (görsel temizliği için)
        yield return new WaitForSeconds(0.2f);
        if (buildFarmBar != null)
        {
            buildFarmBar.transform.localScale = new Vector3(0f, buildFarmBar.transform.localScale.y, buildFarmBar.transform.localScale.z);
            Debug.Log("Farm inşaatı tamamlandıktan sonra çubuk sıfırlandı.");
        }
    }




    public IEnumerator BlacksmithIsFinished(Blacksmith blacksmith, System.Action<bool> onCompletion)
    {
        Debug.Log("BlacksmithIsFinished başlatıldı. İnşa süresi: " + blacksmith.buildTime);

        // İlerleme çubuğunu bul
        buildBlacksmithBar = GameObject.Find("BuildBlacksmithBar");

        if (buildBlacksmithBar == null)
        {
            Debug.LogError("BuildBlacksmithBar objesi bulunamadı! İlerleme çubuğu çalışmayacak.");

            // Alternatif olarak canvas'ları tara
            Canvas[] canvaslar = FindObjectsOfType<Canvas>();
            foreach (Canvas c in canvaslar)
            {
                Transform bar = c.transform.Find("BuildBlacksmithBar");
                if (bar != null)
                {
                    buildBlacksmithBar = bar.gameObject;
                    Debug.Log("BuildBlacksmithBar bulundu: " + buildBlacksmithBar.name);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("BuildBlacksmithBar başarıyla bulundu: " + buildBlacksmithBar.name);
        }

        // Yeni inşaata izin kontrolü
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inşaat aynı anda aktif olabilir.");
            onCompletion(false);
            yield break;
        }

        Debug.Log("İnşaata izin verildi - Blacksmith inşaatına devam ediliyor");

        string panelName = "BlacksmithBuildingProcessPanel";
        blacksmithPanelController.cancelBlacksmithButton.gameObject.SetActive(true);
        blacksmithPanelController.isBuildCanceled = false;

        // Progress bar sıfırla
        if (buildBlacksmithBar != null)
        {
            buildBlacksmithBar.transform.localScale = new Vector3(0, buildBlacksmithBar.transform.localScale.y, buildBlacksmithBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu sıfırlandı: " + buildBlacksmithBar.transform.localScale);
        }

        panelManager.CreatePanel(panelName, blacksmith.buildingName, blacksmith.buildTime, "Building");

        isBlacksmithBuildingActive = true;
        float elapsedTime = 0f;
        float nextLogTime = 0.5f;
        Time.timeScale = 1;

        while (elapsedTime < blacksmith.buildTime)
        {
            Debug.Log("While Döngüsünün İçeriisindeyim");
            if (blacksmithPanelController.isBuildCanceled)
            {
                Debug.Log("İnşaat kullanıcı tarafından iptal edildi.");
                if (buildBlacksmithBar != null)
                {
                    buildBlacksmithBar.transform.localScale = new Vector3(0, buildBlacksmithBar.transform.localScale.y, buildBlacksmithBar.transform.localScale.z);
                }
                onCompletion(false);
                isBlacksmithBuildingActive = false;
                yield break;
            }

            elapsedTime += Time.deltaTime;
            Debug.Log("elapsedTime += Time.deltaTime; Satırı çalıştırıldı.ElapsedTime : " + elapsedTime);
            if (buildBlacksmithBar != null)
            {
                Debug.Log("İf'in içerisine girdim. buildBlacksmith Barı Null Değil.");
                float progress = elapsedTime / blacksmith.buildTime;
                buildBlacksmithBar.transform.localScale = new Vector3(
                    progress,
                    buildBlacksmithBar.transform.localScale.y,
                    buildBlacksmithBar.transform.localScale.z
                );

                if (elapsedTime > nextLogTime)
                {
                    Debug.Log("İlerleme çubuğu durumu: Scale=" + buildBlacksmithBar.transform.localScale +
                             ", Aktif=" + buildBlacksmithBar.activeInHierarchy +
                             ", İlerleme=%" + (progress * 100).ToString("F1"));
                    nextLogTime += 0.5f;
                }
            }

            yield return null;
        }

        Debug.Log("İnşaat başarıyla tamamlandı");

        if (buildBlacksmithBar != null)
        {
            buildBlacksmithBar.transform.localScale = new Vector3(1f, buildBlacksmithBar.transform.localScale.y, buildBlacksmithBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu tam dolu duruma getirildi: " + buildBlacksmithBar.transform.localScale);
        }

        blacksmithPanelController.cancelBlacksmithButton.gameObject.SetActive(false);
        isBlacksmithBuildingActive = false;

        panelManager.DestroyPanel("BlacksmithBuildingProcessPanel");
        blacksmithPanelController.refreshBlacksmith(); // refresh fonksiyonun varsa
        onCompletion(true);

        if (buildBlacksmithBar != null)
        {
            buildBlacksmithBar.transform.localScale = new Vector3(0f, buildBlacksmithBar.transform.localScale.y, buildBlacksmithBar.transform.localScale.z);
            Debug.Log("İnşaat tamamlandıktan sonra ilerleme çubuğu sıfırlandı");
        }
    }





    public IEnumerator LabIsFinished(Lab lab, System.Action<bool> onCompletion)
    {
        Debug.Log("LabIsFinished başlatıldı. İnşa süresi: " + lab.buildTime);

        // İlerleme çubuğunu bul
        buildLabBar = GameObject.Find("BuildLabBar");

        if (buildLabBar == null)
        {
            Debug.LogError("BuildLabBar objesi bulunamadı! İlerleme çubuğu çalışmayacak.");

            // Alternatif olarak canvas'ları tara
            Canvas[] canvaslar = FindObjectsOfType<Canvas>();
            foreach (Canvas c in canvaslar)
            {
                Transform bar = c.transform.Find("BuildLabBar");
                if (bar != null)
                {
                    buildLabBar = bar.gameObject;
                    Debug.Log("BuildLabBar bulundu: " + buildLabBar.name);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("BuildLabBar başarıyla bulundu: " + buildLabBar.name);
        }

        // Araştırma sırasında bina yükseltmesi kontrolü
        if (ResearchButtonEvents.isAnyResearchActive)
        {
            Debug.Log("Araştırma sırasında bina yükseltmesi yapamazsınız.");
            onCompletion(false); // Başarısızlık durumunu bildir
            yield break; // Coroutine sonlandır
        }

        // Yeni inşaata izin kontrolü
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inşaat aynı anda aktif olabilir.");
            onCompletion(false);
            yield break;
        }

        Debug.Log("İnşaata izin verildi - Lab inşaatına devam ediliyor");

        string panelName = "LabBuildingProcessPanel";
        labPanelController.cancelLabButton.gameObject.SetActive(true);
        labPanelController.isBuildCanceled = false;

        // Progress bar sıfırla
        if (buildLabBar != null)
        {
            buildLabBar.transform.localScale = new Vector3(0, buildLabBar.transform.localScale.y, buildLabBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu sıfırlandı: " + buildLabBar.transform.localScale);
        }

        panelManager.CreatePanel(panelName, lab.buildingName, lab.buildTime, "Building");

        isLabBuildActive = true;
        float elapsedTime = 0f;
        float nextLogTime = 0.5f;

        while (elapsedTime < lab.buildTime)
        {
            if (labPanelController.isBuildCanceled)
            {
                Debug.Log("İnşaat kullanıcı tarafından iptal edildi.");
                if (buildLabBar != null)
                {
                    buildLabBar.transform.localScale = new Vector3(0, buildLabBar.transform.localScale.y, buildLabBar.transform.localScale.z);
                }
                onCompletion(false);
                isLabBuildActive = false;
                yield break;
            }

            elapsedTime += Time.deltaTime;

            if (buildLabBar != null)
            {
                float progress = elapsedTime / lab.buildTime;
                buildLabBar.transform.localScale = new Vector3(
                    progress,
                    buildLabBar.transform.localScale.y,
                    buildLabBar.transform.localScale.z
                );

                if (elapsedTime > nextLogTime)
                {
                    Debug.Log("İlerleme çubuğu durumu: Scale=" + buildLabBar.transform.localScale +
                             ", Aktif=" + buildLabBar.activeInHierarchy +
                             ", İlerleme=%" + (progress * 100).ToString("F1"));
                    nextLogTime += 0.5f;
                }
            }

            yield return null;
        }

        Debug.Log("İnşaat başarıyla tamamlandı");

        if (buildLabBar != null)
        {
            buildLabBar.transform.localScale = new Vector3(1f, buildLabBar.transform.localScale.y, buildLabBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu tam dolu duruma getirildi: " + buildLabBar.transform.localScale);
        }

        labPanelController.cancelLabButton.gameObject.SetActive(false);
        isLabBuildActive = false;

        panelManager.DestroyPanel("LabBuildingProcessPanel");
        labPanelController.refreshLab();
        onCompletion(true);

        if (buildLabBar != null)
        {
            buildLabBar.transform.localScale = new Vector3(0f, buildLabBar.transform.localScale.y, buildLabBar.transform.localScale.z);
            Debug.Log("İnşaat tamamlandıktan sonra ilerleme çubuğu sıfırlandı");
        }
    }



    public IEnumerator BarracksIsFinished(Barracks barracks, System.Action<bool> onCompletion)
    {
        // Asker üretimi kontrolü
        if (isUnitCreationActive)
        {
            Debug.Log("Asker üretimi yaparken bina yükseltmesi yapılamaz.");
            onCompletion(false); // Başarısızlık durumunu bildir
            yield break; // Coroutine sonlandır
        }

        Debug.Log("BarracksIsFinished başlatıldı. İnşa süresi: " + barracks.buildTime);

        // İlerleme çubuğunu bul
        buildBarracksBar = GameObject.Find("BuildBarracksBar");

        if (buildBarracksBar == null)
        {
            Debug.LogError("BuildBarracksBar objesi bulunamadı! İlerleme çubuğu çalışmayacak.");

            // Alternatif olarak canvas'ları tara
            Canvas[] canvaslar = FindObjectsOfType<Canvas>();
            foreach (Canvas c in canvaslar)
            {
                Transform bar = c.transform.Find("BuildBarracksBar");
                if (bar != null)
                {
                    buildBarracksBar = bar.gameObject;
                    Debug.Log("BuildBarracksBar bulundu: " + buildBarracksBar.name);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("BuildBarracksBar başarıyla bulundu: " + buildBarracksBar.name);
        }

        // Yeni inşaata izin kontrolü
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inşaat aynı anda aktif olabilir.");
            onCompletion(false);
            yield break;
        }

        Debug.Log("İnşaata izin verildi - Barracks inşaatına devam ediliyor");

        string panelName = "BarracksBuildingProcessPanel";
        barracksPanelController.cancelBarracksButton.gameObject.SetActive(true);
        barracksPanelController.isBuildCanceled = false;

        // Progress bar sıfırla
        if (buildBarracksBar != null)
        {
            buildBarracksBar.transform.localScale = new Vector3(0, buildBarracksBar.transform.localScale.y, buildBarracksBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu sıfırlandı: " + buildBarracksBar.transform.localScale);
        }

        panelManager.CreatePanel(panelName, barracks.buildingName, barracks.buildTime, "Building");

        isBarracksBuildActive = true;
        float elapsedTime = 0f;
        float nextLogTime = 0.5f;

        while (elapsedTime < barracks.buildTime)
        {
            if (barracksPanelController.isBuildCanceled)
            {
                Debug.Log("İnşaat kullanıcı tarafından iptal edildi.");
                if (buildBarracksBar != null)
                {
                    buildBarracksBar.transform.localScale = new Vector3(0, buildBarracksBar.transform.localScale.y, buildBarracksBar.transform.localScale.z);
                }
                onCompletion(false);
                isBarracksBuildActive = false;
                yield break;
            }

            elapsedTime += Time.deltaTime;

            if (buildBarracksBar != null)
            {
                float progress = elapsedTime / barracks.buildTime;
                buildBarracksBar.transform.localScale = new Vector3(
                    progress,
                    buildBarracksBar.transform.localScale.y,
                    buildBarracksBar.transform.localScale.z
                );

                if (elapsedTime > nextLogTime)
                {
                    Debug.Log("İlerleme çubuğu durumu: Scale=" + buildBarracksBar.transform.localScale +
                             ", Aktif=" + buildBarracksBar.activeInHierarchy +
                             ", İlerleme=%" + (progress * 100).ToString("F1"));
                    nextLogTime += 0.5f;
                }
            }

            yield return null;
        }

        Debug.Log("İnşaat başarıyla tamamlandı");

        if (buildBarracksBar != null)
        {
            buildBarracksBar.transform.localScale = new Vector3(1f, buildBarracksBar.transform.localScale.y, buildBarracksBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu tam dolu duruma getirildi: " + buildBarracksBar.transform.localScale);
        }

        barracksPanelController.cancelBarracksButton.gameObject.SetActive(false);
        isBarracksBuildActive = false;

        panelManager.DestroyPanel("BarracksBuildingProcessPanel");
        barracksPanelController.refreshBarracks();
        onCompletion(true);

        if (buildBarracksBar != null)
        {
            buildBarracksBar.transform.localScale = new Vector3(0f, buildBarracksBar.transform.localScale.y, buildBarracksBar.transform.localScale.z);
            Debug.Log("İnşaat tamamlandıktan sonra ilerleme çubuğu sıfırlandı");
        }
    }



    public IEnumerator HospitalIsFinished(Hospital hospital, System.Action<bool> onCompletion)
    {
        // İyileştirme aktifse bina yükseltme yapılamaz
        if (isHealActive)
        {
            Debug.Log("İyileştirme sırasında bina yükseltilemez. İyileştirmeyi iptal edip tekrar deneyin.");
            onCompletion(false);
            yield break;
        }

        Debug.Log("HospitalIsFinished başlatıldı. İnşa süresi: " + hospital.buildTime);

        // İlerleme çubuğunu bul
        buildHospitalBar = GameObject.Find("BuildHospitalBar");

        if (buildHospitalBar == null)
        {
            Debug.LogError("BuildHospitalBar objesi bulunamadı! İlerleme çubuğu çalışmayacak.");

            // Alternatif olarak canvas'ları tara
            Canvas[] canvaslar = FindObjectsOfType<Canvas>();
            foreach (Canvas c in canvaslar)
            {
                Transform bar = c.transform.Find("BuildHospitalBar");
                if (bar != null)
                {
                    buildHospitalBar = bar.gameObject;
                    Debug.Log("BuildHospitalBar bulundu: " + buildHospitalBar.name);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("BuildHospitalBar başarıyla bulundu: " + buildHospitalBar.name);
        }

        // Yeni inşaata izin kontrolü
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inşaat aynı anda aktif olabilir.");
            onCompletion(false);
            yield break;
        }

        Debug.Log("İnşaata izin verildi - Hospital inşaatına devam ediliyor");

        string panelName = "HospitalBuildingProcessPanel";
        hospitalPanelController.cancelHospitalButton.gameObject.SetActive(true);
        hospitalPanelController.isBuildCanceled = false;

        if (buildHospitalBar != null)
        {
            buildHospitalBar.transform.localScale = new Vector3(0, buildHospitalBar.transform.localScale.y, buildHospitalBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu sıfırlandı: " + buildHospitalBar.transform.localScale);
        }

        panelManager.CreatePanel(panelName, hospital.buildingName, hospital.buildTime, "Building");

        isHospitalBuildActive = true;
        float elapsedTime = 0f;
        float nextLogTime = 0.5f;

        while (elapsedTime < hospital.buildTime)
        {
            if (hospitalPanelController.isBuildCanceled)
            {
                Debug.Log("İnşaat kullanıcı tarafından iptal edildi.");
                if (buildHospitalBar != null)
                {
                    buildHospitalBar.transform.localScale = new Vector3(0, buildHospitalBar.transform.localScale.y, buildHospitalBar.transform.localScale.z);
                }
                onCompletion(false);
                isHospitalBuildActive = false;
                yield break;
            }

            elapsedTime += Time.deltaTime;

            if (buildHospitalBar != null)
            {
                float progress = elapsedTime / hospital.buildTime;
                buildHospitalBar.transform.localScale = new Vector3(
                    progress,
                    buildHospitalBar.transform.localScale.y,
                    buildHospitalBar.transform.localScale.z
                );

                if (elapsedTime > nextLogTime)
                {
                    Debug.Log("İlerleme çubuğu durumu: Scale=" + buildHospitalBar.transform.localScale +
                              ", Aktif=" + buildHospitalBar.activeInHierarchy +
                              ", İlerleme=%" + (progress * 100).ToString("F1"));
                    nextLogTime += 0.5f;
                }
            }

            yield return null;
        }

        Debug.Log("Hospital inşaatı başarıyla tamamlandı");

        if (buildHospitalBar != null)
        {
            buildHospitalBar.transform.localScale = new Vector3(1f, buildHospitalBar.transform.localScale.y, buildHospitalBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu tam dolu duruma getirildi: " + buildHospitalBar.transform.localScale);
        }

        hospitalPanelController.cancelHospitalButton.gameObject.SetActive(false);
        isHospitalBuildActive = false;

        panelManager.DestroyPanel(panelName);
        hospitalPanelController.refreshHospital();
        onCompletion(true);

        if (buildHospitalBar != null)
        {
            buildHospitalBar.transform.localScale = new Vector3(0f, buildHospitalBar.transform.localScale.y, buildHospitalBar.transform.localScale.z);
            Debug.Log("İnşaat tamamlandıktan sonra ilerleme çubuğu sıfırlandı");
        }
    }



    public IEnumerator CastleIsFinished(Castle castle, System.Action<bool> onCompletion)
    {
        // Yeni inşaata izin kontrolü
        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inşaat aynı anda aktif olabilir.");
            onCompletion(false);
            yield break;
        }

        Debug.Log("CastleIsFinished başlatıldı. İnşa süresi: " + castle.buildTime);

        // İlerleme çubuğunu bul
        upgradeCastleBar = GameObject.Find("UpgradeCastleBar");

        if (upgradeCastleBar == null)
        {
            Debug.LogError("UpgradeCastleBar objesi bulunamadı! İlerleme çubuğu çalışmayacak.");

            // Alternatif olarak canvas'ları tara
            Canvas[] canvaslar = FindObjectsOfType<Canvas>();
            foreach (Canvas c in canvaslar)
            {
                Transform bar = c.transform.Find("UpgradeCastleBar");
                if (bar != null)
                {
                    upgradeCastleBar = bar.gameObject;
                    Debug.Log("UpgradeCastleBar bulundu: " + upgradeCastleBar.name);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("UpgradeCastleBar başarıyla bulundu: " + upgradeCastleBar.name);
        }

        string panelName = "CastleUpgradeProcessPanel";
        castlePanelController.cancelUpgradeCastleButton.gameObject.SetActive(true);
        castlePanelController.isBuildCanceled = false;

        if (upgradeCastleBar != null)
        {
            upgradeCastleBar.transform.localScale = new Vector3(0, upgradeCastleBar.transform.localScale.y, upgradeCastleBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu sıfırlandı: " + upgradeCastleBar.transform.localScale);
        }

        panelManager.CreatePanel(panelName, castle.buildingName, castle.buildTime, "Building");

        isCastleBuildingActive = true;
        float elapsedTime = 0f;
        float nextLogTime = 0.5f;

        while (elapsedTime < castle.buildTime)
        {
            if (castlePanelController.isBuildCanceled)
            {
                Debug.Log("İnşaat kullanıcı tarafından iptal edildi.");
                if (upgradeCastleBar != null)
                {
                    upgradeCastleBar.transform.localScale = new Vector3(0, upgradeCastleBar.transform.localScale.y, upgradeCastleBar.transform.localScale.z);
                }
                onCompletion(false);
                isCastleBuildingActive = false;
                yield break;
            }

            elapsedTime += Time.deltaTime;

            if (upgradeCastleBar != null)
            {
                float progress = elapsedTime / castle.buildTime;
                upgradeCastleBar.transform.localScale = new Vector3(
                    progress,
                    upgradeCastleBar.transform.localScale.y,
                    upgradeCastleBar.transform.localScale.z
                );

                if (elapsedTime > nextLogTime)
                {
                    Debug.Log("İlerleme çubuğu durumu: Scale=" + upgradeCastleBar.transform.localScale +
                              ", Aktif=" + upgradeCastleBar.activeInHierarchy +
                              ", İlerleme=%" + (progress * 100).ToString("F1"));
                    nextLogTime += 0.5f;
                }
            }

            yield return null;
        }

        Debug.Log("Castle inşaatı başarıyla tamamlandı.");

        if (upgradeCastleBar != null)
        {
            upgradeCastleBar.transform.localScale = new Vector3(1f, upgradeCastleBar.transform.localScale.y, upgradeCastleBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu tam dolu duruma getirildi: " + upgradeCastleBar.transform.localScale);
        }

        castlePanelController.cancelUpgradeCastleButton.gameObject.SetActive(false);
        isCastleBuildingActive = false;

        panelManager.DestroyPanel(panelName);
        castlePanelController.refreshCastle();
        onCompletion(true);

        if (upgradeCastleBar != null)
        {
            upgradeCastleBar.transform.localScale = new Vector3(0f, upgradeCastleBar.transform.localScale.y, upgradeCastleBar.transform.localScale.z);
            Debug.Log("İnşaat tamamlandıktan sonra ilerleme çubuğu sıfırlandı");
        }
    }



    public IEnumerator TowerIsFinished(Tower tower, System.Action<bool> onCompletion)
    {
        if (isTowerBuildingActive)
        {
            Debug.Log("Halihazırda bir işlem devam ederken yeni işlem gerçekleştirilemez.");
            onCompletion(false);
            yield break;
        }

        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inşaat aynı anda aktif olabilir.");
            onCompletion(false);
            yield break;
        }

        Debug.Log("TowerIsFinished başlatıldı. İnşa süresi: " + tower.buildTime);

        // İlerleme çubuğunu bul
        buildTowerBar = GameObject.Find("BuildTowerBar");

        if (buildTowerBar == null)
        {
            Debug.LogError("BuildTowerBar objesi bulunamadı! İlerleme çubuğu çalışmayacak.");

            Canvas[] canvaslar = FindObjectsOfType<Canvas>();
            foreach (Canvas c in canvaslar)
            {
                Transform bar = c.transform.Find("BuildTowerBar");
                if (bar != null)
                {
                    buildTowerBar = bar.gameObject;
                    Debug.Log("BuildTowerBar bulundu: " + buildTowerBar.name);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("BuildTowerBar başarıyla bulundu: " + buildTowerBar.name);
        }

        string panelName = "TowerBuildingProcessPanel";
        towerPanelController.cancelTowerButton.gameObject.SetActive(true);
        towerPanelController.isBuildCanceled = false;

        if (buildTowerBar != null)
        {
            buildTowerBar.transform.localScale = new Vector3(0, buildTowerBar.transform.localScale.y, buildTowerBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu sıfırlandı: " + buildTowerBar.transform.localScale);
        }

        panelManager.CreatePanel(panelName, tower.buildingName, tower.buildTime, "Building");

        isTowerBuildingActive = true;
        float elapsedTime = 0f;
        float nextLogTime = 0.5f;

        while (elapsedTime < tower.buildTime)
        {
            if (towerPanelController.isBuildCanceled)
            {
                Debug.Log("İnşaat kullanıcı tarafından iptal edildi.");
                if (buildTowerBar != null)
                {
                    buildTowerBar.transform.localScale = new Vector3(0, buildTowerBar.transform.localScale.y, buildTowerBar.transform.localScale.z);
                }
                onCompletion(false);
                isTowerBuildingActive = false;
                yield break;
            }

            elapsedTime += Time.deltaTime;

            if (buildTowerBar != null)
            {
                float progress = elapsedTime / tower.buildTime;
                buildTowerBar.transform.localScale = new Vector3(
                    progress,
                    buildTowerBar.transform.localScale.y,
                    buildTowerBar.transform.localScale.z
                );

                if (elapsedTime > nextLogTime)
                {
                    Debug.Log("İlerleme çubuğu durumu: Scale=" + buildTowerBar.transform.localScale +
                              ", Aktif=" + buildTowerBar.activeInHierarchy +
                              ", İlerleme=%" + (progress * 100).ToString("F1"));
                    nextLogTime += 0.5f;
                }
            }

            yield return null;
        }

        Debug.Log("Tower inşaatı başarıyla tamamlandı.");

        if (buildTowerBar != null)
        {
            buildTowerBar.transform.localScale = new Vector3(1f, buildTowerBar.transform.localScale.y, buildTowerBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu tam dolu duruma getirildi: " + buildTowerBar.transform.localScale);
        }

        towerPanelController.cancelTowerButton.gameObject.SetActive(false);
        isTowerBuildingActive = false;

        panelManager.DestroyPanel(panelName);
        onCompletion(true);

        if (buildTowerBar != null)
        {
            buildTowerBar.transform.localScale = new Vector3(0f, buildTowerBar.transform.localScale.y, buildTowerBar.transform.localScale.z);
            Debug.Log("İnşaat tamamlandıktan sonra ilerleme çubuğu sıfırlandı");
        }
    }


    public IEnumerator TrapIsFinished(Trap trap, System.Action<bool> onCompletion)
    {
        if (isAnyTrapActive)
        {
            Debug.Log("Halihazırda bir tuzak işlemi devam ederken yeni işlem başlatılamaz.");
            onCompletion(false);
            yield break;
        }

        if (!constructionController.CanStartNewConstruction())
        {
            Debug.Log("En fazla 2 inşaat aynı anda aktif olabilir.");
            onCompletion(false);
            yield break;
        }

        Debug.Log("TrapIsFinished başlatıldı. İnşa süresi: " + trap.buildTime);

        // İlerleme çubuğunu bul
        buildTrapBar = GameObject.Find("BuildTrapBar");

        if (buildTrapBar == null)
        {
            Debug.LogError("BuildTrapBar objesi bulunamadı! İlerleme çubuğu çalışmayacak.");

            Canvas[] canvaslar = FindObjectsOfType<Canvas>();
            foreach (Canvas c in canvaslar)
            {
                Transform bar = c.transform.Find("BuildTrapBar");
                if (bar != null)
                {
                    buildTrapBar = bar.gameObject;
                    Debug.Log("BuildTrapBar bulundu: " + buildTrapBar.name);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("BuildTrapBar başarıyla bulundu: " + buildTrapBar.name);
        }

        string panelName = "TrapBuildingProcessPanel";
        trapPanelController.cancelTrapButton.gameObject.SetActive(true);
        trapPanelController.isBuildCanceled = false;

        if (buildTrapBar != null)
        {
            buildTrapBar.transform.localScale = new Vector3(0, buildTrapBar.transform.localScale.y, buildTrapBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu sıfırlandı: " + buildTrapBar.transform.localScale);
        }

        panelManager.CreatePanel(panelName, trap.buildingName, trap.buildTime, "Building");

        isAnyTrapActive = true;
        float elapsedTime = 0f;
        float nextLogTime = 0.5f;

        while (elapsedTime < trap.buildTime)
        {
            if (trapPanelController.isBuildCanceled)
            {
                Debug.Log("Tuzak inşaatı kullanıcı tarafından iptal edildi.");
                if (buildTrapBar != null)
                {
                    buildTrapBar.transform.localScale = new Vector3(0, buildTrapBar.transform.localScale.y, buildTrapBar.transform.localScale.z);
                }
                onCompletion(false);
                isAnyTrapActive = false;
                yield break;
            }

            elapsedTime += Time.deltaTime;

            if (buildTrapBar != null)
            {
                float progress = elapsedTime / trap.buildTime;
                buildTrapBar.transform.localScale = new Vector3(
                    progress,
                    buildTrapBar.transform.localScale.y,
                    buildTrapBar.transform.localScale.z
                );

                if (elapsedTime > nextLogTime)
                {
                    Debug.Log("İlerleme çubuğu durumu: Scale=" + buildTrapBar.transform.localScale +
                              ", Aktif=" + buildTrapBar.activeInHierarchy +
                              ", İlerleme=%" + (progress * 100).ToString("F1"));
                    nextLogTime += 0.5f;
                }
            }

            yield return null;
        }

        Debug.Log("Tuzak inşaatı başarıyla tamamlandı.");

        if (buildTrapBar != null)
        {
            buildTrapBar.transform.localScale = new Vector3(1f, buildTrapBar.transform.localScale.y, buildTrapBar.transform.localScale.z);
            Debug.Log("İlerleme çubuğu tam dolu duruma getirildi: " + buildTrapBar.transform.localScale);
        }

        trapPanelController.cancelTrapButton.gameObject.SetActive(false);
        isAnyTrapActive = false;

        panelManager.DestroyPanel(panelName);
        onCompletion(true);

        if (buildTrapBar != null)
        {
            buildTrapBar.transform.localScale = new Vector3(0f, buildTrapBar.transform.localScale.y, buildTrapBar.transform.localScale.z);
            Debug.Log("İnşaat tamamlandıktan sonra ilerleme çubuğu sıfırlandı");
        }
    }



}
