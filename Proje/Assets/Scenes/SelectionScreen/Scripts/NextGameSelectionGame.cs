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

    void Awake()
    {
        // MasterClient sahne yükleyince herkese senkron geçiş yapsın
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Bu fonksiyon UI butonuna bağlı
    public void goWarScene()
    {
        // Single-player kontrolleri...
        if (ScreenTransitions2.ScreenNavigator.previousScreen == "Simple" ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Mid"    ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Hard")
        {
            // Singleplayer’da direkt bekleme yerine savaş sahnesine geçiş de olabilir.
            // Ama eğer bekleme sahnesine geçeceksek:
            photonView.RPC(nameof(RPC_LoadWaitingScene), RpcTarget.AllBufferedViaServer);
            return;
        }

        // Multiplayer: önce rakip adını al
        opponentName = RegionClickHandler.opponentName;
        if (string.IsNullOrEmpty(opponentName))
        {
            Debug.LogWarning("Rakip adı boş: yine de roller atanmadan bekleme sahnesine geçiliyor...");
            photonView.RPC(nameof(RPC_LoadWaitingScene), RpcTarget.AllBufferedViaServer);
        }
        else
        {
            // Roller atama işlemini MasterClient’a bırak
            photonView.RPC(
                nameof(RPC_AssignPlayerRoles),
                RpcTarget.MasterClient,
                opponentName
            );
        }
    }

    [PunRPC]
    private void RPC_AssignPlayerRoles(string defenderName)
    {
        // Sadece MasterClient burayı çalıştırır
        if (!PhotonNetwork.IsMasterClient) return;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("PlayerName", out object pn))
            {
                string pnStr = pn.ToString();
                string role = "spectator";

                if (pnStr == (string)PhotonNetwork.LocalPlayer.CustomProperties["PlayerName"])
                    role = "attacker";
                else if (pnStr == defenderName)
                    role = "defender";

                player.SetCustomProperties(
                    new Hashtable { { "Role", role } }
                );
                Debug.Log($"[{player.NickName}] role set to {role}");
            }
        }

        // Opsiyonel: odanın CustomProperties’ine de yaz
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
        {
            { "war_opponent", defenderName }
        });

        // Roller atandı, şimdi bekleme sahnesine geç
        photonView.RPC(nameof(RPC_LoadWaitingScene), RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void RPC_LoadWaitingScene()
    {
        Debug.Log("WaitingRoom sahnesine geçiliyor...");
        SceneManager.LoadScene(SCENE_WAITING);
    }
}
