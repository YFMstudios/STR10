using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon; // Hashtable için

public class BattleScenePlayerSpawner : MonoBehaviourPunCallbacks
{
    [Header("Spawn Points")]
    public Transform attackerSpawnPoint;
    public Transform defenderSpawnPoint;

    [Header("Character Objects")]
    public GameObject playerObject;
    public GameObject enemyObject;

    [Header("Controller")]
    public WarController warController;

    void Start()
    {
        if (playerObject == null || enemyObject == null)
        {
            Debug.LogError("Player veya Enemy objelerini inspector'dan atamayı unutma!");
            return;
        }

        playerObject.SetActive(true);
        enemyObject.SetActive(true);

        AssignAndSpawnPlayerRole();
    }

    private void AssignAndSpawnPlayerRole()
    {
        var sortedPlayers = PhotonNetwork.PlayerList;

        if (sortedPlayers.Length < 2)
        {
            Debug.LogWarning("2 oyuncudan az, eşleşme için yeterli oyuncu yok.");
            return;
        }

        Player attackerPlayer = sortedPlayers[0];
        Player defenderPlayer = sortedPlayers[1];

        string myRole = "observer";
        if (PhotonNetwork.LocalPlayer == attackerPlayer)
        {
            myRole = "attacker";
        }
        else if (PhotonNetwork.LocalPlayer == defenderPlayer)
        {
            myRole = "defender";
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "Role", myRole } });

        string myKingdom = PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Kingdom", out object kingdomValue)
            ? kingdomValue.ToString()
            : "Bilinmiyor";

        if (myRole == "attacker")
        {
            warController.AttackerKingdom = myKingdom;

            if (defenderPlayer.CustomProperties.TryGetValue("Kingdom", out object defenderKingdom))
                warController.DefenderKingdom = defenderKingdom.ToString();

            playerObject.transform.SetPositionAndRotation(attackerSpawnPoint.position, attackerSpawnPoint.rotation);
            enemyObject.transform.SetPositionAndRotation(defenderSpawnPoint.position, defenderSpawnPoint.rotation);

            photonView.RPC("SetOwnership", RpcTarget.AllBuffered, playerObject.GetComponent<PhotonView>().ViewID, PhotonNetwork.LocalPlayer);
        }
        else if (myRole == "defender")
        {
            warController.DefenderKingdom = myKingdom;

            if (attackerPlayer.CustomProperties.TryGetValue("Kingdom", out object attackerKingdom))
                warController.AttackerKingdom = attackerKingdom.ToString();

            playerObject.transform.SetPositionAndRotation(attackerSpawnPoint.position, attackerSpawnPoint.rotation);
            enemyObject.transform.SetPositionAndRotation(defenderSpawnPoint.position, defenderSpawnPoint.rotation);

            photonView.RPC("SetOwnership", RpcTarget.AllBuffered, enemyObject.GetComponent<PhotonView>().ViewID, PhotonNetwork.LocalPlayer);
        }
        else
        {
            Debug.LogWarning("Bu sistem yalnızca 2 oyuncu içindir.");
        }

        Debug.Log($"[LocalPlayer] Rol: {myRole} | Kingdom: {myKingdom}");
        Debug.Log("Saldıran Krallık = " + warController.AttackerKingdom);
        Debug.Log("Savunan Krallık = " + warController.DefenderKingdom);
    }

    [PunRPC]
    private void SetOwnership(int viewID, Player newOwner)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null)
        {
            targetView.TransferOwnership(newOwner);
        }
        else
        {
            Debug.LogError("PhotonView (" + viewID + ") bulunamadı!");
        }
    }
}
