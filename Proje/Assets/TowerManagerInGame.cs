using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManagerInGame : MonoBehaviour
{
    public GetPlayerData getPlayerData;

    public GameObject towerOneObject;
    public GameObject towerTwoObject;

     private  ObjectiveStats objectiveStats;

      private  Turret turret;

    void Start()
    {

        towerOneObject.SetActive(false);
        towerTwoObject.SetActive(false);
       
        
        if (getPlayerData.TowerOneIsBuilded && towerOneObject != null)
        {
            towerOneObject.SetActive(true);
            Debug.Log("Tower1 aktif edildi.");
            turret.attackDamage=20;
            turret.attackCooldown=0.75f;
            turret.attackRange=12f;
            objectiveStats.health=1000;
            Debug.Log("tower1 Sırasıyla hasar cooldown range ve can :"  + turret.attackDamage + "," + turret.attackCooldown + ","+ turret.attackRange + ","+ objectiveStats.health);
        }

        if (getPlayerData.TowerTwoIsBuilded && towerTwoObject != null)
        {
            towerTwoObject.SetActive(true);
            Debug.Log("Tower2 aktif edildi.");
             turret.attackDamage=20;
            turret.attackCooldown=0.75f;
            turret.attackRange=12f;
            objectiveStats.health=1000;
            Debug.Log(" tower2 Sırasıyla hasar cooldown range ve can :" + turret.attackDamage + "," + turret.attackCooldown + ","+ turret.attackRange + ","+ objectiveStats.health);
        }

         if (getPlayerData.TowerOneIsBuilded && towerOneObject != null && getPlayerData.TowerOneLevel==2)
        {
           
            Debug.Log("Tower1 2 level oldu .");
             turret.attackDamage=30;
            turret.attackCooldown=0.60f;
            turret.attackRange=13.5f;
            objectiveStats.health=1500;
            Debug.Log("tower1 Sırasıyla hasar cooldown range ve can :"  + turret.attackDamage + "," + turret.attackCooldown + ","+ turret.attackRange + ","+ objectiveStats.health);
        }

        if (getPlayerData.TowerTwoIsBuilded && towerTwoObject != null && getPlayerData.TowerTwoLevel==2)
        {
           
            Debug.Log("Tower2 2 level oldu .");
            turret.attackDamage=30;
            turret.attackCooldown=0.60f;
            turret.attackRange=13.5f;
            objectiveStats.health=1500;
            Debug.Log("tower2 Sırasıyla hasar cooldown range ve can :"  + turret.attackDamage + "," + turret.attackCooldown + ","+ turret.attackRange + ","+ objectiveStats.health);
        }

        if (getPlayerData.TowerOneIsBuilded && towerOneObject != null && getPlayerData.TowerOneLevel==3)
        {
           
            Debug.Log("Tower1 3 level oldu .");
             turret.attackDamage=50;
            turret.attackCooldown=0.50f;
            turret.attackRange=15f;
            objectiveStats.health=2000;
            Debug.Log("tower1 Sırasıyla hasar cooldown range ve can :"  + turret.attackDamage + "," + turret.attackCooldown + ","+ turret.attackRange + ","+ objectiveStats.health);
        }

        if (getPlayerData.TowerTwoIsBuilded && towerTwoObject != null && getPlayerData.TowerTwoLevel==3)
        {
           
            Debug.Log("Tower2 3 level oldu .");
             turret.attackDamage=50;
            turret.attackCooldown=0.50f;
            turret.attackRange=15f;
            objectiveStats.health=2000;
            Debug.Log("tower2 Sırasıyla hasar cooldown range ve can :"  + turret.attackDamage + "," + turret.attackCooldown + ","+ turret.attackRange + ","+ objectiveStats.health);
        }

       
    }

     public void SetObjectiveStats(ObjectiveStats objective_Stats){
            objectiveStats = objective_Stats;
    }

    public void SetTurret(Turret turret_){
            turret = turret_;
    }
}
