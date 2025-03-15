using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Bina türlerini tanýmlayan enum
public enum BuildingType
{
    ResourceProduction,
    UnitProduction,
    Defense,
    Medical,
    Research,
    // Diðer bina türleri buraya eklenebilir
}

// Bina sýnýfý
public class Building : MonoBehaviour
{
    // Bina özellikleri
    public string buildingName;
    public BuildingType buildingType;
    public int health;
    public int buildGoldCost;
    public int buildFoodCost;
    public int buildIronCost;
    public int buildStoneCost;
    public int buildTimberCost;
    public float buildTime;

    public bool wasBuildCreated;

    // Ýþlevler
    public virtual void StartConstruction()
    {
        // Bina yapýmýný baþlatma kodu buraya gelebilir
    }

    public virtual void CompleteConstruction()
    {
        // Bina yapýmýný tamamlama kodu buraya gelebilir
    }

    public virtual void DestroyBuilding()
    {
        // Bina yok etme kodu buraya gelebilir
    }

    // Örnek iþlevler - Bina türüne baðlý olarak geniþletilebilir
    public virtual void ProduceResource()
    {
        // Kaynak üretme kodu buraya gelebilir
    }

    public virtual void ProduceUnit()
    {
        // Birim üretme kodu buraya gelebilir
    }
    public virtual void UpdateCosts()
    {
        // Varsayýlan maliyet güncelleme iþlemleri burada yapýlabilir.
        // Eðer her bina için genel bir kural varsa burada tanýmlanabilir.
    }

    // Diðer özellikler ve iþlevler buraya eklenebilir
}
