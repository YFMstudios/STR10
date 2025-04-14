using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoBehaviour
{
     public GetPlayerData getPlayerData;

    public GameObject CastleObject;
  

    void Start()
    {
        if (getPlayerData.CastleLevel == 1)
        {
            Debug.Log("Level 1");
        }
        if (getPlayerData.CastleLevel == 2)
        {
              Debug.Log("Level 2");
        }
        if (getPlayerData.CastleLevel == 3)
        {
              Debug.Log("Level 3");
        }
      
    }
}
