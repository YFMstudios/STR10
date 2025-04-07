
/*
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
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class EndMatchController : MonoBehaviourPunCallbacks
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
        // 1) Local player'ın Kingdom bilgisini öğren
        string myKingdom = "Bilinmiyor";
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Kingdom", out object kingdomValue))
        {
            myKingdom = kingdomValue.ToString();
        }

        // 2) RPC ile herkese bildir
        photonView.RPC("BroadcastKingdom", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName, myKingdom);

        // Devamında maçı bitirme işlemleri
        Time.timeScale = 1; // Oyunu tekrar normalleştir
        SceneManager.LoadScene(6); // Örnek olarak 6. index sahneyi yükle
    }

    // "No" butonuna basıldığında
    public void CancelEndMatch()
    {
        endMatchPanel.SetActive(false); // Paneli kapat
        pausePanel.SetActive(false);
        Time.timeScale = 1; // Oyunu devam ettir
    }

    // 3) RPC metodu: Tüm oyuncuların konsolunda debug bilgisi çıksın
    [PunRPC]
    private void BroadcastKingdom(string playerName, string kingdom)
    {
        Debug.Log($"[EndMatch] Oyuncu: {playerName} | Kingdom: {kingdom} maçı bitirdi!");
    }
}