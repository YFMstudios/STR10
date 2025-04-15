using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class OwnerShipResetter : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private int lobbySceneIndex = 0;
    [SerializeField] private float initialDelay = 1f;
    [SerializeField] private float forceLoadDelay = 3f;

    private PhotonView cachedPhotonView;
    private string masterClientName = "Bilinmeyen Oyuncu";
    private const byte EVENT_RESET_ROLES = 1;
    private const byte EVENT_RETURN_TO_LOBBY = 2;
    private bool isSceneTransitionRequested = false;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        cachedPhotonView = GetComponent<PhotonView>();
        if (cachedPhotonView == null)
        {
            Debug.LogError("[OwnerShipResetter] Bu GameObject'e PhotonView eklenmemiş! Lütfen hemen bir PhotonView ekleyin.");
        }
    }

    private void Start()
    {
        if (PhotonNetwork.MasterClient != null)
        {
            masterClientName = GetPlayerNameFromPhoton(PhotonNetwork.MasterClient);
            Debug.Log($"[OwnerShipResetter] Şu anki MasterClient: {masterClientName}");
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        masterClientName = GetPlayerNameFromPhoton(newMasterClient);
        Debug.Log($"[OwnerShipResetter] MasterClient değişti, yeni Master: {masterClientName}");
    }

    private string GetPlayerNameFromPhoton(Player player)
    {
        if (player != null && player.CustomProperties.TryGetValue("PlayerName", out object playerNameObj))
        {
            return playerNameObj.ToString();
        }
        return "Bilinmeyen Oyuncu";
    }

    public void ResetAllRolesAndOwnership()
    {
        string playerName = GetPlayerNameFromPhoton(PhotonNetwork.LocalPlayer);
        Debug.Log($"[OwnerShipResetter] ResetAllRolesAndOwnership başlatılıyor... İstemci: {playerName}");

        Debug.Log("[OwnerShipResetter] Önce yerel role sıfırlama yapılıyor");
        ResetAllPlayerRoles();
        DisableCharacterControllers();

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("[OwnerShipResetter] MasterClient olarak sahiplik sıfırlaması yapılıyor");
            ResetGameObjectOwnerships();
            SendCustomEventToAll(EVENT_RESET_ROLES, "reset_roles");
        }
        else
        {
            Debug.Log($"[OwnerShipResetter] MasterClient olmadığı için sadece rol sıfırlandı. MasterClient: {masterClientName}");
        }
    }

    private void SendCustomEventToAll(byte eventCode, string eventName)
    {
        object[] content = new object[] { eventName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    private void SendCustomEventToMaster(byte eventCode, string eventName)
    {
        object[] content = new object[] { eventName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EVENT_RESET_ROLES)
        {
            object[] data = (object[])photonEvent.CustomData;
            string eventName = (string)data[0];

            if (eventName == "reset_roles")
            {
                Debug.Log("[OwnerShipResetter] 'reset_roles' eventi alındı, roller sıfırlanıyor");
                ResetAllPlayerRoles();
                DisableCharacterControllers();
            }
        }
        else if (photonEvent.Code == EVENT_RETURN_TO_LOBBY)
        {
            object[] data = (object[])photonEvent.CustomData;
            string eventName = (string)data[0];

            if (eventName == "return_to_lobby" && PhotonNetwork.IsMasterClient)
            {
                Debug.Log("[OwnerShipResetter] MasterClient 'return_to_lobby' eventini aldı. Sahne geçişi yapılıyor...");
                PhotonNetwork.LoadLevel(lobbySceneIndex);
            }
        }
    }

    private void ResetAllPlayerRoles()
    {
        string playerName = GetPlayerNameFromPhoton(PhotonNetwork.LocalPlayer);
        Debug.Log("[OwnerShipResetter] Tüm oyuncu rolleri sıfırlanıyor...");

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Role"))
        {
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties["Role"] = "spectator";
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
            Debug.Log($"[OwnerShipResetter] Yerel oyuncu ({playerName}) rolü 'spectator' olarak sıfırlandı.");
        }

        SoldierController soldierController = FindObjectOfType<SoldierController>();
        if (soldierController != null)
        {
            soldierController.PlayerRole = "spectator";
            Debug.Log("[OwnerShipResetter] SoldierController PlayerRole 'spectator' olarak güncellendi.");
        }

        BattleScenePlayerSpawner battleSpawner = FindObjectOfType<BattleScenePlayerSpawner>();
        if (battleSpawner != null)
        {
            Debug.Log("[OwnerShipResetter] BattleScenePlayerSpawner bulundu, role değişimi otomatik algılanacak.");
        }
        else
        {
            Debug.LogWarning("[OwnerShipResetter] BattleScenePlayerSpawner bulunamadı!");
        }
    }

    private void ResetGameObjectOwnerships()
    {
        Debug.Log("[OwnerShipResetter] Oyun nesnelerinin sahiplikleri sıfırlanıyor...");

        if (playerObject == null || enemyObject == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (players.Length > 0) playerObject = players[0];
            if (enemies.Length > 0) enemyObject = enemies[0];

            if (playerObject == null || enemyObject == null)
            {
                Debug.LogError("[OwnerShipResetter] Player veya Enemy objeleri bulunamadı!");
                return;
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView playerView = playerObject.GetComponent<PhotonView>();
            PhotonView enemyView = enemyObject.GetComponent<PhotonView>();

            if (playerView != null)
            {
                playerView.TransferOwnership(PhotonNetwork.MasterClient);
                Debug.Log("[OwnerShipResetter] Player objesi sahipliği MasterClient'a transfer edildi.");
            }
            else
            {
                Debug.LogWarning("[OwnerShipResetter] Player objesinde PhotonView bulunamadı!");
            }

            if (enemyView != null)
            {
                enemyView.TransferOwnership(PhotonNetwork.MasterClient);
                Debug.Log("[OwnerShipResetter] Enemy objesi sahipliği MasterClient'a transfer edildi.");
            }
            else
            {
                Debug.LogWarning("[OwnerShipResetter] Enemy objesinde PhotonView bulunamadı!");
            }
        }
    }

    private void DisableCharacterControllers()
    {
        Debug.Log("[OwnerShipResetter] Karakter kontrolcüleri devre dışı bırakılıyor...");
        DisableControllersOnObject(playerObject);
        DisableControllersOnObject(enemyObject);
    }

    private void DisableControllersOnObject(GameObject obj)
    {
        if (obj == null) return;

        MonoBehaviour[] controllers = obj.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour controller in controllers)
        {
            if (controller.GetType().Name.Contains("Controller") ||
                controller.GetType().Name.Contains("Input") ||
                controller.GetType().Name.Contains("Movement"))
            {
                controller.enabled = false;
                Debug.Log($"[OwnerShipResetter] {obj.name} kontrolcüsü devre dışı bırakıldı: {controller.GetType().Name}");
            }
        }
    }

    public void ReturnToLobby()
    {
        Debug.Log("[OwnerShipResetter] ReturnToLobby çağrıldı...");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("[OwnerShipResetter] MasterClient sahne geçişi başlatıyor...");
            PhotonNetwork.LoadLevel(lobbySceneIndex);
        }
        else
        {
            Debug.Log("[OwnerShipResetter] MasterClient değiliz, ona sahne geçişi emrini yolluyoruz...");
            SendCustomEventToMaster(EVENT_RETURN_TO_LOBBY, "return_to_lobby");
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}