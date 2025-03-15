using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextGame2 : MonoBehaviour
{ 
    public void startLevel1() {

    SceneManager.LoadScene(2);
}
    public void goWarScene()
    {
        SceneManager.LoadScene(7);
    }


}
