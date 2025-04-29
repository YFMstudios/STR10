using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

[CreateAssetMenu(fileName = "KaynakYoneticisi", menuName = "ScriptableObjects/KaynakYoneticisi")]
public class KaynakYoneticisi : ScriptableObject
{
    public static int FoodAmount   = 100000;
    public static int StoneAmount  = 100000;
    public static int GoldAmount   = 100000;
    public static int WoodAmount   = 100000;
    public static int IronAmount   = 100000;
    public static int WarPower     = 0;

    public  bool needsSync = false; // <<< EKLENDİ!

    public void SyncToPhoton()
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.LocalPlayer == null) return;

        var props = new Hashtable
        {
            { "FoodAmount",   FoodAmount },
            { "StoneAmount",  StoneAmount },
            { "GoldAmount",   GoldAmount },
            { "WoodAmount",   WoodAmount },
            { "IronAmount",   IronAmount },
            { "WarPower",     WarPower }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void WarPowerArttirma(int warPower)
    {
        WarPower += warPower;
        needsSync = true; // <<< EKLENDİ!
    }

    // BURASI YENİ: Dışarıdan çağrılan sync kontrolcüsü
    public void TrySync()
    {
        if (needsSync)
        {
            SyncToPhoton();
            needsSync = false;
        }
    }

    // Kaynak arttırma/azaltma yapan her fonksiyonun sonunda needsSync = true; olacak!
    public void AddGold(int amount)
    {
        GoldAmount += amount;
        needsSync = true;
    }

    public void ReduceGold(int amount)
    {
        GoldAmount -= amount;
        needsSync = true;
    }

    // Aynı şekilde Food, Wood, Stone, Iron için de ekleyeceğiz.
    // (istersen devamını ister misin yazayım?)

}
