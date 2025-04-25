using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSifirlama : MonoBehaviour
{
    public ResearchActions researchActions;
    
    void Start()
    {
      researchActions.MinionDamageSifirlama();
      researchActions.KaleKuleCanSifirlama();
      researchActions.MinionHareketHiziSifirlama();
      researchActions.KarakterCanVeHasarSifirlama();
      researchActions.TrapHasarSifirlama();
    }
}
