using UnityEngine;
using System.Collections.Generic;

public static class WarDataManager
{
    // Savaþ sonucunu saklamak için bilgiler
    public static bool IsTerritoryChangeNeeded = false;
    public static string ConqueringKingdom = "";
    public static string ConqueredKingdom = "";

    // Toprak deðiþimi gerçekleþtiyse bunu true yap
    public static bool TerritoryChangeCompleted = false;

    // Toprak deðiþimi verilerini ayarla
    public static void SetTerritoryChangeData(string conqueror, string conquered)
    {
        IsTerritoryChangeNeeded = true;
        ConqueringKingdom = conqueror;
        ConqueredKingdom = conquered;
        TerritoryChangeCompleted = false;

        // PlayerPrefs'e kaydet (sahne geçiþlerinde korunmasý için)
        PlayerPrefs.SetInt("IsTerritoryChangeNeeded", 1);
        PlayerPrefs.SetString("ConqueringKingdom", conqueror);
        PlayerPrefs.SetString("ConqueredKingdom", conquered);
        PlayerPrefs.SetInt("TerritoryChangeCompleted", 0);
        PlayerPrefs.Save();

        Debug.Log($"<color=purple>[WarDataManager] Toprak deðiþimi bilgileri kaydedildi: {conqueror} -> {conquered}</color>");
    }

    // Verileri PlayerPrefs'ten yükle
    public static void LoadDataFromPlayerPrefs()
    {
        IsTerritoryChangeNeeded = PlayerPrefs.GetInt("IsTerritoryChangeNeeded", 0) == 1;
        ConqueringKingdom = PlayerPrefs.GetString("ConqueringKingdom", "");
        ConqueredKingdom = PlayerPrefs.GetString("ConqueredKingdom", "");
        TerritoryChangeCompleted = PlayerPrefs.GetInt("TerritoryChangeCompleted", 0) == 1;

        if (IsTerritoryChangeNeeded)
        {
            Debug.Log($"<color=purple>[WarDataManager] Toprak deðiþimi bilgileri yüklendi: {ConqueringKingdom} -> {ConqueredKingdom}, Tamamlandý: {TerritoryChangeCompleted}</color>");
        }
    }

    // Ýþlem tamamlandýðýnda çaðýr
    public static void MarkTerritoryChangeCompleted()
    {
        TerritoryChangeCompleted = true;
        PlayerPrefs.SetInt("TerritoryChangeCompleted", 1);
        PlayerPrefs.Save();
        Debug.Log("<color=purple>[WarDataManager] Toprak deðiþimi tamamlandý olarak iþaretlendi</color>");
    }

    // MainMenu'den çaðrýlacak - tüm verileri temizle
    public static void ClearAllData()
    {
        IsTerritoryChangeNeeded = false;
        ConqueringKingdom = "";
        ConqueredKingdom = "";
        TerritoryChangeCompleted = false;

        PlayerPrefs.DeleteKey("IsTerritoryChangeNeeded");
        PlayerPrefs.DeleteKey("ConqueringKingdom");
        PlayerPrefs.DeleteKey("ConqueredKingdom");
        PlayerPrefs.DeleteKey("TerritoryChangeCompleted");
        PlayerPrefs.Save();

        Debug.Log("<color=purple>[WarDataManager] Tüm savaþ verileri temizlendi</color>");
    }
}