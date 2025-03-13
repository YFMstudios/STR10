using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // Photon ile çalışmak için gerekli

public class ChatActive : MonoBehaviour
{
    public GameObject ChatPanel; // GameObject olarak tanımlandı

    private void Start()
    {
        ChatPanelActive(); // Oyun başladığında çalışır
    }

    public void ChatPanelActive()
    {
        // Photon'da bir odadaysa panel görünür olsun
        if (PhotonNetwork.InRoom)
        {
            ChatPanel.SetActive(true); // Paneli görünür yap
        }
        else
        {
            // Öncelikle Single-player kontrolü yapılır
            if (ScreenTransitions2.ScreenNavigator.previousScreen == "Simple" ||
                ScreenTransitions2.ScreenNavigator.previousScreen == "Mid" ||
                ScreenTransitions2.ScreenNavigator.previousScreen == "Hard")
            {
                ChatPanel.SetActive(false); // Paneli gizlemek için false kullanılır
            }
        }
    }
}
