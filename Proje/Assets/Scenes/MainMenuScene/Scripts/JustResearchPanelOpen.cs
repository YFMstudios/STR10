using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustResearchPanelOpen : MonoBehaviour
{
    public GameObject binalarPaneli;
    public GameObject kaleSistemiPaneli;
    public GameObject karakterPaneli;

    public void closeOthers()
    {
        binalarPaneli.SetActive(false);
        kaleSistemiPaneli.SetActive(false);
        karakterPaneli.SetActive(false);
    }
}
