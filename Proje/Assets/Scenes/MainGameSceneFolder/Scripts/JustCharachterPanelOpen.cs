using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustCharachterPanelOpen : MonoBehaviour
{
    public GameObject binalarPaneli;
    public GameObject kaleSistemiPaneli;
    public GameObject arastirmaPaneli;

    public void closeOthers()
    {
        binalarPaneli.SetActive(false);
        kaleSistemiPaneli.SetActive(false);
        arastirmaPaneli.SetActive(false);
    }
}
