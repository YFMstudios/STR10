using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli

public class BackButton12 : MonoBehaviour
{
    // Bu metot geri tuşuna tıklandığında çağrılacak
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0); // 0. sahneye geçiş yapar
    }
}
