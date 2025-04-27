using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    public GameObject panel;
    public GameObject imageObject; // 🔥 Casus butonun burada
    public GameObject alternatePanel;

    public static bool spyPanelIsOpen = false;
    public static bool canToggle = true;

    private void Start()
    {
        panel?.SetActive(false);
        imageObject?.SetActive(true); // Başta buton açık olacak
        alternatePanel?.SetActive(false);
    }

    public void TogglePanel()
    {
        if (!canToggle) return;

        canToggle = false;
        spyPanelIsOpen = true;

        // Casus butonuna bastın: hemen butonu gizle
        imageObject?.SetActive(false);

        Invoke(nameof(ResetToggle), 60f);

        panel?.SetActive(false);
        alternatePanel?.SetActive(false);

        int randomValue = Random.Range(1, 11);
        if (randomValue > 6)
        {
            panel.SetActive(true); // 🔥 Sadece panel açılıyor
        }
        else
        {
            alternatePanel.SetActive(true);
        }
    }

    private void ResetToggle()
    {
        canToggle = true;
        spyPanelIsOpen = false;

        panel?.SetActive(false);
        alternatePanel?.SetActive(false);

        // 🔥 Sadece 60 saniye dolunca geri açılır
        if (canToggle)
        {
            imageObject?.SetActive(true);
        }
    }
}
