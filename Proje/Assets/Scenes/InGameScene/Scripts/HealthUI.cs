using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class HealthUI : MonoBehaviourPun
{
    public Slider healthSlider2D;

    private void Start()
    {
        // Sadece karakterlerde 2D bar aktif olacak
        if (!(CompareTag("Player") || CompareTag("Enemy")) && healthSlider2D != null)
            healthSlider2D.gameObject.SetActive(false);

        // Sadece yerel oyuncu veya enemy ise başlat
        if ((photonView.IsMine || CompareTag("Enemy")) && healthSlider2D != null)
        {
            healthSlider2D.maxValue = 100f;
            healthSlider2D.value = 100f;
        }
    }

    public void Update2DSlider(float maxValue, float value)
    {
        // Sadece local player veya düşmanlarda 2D bar güncellenir
        if (!photonView.IsMine && !CompareTag("Enemy")) return;

        if (healthSlider2D != null)
        {
            healthSlider2D.maxValue = maxValue;
            healthSlider2D.value = value;
        }
    }
}
