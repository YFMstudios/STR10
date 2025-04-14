using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    // Start is called before the first frame update
    private BattleScenePlayerSpawner _battleScenePlayerSpawner;
      private MinionSpawner _minionSpawner;
        private EnemyMinionSpawner _enemyMinionSpawner;
    public string PlayerRole;

    public GetPlayerData getPlayerData;
    
void Start()
{
    Debug.Log("[SoldierController] Start() çağrıldı. Rol: " + PlayerRole);

    // Sahnedeki minion spawner referanslarını çek ve kendini ata
    MinionSpawner minionSpawner = FindObjectOfType<MinionSpawner>();
    if (minionSpawner != null)
    {
        _minionSpawner = minionSpawner;
        minionSpawner.soldierManager = this;
    }

    EnemyMinionSpawner enemyMinionSpawner = FindObjectOfType<EnemyMinionSpawner>();
    if (enemyMinionSpawner != null)
    {
        _enemyMinionSpawner = enemyMinionSpawner;
        enemyMinionSpawner.soldierManager = this;
    }
}


void Update()
{
    if (Input.GetKeyDown(KeyCode.P))
    {
        Debug.Log("[SoldierController] Anlık Rol: " + PlayerRole);
    }
}

    public void setBattleScenePlayerSpawner(BattleScenePlayerSpawner battleScenePlayerSpawner){
            _battleScenePlayerSpawner = battleScenePlayerSpawner;
    }
     public void setMinionSpawner(MinionSpawner minionSpawner){
            _minionSpawner = minionSpawner;
    }
     public void setEnemyMinionSpawner(EnemyMinionSpawner enemyMinionSpawner){
           _enemyMinionSpawner  = enemyMinionSpawner;
    }

    public void setSoldierAmount()
{
    Debug.Log("Buton Tıklama Fonksiyonuna Girdi. Role :" + PlayerRole);

    if (PlayerRole == "attacker")
    {
        if (_minionSpawner != null)
        {
            getPlayerData.currentArcherAmount = _minionSpawner.kalanOkcu;
            getPlayerData.currentSoldierAmount = _minionSpawner.kalanSavasci;
            Debug.Log("Attacker Kalan Asker Atamaları Yapıldı");
            Debug.Log("Kalan Savasci : " + getPlayerData.currentSoldierAmount);
            Debug.Log("Kalan oKCU : " + getPlayerData.currentArcherAmount);
        }
        else
        {
            Debug.LogError("MinionSpawner NULL!");
        }
    }
    else if (PlayerRole == "defender")
    {
        if (_enemyMinionSpawner != null)
        {
            getPlayerData.currentArcherAmount = _enemyMinionSpawner.kalanOkcu;
            getPlayerData.currentSoldierAmount = _enemyMinionSpawner.kalanSavasci;
            Debug.Log("Defender Kalan Asker Atamaları Yapıldı");
        }
        else
        {
            Debug.LogError("EnemyMinionSpawner NULL!");
        }
    }
    else
    {
        Debug.Log("ROLE = SPECTATOR");
    }
}


}
