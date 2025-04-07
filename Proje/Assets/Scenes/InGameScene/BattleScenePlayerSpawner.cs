/*
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BattleScenePlayerSpawner : MonoBehaviourPunCallbacks
{
    [Header("Spawn Points")]
    public Transform attackerSpawnPoint;
    public Transform defenderSpawnPoint;

    [Header("Character Objects")]
    public GameObject playerObject; // örn PhotonView ID: 7
    public GameObject enemyObject;  // örn PhotonView ID: 8

    void Start()
    {
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

        // LocalPlayer'ın Kingdom bilgisi (yoksa "Bilinmiyor" diyelim)
        string myKingdom = PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Kingdom", out object kingdomValue)
            ? kingdomValue.ToString()
            : "Bilinmiyor";

        Debug.Log($"[LocalPlayer] Atanan rol: {myRole} | Kingdom: {myKingdom}");

        // İstersen diğer oyuncuların da “Role” ve “Kingdom” bilgilerini görmek için:
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.TryGetValue("Role", out object r) &&
                p.CustomProperties.TryGetValue("Kingdom", out object k))
            {
                Debug.Log($"[Player {p.NickName}] Role: {r}, Kingdom: {k}");
            }
        }

        // Objeleri ilgili spawn noktalarına yerleştirip ownership verelim
        if (myRole == "attacker")
        {
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
*/

/*
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
    public WarController warController;
    void Start()
    {
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
        // 1) PlayerListOrderedByActorNumber: En küçük actorNumber -> en önce giren
        var sortedPlayers = PhotonNetwork.PlayerList;
        // PhotonNetwork.PlayerList zaten genelde actorNumber sıralı gelir ama 
        // daha garanti olsun isterseniz PlayerListOrderedByActorNumber kullanın.

        // sortedPlayers[0] her zaman ActorNumber en düşük olan
        // (2 kişilik oda varsayıyoruz)

        Player attackerPlayer = sortedPlayers[0]; // en küçük ActorNumber
        Player defenderPlayer = null;
        if (sortedPlayers.Length > 1)
        {
            defenderPlayer = sortedPlayers[1];
        }

        // Local player "attacker" mı "defender" mı?
        string myRole = "observer"; // 2 kişiden fazlaysa bu devreye girer
        if (PhotonNetwork.LocalPlayer == attackerPlayer)
        {
            myRole = "attacker";
        }
        else if (PhotonNetwork.LocalPlayer == defenderPlayer)
        {
            myRole = "defender";
        }

        // Rolü SetCustomProperties ile kaydedelim:
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "Role", myRole } });

        // LocalPlayer'ın Kingdom bilgisi (yoksa "Bilinmiyor" diyelim)
        string myKingdom = PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Kingdom", out object kingdomValue)
            ? kingdomValue.ToString()
            : "Bilinmiyor";

        Debug.Log($"[LocalPlayer] Atanan rol: {myRole} | Kingdom: {myKingdom}");
        //Attacker = myKingdom;
        warController.AttackerKingdom = myKingdom;
        Debug.Log("Saldıran Krallık = " + warController.AttackerKingdom);

        // Tüm oyuncuların rol/kingdom bilgilerini yazdıralım
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            string roleStr = p.CustomProperties.TryGetValue("Role", out object roleVal)
                ? roleVal.ToString()
                : "Bilinmiyor";
            string kingdomStr = p.CustomProperties.TryGetValue("Kingdom", out object kingdomVal)
                ? kingdomVal.ToString()
                : "Bilinmiyor";

            Debug.Log($"[Player {p.NickName}] Role: {roleStr}, Kingdom: {kingdomStr}");
            //Defender = KingdomStr;
            warController.DefenderKingdom = kingdomStr;
            Debug.Log("Savunan Krallık : " + warController.DefenderKingdom);
        }

        // Objeleri ilgili spawn noktalarına yerleştirip ownership verelim
        if (myRole == "attacker")
        {
            playerObject.transform.position = attackerSpawnPoint.position;
            playerObject.transform.rotation = attackerSpawnPoint.rotation;

            int playerViewID = playerObject.GetComponent<PhotonView>().ViewID;
            photonView.RPC("SetOwnership", RpcTarget.AllBuffered, playerViewID, PhotonNetwork.LocalPlayer);

            // Defender objesini de sahnede tut
            enemyObject.transform.position = defenderSpawnPoint.position;
            enemyObject.transform.rotation = defenderSpawnPoint.rotation;
        }
        else if (myRole == "defender")
        {
            enemyObject.transform.position = defenderSpawnPoint.position;
            enemyObject.transform.rotation = defenderSpawnPoint.rotation;

            int enemyViewID = enemyObject.GetComponent<PhotonView>().ViewID;
            photonView.RPC("SetOwnership", RpcTarget.AllBuffered, enemyViewID, PhotonNetwork.LocalPlayer);

            // Attacker objesini de sahnede tut
            playerObject.transform.position = attackerSpawnPoint.position;
            playerObject.transform.rotation = attackerSpawnPoint.rotation;
        }
        else
        {
            // 3. veya 4. oyuncu vs. girerse... "observer" gibi bir mantık
            Debug.LogWarning("2 oyuncudan fazla kişi girdi; bu kodda sadece attacker/defender var.");
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
*/

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
