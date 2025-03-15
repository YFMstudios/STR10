using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemirciEkle : MonoBehaviour
{
    public Image slot1;
    public Image slot2;
    public Image slot3;
    public Image slot4;
    public Image slot5;
    public Image slot6;


    public Sprite blacksmith;

    public void changeSlot1()
    {
        slot1.sprite = blacksmith;
    }

    public void changeSlot2()
    {
        slot2.sprite = blacksmith;
    }

    public void changeSlot3()
    {
        slot3.sprite = blacksmith;
    }

    public void changeSlot4()
    {
        slot4.sprite = blacksmith;
    }

    public void changeSlot5()
    {
        slot5.sprite = blacksmith;
    }

    public void changeSlot6()
    {
        slot6.sprite = blacksmith;
    }
}
