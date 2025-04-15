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
        Debug.Log("Single-player kontrolü başlatılıyor...");

        if (ScreenTransitions2.ScreenNavigator.previousScreen == "Simple" ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Mid" ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Hard")
        {
            Debug.Log("Single-player modunda. 7. ekrana yönlendiriliyor...");
            GoToWarScene();
            return;
        }

        Debug.Log("Multiplayer modunda. Rakip kontrolü ve rol atamaları başlatılıyor...");

        opponentName = RegionClickHandler.opponentName;

        if (string.IsNullOrEmpty(opponentName))
        {
            Debug.LogWarning("Rakip adı null ancak yine de 7. sahneye geçiyoruz...");
        }
        else
        {
            Debug.Log("Opponent Name (Savunan Kişi): " + opponentName);

            // Roller burada atanıyor
            AssignPlayerRoles(opponentName);
        }

        GoToWarScene();
    }

    private void AssignPlayerRoles(string defenderName)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("PlayerName", out object playerNameObj))
            {
                string playerName = playerNameObj.ToString();

                if (playerName == PhotonNetwork.LocalPlayer.CustomProperties["PlayerName"].ToString())
                {
                    // Bu butona basan oyuncuyu attacker olarak ayarla
                    player.SetCustomProperties(new Hashtable { { "Role", "attacker" } });
                    Debug.Log(playerName + " rolü: attacker");
                }
                else if (playerName == defenderName)
                {
                    // Rakip olan oyuncuyu defender olarak ayarla
                    player.SetCustomProperties(new Hashtable { { "Role", "defender" } });
                    Debug.Log(playerName + " rolü: defender");
                }
                else
                {
                    // Diğer tüm oyuncuları spectator yap
                    player.SetCustomProperties(new Hashtable { { "Role", "spectator" } });
                    Debug.Log(playerName + " rolü: spectator");
                }
            }
            else
            {
                Debug.LogWarning($"Player '{player.ActorNumber}' için PlayerName bulunamadı.");
            }

        }

        // Ek olarak, savaş bilgilerini oda özelliklerine yazabilirsin:
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
        {
            { "war", new Hashtable { { "opponentName", defenderName } } }
        });

        Debug.Log("War bilgileri odada güncellendi. opponentName: " + defenderName);
    }

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