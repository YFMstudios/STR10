using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoBehaviour
{
     public GetPlayerData getPlayerData;

    public GameObject CastleObject;
    
     private  ObjectiveStats objectiveStats;

      private  Turret turret;

    void Start()
    {
        if (getPlayerData.CastleLevel == 1)
        {
            Debug.Log("kale 1 level oldu .");
            turret.attackDamage=25;
            turret.attackCooldown=0.60f;
            turret.attackRange=15f;
            objectiveStats.health=1500;
            Debug.Log("kale Sırasıyla hasar cooldown range ve can :"  + turret.attackDamage + "," + turret.attackCooldown + ","+ turret.attackRange + ","+ objectiveStats.health);
        }
        if (getPlayerData.CastleLevel == 2)
        {
            Debug.Log("kale 2 level oldu .");
            turret.attackDamage=35;
            turret.attackCooldown=0.50f;
            turret.attackRange=17.5f;
            objectiveStats.health=2000;
            Debug.Log("kale Sırasıyla hasar cooldown range ve can :"  + turret.attackDamage + "," + turret.attackCooldown + ","+ turret.attackRange + ","+ objectiveStats.health);
        }
        if (getPlayerData.CastleLevel == 3)
        {
            Debug.Log("kale 3 level oldu .");
            turret.attackDamage=55;
            turret.attackCooldown=0.45f;
            turret.attackRange=20f;
            objectiveStats.health=2500;
            Debug.Log("kale Sırasıyla hasar cooldown range ve can :"  + turret.attackDamage + "," + turret.attackCooldown + ","+ turret.attackRange + ","+ objectiveStats.health);
        }
      
    }

     public void SetObjectiveStats(ObjectiveStats objective_Stats){
            objectiveStats = objective_Stats;
    }

    public void SetTurret(Turret turret_){
            turret = turret_;
    }
}
