using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelTextController : MonoBehaviour//Bozulan
{

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
            foodAmount.text = Kingdom.Kingdoms[2].FoodAmount.ToString();
            stoneAmount.text = Kingdom.Kingdoms[2].StoneAmount.ToString();
            goldAmount.text = Kingdom.Kingdoms[2].GoldAmount.ToString();
            woodAmount.text = Kingdom.Kingdoms[2].WoodAmount.ToString();
            ironAmount.text = Kingdom.Kingdoms[2].IronAmount.ToString();
            warPower.text = Kingdom.Kingdoms[2].WarPower.ToString();
            Kingdom.myKingdom = Kingdom.Kingdoms[2];

        }
        else if (spriteNum == 3)
        {
            kingdomName.text = "Alfgard";
            imageComponent.sprite = Kingdom.Kingdoms[1].Flag;
            foodAmount.text = Kingdom.Kingdoms[1].FoodAmount.ToString();
            stoneAmount.text = Kingdom.Kingdoms[1].StoneAmount.ToString();
            goldAmount.text = Kingdom.Kingdoms[1].GoldAmount.ToString();
            woodAmount.text = Kingdom.Kingdoms[1].WoodAmount.ToString();
            ironAmount.text = Kingdom.Kingdoms[1].IronAmount.ToString();
            warPower.text = Kingdom.Kingdoms[1].WarPower.ToString();
            Kingdom.myKingdom = Kingdom.Kingdoms[1];
        }
        else if (spriteNum == 4)
        {
            kingdomName.text = "Arianopol";
            imageComponent.sprite = Kingdom.Kingdoms[0].Flag;
            foodAmount.text = Kingdom.Kingdoms[0].FoodAmount.ToString();
            stoneAmount.text = Kingdom.Kingdoms[0].StoneAmount.ToString();
            goldAmount.text = Kingdom.Kingdoms[0].GoldAmount.ToString();
            woodAmount.text = Kingdom.Kingdoms[0].WoodAmount.ToString();
            ironAmount.text = Kingdom.Kingdoms[0].IronAmount.ToString();
            warPower.text = Kingdom.Kingdoms[0].WarPower.ToString();
            Kingdom.myKingdom = Kingdom.Kingdoms[0];
        }
        else if (spriteNum == 5)
        {
            kingdomName.text = "Dhamuron";
            imageComponent.sprite = Kingdom.Kingdoms[3].Flag;
            foodAmount.text = Kingdom.Kingdoms[3].FoodAmount.ToString();
            stoneAmount.text = Kingdom.Kingdoms[3].StoneAmount.ToString();
            goldAmount.text = Kingdom.Kingdoms[3].GoldAmount.ToString();
            woodAmount.text = Kingdom.Kingdoms[3].WoodAmount.ToString();
            ironAmount.text = Kingdom.Kingdoms[3].IronAmount.ToString();
            warPower.text = Kingdom.Kingdoms[3].WarPower.ToString();
            Kingdom.myKingdom = Kingdom.Kingdoms[3];
        }
        else if (spriteNum == 6)
        {
            kingdomName.text = "Lexion";
            imageComponent.sprite = Kingdom.Kingdoms[4].Flag;
            foodAmount.text = Kingdom.Kingdoms[4].FoodAmount.ToString();
            stoneAmount.text = Kingdom.Kingdoms[4].StoneAmount.ToString();
            goldAmount.text = Kingdom.Kingdoms[4].GoldAmount.ToString();
            woodAmount.text = Kingdom.Kingdoms[4].WoodAmount.ToString();
            ironAmount.text = Kingdom.Kingdoms[4].IronAmount.ToString();
            warPower.text = Kingdom.Kingdoms[4].WarPower.ToString();
            Kingdom.myKingdom = Kingdom.Kingdoms[4];
        }
        else
        {
            Debug.Log("Se�ili Krall�k Bulunmuyor");
            kingdomName.text = "Alfgard";
            imageComponent.sprite = Kingdom.Kingdoms[1].Flag;
            foodAmount.text = Kingdom.Kingdoms[1].FoodAmount.ToString();
            stoneAmount.text = Kingdom.Kingdoms[1].StoneAmount.ToString();
            goldAmount.text = Kingdom.Kingdoms[1].GoldAmount.ToString();
            woodAmount.text = Kingdom.Kingdoms[1].WoodAmount.ToString();
            ironAmount.text = Kingdom.Kingdoms[1].IronAmount.ToString();
            warPower.text = Kingdom.Kingdoms[1].WarPower.ToString();
            Kingdom.myKingdom = Kingdom.Kingdoms[1];
        }
    }

    void Update()
    {
        refreshKingdomResources();
    }

    public void refreshKingdomResources()
    {

        foodAmount.text = Kingdom.myKingdom.FoodAmount.ToString();
        stoneAmount.text = Kingdom.myKingdom.StoneAmount.ToString();
        goldAmount.text = Kingdom.myKingdom.GoldAmount.ToString();
        woodAmount.text = Kingdom.myKingdom.WoodAmount.ToString();
        ironAmount.text = Kingdom.myKingdom.IronAmount.ToString();
        warPower.text = Kingdom.myKingdom.WarPower.ToString();
    }

    
}
