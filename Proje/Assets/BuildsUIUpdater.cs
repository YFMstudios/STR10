using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildsUIUpdater : MonoBehaviour
{
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

     public Button buildStonePitButton;
    public Button buildBlacksmithButton;
    public Button buildSawmillButton;
    public Button buildBarracksButton;
    public Button buildFarmButton;
    public Button buildHospitalButton;
    public Button buildLabButton;
 
    public Button buildWarehouseButton;
    public Button buildCastleButton;
    public Button buildTowerOneButton;
    public Button buildTowerTwoButton;
   
    public Button buildTrapOneButton;
    public Button buildTrapTwoButton;
    public Button buildTrapThreeButton;

    private Text buttonText;
    void Start()
    {
        farmPanelController.refreshFarm();
        wareHousePanelController.refreshWarehouse();
        stonepitPanelController.refreshStonePit();
        sawmillPanelController.refreshSawmill();
        blacksmithPanelController.refreshBlacksmith();
        labPanelController.refreshLab();
        barracksPanelController.refreshBarracks();
        hospitalPanelController.refreshHospital();
        castlePanelController.refreshCastle();
        towerPanelController.refreshTowerOne();
        towerPanelController.refreshTowerTwo();
        trapPanelController.refreshTrapOne();
        trapPanelController.refreshTrapTwo();
        trapPanelController.refreshTrapThree();
    }

  
  
}
