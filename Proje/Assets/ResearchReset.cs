using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchReset : MonoBehaviour
{
    // Start is called before the first frame update
   void Start()
{
    for (int i = 0; i < ResearchResetter.isResearched.Length; i++)
    {
        ResearchResetter.isResearched[i] = false;
    }

    Debug.Log("Tüm araştırmalar sıfırlandı.");
    ResearchButtonEvents.isAnyResearchActive = false;
}

}
