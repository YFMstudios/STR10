using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel; // Paneli bağlamak için.

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log($"Before Pause: Time Scale = {Time.timeScale}");
            isPaused = !isPaused;
            PauseGame(isPaused);
            Debug.Log($"After Pause: Time Scale = {Time.timeScale}");
        }
    }


    public void PauseGame(bool pause)
    {
        pausePanel.SetActive(pause); // Paneli aç/kapat.
        Time.timeScale = pause ? 0 : 1; // Oyun durduruluyor/devam ediyor.
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex); // Belirtilen sahne indeksini yükler
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Oyunu devam ettir
        pausePanel.SetActive(false); // Paneli kapat
        Debug.Log("Game Resumed!"); // Konsol çıktısı, test amaçlı
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // Oyun Unity Editör içindeyse Play Mode'dan çık
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Derlenmiş oyundaysa oyundan çık
        Application.Quit();
#endif
        Debug.Log("Game Quit!"); // Test amaçlı konsola yazdır
    }
}
