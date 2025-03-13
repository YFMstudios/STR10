using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;

[CreateAssetMenu(fileName = "GetPlayerData", menuName = "ScriptableObjects/GetPlayerData", order = 1)]
public class GetPlayerData : ScriptableObject
{
    public int currentSoldierAmount;  
    public int currentArcherAmount;

    public int CastleLevel = 0;
    public int TowerOneLevel = 0;
    public int TowerTwoLevel = 0;
    public int TrapOneLevel = 0;
    public int TrapTwoLevel = 0;
    public int TrapThreeLevel = 0;
    
    public bool TowerOneIsBuilded = false;//Kule1 �n�a Edildi Mi?
    public bool TowerTwoIsBuilded = false;//Kule2 �n�a Edildi Mi?
    public bool TrapOneIsBuilded = false;//Tuzak1 �n�a Edildi Mi?
    public bool TrapTwoIsBuilded = false;//Tuzak2 �n�a Edildi Mi?
    public bool TrapThreeIsBuilded = false;//Tuzak3 �n�a Edildi Mi?

    //----------------------------------- B�NA AKTF�LE�T�RME FONKS�YONLARI --------------------------------------------------------------//
    public void ActiveTowerOne()
    {
        TowerOneIsBuilded = true;
        Debug.Log("MainGame'de TowerOne �n�a Edildi. InGame'de aktifle�tirilmesi bekleniyor.");
    }

    public void ActiveTowerTwo()
    {
        TowerTwoIsBuilded=true;
        Debug.Log("MainGame'de TowerTwo �n�a Edildi. InGame'de aktifle�tirilmesi bekleniyor.");
    }

    public void ActiveTrapOne()
    {
        TrapOneIsBuilded = true;
        Debug.Log("MainGame'de TrapOne �n�a Edildi. InGame'de aktifle�tirilmesi bekleniyor.");
    }

    public void ActiveTrapTwo()
    {
        TrapTwoIsBuilded = true;
        Debug.Log("MainGame'de TrapTwo �n�a Edildi. InGame'de aktifle�tirilmesi bekleniyor.");
    }

    public void ActiveTrapThree()
    {
        TrapThreeIsBuilded = true;
        Debug.Log("MainGame'de TrapThree �n�a Edildi. InGame'de aktifle�tirilmesi bekleniyor.");
    }

    //---------------------------------------B�NA Y�KSELTME FONKS�YONLARI----------------------------------------------------------------

    public void UpgradeCastleStats(int level)
    {
        CastleLevel = level;
        if (CastleLevel == 1)
        {
            Debug.Log("Castle Bina Seviyesi 1 Oldu."); // Burada bi�ey yapmana gerek yok.
        }

        if (CastleLevel == 2)
        {
            Debug.Log("CastleBina Seviyesi 2 Oldu. Bina Stat'slar�n�n artt�r�lmas� bekleniyor.");
        }

        if (CastleLevel == 3)
        {
            Debug.Log("CastleBina Seviyesi 3 Oldu. Bina Stat'slar�n�n artt�r�lmas� bekleniyor.");
        }

    }

    public void UpgradeTowerOneStats(int level)
    {
        TowerOneLevel = level;
        if(TowerOneLevel == 1) 
        {
            Debug.Log("TowerOne Bina Seviyesi = 1");
        }

        else if(TowerOneLevel == 2)
        {
            Debug.Log("TowerOne Bina Seviyesi 2 Oldu. Bina Stat'slar�n�n artt�r�lmas� bekleniyor.");
        }
        else if(TowerOneLevel == 3)
        {
            Debug.Log("TowerOne Bina Seviyesi 3 Oldu. Bina Stat'slar�n�n artt�r�lmas� bekleniyor.");
        }
        else
        {
            Debug.Log("Ar�za Var");
        }
    }

    public void UpgradeTowerTwoStats(int level)
    {
        TowerTwoLevel = level;
        if (TowerTwoLevel == 1)
        {
            Debug.Log("TowerTwo Bina Seviyesi = 1");
        }

        else if (TowerTwoLevel == 2)
        {
            Debug.Log("TowerTwo Bina Seviyesi 2 Oldu. Bina Stat'slar�n�n artt�r�lmas� bekleniyor.");
        }
        else if (TowerTwoLevel == 3)
        {
            Debug.Log("TowerTwo Bina Seviyesi 3 Oldu. Bina Stat'slar�n�n artt�r�lmas� bekleniyor.");
        }
        else
        {
            Debug.Log("Ar�za Var");
        }
    }

    public void UpgradeTrapOneStats(int level)
    {
        TrapOneLevel = level;
        if(TrapOneLevel == 1)
        {
            Debug.Log("TrapOne Bina Seviyesi = 1");
        }
        else if (TrapOneLevel == 2)
        {
            Debug.Log("TrapOne Bina Seviyesi 2 Oldu. Bina Stat'slar�n�n artt�r�lmas� bekleniyor.");
        }
        else if (TrapOneLevel == 3)
        {
            Debug.Log("TrapOne Bina Seviyesi 3 Oldu. Bina Stat'slar�n�n artt�r�lmas� bekleniyor.");
        }
        else
        {
            Debug.Log("Ar�za Var");
        }
    }

    public void UpgradeTrapTwoStats(int level)
    {
        TrapTwoLevel = level;
        if (TrapTwoLevel == 1)
        {
            Debug.Log("TrapTwo Bina Seviyesi = 1");
        }
        else if (TrapTwoLevel == 2)
        {
            Debug.Log("TrapTwo Bina Seviyesi 2 Oldu. Bina Stat'slar�n�n artt�r�lmas� bekleniyor.");
        }
        else if (TrapTwoLevel == 3)
        {
            Debug.Log("TrapTwo Bina Seviyesi 3 Oldu. Bina Stat'slar�n�n artt�r�lmas� bekleniyor.");
        }
        else
        {
            Debug.Log("Ar�za Var");
        }
    }

    public void UpgradeTrapThreeStats(int level)
    {
        TrapThreeLevel = level;
        if (TrapThreeLevel == 1)
        {
            Debug.Log("TrapThree Bina Seviyesi = 1");
        }
        else if (TrapThreeLevel == 2)
        {
            Debug.Log("TrapThree Bina Seviyesi 2 Oldu. Bina Stat'slar�n�n artt�r�lmas� bekleniyor.");
        }
        else if (TrapThreeLevel == 3)
        {
            Debug.Log("TrapThree Bina Seviyesi 3 Oldu. Bina Stat'slar�n�n artt�r�lmas� bekleniyor.");
        }
        else
        {
            Debug.Log("Ar�za Var");
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------------
    public void UpdateSoldierAmount(float savasciSayisi, float okcuSayisi )
    {
        currentSoldierAmount = (int)savasciSayisi;
        currentArcherAmount = (int)okcuSayisi;
    }

}
