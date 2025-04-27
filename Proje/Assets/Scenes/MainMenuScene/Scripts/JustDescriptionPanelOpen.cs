using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustDescriptionPanelOpen : MonoBehaviour
{

    public GameObject arastirmaPaneli;
    public GameObject binalarPaneli;
    public GameObject karakterPaneli;

    public void closeOthers()
    {
        arastirmaPaneli.SetActive(false);
        binalarPaneli.SetActive(false);
        karakterPaneli.SetActive(false);
    }

}
