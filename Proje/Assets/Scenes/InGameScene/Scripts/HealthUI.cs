using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class HealthUI : MonoBehaviourPun
{
    public Slider healthSlider3D;
    public Slider healthSlider2D;
private void Start()
{
    // 2D slider'ı sadece karakter objelerinde aktif tut
    if (!(CompareTag("Player") || CompareTag("Enemy")) && healthSlider2D != null)
        healthSlider2D.gameObject.SetActive(false);

    // Hem yerel oyuncu hem düşmanlar için başlat
    if ((photonView.IsMine || CompareTag("Enemy")) && healthSlider2D != null && healthSlider3D != null)
    {
        healthSlider2D.maxValue = healthSlider3D.maxValue;
        healthSlider2D.value = healthSlider3D.value;
    }
}


    public void Start3DSlider(float maxValue)
    {
        if (healthSlider3D != null)
        {
            healthSlider3D.maxValue = maxValue;
            healthSlider3D.value = maxValue;
        }

        // **Yeni**: 2D slider’ı da başlat (sadece local)
        if (photonView.IsMine && healthSlider2D != null)
        {
            healthSlider2D.maxValue = maxValue;
            healthSlider2D.value = maxValue;
        }
    }

    public void Update3DSlider(float value)
    {
        if (healthSlider3D != null)
            healthSlider3D.value = value;
    }

   public void Update2DSlider(float maxValue, float value)
{
    // Hem yerel oyuncu hem düşman karakterleri için güncelleme izni ver
    if (!photonView.IsMine && !CompareTag("Enemy")) return;
    
    if (healthSlider2D != null)
    {
        healthSlider2D.maxValue = maxValue;
        healthSlider2D.value = value;
    }
}
}
