// NextGame.cs
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

[RequireComponent(typeof(PhotonView))]
public class NextGame : MonoBehaviourPunCallbacks
{
    private const byte SCENE_WAITING = 14;
    private string opponentName;
    private string myName;

    void Awake()
    {
        // MasterClient sahne yüklediğinde diğer istemciler de otomatik geçiş yapsın
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void goWarScene()
    {
        // ▶ SINGLE-PLAYER ◀
        if (ScreenTransitions2.ScreenNavigator.previousScreen == "Simple" ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Mid"   ||
            ScreenTransitions2.ScreenNavigator.previousScreen == "Hard")
        {
            // MasterClient doğrudan yüklesin
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.LoadLevel(SCENE_WAITING);
            return;
        }

        // ▶ MULTI-PLAYER ◀
        opponentName = RegionClickHandler.opponentName;
        myName       = PhotonNetwork.LocalPlayer.CustomProperties["PlayerName"].ToString();

        if (string.IsNullOrEmpty(opponentName) || string.IsNullOrEmpty(myName))
        {
            Debug.LogWarning("Rakip veya kendi ismim boş: yine de bekleme sahnesine geçiliyor...");
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.LoadLevel(SCENE_WAITING);
            return;
        }

        // Roller atama işini MasterClient’a havale et
        photonView.RPC(
            nameof(RPC_AssignPlayerRoles),
            RpcTarget.MasterClient,
            myName,        // saldıran
            "attacker",
            opponentName,  // savunan
            "defender"
        );
    }

    [PunRPC]
    private void RPC_AssignPlayerRoles(string attackerName, string attackerRole,
                                       string defenderName, string defenderRole)
    {
        // **SADECE** MasterClient bu bloğu çalıştırır
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (!p.CustomProperties.TryGetValue("PlayerName", out object pn)) 
                continue;

            string role = "spectator";
            if (pn.ToString() == attackerName) role = attackerRole;
            if (pn.ToString() == defenderName) role = defenderRole;

            p.SetCustomProperties(new Hashtable { { "Role", role } });
            Debug.Log($"[{p.NickName}] rolü → {role}");
        }

        // Roller atandıktan hemen sonra MasterClient sahneyi yükler,
        // diğer tüm istemciler de AutomaticallySyncScene sayesinde takip eder.
        PhotonNetwork.LoadLevel(SCENE_WAITING);
    }
}
