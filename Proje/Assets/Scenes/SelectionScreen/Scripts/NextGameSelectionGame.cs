using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class NextGame : MonoBehaviourPunCallbacks
{
    private string opponentName; // Rakibin ismi

    // Butona tıklandığında çalışacak olan fonksiyon
    public void goWarScene()
    {
        // Debug: Single-player kontrolü yapılırken
        Debug.Log("Single-player kontrolü başlatılıyor...");

        // Öncelikle Single-player kontrolü yapılır
        if (ScreenTransitions2.ScreenNavigator.previousScreen == "Simple" ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Mid" ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Hard")
        {
            // Single-player modunda, direkt 7. sahneye geçiş yap
            Debug.Log("Single-player modunda. 7. ekrana yönlendiriliyor...");
            GoToWarScene();
            return; 
        }

        // Multiplayer modunda
        Debug.Log("Multiplayer modunda. Rakip kontrolü ve diğer işlemler başlatılıyor...");

        // RegionClickHandler'ın opponentName özelliğini al, null olsa bile engel çıkarmadan devam edelim
        opponentName = RegionClickHandler.opponentName;
        if (opponentName == null)
        {
            Debug.LogWarning("Rakip adı null ancak yine de 7. sahneye geçiyoruz...");
        }
        else
        {
            Debug.Log("Opponent Name (Savunan Kişi): " + opponentName);
            // Oda bilgilerine war özelliğini opponentName ile ekle
            SetWarInfo(opponentName);
        }

        // 7. sahneye git
        GoToWarScene();
    }

    // Oda bilgilerine war özelliğini opponentName ile ekliyoruz
    private void SetWarInfo(string opponentName)
    {
        Debug.Log("SetWarInfo fonksiyonu çağrıldı. opponentName: " + opponentName);

        ExitGames.Client.Photon.Hashtable warInfo = new ExitGames.Client.Photon.Hashtable
        {
            { "opponentName", opponentName }
        };

        // Oda bilgilerini güncelliyoruz
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "war", warInfo }
        });

        Debug.Log("War bilgileri odada güncellendi. opponentName: " + opponentName);
    }

    // 7. Ekrana gitme fonksiyonu
    private void GoToWarScene()
    {
        try
        {
            Debug.Log("7. sahneye yönlendiriliyor...");
            SceneManager.LoadScene(7);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Sahne yüklenirken hata oluştu: " + e.Message);
        }
    }
}
