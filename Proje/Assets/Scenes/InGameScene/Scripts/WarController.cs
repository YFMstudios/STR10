using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarController : MonoBehaviour
{
    public string AttackerKingdom;
    public string DefenderKingdom;
    public GetPlayerData getPlayerData;
    public static bool playerIsDead;
    public static bool castleIsDestroy;
    void Start()
    {
        castleIsDestroy = false;
        playerIsDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((getPlayerData.currentSoldierAmount + getPlayerData.currentArcherAmount) == 0 && playerIsDead)
        {
            //Saldýran Kaybetti
            Debug.Log("Minion Kalmadý ve Player Öldü. Fethetme Fonksiyonunu çaðýr.");
            //Sahneye geri dön.
        }
        if(castleIsDestroy)
        {
            getPlayerData.conquerKingdom(AttackerKingdom, DefenderKingdom);
            //Sahneye geri dön.
            //Savunan oyuncuyu oyundan at.
        }
    }
}
