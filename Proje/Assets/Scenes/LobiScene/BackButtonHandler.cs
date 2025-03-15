using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class BackButtonHandler : MonoBehaviour
{
    public void GoToMenuScene()
    {
    
        // Menü sahnesine dön
        SceneManager.LoadScene(0);
    }
}
