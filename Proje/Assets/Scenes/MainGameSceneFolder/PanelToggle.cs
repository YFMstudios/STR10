using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    public GameObject panel;
    public GameObject imageObject;
    public GameObject alternatePanel;

   public static bool canToggle = true;

    private void Start()
    {
        panel?.SetActive(false);
        imageObject?.SetActive(false);
        alternatePanel?.SetActive(false);
    }

    public void TogglePanel()
    {
        if (!canToggle) return;

        canToggle = false;
        Invoke(nameof(ResetToggle), 60f);

        // Burada başka hiçbir şeye dokunmadım
        panel.SetActive(false);
        imageObject.SetActive(false);
        alternatePanel.SetActive(false);

        int randomValue = Random.Range(1, 11);
        if (randomValue > 6)
        {
            panel.SetActive(true);
            imageObject.SetActive(true);
        }
        else
        {
            alternatePanel.SetActive(true);
        }
    }

    private void ResetToggle()
    {
        canToggle = true;
    }
}
