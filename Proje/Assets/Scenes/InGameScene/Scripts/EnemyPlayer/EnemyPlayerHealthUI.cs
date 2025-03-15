using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPlayerHealthUI : MonoBehaviour
{
    public Slider healthSlider3D;   // 3 boyutlu sa�l�k kayd�r�c�s�
    public Slider healthSlider2D;   // 2 boyutlu sa�l�k kayd�r�c�s�

    // 3 boyutlu sa�l�k kayd�r�c�s�n� ba�latan fonksiyon
    public void start3DSlider(float maxValue)
    {
        healthSlider3D.maxValue = maxValue;  // Maksimum de�eri ayarla
        healthSlider3D.value = maxValue;     // �u anki de�eri maksimum olarak ayarla
    }

    // 3 boyutlu sa�l�k kayd�r�c�s�n� g�ncelleyen fonksiyon
    public void update3DSlider(float value)
    {
        healthSlider3D.value = value;   // De�eri g�ncelle
    }

    // 2 boyutlu sa�l�k kayd�r�c�s�n� g�ncelleyen fonksiyon
    public void Update2DSlider(float maxValue, float value)
    {
        if (gameObject.CompareTag("EnemyPlayer"))   // E�er bu UI nesnesi "Player" etiketine sahipse
        {
            healthSlider2D.maxValue = maxValue;  // Maksimum de�eri ayarla
            healthSlider2D.value = value;        // �u anki de�eri ayarla
        }
    }
}
