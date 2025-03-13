using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    // Panel GameObject'i buraya atanacak
    public GameObject panel;

    // Panelin ilk durumu (aktif/pasif) kontrol ediliyor
    private void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false); // Panel başlangıçta kapalı olacak
        }
        else
        {
            Debug.LogError("Panel atanmadı! Lütfen paneli Inspector'dan bağlayın.");
        }
    }

    // Butona tıklandığında çağrılacak fonksiyon
    public void TogglePanel()
    {
        if (panel != null)
        {
            // Panelin aktiflik durumunu tersine çevir
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
        }
    }
}
