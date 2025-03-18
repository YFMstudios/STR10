using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BattleScenePlayerSpawner : MonoBehaviourPunCallbacks
{
    [Header("Spawn Points")]
    public Transform attackerSpawnPoint;
    public Transform defenderSpawnPoint;

    [Header("Character Objects")]
    public GameObject playerObject; // PhotonView ID: örn 7
    public GameObject enemyObject;  // PhotonView ID: örn 8

    void Start()
    {
        // *** Kamerayla ilgili satırları tamamen sildik. ***
        // Artık sahnede “MainCamera” yoksa hata almazsın.

        if (playerObject == null || enemyObject == null)
        {
            Debug.LogError("Player veya Enemy objelerini inspector'dan atamayı unutma!");
            return;
        }

        // İki obje de sahnede aktif olsun (herkes görsün)
        playerObject.SetActive(true);
        enemyObject.SetActive(true);

        AssignAndSpawnPlayerRole();
    }

    private void AssignAndSpawnPlayerRole()
    {
        DetermineAndAssignRole();
    }

    private void DetermineAndAssignRole()
    {
        // Basit mantık: ilk giren attacker, ikinci giren defender
        bool attackerExists = false;
        foreach (Player otherPlayer in PhotonNetwork.PlayerList)
        {
            if (otherPlayer.CustomProperties.TryGetValue("Role", out object existingRole))
            {
                if (existingRole.ToString() == "attacker")
                {
                    attackerExists = true;
                    break;
                }
            }
        }

        string myRole = attackerExists ? "defender" : "attacker";
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable { { "Role", myRole } }
        );

        Debug.Log("Atanan rol: " + myRole);

        if (myRole == "attacker")
        {
            // Attacker, "playerObject"i kontrol eder
            playerObject.transform.position = attackerSpawnPoint.position;
            playerObject.transform.rotation = attackerSpawnPoint.rotation;

            int playerViewID = playerObject.GetComponent<PhotonView>().ViewID;
            photonView.RPC("SetOwnership", RpcTarget.AllBuffered, playerViewID, PhotonNetwork.LocalPlayer);

            // Defender objesini de sahnede tut
            enemyObject.transform.position = defenderSpawnPoint.position;
            enemyObject.transform.rotation = defenderSpawnPoint.rotation;
        }
        else
        {
            // Defender, "enemyObject"i kontrol eder
            enemyObject.transform.position = defenderSpawnPoint.position;
            enemyObject.transform.rotation = defenderSpawnPoint.rotation;

            int enemyViewID = enemyObject.GetComponent<PhotonView>().ViewID;
            photonView.RPC("SetOwnership", RpcTarget.AllBuffered, enemyViewID, PhotonNetwork.LocalPlayer);

            // Attacker objesini de sahnede tut
            playerObject.transform.position = attackerSpawnPoint.position;
            playerObject.transform.rotation = attackerSpawnPoint.rotation;
        }
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
