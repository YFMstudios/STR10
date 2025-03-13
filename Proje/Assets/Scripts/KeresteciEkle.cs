using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeresteciEkle : MonoBehaviour
{
    public Image slot1;
    public Image slot2;
    public Image slot3;
    public Image slot4;
    public Image slot5;
    public Image slot6;


    public Sprite sawyer;

    public void changeSlot1()
    {
        slot1.sprite = sawyer;
    }

    public void changeSlot2()
    {
        slot2.sprite = sawyer;
    }

    public void changeSlot3()
    {
        slot3.sprite = sawyer;
    }

    public void changeSlot4()
    {
        slot4.sprite = sawyer;
    }

    public void changeSlot5()
    {
        slot5.sprite = sawyer;
    }

    public void changeSlot6()
    {
        slot6.sprite = sawyer;
    }
}
