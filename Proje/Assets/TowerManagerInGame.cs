using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManagerInGame : MonoBehaviour
{
    public GetPlayerData getPlayerData;

    public GameObject towerOneObject;
    public GameObject towerTwoObject;


    void Start()
    {

        towerOneObject.SetActive(false);
        towerTwoObject.SetActive(false);
       
        
        if (getPlayerData.TowerOneIsBuilded && towerOneObject != null)
        {
            towerOneObject.SetActive(true);
            Debug.Log("Tower1 aktif edildi.");
        }

        if (getPlayerData.TowerTwoIsBuilded && towerTwoObject != null)
        {
            towerTwoObject.SetActive(true);
            Debug.Log("Tower2 aktif edildi.");
        }

       
    }
}
