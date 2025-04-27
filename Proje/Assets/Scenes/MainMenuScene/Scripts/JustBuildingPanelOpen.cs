using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustBuildingsPanelOpen : MonoBehaviour
{
    public GameObject arastirmaPaneli;
    public GameObject kaleSistemiPaneli;
    public GameObject karakterPaneli;

    public void closeOthers()
    {
        arastirmaPaneli.SetActive(false);
        kaleSistemiPaneli.SetActive(false);
        karakterPaneli.SetActive(false);
    }

}
