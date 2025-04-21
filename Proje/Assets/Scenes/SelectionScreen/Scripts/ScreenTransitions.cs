using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransitions : MonoBehaviour
{
    public GameObject voiceOnButton;
    public GameObject voiceOffButton;

    public static class ScreenNavigator
{
    public static string previousScreen; // Önceki ekranı saklar
}
    public void Start()
    {
        voiceOnButton = GameObject.Find("VoiceOnButton");
        voiceOffButton = GameObject.Find("VoiceOffButton");
    }
    public void storiesButton()
    {
        SceneManager.LoadScene(1);
    }

    public void singlePlayerButton()
    {
        SceneManager.LoadScene(2);
    }

    public void newGameButton()
    {
        SceneManager.LoadScene(3);
    }

    public void saveGameButton()
    {

    }

    public void loadGameButton()
    {

    }

    public void cancel1()
    {
        SceneManager.LoadScene(0);
    }

    public void simpleButton()
{
    ScreenNavigator.previousScreen = "Simple";
    SceneManager.LoadScene(10); // Krallık seçme ekranı
}

public void midButton()
{
    ScreenNavigator.previousScreen = "Mid";
    SceneManager.LoadScene(10); // Krallık seçme ekranı
}

public void hardButton()
{
    ScreenNavigator.previousScreen = "Hard";
    SceneManager.LoadScene(10); // Krallık seçme ekranı
}


    public void cancel2Button()
    {
        SceneManager.LoadScene(2);
    }


    public void continueButton()
    {
        SceneManager.LoadScene(6);
    }

    public void readMoreButton()
    {
        SceneManager.LoadScene(1);
    }

    public void cancelBbutton()
    {
        SceneManager.LoadScene(9);
    }


    public void multiPlayerButton()
    {
        
    }

    public void settingsButton()
    {
        
    }

    public void creditsButton()
    {
        
    }
    public void quitGame()
    {
        #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false; // Unity Editor'de oyunu durdur
        #else
        Application.Quit(); // Build alınmış oyunda çıkış yap
        #endif
    }

    public void voiceButton()
    {
        if (voiceOnButton != null)
        {
            voiceOnButton.SetActive(true);
            voiceOffButton.SetActive(false);

        }
        else
        {
            voiceOnButton.SetActive(false);
            voiceOffButton.SetActive(true);
        }
        
    }




}
