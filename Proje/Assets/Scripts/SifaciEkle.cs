using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SifaciEkle : MonoBehaviour
{
    public Image sifaci1;
    public Image sifaci2;
    public Image sifaci3;
    public Image sifaci4;
    public Image sifaci5;
    public Image sifaci6;

    public Sprite s_sifaci1;

    public void changeEgitmen1()
    {
        sifaci1.sprite = s_sifaci1;
    }

    public void changeEgitmen2()
    {
        sifaci2.sprite = s_sifaci1;
    }

    public void changeEgitmen3()
    {
        sifaci3.sprite = s_sifaci1;
    }

    public void changeEgitmen4()
    {
        sifaci4.sprite = s_sifaci1;
    }

    public void changeEgitmen5()
    {
        sifaci5.sprite = s_sifaci1;
    }

    public void changeEgitmen6()
    {
        sifaci6.sprite = s_sifaci1;
    }
}
