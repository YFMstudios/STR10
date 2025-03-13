using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPlayerManaSystem : MonoBehaviour
{
    public float maxMana = 100;         // Maksimum mana miktar�
    public float startingMana = 100;    // Ba�lang��ta verilen mana miktar�
    public float manaRegenRate = 5;     // Mana yenilenme h�z�

    public Slider manaBar2d;            // 2D UI i�in mana �ubu�u
    public Slider manaBar3d;            // 3D UI i�in mana �ubu�u
    public Text manaText2d;             // 2D UI i�in mana metni

    private float currentMana;          // �u anki mana miktar�

    // Start is called before the first frame update
    void Start()
    {
        currentMana = startingMana;     // Ba�lang��ta mana miktar�n� ayarla
        UpdateManaUI();                 // UI'y� g�ncelle
    }

    // Update is called once per frame
    void Update()
    {
        RegenerateMana();               // Mana yenilenmesini sa�la
    }

    // Mana yenilenmesini sa�layan fonksiyon
    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;  // Zamanla mana miktar�n� artt�r
            currentMana = Mathf.Clamp(currentMana, 0f, maxMana);  // Mana miktar�n� s�n�rla (0 ile maxMana aras�nda)
            UpdateManaUI();                                 // UI'y� g�ncelle
        }
    }

    // UI'y� g�ncellemek i�in kullan�lan fonksiyon
    public void UpdateManaUI()
    {
        if (manaBar2d != null)
        {
            manaBar2d.value = currentMana / maxMana;    // 2D mana �ubu�unu g�ncelle (oran olarak)
        }
        if (manaBar3d != null)
        {
            manaBar3d.value = currentMana / maxMana;    // 3D mana �ubu�unu g�ncelle (oran olarak)
        }
        if (manaText2d != null)
        {
            manaText2d.text = Mathf.RoundToInt(currentMana).ToString() + " / " + maxMana;  // 2D mana metnini g�ncelle
        }
    }

    // Belirli bir yetene�i kullanma maliyetini kar��lay�p kar��layamayaca��n� kontrol eden fonksiyon
    public bool CanAffordAbility(float abilityCost)
    {
        return currentMana >= abilityCost;   // E�er �u anki mana yetenek maliyetini kar��l�yorsa true d�nd�r
    }

    // Belirli bir yetene�i kullanma fonksiyonu
    public void UseAbility(float abilityCost)
    {
        currentMana -= abilityCost;             // Yetenek maliyetini d��
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);  // Mana miktar�n� s�n�rla (0 ile maxMana aras�nda)
        UpdateManaUI();                         // UI'y� g�ncelle
    }
}
