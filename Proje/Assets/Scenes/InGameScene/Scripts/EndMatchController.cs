using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMatchController : MonoBehaviour
{
    public GameObject endMatchPanel; // Paneli buraya sürükle
    public GameObject pausePanel;

    // Paneli açma
    public void ShowEndMatchPanel()
    {
        endMatchPanel.SetActive(true); // Paneli aç
        Time.timeScale = 0; // Oyunu durdur
    }

    // "Yes" butonuna basıldığında
    public void ConfirmEndMatch()
    {
        Time.timeScale = 1; // Oyunu tekrar normalleştir
        SceneManager.LoadScene(6); // Ana sahneyi yükle (0 yerine ana ekran sahnenin indeksini koy)
    }

    // "No" butonuna basıldığında
    public void CancelEndMatch()
    {
        endMatchPanel.SetActive(false); // Paneli kapat
        pausePanel.SetActive(false);
        Time.timeScale = 1; // Oyunu devam ettir
    }
}
