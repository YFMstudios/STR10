using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelTextController : MonoBehaviour//Bozulan
{
    // Sol üst bayrak Scripti
    public int spriteNum;
    public TMP_Text kingdomName;
    public TMP_Text foodAmount;
    public TMP_Text stoneAmount;
    public TMP_Text goldAmount;
    public TMP_Text woodAmount;
    public TMP_Text ironAmount;
    public TMP_Text warPower;


    public Image imageComponent;



    void Start()
    {

        spriteNum = GetVariableFromHere.currentSpriteNum;
        if (spriteNum == 2)
        {
            kingdomName.text = "Akhadzria";
            imageComponent.sprite = Kingdom.Kingdoms[2].Flag;
            foodAmount.text = KaynakYoneticisi.FoodAmount.ToString();
            stoneAmount.text = KaynakYoneticisi.StoneAmount.ToString();
            goldAmount.text = KaynakYoneticisi.GoldAmount.ToString();
            woodAmount.text = KaynakYoneticisi.WoodAmount.ToString();
            ironAmount.text = KaynakYoneticisi.IronAmount.ToString();
            warPower.text = KaynakYoneticisi.WarPower.ToString();
            Kingdom.myKingdom = Kingdom.Kingdoms[2];

        }
        else if (spriteNum == 3)
        {
            kingdomName.text = "Alfgard";
            imageComponent.sprite = Kingdom.Kingdoms[1].Flag;
            foodAmount.text = KaynakYoneticisi.FoodAmount.ToString();
            stoneAmount.text = KaynakYoneticisi.StoneAmount.ToString();
            goldAmount.text = KaynakYoneticisi.GoldAmount.ToString();
            woodAmount.text = KaynakYoneticisi.WoodAmount.ToString();
            ironAmount.text = KaynakYoneticisi.IronAmount.ToString();
            warPower.text = KaynakYoneticisi.WarPower.ToString();
            Kingdom.myKingdom = Kingdom.Kingdoms[1];
        }
        else if (spriteNum == 4)
        {
            kingdomName.text = "Arianopol";
            imageComponent.sprite = Kingdom.Kingdoms[0].Flag;
             foodAmount.text = KaynakYoneticisi.FoodAmount.ToString();
            stoneAmount.text = KaynakYoneticisi.StoneAmount.ToString();
            goldAmount.text = KaynakYoneticisi.GoldAmount.ToString();
            woodAmount.text = KaynakYoneticisi.WoodAmount.ToString();
            ironAmount.text = KaynakYoneticisi.IronAmount.ToString();
            warPower.text = KaynakYoneticisi.WarPower.ToString();
            Kingdom.myKingdom = Kingdom.Kingdoms[0];
        }
        else if (spriteNum == 5)
        {
            kingdomName.text = "Dhamuron";
            imageComponent.sprite = Kingdom.Kingdoms[3].Flag;
            foodAmount.text = KaynakYoneticisi.FoodAmount.ToString();
            stoneAmount.text = KaynakYoneticisi.StoneAmount.ToString();
            goldAmount.text = KaynakYoneticisi.GoldAmount.ToString();
            woodAmount.text = KaynakYoneticisi.WoodAmount.ToString();
            ironAmount.text = KaynakYoneticisi.IronAmount.ToString();
            warPower.text = KaynakYoneticisi.WarPower.ToString();
            Kingdom.myKingdom = Kingdom.Kingdoms[3];
        }
        else if (spriteNum == 6)
        {
            kingdomName.text = "Lexion";
            imageComponent.sprite = Kingdom.Kingdoms[4].Flag;
             foodAmount.text = KaynakYoneticisi.FoodAmount.ToString();
            stoneAmount.text = KaynakYoneticisi.StoneAmount.ToString();
            goldAmount.text = KaynakYoneticisi.GoldAmount.ToString();
            woodAmount.text = KaynakYoneticisi.WoodAmount.ToString();
            ironAmount.text = KaynakYoneticisi.IronAmount.ToString();
            warPower.text = KaynakYoneticisi.WarPower.ToString();
            Kingdom.myKingdom = Kingdom.Kingdoms[4];
        }
        else
        {
            
            kingdomName.text = "Zephrion";
            imageComponent.sprite = Kingdom.Kingdoms[5].Flag;
             foodAmount.text = KaynakYoneticisi.FoodAmount.ToString();
            stoneAmount.text = KaynakYoneticisi.StoneAmount.ToString();
            goldAmount.text = KaynakYoneticisi.GoldAmount.ToString();
            woodAmount.text = KaynakYoneticisi.WoodAmount.ToString();
            ironAmount.text = KaynakYoneticisi.IronAmount.ToString();
            warPower.text = KaynakYoneticisi.WarPower.ToString();
            Kingdom.myKingdom = Kingdom.Kingdoms[5];
        }
    }

    void Update()
    {
        refreshKingdomResources();
    }

    public void refreshKingdomResources()
    {

        foodAmount.text = KaynakYoneticisi.FoodAmount.ToString();
            stoneAmount.text = KaynakYoneticisi.StoneAmount.ToString();
            goldAmount.text = KaynakYoneticisi.GoldAmount.ToString();
            woodAmount.text = KaynakYoneticisi.WoodAmount.ToString();
            ironAmount.text = KaynakYoneticisi.IronAmount.ToString();
            warPower.text = KaynakYoneticisi.WarPower.ToString();
    }

    
}
