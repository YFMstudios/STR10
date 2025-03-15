using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EgitmenEkle : MonoBehaviour
{
    public Image egitmen1;
    public Image egitmen2;
    public Image egitmen3;
    public Image egitmen4;
    public Image egitmen5;
    public Image egitmen6;

    public Sprite s_egitmen1;
    public Sprite s_egitmen2;
    public Sprite s_egitmen3;
    public Sprite s_egitmen4;
    public Sprite s_egitmen5;
    public Sprite s_egitmen6;

    public void changeEgitmen1()
    {
        egitmen1.sprite = s_egitmen1;
    }

    public void changeEgitmen2()
    {
        egitmen2.sprite = s_egitmen2;
    }

    public void changeEgitmen3()
    {
        egitmen3.sprite = s_egitmen3;
    }

    public void changeEgitmen4()
    {
        egitmen4.sprite = s_egitmen4;
    }

    public void changeEgitmen5()
    {
        egitmen5.sprite = s_egitmen5;
    }

    public void changeEgitmen6()
    {
        egitmen6.sprite = s_egitmen6;
    }

}
