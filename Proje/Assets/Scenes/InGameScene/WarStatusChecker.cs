using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class WarStatusChecker : MonoBehaviour
{
    void Start()
    {
        // War durumlarını kontrol et ve gerekli oyuncuları geri yönlendir
        CheckWarStatus();
    }

    void CheckWarStatus()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            // Oyuncunun warIsOnline bilgisini kontrol ediyoruz
            bool warIsOnline = player.CustomProperties.ContainsKey("warIsOnline")
                ? (bool)player.CustomProperties["warIsOnline"]
                : false;

            // Eğer warIsOnline false ise oyuncuyu 6. ekrana yönlendiriyoruz
            if (!warIsOnline)
            {
                Debug.Log(player.NickName + " için War Is Online: false, 6. ekrana yönlendiriliyor...");
                // Burada oyuncuyu 6. ekrana yönlendireceğiz
                RedirectToScene6();
                break; // İlk oyuncu bulunduğunda işlemi sonlandırıyoruz
            }
        }
    }

    // 6. ekrana yönlendiren fonksiyon
    private void RedirectToScene6()
    {
        try
        {
            // Sahne yükleniyor
            SceneManager.LoadScene(6); // 6. sahne
        }
        catch (System.Exception e)
        {
            // Eğer sahne yüklenirken bir hata olursa
            Debug.LogError("Sahne yüklenirken hata oluştu: " + e.Message);
        }
    }
}
