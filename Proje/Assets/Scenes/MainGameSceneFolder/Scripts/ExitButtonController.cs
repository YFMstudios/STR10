using UnityEngine;

public class ExitButtonController : MonoBehaviour
{
    public GameObject exitPanel; // Exit panelini referans al

    // Çarpı butonuna basıldığında exit panelini aç
    public void OpenExitPanel()
    {
        exitPanel.SetActive(true); // Exit paneli açılır
    }
}
