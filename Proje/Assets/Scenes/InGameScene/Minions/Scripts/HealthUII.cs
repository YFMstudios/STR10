using UnityEngine;
using UnityEngine.UI;

public class HealthUII : MonoBehaviour
{
    public Slider healthSlider3D; // 3D sa�l�k �ubu�u i�in Slider referans�

    // 3D sa�l�k �ubu�unun ba�lang�� de�erlerini ayarlar
    public void Start3DSlider(float maxValue)
    {
        healthSlider3D.maxValue = maxValue; // Sa�l�k �ubu�unun maksimum de�erini ayarlar
        healthSlider3D.value = maxValue;    // Sa�l�k �ubu�unun mevcut de�erini ba�lang��ta maksimum olarak ayarlar
    }

    // 3D sa�l�k �ubu�unun g�ncel de�erini ayarlar
    public void Update3DSlider(float value)
    {
        healthSlider3D.value = value; // Sa�l�k �ubu�unun mevcut de�erini g�nceller
    }
}
