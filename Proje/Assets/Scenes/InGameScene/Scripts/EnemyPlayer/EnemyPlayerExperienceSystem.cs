using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPlayerExperienceSystem : MonoBehaviour
{
    public int currentLevel = 1;        // Mevcut seviye
    public int currentExp = 0;          // Mevcut deneyim puan�
    public int expToLevelUp = 100;      // Seviye atlamak i�in gereken toplam deneyim puan�
    public int expIncreaseFactor = 2;   // Seviye atlay�nca deneyim puan� gereksiniminin art�� fakt�r�

    public Slider expBarSlider;         // Deneyim �ubu�u UI eleman�
    public Text levelText2D;            // 2 boyutlu seviye metni UI eleman�
    public Text levelText3D;            // 3 boyutlu seviye metni UI eleman�

    // Her g�ncelleme �er�evesinde UI'yi g�nceller
    void Update()
    {
        UpdateUI();
    }

    // D��mandan kazan�lan deneyim puan� miktar�n� al�r ve deneyim kazanma i�levini �a��r�r
    private void GainExperienceFromEnemy(int amount)
    {
        GainExperience(amount);
    }

    // Deneyim kazanma i�levi
    private void GainExperience(int amount)
    {
        currentExp += amount;   // Deneyim puan�n� art�r

        // E�er mevcut deneyim puan� seviye atlamak i�in gereken deneyim puan�n� ge�erse
        while (currentExp >= expToLevelUp)
        {
            LevelUp();   // Seviye atla i�levini �a��r
        }
    }

    // Seviye atlama i�levi
    private void LevelUp()
    {
        currentLevel++;          // Seviyeyi art�r
        currentExp -= expToLevelUp;   // Mevcut deneyim puan�ndan seviye atlamak i�in gereken puan� ��kar
        expToLevelUp *= expIncreaseFactor;   // Bir sonraki seviye i�in gereken deneyim puan�n� art�r
    }

    // UI elemanlar�n� g�ncelleyen i�lev
    private void UpdateUI()
    {
        if (expBarSlider != null)
        {
            expBarSlider.maxValue = expToLevelUp;   // Deneyim �ubu�unun maksimum de�erini g�ncelle
            expBarSlider.value = currentExp;        // Deneyim �ubu�unun mevcut de�erini g�ncelle
        }

        if (levelText2D != null)
        {
            levelText2D.text = currentLevel.ToString();   // 2 boyutlu seviye metnini g�ncelle
        }

        if (levelText3D != null)
        {
            levelText3D.text = currentLevel.ToString();   // 3 boyutlu seviye metnini g�ncelle
        }
    }
}
