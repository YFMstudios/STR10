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
        // Kaynaklar�n mevcut miktarlar�n� kontrol et
        if (KaynakYoneticisi.GoldAmount >= requiredGold &&
            KaynakYoneticisi.FoodAmount >= requiredFood &&
            KaynakYoneticisi.IronAmount >= requiredIron &&
            KaynakYoneticisi.WoodAmount >= requiredWood &&
            KaynakYoneticisi.StoneAmount >= requiredStone)
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

        // Ara�t�rma seviyesine g�re maliyetleri belirle
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
                Debug.LogError("Ge�ersiz ara�t�rma seviyesi!");
                return false;
        }

        // Kaynaklar�n yeterli olup olmad���n� kontrol et
        if (AreResourcesSufficient(requiredGold, requiredFood, requiredIron, requiredWood, requiredStone))
        {
            Debug.Log($"Ara�t�rma seviyesi {researchLevel} i�in yeterli kaynak mevcut.");
            return true;
        }
        else
        {
            Debug.Log($"Ara�t�rma seviyesi {researchLevel} i�in yeterli kaynak yok!");
            return false;
        }
    }

    public void level1Research()
    {
        if (Lab.wasLabCreated == true)
        {
            if (CanStartResearch(1))
            {
                KaynakYoneticisi.GoldAmount -= 250;
                KaynakYoneticisi.FoodAmount -= 180;
                KaynakYoneticisi.IronAmount -= 220;
                KaynakYoneticisi.WoodAmount -= 130;
                KaynakYoneticisi.StoneAmount -= 180;

                imageColorTransition.StartColorTransitionSeviye1();
                Destroy(button[0].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar� �n�a Etmelisin.");
        }
    }

    public void level2Research()
    {
        if (Lab.buildLevel >= 1 && isResearched[0] && !isAnyResearchActive)
        {
            if (CanStartResearch(2))
            {
                KaynakYoneticisi.GoldAmount -= 200;
                KaynakYoneticisi.FoodAmount -= 350;
                KaynakYoneticisi.IronAmount -= 140;
                KaynakYoneticisi.WoodAmount -= 180;
                KaynakYoneticisi.StoneAmount -= 180;

                imageColorTransition.StartColorTransitionSeviye2();
                Destroy(button[1].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az seviye 1 oldu�undan ve Level 1 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level3Research()
    {
        Debug.Log("Level3 research fonksiyonuna girdim.");
        Debug.Log("Lab leveli: " + Lab.buildLevel);
        Debug.Log("1.arastırma durumu: " + isResearched[0]);
        Debug.Log("Herhangi bir arastırma aktif mi: " + isAnyResearchActive);
        if (Lab.buildLevel >= 1 && isResearched[0] && !isAnyResearchActive)
        {
                 Debug.Log("Level3 research fonksiyonundaki ifin icine girdim. girdim.");
            if (CanStartResearch(3))
            {
                KaynakYoneticisi.GoldAmount -= 350;
                KaynakYoneticisi.FoodAmount -= 220;
                KaynakYoneticisi.IronAmount -= 400;
                KaynakYoneticisi.WoodAmount -= 180;
                KaynakYoneticisi.StoneAmount -= 220;

                imageColorTransition.StartColorTransitionSeviye3();
                Destroy(button[2].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az seviye 1 oldu�undan ve Level 1 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level4Research()
    {
        if (Lab.buildLevel >= 1 && isResearched[1] && !isAnyResearchActive)
        {
            if (CanStartResearch(4))
            {
                KaynakYoneticisi.GoldAmount -= 180;
                KaynakYoneticisi.FoodAmount -= 270;
                KaynakYoneticisi.IronAmount -= 180;
                KaynakYoneticisi.WoodAmount -= 400;
                KaynakYoneticisi.StoneAmount -= 220;

                imageColorTransition.StartColorTransitionSeviye4();
                Destroy(button[3].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az seviye 1 oldu�undan ve Level 2 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level5Research()
    {
        if (Lab.buildLevel >= 1 && isResearched[2] && !isAnyResearchActive)
        {
            if (CanStartResearch(5))
            {
                KaynakYoneticisi.GoldAmount -= 250;
                KaynakYoneticisi.FoodAmount -= 180;
                KaynakYoneticisi.IronAmount -= 180;
                KaynakYoneticisi.WoodAmount -= 180;
                KaynakYoneticisi.StoneAmount -= 400;

                imageColorTransition.StartColorTransitionSeviye5();
                Destroy(button[4].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az seviye 1 oldu�undan ve Level 3 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level6Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[3] && isResearched[4] && !isAnyResearchActive)
        {
            if (CanStartResearch(6))
            {
                KaynakYoneticisi.GoldAmount -= 450;
                KaynakYoneticisi.FoodAmount -= 350;
                KaynakYoneticisi.IronAmount -= 350;
                KaynakYoneticisi.WoodAmount -= 270;
                KaynakYoneticisi.StoneAmount -= 270;

                imageColorTransition.StartColorTransitionSeviye6();
                Destroy(button[5].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az seviye 2 oldu�undan ve Level 4,5 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level7Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[3] && isResearched[4] && !isAnyResearchActive)
        {
            if (CanStartResearch(7))
            {
                KaynakYoneticisi.GoldAmount -= 350;
                KaynakYoneticisi.FoodAmount -= 270;
                KaynakYoneticisi.IronAmount -= 270;
                KaynakYoneticisi.WoodAmount -= 270;
                KaynakYoneticisi.StoneAmount -= 180;

                imageColorTransition.StartColorTransitionSeviye7();
                Destroy(button[6].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az seviye 2 oldu�undan ve Level 4,5 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    // Di�er ara�t�rma seviyeleri i�in ayn� �ekilde fonksiyonlar� olu�turabilirsiniz...


    public void level8Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[3] && isResearched[4] && !isAnyResearchActive)
        {
            if (CanStartResearch(8))
            {
                KaynakYoneticisi.GoldAmount -= 400;
                KaynakYoneticisi.FoodAmount -= 270;
                KaynakYoneticisi.IronAmount -= 180;
                KaynakYoneticisi.WoodAmount -= 180;
                KaynakYoneticisi.StoneAmount -= 130;

                imageColorTransition.StartColorTransitionSeviye8();
                Destroy(button[7].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az 2.seviye oldu�undan ve Level 4,5 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level9Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[5] && isResearched[6] && !isAnyResearchActive)
        {
            if (CanStartResearch(9))
            {
                KaynakYoneticisi.GoldAmount -= 550;
                KaynakYoneticisi.FoodAmount -= 300;
                KaynakYoneticisi.IronAmount -= 270;
                KaynakYoneticisi.WoodAmount -= 270;
                KaynakYoneticisi.StoneAmount -= 270;

                imageColorTransition.StartColorTransitionSeviye9();
                Destroy(button[8].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az 2.seviye oldu�undan ve Level 6,7 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level10Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[6] && isResearched[7] && !isAnyResearchActive)
        {
            if (CanStartResearch(10))
            {
                KaynakYoneticisi.GoldAmount -= 450;
                KaynakYoneticisi.FoodAmount -= 350;
                KaynakYoneticisi.IronAmount -= 450;
                KaynakYoneticisi.WoodAmount -= 350;
                KaynakYoneticisi.StoneAmount -= 350;

                imageColorTransition.StartColorTransitionSeviye10();
                Destroy(button[9].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az 2.seviye oldu�undan ve Level 7,8 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level11Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[8] && !isAnyResearchActive)
        {
            if (CanStartResearch(11))
            {
                KaynakYoneticisi.GoldAmount -= 600;
                KaynakYoneticisi.FoodAmount -= 400;
                KaynakYoneticisi.IronAmount -= 550;
                KaynakYoneticisi.WoodAmount -= 450;
                KaynakYoneticisi.StoneAmount -= 400;

                imageColorTransition.StartColorTransitionSeviye11();
                Destroy(button[10].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az 2.seviye oldu�undan ve Level 9 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level12Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[8] && isResearched[9] && !isAnyResearchActive)
        {
            if (CanStartResearch(12))
            {
                KaynakYoneticisi.GoldAmount -= 500;
                KaynakYoneticisi.FoodAmount -= 350;
                KaynakYoneticisi.IronAmount -= 300;
                KaynakYoneticisi.WoodAmount -= 400;
                KaynakYoneticisi.StoneAmount -= 300;

                imageColorTransition.StartColorTransitionSeviye12();
                Destroy(button[11].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az 2.seviye oldu�undan ve Level 9,10 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level13Research()
    {
        if (Lab.buildLevel >= 2 && isResearched[9] && !isAnyResearchActive)
        {
            if (CanStartResearch(13))
            {
                KaynakYoneticisi.GoldAmount -= 550;
                KaynakYoneticisi.FoodAmount -= 250;
                KaynakYoneticisi.IronAmount -= 500;
                KaynakYoneticisi.WoodAmount -= 400;
                KaynakYoneticisi.StoneAmount -= 350;

                imageColorTransition.StartColorTransitionSeviye13();
                Destroy(button[12].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az 2.seviye oldu�undan ve Level 10 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level14Research()
    {
        if (Lab.buildLevel >= 3 && isResearched[10] && isResearched[11] && !isAnyResearchActive)
        {
            if (CanStartResearch(14))
            {
                KaynakYoneticisi.GoldAmount -= 450;
                KaynakYoneticisi.FoodAmount -= 400;
                KaynakYoneticisi.IronAmount -= 400;
                KaynakYoneticisi.WoodAmount -= 300;
                KaynakYoneticisi.StoneAmount -= 300;

                imageColorTransition.StartColorTransitionSeviye14();
                Destroy(button[13].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az 3.seviye oldu�undan ve Level 11,12 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level15Research()
    {
        if (Lab.buildLevel >= 3 && isResearched[11] && isResearched[12] && !isAnyResearchActive)
        {
            if (CanStartResearch(15))
            {
                KaynakYoneticisi.GoldAmount -= 400;
                KaynakYoneticisi.FoodAmount -= 400;
                KaynakYoneticisi.IronAmount -= 300;
                KaynakYoneticisi.WoodAmount -= 450;
                KaynakYoneticisi.StoneAmount -= 500;

                imageColorTransition.StartColorTransitionSeviye15();
                Destroy(button[14].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az 3.seviye oldu�undan ve Level 12,13 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level16Research()
    {
        if (Lab.buildLevel >= 3 && isResearched[13] && !isAnyResearchActive)
        {
            if (CanStartResearch(16))
            {
                KaynakYoneticisi.GoldAmount -= 500;
                KaynakYoneticisi.FoodAmount -= 300;
                KaynakYoneticisi.IronAmount -= 200;
                KaynakYoneticisi.WoodAmount -= 200;
                KaynakYoneticisi.StoneAmount -= 300;

                imageColorTransition.StartColorTransitionSeviye16();
                Destroy(button[15].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az 3.seviye oldu�undan ve Level 14 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }
    public void level17Research()
    {
        if (Lab.buildLevel >= 3 && isResearched[14] && !isAnyResearchActive)
        {
            if (CanStartResearch(17))
            {
                KaynakYoneticisi.GoldAmount -= 450;
                KaynakYoneticisi.FoodAmount -= 400;
                KaynakYoneticisi.IronAmount -= 350;
                KaynakYoneticisi.WoodAmount -= 450;
                KaynakYoneticisi.StoneAmount -= 350;

                imageColorTransition.StartColorTransitionSeviye17();
                Destroy(button[16].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az 3.seviye oldu�undan ve Level 15 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }

    public void level18Research()
    {
        if (Lab.buildLevel >= 3 && isResearched[15] && isResearched[16] && !isAnyResearchActive)
        {
            if (CanStartResearch(18))
            {
                KaynakYoneticisi.GoldAmount -= 600;
                KaynakYoneticisi.FoodAmount -= 500;
                KaynakYoneticisi.IronAmount -= 450;
                KaynakYoneticisi.WoodAmount -= 500;
                KaynakYoneticisi.StoneAmount -= 400;

                imageColorTransition.StartColorTransitionSeviye18();
                Destroy(button[17].gameObject);
            }
            else
            {
                Debug.Log("Yeterli Kaynak Bulunmamaktad�r.");
            }
        }
        else
        {
            Debug.Log("Laboratuvar�n en az 3.seviye oldu�undan ve Level 16,17 Ara�t�rmas�n� yapt���n�zdan emin olun!");
        }
    }





}


