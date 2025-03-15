using Photon.Pun;
using UnityEngine;

public class KingdomResourceUpdater : MonoBehaviour
{
    private int spriteNum;

    // Krallıkların kaynakları
    private void UpdateKingdomResources()
    {
        // Krallık bilgilerine göre kaynakları Photon'a gönder
        if (spriteNum == 2)
        {
            // Akhadzria krallığının kaynakları
            UpdatePhotonProperties(Kingdom.Kingdoms[2]);
        }
        else if (spriteNum == 3)
        {
            // Alfgard krallığının kaynakları
            UpdatePhotonProperties(Kingdom.Kingdoms[1]);
        }
        else if (spriteNum == 4)
        {
            // Arianopol krallığının kaynakları
            UpdatePhotonProperties(Kingdom.Kingdoms[0]);
        }
        else if (spriteNum == 5)
        {
            // Dhamuron krallığının kaynakları
            UpdatePhotonProperties(Kingdom.Kingdoms[3]);
        }
        else if (spriteNum == 6)
        {
            // Lexion krallığının kaynakları
            UpdatePhotonProperties(Kingdom.Kingdoms[4]);
        }
        else
        {
            Debug.Log("Seçili Krallık Bulunmuyor");
            // Varsayılan olarak Alfgard'ın kaynakları
            UpdatePhotonProperties(Kingdom.Kingdoms[1]);
        }
    }

    // Bu fonksiyon krallık verilerini Photon'a gönderiyor
    private void UpdatePhotonProperties(Kingdom kingdom)
    {
        // Krallığın değerlerini customProperties olarak güncelle
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["FoodAmount"] = kingdom.FoodAmount;
        customProperties["StoneAmount"] = kingdom.StoneAmount;
        customProperties["GoldAmount"] = kingdom.GoldAmount;
        customProperties["WoodAmount"] = kingdom.WoodAmount;
        customProperties["IronAmount"] = kingdom.IronAmount;
        customProperties["WarPower"] = kingdom.WarPower;

        // Photon'a bu değerleri gönder
        PhotonNetwork.SetPlayerCustomProperties(customProperties);
    }

    // Start metodunda bu fonksiyonu çağırıyoruz
    void Start()
    {
        spriteNum = GetVariableFromHere.currentSpriteNum;
        UpdateKingdomResources();
    }

    // Bu fonksiyon, custom propertyleri sürekli güncellemek için bir Update metodunda kullanılabilir
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateKingdomResources();
        }
    }
}
