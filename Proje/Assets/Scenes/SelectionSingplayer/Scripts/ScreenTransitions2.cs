using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransitions2 : MonoBehaviour
{
    public GameObject voiceOnButton;
    public GameObject voiceOffButton;

    public static string selectedKingdom; // Seçilen krallık bilgisini tutar

    public static class ScreenNavigator
{
    public static string previousScreen; // Önceki ekranı saklar
    public static string selectedKingdom; // Seçilen krallık bilgisini tutar

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
    ScreenTransitions2.selectedKingdom = "Simple Kingdom";  // Seçilen krallık burada atanıyor
    SceneManager.LoadScene(10); // Krallık seçme ekranına gidiliyor
}

public void midButton()
{
    ScreenNavigator.previousScreen = "Mid";
    ScreenTransitions2.selectedKingdom = "Mid Kingdom";  // Seçilen krallık burada atanıyor
    SceneManager.LoadScene(10); // Krallık seçme ekranına gidiliyor
}

public void hardButton()
{
    ScreenNavigator.previousScreen = "Hard";
    ScreenTransitions2.selectedKingdom = "Hard Kingdom";  // Seçilen krallık burada atanıyor
    SceneManager.LoadScene(10); // Krallık seçme ekranına gidiliyor
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
        SceneManager.LoadScene(3);
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
        UnityEditor.EditorApplication.isPlaying = false; // Sadece Unity Editor'de çalışır
    #else
    Application.Quit(); // Build alındığında çalışır
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
