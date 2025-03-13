 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchButtonEvents : MonoBehaviour
{
    public static bool[] isResearched = new bool[18];
    public Image[] researchItems = new Image[18];
    public Button[] button = new Button[18];
    public static bool isAnyResearchActive = false;

    public ImageColorTransition imageColorTransition;

    public bool AreResourcesSufficient(float requiredGold, float requiredFood, float requiredIron, float requiredWood, float requiredStone)
    {
        // Kaynaklarýn mevcut miktarlarýný kontrol et
        if (Kingdom.myKingdom.GoldAmount >= requiredGold &&
            Kingdom.myKingdom.FoodAmount >= requiredFood &&
            Kingdom.myKingdom.IronAmount >= requiredIron &&
            Kingdom.myKingdom.WoodAmount >= requiredWood &&
            Kingdom.myKingdom.StoneAmount >= requiredStone)
        {
            return true; // Yeterli kaynak mevcut
        }
        else
        {
            return false; // Yeterli kaynak yok
        }
    }


    public bool CanStartResearch(int researchLevel)
    {
        float requiredGold = 0;
        float requiredFood = 0;
        float requiredIron = 0;
        float requiredWood = 0;
        float requiredStone = 0;

        // Araþtýrma seviyesine göre maliyetleri belirle
        switch (researchLevel)
        {
            case 1:
                requiredGold = 250;
                requiredFood = 180;
                requiredIron = 220;
                requiredWood = 130;
                requiredStone = 180;
                break;
            case 2:
                requiredGold = 200;
                requiredFood = 350;
                requiredIron = 140;
                requiredWood = 180;
                requiredStone = 180;
                break;
            case 3:
                requiredGold = 350;
                requiredFood = 220;
                requiredIron = 400;
                requiredWood = 180;
                requiredStone = 220;
                break;
            case 4:
                requiredGold = 180;
                requiredFood = 270;
                requiredIron = 180;
                requiredWood = 400;
                requiredStone = 220;
                break;
            case 5:
                requiredGold = 250;
                requiredFood = 180;
                requiredIron = 180;
                requiredWood = 180;
                requiredStone = 400;
                break;
            case 6:
                requiredGold = 450;
                requiredFood = 350;
                requiredIron = 350;
                requiredWood = 270;
                requiredStone = 270;
                break;
            case 7:
                requiredGold = 350;
                requiredFood = 270;
                requiredIron = 270;
                requiredWood = 270;
                requiredStone = 180;
                break;
            case 8:
                requiredGold = 400;
                requiredFood = 270;
                requiredIron = 180;
                requiredWood = 180;
                requiredStone = 130;
                break;
            case 9:
                requiredGold = 550;
                requiredFood = 300;
                requiredIron = 270;
                requiredWood = 270;
                requiredStone = 270;
                break;
            case 10:
                requiredGold = 450;
                requiredFood = 350;
                requiredIron = 450;
                requiredWood = 350;
                requiredStone = 350;
                break;
            case 11:
                requiredGold = 600;
                requiredFood = 400;
                requiredIron = 550;
                requiredWood = 450;
                requiredStone = 400;
                break;
            case 12:
                requiredGold = 500;
                requiredFood = 350;
                requiredIron = 300;
                requiredWood = 400;
                requiredStone = 300;
                break;
            case 13:
                requiredGold = 550;
                requiredFood = 250;
                requiredIron = 500;
                requiredWood = 400;
                requiredStone = 350;
                break;
            case 14:
                requiredGold = 450;
                requiredFood = 400;
                requiredIron = 400;
                requiredWood = 300;
                requiredStone = 300;
                break;
            case 15:
                requiredGold = 400;
                requiredFood = 400;
                requiredIron = 300;
                requiredWood = 450;
                requiredStone = 500;
                break;
            case 16:
                requiredGold = 500;
                requiredFood = 300;
                requiredIron = 200;
                requiredWood = 200;
                requiredStone = 300;
                break;
            case 17:
                requiredGold = 450;
                requiredFood = 400;
                requiredIron = 350;
                requiredWood = 450;
                requiredStone = 350;
                break;
            case 18:
                requiredGold = 600;
                requiredFood = 500;
                requiredIron = 450;
                requiredWood = 500;
                requiredStone = 400;
                break;
            default:
                Debug.LogError("Geçersiz araþtýrma seviyesi!");
                return false;
        }

        // Kaynaklarýn yeterli olup olmadýðýný kontrol et
        if (AreResourcesSufficient(requiredGold, requiredFood, requiredIron, requiredWood, requiredStone))
        {
            Debug.Log($"Araþtýrma seviyesi {researchLevel} için yeterli kaynak mevcut.");
            return true;
        }
        else
        {
            Debug.Log($"Araþtýrma seviyesi {researchLevel} için yeterli kaynak yok!");
            return false;
        }
    }

    public void level1Research()
    {
        if (Lab.wasLabCreated == true)
        {
            if (CanStartResearch(1))
            {
                Kingdom.myKingdom.GoldAmount -= 250;
                Kingdom.myKingdom.FoodAmount -= 180;
                Kingdom.myKingdom.IronAmount -= 220;
                Kingdom.myKingdom.WoodAmount -= 130;
                Kingdom.myKingdom.StoneAmount -= 180;

                imageColorTransition.StartColorTransitionSeviye1();
                Destroy(button[0].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarý Ýnþa Etmelisin.");
        }
    }

    public void level2Research()
    {
        if (Lab.buildLevel >= 1 && isResearched[0] && !isAnyResearchActive)
        {
            if (CanStartResearch(2))
            {
                Kingdom.myKingdom.GoldAmount -= 200;
                Kingdom.myKingdom.FoodAmount -= 350;
                Kingdom.myKingdom.IronAmount -= 140;
                Kingdom.myKingdom.WoodAmount -= 180;
                Kingdom.myKingdom.StoneAmount -= 180;

                imageColorTransition.StartColorTransitionSeviye2();
                Destroy(button[1].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az seviye 1 olduðundan ve Level 1 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level3Research()
    {
        if (Lab.buildLevel >= 1 && isResearched[0] && !isAnyResearchActive)
        {
            if (CanStartResearch(3))
            {
                Kingdom.myKingdom.GoldAmount -= 350;
                Kingdom.myKingdom.FoodAmount -= 220;
                Kingdom.myKingdom.IronAmount -= 400;
                Kingdom.myKingdom.WoodAmount -= 180;
                Kingdom.myKingdom.StoneAmount -= 220;

                imageColorTransition.StartColorTransitionSeviye3();
                Destroy(button[2].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az seviye 1 olduðundan ve Level 1 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level4Research()
    {
        if (Lab.buildLevel >= 1 && isResearched[1] && !isAnyResearchActive)
        {
            if (CanStartResearch(4))
            {
                Kingdom.myKingdom.GoldAmount -= 180;
                Kingdom.myKingdom.FoodAmount -= 270;
                Kingdom.myKingdom.IronAmount -= 180;
                Kingdom.myKingdom.WoodAmount -= 400;
                Kingdom.myKingdom.StoneAmount -= 220;

                imageColorTransition.StartColorTransitionSeviye4();
                Destroy(button[3].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az seviye 1 olduðundan ve Level 2 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level5Research()
    {
        if (Lab.buildLevel >= 1 && isResearched[2] && !isAnyResearchActive)
        {
            if (CanStartResearch(5))
            {
                Kingdom.myKingdom.GoldAmount -= 250;
                Kingdom.myKingdom.FoodAmount -= 180;
                Kingdom.myKingdom.IronAmount -= 180;
                Kingdom.myKingdom.WoodAmount -= 180;
                Kingdom.myKingdom.StoneAmount -= 400;

                imageColorTransition.StartColorTransitionSeviye5();
                Destroy(button[4].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az seviye 1 olduðundan ve Level 3 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level6Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[3] && isResearched[4] && !isAnyResearchActive)
        {
            if (CanStartResearch(6))
            {
                Kingdom.myKingdom.GoldAmount -= 450;
                Kingdom.myKingdom.FoodAmount -= 350;
                Kingdom.myKingdom.IronAmount -= 350;
                Kingdom.myKingdom.WoodAmount -= 270;
                Kingdom.myKingdom.StoneAmount -= 270;

                imageColorTransition.StartColorTransitionSeviye6();
                Destroy(button[5].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az seviye 2 olduðundan ve Level 4,5 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level7Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[3] && isResearched[4] && !isAnyResearchActive)
        {
            if (CanStartResearch(7))
            {
                Kingdom.myKingdom.GoldAmount -= 350;
                Kingdom.myKingdom.FoodAmount -= 270;
                Kingdom.myKingdom.IronAmount -= 270;
                Kingdom.myKingdom.WoodAmount -= 270;
                Kingdom.myKingdom.StoneAmount -= 180;

                imageColorTransition.StartColorTransitionSeviye7();
                Destroy(button[6].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az seviye 2 olduðundan ve Level 4,5 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    // Diðer araþtýrma seviyeleri için ayný þekilde fonksiyonlarý oluþturabilirsiniz...


    public void level8Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[3] && isResearched[4] && !isAnyResearchActive)
        {
            if (CanStartResearch(8))
            {
                Kingdom.myKingdom.GoldAmount -= 400;
                Kingdom.myKingdom.FoodAmount -= 270;
                Kingdom.myKingdom.IronAmount -= 180;
                Kingdom.myKingdom.WoodAmount -= 180;
                Kingdom.myKingdom.StoneAmount -= 130;

                imageColorTransition.StartColorTransitionSeviye8();
                Destroy(button[7].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az 2.seviye olduðundan ve Level 4,5 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level9Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[5] && isResearched[6] && !isAnyResearchActive)
        {
            if (CanStartResearch(9))
            {
                Kingdom.myKingdom.GoldAmount -= 550;
                Kingdom.myKingdom.FoodAmount -= 300;
                Kingdom.myKingdom.IronAmount -= 270;
                Kingdom.myKingdom.WoodAmount -= 270;
                Kingdom.myKingdom.StoneAmount -= 270;

                imageColorTransition.StartColorTransitionSeviye9();
                Destroy(button[8].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az 2.seviye olduðundan ve Level 6,7 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level10Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[6] && isResearched[7] && !isAnyResearchActive)
        {
            if (CanStartResearch(10))
            {
                Kingdom.myKingdom.GoldAmount -= 450;
                Kingdom.myKingdom.FoodAmount -= 350;
                Kingdom.myKingdom.IronAmount -= 450;
                Kingdom.myKingdom.WoodAmount -= 350;
                Kingdom.myKingdom.StoneAmount -= 350;

                imageColorTransition.StartColorTransitionSeviye10();
                Destroy(button[9].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az 2.seviye olduðundan ve Level 7,8 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level11Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[8] && !isAnyResearchActive)
        {
            if (CanStartResearch(11))
            {
                Kingdom.myKingdom.GoldAmount -= 600;
                Kingdom.myKingdom.FoodAmount -= 400;
                Kingdom.myKingdom.IronAmount -= 550;
                Kingdom.myKingdom.WoodAmount -= 450;
                Kingdom.myKingdom.StoneAmount -= 400;

                imageColorTransition.StartColorTransitionSeviye11();
                Destroy(button[10].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az 2.seviye olduðundan ve Level 9 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level12Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[8] && isResearched[9] && !isAnyResearchActive)
        {
            if (CanStartResearch(12))
            {
                Kingdom.myKingdom.GoldAmount -= 500;
                Kingdom.myKingdom.FoodAmount -= 350;
                Kingdom.myKingdom.IronAmount -= 300;
                Kingdom.myKingdom.WoodAmount -= 400;
                Kingdom.myKingdom.StoneAmount -= 300;

                imageColorTransition.StartColorTransitionSeviye12();
                Destroy(button[11].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az 2.seviye olduðundan ve Level 9,10 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level13Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[9] && !isAnyResearchActive)
        {
            if (CanStartResearch(13))
            {
                Kingdom.myKingdom.GoldAmount -= 550;
                Kingdom.myKingdom.FoodAmount -= 250;
                Kingdom.myKingdom.IronAmount -= 500;
                Kingdom.myKingdom.WoodAmount -= 400;
                Kingdom.myKingdom.StoneAmount -= 350;

                imageColorTransition.StartColorTransitionSeviye13();
                Destroy(button[12].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az 2.seviye olduðundan ve Level 10 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level14Research()
    {
        if (Lab.buildLevel >= 3 && isResearched[10] && isResearched[11] && !isAnyResearchActive)
        {
            if (CanStartResearch(14))
            {
                Kingdom.myKingdom.GoldAmount -= 450;
                Kingdom.myKingdom.FoodAmount -= 400;
                Kingdom.myKingdom.IronAmount -= 400;
                Kingdom.myKingdom.WoodAmount -= 300;
                Kingdom.myKingdom.StoneAmount -= 300;

                imageColorTransition.StartColorTransitionSeviye14();
                Destroy(button[13].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az 3.seviye olduðundan ve Level 11,12 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level15Research()
    {
        if (Lab.buildLevel >= 3 && isResearched[11] && isResearched[12] && !isAnyResearchActive)
        {
            if (CanStartResearch(15))
            {
                Kingdom.myKingdom.GoldAmount -= 400;
                Kingdom.myKingdom.FoodAmount -= 400;
                Kingdom.myKingdom.IronAmount -= 300;
                Kingdom.myKingdom.WoodAmount -= 450;
                Kingdom.myKingdom.StoneAmount -= 500;

                imageColorTransition.StartColorTransitionSeviye15();
                Destroy(button[14].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az 3.seviye olduðundan ve Level 12,13 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level16Research()
    {
        if (Lab.buildLevel >= 3 && isResearched[13] && !isAnyResearchActive)
        {
            if (CanStartResearch(16))
            {
                Kingdom.myKingdom.GoldAmount -= 500;
                Kingdom.myKingdom.FoodAmount -= 300;
                Kingdom.myKingdom.IronAmount -= 200;
                Kingdom.myKingdom.WoodAmount -= 200;
                Kingdom.myKingdom.StoneAmount -= 300;

                imageColorTransition.StartColorTransitionSeviye16();
                Destroy(button[15].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az 3.seviye olduðundan ve Level 14 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }
    public void level17Research()
    {
        if (Lab.buildLevel >= 3 && isResearched[14] && !isAnyResearchActive)
        {
            if (CanStartResearch(17))
            {
                Kingdom.myKingdom.GoldAmount -= 450;
                Kingdom.myKingdom.FoodAmount -= 400;
                Kingdom.myKingdom.IronAmount -= 350;
                Kingdom.myKingdom.WoodAmount -= 450;
                Kingdom.myKingdom.StoneAmount -= 350;

                imageColorTransition.StartColorTransitionSeviye17();
                Destroy(button[16].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az 3.seviye olduðundan ve Level 15 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }

    public void level18Research()
    {
        if (Lab.buildLevel >= 3 && isResearched[15] && isResearched[16] && !isAnyResearchActive)
        {
            if (CanStartResearch(18))
            {
                Kingdom.myKingdom.GoldAmount -= 600;
                Kingdom.myKingdom.FoodAmount -= 500;
                Kingdom.myKingdom.IronAmount -= 450;
                Kingdom.myKingdom.WoodAmount -= 500;
                Kingdom.myKingdom.StoneAmount -= 400;

                imageColorTransition.StartColorTransitionSeviye18();
                Destroy(button[17].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktadýr.");
            }
        }
        else
        {
            Debug.Log("Laboratuvarýn en az 3.seviye olduðundan ve Level 16,17 Araþtýrmasýný yaptýðýnýzdan emin olun!");
        }
    }





}


