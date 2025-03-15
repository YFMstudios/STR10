using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli

public class ExitPanelController : MonoBehaviour
{
    public GameObject pausePanel; // Paneli referans al

    // Continue butonuna basıldığında paneli kapat
    public void ContinueGame()
    {
        pausePanel.SetActive(false); // Paneli kapat
    }

    // Exit butonuna basıldığında ana menüye dön (sahne numarası 0)
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0); // Ana menü sahnesine geçiş yap (sahne 0)
    }
}
