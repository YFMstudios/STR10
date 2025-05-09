using UnityEngine;

public class PlayerDataResetter : MonoBehaviour
{
    public GetPlayerData getPlayerData;
    public HealController healController;
    private void Awake()
    {
        ResetPlayerData();
    }


    private void ResetPlayerData()
    {
        getPlayerData.currentSoldierAmount = 0;
        getPlayerData.currentArcherAmount = 0;

        getPlayerData.CastleLevel = 0;
        getPlayerData.TowerOneLevel = 0;
        getPlayerData.TowerTwoLevel = 0;
        getPlayerData.TrapOneLevel = 0;
        getPlayerData.TrapTwoLevel = 0;
        getPlayerData.TrapThreeLevel = 0;

        getPlayerData.TowerOneIsBuilded = false;
        getPlayerData.TowerTwoIsBuilded = false;
        getPlayerData.TrapOneIsBuilded = false;
        getPlayerData.TrapTwoIsBuilded = false;
        getPlayerData.TrapThreeIsBuilded = false;

        getPlayerData.currentArcherAmount = 0;
        getPlayerData.currentSoldierAmount = 0;

        healController.resetWoundedSoldiers();
        // 6 krallýk oluþtur
        string[] kingdomNames = new string[]
        {
        "Akhadzria",
        "Alfgard",
        "Arianopol",
        "Dhamuron",
        "Lexion",
        "Zephyrion"
        };

        // Sistemi baþlat
        ConquestManager.Initialize(kingdomNames);
    }

    // MainMenuManager.cs içinde (veya uygun bir yerde)

    public void ClearWarConquestData()
    {
        PlayerPrefs.DeleteKey("IsTerritoryChangeNeeded");
        PlayerPrefs.DeleteKey("ConqueringKingdom");
        PlayerPrefs.DeleteKey("ConqueredKingdom");
        PlayerPrefs.DeleteKey("TerritoryChangeCompleted");
        PlayerPrefs.Save();

        Debug.Log("<color=purple>[MainMenuManager] Tüm savaþ verileri temizlendi</color>");
    }
}
