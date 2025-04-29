// NextGame.cs
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class NextGame : MonoBehaviourPunCallbacks
{
    private const byte SCENE_WAITING = 14;
    private string opponentName;
    private string myName;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Bu fonksiyon UI butonuna bağlı
    public void goWarScene()
    {
        // Single-player kontrolü
        if (ScreenTransitions2.ScreenNavigator.previousScreen == "Simple" ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Mid" ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Hard")
        {
            photonView.RPC(nameof(RPC_LoadWaitingScene), RpcTarget.AllBufferedViaServer);
            return;
        }

        // Multiplayer
        opponentName = RegionClickHandler.opponentName;
        myName = (string)PhotonNetwork.LocalPlayer.CustomProperties["PlayerName"];

        if (string.IsNullOrEmpty(opponentName) || string.IsNullOrEmpty(myName))
        {
            Debug.LogWarning("Rakip veya kendi ismim boş: yine de bekleme sahnesine geçiliyor...");
            photonView.RPC(nameof(RPC_LoadWaitingScene), RpcTarget.AllBufferedViaServer);
        }
        else
        {
            // MasterClient'a savaş bilgilerini yollarız
            photonView.RPC(
                nameof(RPC_AssignPlayerRoles),
                RpcTarget.MasterClient,
                myName,         // Savaş açanın ismi
                "attacker",     // Savaş açanın rolü
                opponentName,   // Rakibin ismi
                "defender"      // Rakibin rolü
            );
        }
    }

    [PunRPC]
    private void RPC_AssignPlayerRoles(string attackerName, string attackerRole, string defenderName, string defenderRole)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("PlayerName", out object pn))
            {
                string pnStr = pn.ToString();
                string role = "spectator";  // default spectator

                if (pnStr == attackerName)
                {
                    role = attackerRole;
                }
                else if (pnStr == defenderName)
                {
                    role = defenderRole;
                }

                player.SetCustomProperties(new Hashtable
                {
                    { "Role", role }
                    // Krallık bilgisi zaten her oyuncunun içinde var! Ayrı göndermeye gerek yok.
                });

                Debug.Log($"[{player.NickName}] rolü [{role}] olarak ayarlandı.");
            }
        }

        photonView.RPC(nameof(RPC_LoadWaitingScene), RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void RPC_LoadWaitingScene()
    {
        Debug.Log("WaitingRoom sahnesine geçiliyor...");
        SceneManager.LoadScene(SCENE_WAITING);
    }
}
