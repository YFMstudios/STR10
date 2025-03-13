using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Bu metodu butona bağlayacağız
    public void LoadLobiScene()
    {
        // Sahne numarasına göre geçiş yapar (örneğin sıra numarası: 8)
        SceneManager.LoadScene(8);
    }
}
