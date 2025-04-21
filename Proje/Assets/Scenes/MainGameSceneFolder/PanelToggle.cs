using UnityEngine;
using UnityEngine.UI;

public class PanelToggle : MonoBehaviour
{
    // Panel ve Image GameObject'leri Inspector'dan atanacak
    public GameObject panel;
    public GameObject imageObject;

    private void Start()
    {
        if (panel == null)
        {
            Debug.LogError("Panel atanmadı! Lütfen paneli Inspector'dan bağlayın.");
            return;
        }
        panel.SetActive(false);

        if (imageObject == null)
        {
            Debug.LogError("Image atanmadı! Lütfen imageObject'u Inspector'dan bağlayın.");
            return;
        }
        imageObject.SetActive(false);
    }

    public void TogglePanel()
    {
        // Panelin aktiflik durumunu tersine çevir
        bool isNowActive = !panel.activeSelf;
        panel.SetActive(isNowActive);

        // Panel açıldıysa image'i aktif et, kapandıysa pasif yap
        imageObject.SetActive(isNowActive);
    }
}
