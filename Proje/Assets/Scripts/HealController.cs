using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "HealController", menuName = "ScriptableObjects/HealController")]
public class HealController : ScriptableObject
{
    public int woundedSoldier;
    public int woundedArcher;
   
    public void setWoundedSoldier(int soldier)
    {
        float randomPercentage = Random.Range(10f, 40f); // %10 ile %40 arasında rastgele bir değer
        int amountToAdd = Mathf.RoundToInt(soldier * (randomPercentage / 100f)); // yüzde hesabı
        woundedSoldier += amountToAdd; // woundedSoldier'a ekle
        SaveValues();
    }

    public void setWoundedArcher(int archer)
    {
        float randomPercentage = Random.Range(10f, 40f); // %10 ile %40 arasında rastgele bir değer
        int amountToAdd = Mathf.RoundToInt(archer * (randomPercentage / 100f)); // yüzde hesabı
        woundedArcher += amountToAdd; // woundedSoldier'a ekle
        SaveValues();
    }

    public void resetWoundedSoldiers()
    {
        woundedSoldier = 0;
        woundedArcher = 0;
        SaveValues();
    }

    public void OnEnable()
    {
        // Kaydedilmiş değerler varsa yükle
        if (PlayerPrefs.HasKey("WoundedSoldier"))
            woundedSoldier = PlayerPrefs.GetInt("WoundedSoldier");

        if (PlayerPrefs.HasKey("WoundedArcher"))
            woundedArcher = PlayerPrefs.GetInt("WoundedArcher");
    }

    public void SaveValues()
    {
        PlayerPrefs.SetInt("WoundedSoldier", woundedSoldier);
        PlayerPrefs.SetInt("WoundedArcher", woundedArcher);
        PlayerPrefs.Save();
    }

    

}
