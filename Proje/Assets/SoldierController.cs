using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    // ------------------------------------------------------------
    //  Referanslar
    // ------------------------------------------------------------
    private BattleScenePlayerSpawner _battleScenePlayerSpawner;
    private MinionSpawner           _minionSpawner;
    private EnemyMinionSpawner      _enemyMinionSpawner;

    public string PlayerRole;               // attacker / defender / spectator
    public GetPlayerData getPlayerData;     // mevcut ScriptableObject

    // ------------------------------------------------------------
    //  >>>> EKLEDİĞİMİZ  Getter'lar  <<<<
    // ------------------------------------------------------------
    public int GetSoldierCount(string role)
    {
        if (role == "attacker" && _minionSpawner != null)
            return _minionSpawner.kalanSavasci;       // minionSpawner’da tuttuğun sayı
        if (role == "defender" && _enemyMinionSpawner != null)
            return _enemyMinionSpawner.kalanSavasci;  // enemyMinionSpawner’da tuttuğun sayı
        return 0;
    }

    public int GetArcherCount(string role)
    {
        if (role == "attacker" && _minionSpawner != null)
            return _minionSpawner.kalanOkcu;
        if (role == "defender" && _enemyMinionSpawner != null)
            return _enemyMinionSpawner.kalanOkcu;
        return 0;
    }
    // ------------------------------------------------------------

    void Start()
    {
        Debug.Log("[SoldierController] Start() çağrıldı. Rol: " + PlayerRole);

        _minionSpawner      = FindObjectOfType<MinionSpawner>();
        if (_minionSpawner != null)             _minionSpawner.soldierManager = this;

        _enemyMinionSpawner = FindObjectOfType<EnemyMinionSpawner>();
        if (_enemyMinionSpawner != null)        _enemyMinionSpawner.soldierManager = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Debug.Log("[SoldierController] Anlık Rol: " + PlayerRole);
    }

    // ------------------------------------------------------------
    //  Diğer set-fonksiyonların aynı kalıyor
    // ------------------------------------------------------------
    public void setBattleScenePlayerSpawner(BattleScenePlayerSpawner s) => _battleScenePlayerSpawner = s;
    public void setMinionSpawner(MinionSpawner m)                       => _minionSpawner          = m;
    public void setEnemyMinionSpawner(EnemyMinionSpawner e)             => _enemyMinionSpawner     = e;

    // Kalan askerleri GetPlayerData’ya yazan mevcut fonksiyonun aynen duruyor
    public void setSoldierAmount()
    {
        Debug.Log("Buton Tıklama Fonksiyonuna Girdi. Role :" + PlayerRole);

        if (PlayerRole == "attacker" && _minionSpawner != null)
        {
            getPlayerData.currentArcherAmount  = _minionSpawner.kalanOkcu;
            getPlayerData.currentSoldierAmount = _minionSpawner.kalanSavasci;
        }
        else if (PlayerRole == "defender" && _enemyMinionSpawner != null)
        {
            getPlayerData.currentArcherAmount  = _enemyMinionSpawner.kalanOkcu;
            getPlayerData.currentSoldierAmount = _enemyMinionSpawner.kalanSavasci;
        }
    }
}
