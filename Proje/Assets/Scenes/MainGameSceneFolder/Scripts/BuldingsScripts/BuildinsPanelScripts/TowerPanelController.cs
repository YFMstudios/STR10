using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerPanelController : MonoBehaviour
{

    
    public TMP_Text towerOneBuildLevelText;     
    public TMP_Text towerOneMenzilText; 
    public TMP_Text towerOneCanText;
    public TMP_Text towerOneSaldiriHiziText;

    public TMP_Text towerTwoBuildLevelText;
    public TMP_Text towerTwoMenzilText;
    public TMP_Text towerTwoCanText;
    public TMP_Text towerTwoSaldiriHiziText;


    public Button cancelTowerButton;
    public bool isBuildCanceled = false;
    public GameObject progressBar;
    public PanelManager panelManager;
    public void refreshTowerOne()
    {
        if (Tower.towerOneBuildLevel == 1)
        {
            towerOneBuildLevelText.text = "1";
            towerOneMenzilText.text = "10";
            towerOneCanText.text = "1000";
            towerOneSaldiriHiziText.text = "5 h/s";

        }
        else if (Tower.towerOneBuildLevel == 2)
        {
            towerOneBuildLevelText.text = "2";
            towerOneMenzilText.text = "15";
            towerOneCanText.text = "1500";
            towerOneSaldiriHiziText.text = "7.5 h/s";
        }
        else if (Tower.towerOneBuildLevel == 3)
        {
            towerOneBuildLevelText.text = "3";
            towerOneMenzilText.text = "20";
            towerOneCanText.text = "2000";
            towerOneSaldiriHiziText.text = "10 h/s";
            //DestroyComponents();
        }
    }

    public void refreshTowerTwo()
    {
        if (Tower.towerTwoBuildLevel == 1)
        {
            towerTwoBuildLevelText.text = "1";
            towerTwoMenzilText.text = "10";
            towerTwoCanText.text = "1000";
            towerTwoSaldiriHiziText.text = "5 h/s";
        }
        else if (Tower.towerTwoBuildLevel == 2)
        {
            towerTwoBuildLevelText.text = "2";
            towerTwoMenzilText.text = "15";
            towerTwoCanText.text = "1500";
            towerTwoSaldiriHiziText.text = "7.5 h/s";
        }
        else if (Tower.towerTwoBuildLevel == 3)
        {
            towerTwoBuildLevelText.text = "3";
            towerTwoMenzilText.text = "20";
            towerTwoCanText.text = "2000";
            towerTwoSaldiriHiziText.text = "10 h/s";
            //DestroyComponents();

        }
    }

    public void DestroyComponents()
    {
        if(Tower.towerOneBuildLevel == 3 && Tower.towerTwoBuildLevel == 3)
        {

        }

    }

    public void cancelTowerBuild()
    {
        isBuildCanceled = true; // Ýptal iþlemini baþlat
        panelManager.DestroyPanel("TowerBuildingProcessPanel");
        cancelTowerButton.gameObject.SetActive(false);
    }
}
