using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCountOfSoldiers : MonoBehaviour
{
    //Di�er sahneden asker say�s�n� �ekme.
    float countSoldier;

    void Start()
    {
        countSoldier = Kingdom.myKingdom.SoldierAmount;    
        Debug.Log("Asker Say�n�z" +  countSoldier);
    }
}
