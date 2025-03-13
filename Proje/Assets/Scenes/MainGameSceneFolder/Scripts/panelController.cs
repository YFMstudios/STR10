using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelController : MonoBehaviour
{

    /*
    //Panellerin ayný anda sadece birinin açýk olmasý için yazýlmýþ script
    public GameObject[] panels = new GameObject[4];
    public bool[] activePanel = new bool[4];
    public GameObject arastirmaPaneli;
    public GameObject binalarPaneli;
    public GameObject kaleSistemiPaneli;
    public GameObject karakterPaneli;
    public int j = 0;

    public void Start()
    {
        panels[0] = arastirmaPaneli;
        panels[1] = binalarPaneli;
        panels[2] = kaleSistemiPaneli;
        panels[3] = karakterPaneli;

    }

    public void closeOtherPanels()
    {
        activePanel[0] = kaleSistemiPaneli.activeSelf;
        activePanel[1] = binalarPaneli.activeSelf;
        activePanel[2] = arastirmaPaneli.activeSelf;
        activePanel[3] = karakterPaneli.activeSelf;

        for (int i = 0; i < 4; i++)//Hangi panelin aktif olduðunu buluyoruz.
        {
            if (activePanel[i] == true)
            {
                Debug.Log("Aktif Panel : " + i);
                while (j < 4)
                {
                    if(j != i)
                    {
                        panels[j].SetActive(false);
                        //Debug.Log(j);
                    }
                    j++;
                }
            }
        }

    }
    */

    //Panellerin ayný anda sadece birinin açýk olmasý için yazýlmýþ script
    public GameObject[] panels = new GameObject[4];
    public GameObject arastirmaPaneli;
    public GameObject binalarPaneli;
    public GameObject kaleSistemiPaneli;
    public GameObject karakterPaneli;

    void Start()
    {
        panels[0] = arastirmaPaneli;
        panels[1] = binalarPaneli;
        panels[2] = kaleSistemiPaneli;
        panels[3] = karakterPaneli;
    }

    public void closeOtherPanels()
    {
        // Hangi panelin aktif olduðunu buluyoruz
        GameObject activePanel = null;
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                activePanel = panel;
                break;
            }
        }

        // Eðer aktif bir panel varsa, diðer panelleri kapat
        if (activePanel != null)
        {
            foreach (GameObject panel in panels)
            {
                if (panel != activePanel)
                {
                    panel.SetActive(false);
                    Debug.Log("Panel"+ panel.name+ "Deaktif Edildi");
                }
            }
        }
    }



}
