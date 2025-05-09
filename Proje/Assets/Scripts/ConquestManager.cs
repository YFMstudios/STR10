using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Krallýklarýn fetih bilgilerini tutan statik sýnýf
/// </summary>
public static class ConquestManager
{
    // Düðüm yapýsý
    public class KingdomNode
    {
        public string Name { get; set; }
        public KingdomNode Next { get; set; }

        public KingdomNode(string name)
        {
            Name = name;
            Next = null;
        }
    }

    // Krallýk baðlý listelerini tutan sözlük
    public static Dictionary<string, KingdomNode> kingdoms = new Dictionary<string, KingdomNode>();

    /// <summary>
    /// Krallýk sistemini baþlangýç deðerleriyle baþlatýr
    /// </summary>
    /// <param name="kingdomNames">Krallýk isimleri</param>
    public static void Initialize(string[] kingdomNames)
    {
        kingdoms.Clear();

        foreach (string name in kingdomNames)
        {
            kingdoms[name] = new KingdomNode(name);
        }

        Debug.Log("Krallýklar baþarýyla oluþturuldu.");
    }

    /// <summary>
    /// Bir krallýðýn baþka bir krallýðý fethetme iþlemini gerçekleþtirir
    /// </summary>
    /// <param name="conquerorName">Fetheden krallýk adý</param>
    /// <param name="conqueredName">Fethedilen krallýk adý</param>
    /// <returns>Ýþlem sonucu</returns>
    public static bool Conquer(string conquerorName, string conqueredName)
    {
        // Kontroller
        if (!kingdoms.ContainsKey(conquerorName))
        {
            Debug.LogError($"Hata: {conquerorName} krallýðý bulunamadý.");
            return false;
        }

        if (!kingdoms.ContainsKey(conqueredName))
        {
            Debug.LogError($"Hata: {conqueredName} krallýðý zaten fethedilmiþ veya mevcut deðil.");
            return false;
        }

        // Fetheden krallýðýn baðlý listesinin sonuna git
        KingdomNode current = kingdoms[conquerorName];
        while (current.Next != null)
        {
            current = current.Next;
        }

        // Fethedilen krallýðýn adýyla yeni bir düðüm ekle
        current.Next = new KingdomNode(conqueredName);

        // Fethedilen krallýðýn listesini sözlükten kaldýr (silme iþlemi burada gerçekleþiyor)
        kingdoms.Remove(conqueredName);

        Debug.Log($"{conquerorName} krallýðý, {conqueredName} krallýðýný fethetti!");
        return true;
    }

    /// <summary>
    /// Bir krallýðýn fethettiði tüm krallýklarý listeler
    /// </summary>
    /// <param name="kingdomName">Krallýk adý</param>
    /// <returns>Fethedilen krallýklarýn listesi</returns>
    public static List<string> GetConqueredKingdoms(string kingdomName)
    {
        List<string> conqueredKingdoms = new List<string>();

        if (!kingdoms.ContainsKey(kingdomName))
        {
            Debug.LogError($"Hata: {kingdomName} krallýðý bulunamadý.");
            return conqueredKingdoms;
        }

        KingdomNode current = kingdoms[kingdomName].Next;
        while (current != null)
        {
            conqueredKingdoms.Add(current.Name);
            current = current.Next;
        }

        return conqueredKingdoms;
    }

    /// <summary>
    /// Mevcut tüm krallýklarý döndürür
    /// </summary>
    /// <returns>Mevcut krallýk isimleri</returns>
    public static string[] GetExistingKingdoms()
    {
        string[] kingdomNames = new string[kingdoms.Count];
        kingdoms.Keys.CopyTo(kingdomNames, 0);
        return kingdomNames;
    }

    /// <summary>
    /// Tüm krallýklarýn fetih durumunu konsola yazdýrýr
    /// </summary>
    public static void PrintAllKingdoms()
    {
        foreach (var kingdomEntry in kingdoms)
        {
            string kingdomName = kingdomEntry.Key;
            Debug.Log($"Krallýk: {kingdomName}");

            // Fethettiði krallýklarý listele
            List<string> conquered = GetConqueredKingdoms(kingdomName);
            if (conquered.Count > 0)
            {
                Debug.Log($"  Fethettiði krallýklar: {string.Join(", ", conquered)}");
            }
            else
            {
                Debug.Log("  Henüz fethettiði krallýk yok.");
            }
        }
    }
}