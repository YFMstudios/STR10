using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class HealthUI : MonoBehaviourPun
{
    public Slider healthSlider3D; // 3 boyutlu sağlık kaydırıcısı (herkes görür)
    public Slider healthSlider2D; // 2 boyutlu sağlık kaydırıcısı (sadece local player görür)

    // 3 boyutlu sağlık kaydırıcısını başlatan fonksiyon
    public void Start3DSlider(float maxValue)
    {
        if (healthSlider3D != null)
        {
            healthSlider3D.maxValue = maxValue;
            healthSlider3D.value = maxValue;
        }
    }

    // 3 boyutlu sağlık kaydırıcısını güncelleyen fonksiyon (her istemcide çalışır)
    public void Update3DSlider(float value)
    {
        if (healthSlider3D != null)
        {
            healthSlider3D.value = value;
        }
    }

    // 2 boyutlu sağlık kaydırıcısını güncelleyen fonksiyon (sadece local player veya enemy için)
   public void Update2DSlider(float maxValue, float value)
{
    if (healthSlider2D != null)
    {
        healthSlider2D.maxValue = maxValue;
        healthSlider2D.value = value;
    }
}

}