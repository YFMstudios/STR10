using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSources : MonoBehaviour
{
    
    void Start()
    {
        KaynakYoneticisi.FoodAmount=100000;
        KaynakYoneticisi.IronAmount=100000;
        KaynakYoneticisi.WarPower=100000;
        KaynakYoneticisi.WoodAmount=100000;
        KaynakYoneticisi.StoneAmount=100000;
        KaynakYoneticisi.GoldAmount=100000;

    }

    
}
