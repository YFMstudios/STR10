using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class BattleScenePlayerSpawner : MonoBehaviourPunCallbacks
{
    [Header("Spawn Points")]
    public Transform attackerSpawnPoint;
    public Transform defenderSpawnPoint;

    [Header("Character Objects")]
    public GameObject playerObject; // Attacker
    public GameObject enemyObject;  // Defender

    void Start()
{
    // Her istemci (Master veya değil) kod buradan geçer
    Debug.Log($"[Spawner] Start() -> Nick:{PhotonNetwork.NickName}, IsMaster?: {PhotonNetwork.IsMasterClient}");

    if (playerObject == null || enemyObject == null)
    {
        Debug.LogError("[Spawner] Player veya Enemy objeleri inspector'da atamayı unutma!");
        return;
    }

    // (1) Transform senkron ayarları (Opsiyonel, Inspector’dan da yapabilirsiniz)
    SetupTransformSync(playerObject);
    SetupTransformSync(enemyObject);

    // (2) İlk spawn için ikisini de aktif edelim
    playerObject.SetActive(true);
    enemyObject.SetActive(true);

    // (3) Herkes kendi rolünü (attacker/defender) belirlesin
    AssignAndSpawnPlayerRole();

    // (4) Yalnızca MasterClient respawn coroutineleri yönetsin
    if (PhotonNetwork.IsMasterClient)
    {
        Debug.Log("[Spawner] Ben MasterClient -> respawn coroutine'lerini başlatıyorum");
        StartCoroutine(CheckRespawnRoutine_Attacker());
        StartCoroutine(CheckRespawnRoutine_Defender());
    }
    else
    {
        Debug.Log("[Spawner] MasterClient değilim -> coroutine başlatmıyorum");
    }

    // PrintRoleBasedInfo() metodunu direkt çağırmak yerine kısa bir gecikmeyle çalıştırın

    // Start() içinde, AssignAndSpawnPlayerRole() çağrıldıktan sonra:
StartCoroutine(DelayedPrintInfo());

}

// -----------------------------------------------------------------------
//  (Opsiyonel) Attacker/Defender kim, PlayerName ve Kingdom nedir?
// -----------------------------------------------------------------------
private IEnumerator DelayedPrintInfo()
{
    // 2 saniye bekleyip sonra yazdır
    yield return new WaitForSeconds(2f);
    PrintRoleBasedInfo();
}

private void PrintRoleBasedInfo()
{
    Debug.Log("[Spawner] PrintRoleBasedInfo() çağrıldı.");

    // Attacker kim?
    Player attacker = FindPlayerByRole("attacker");
    if (attacker != null)
    {
        attacker.CustomProperties.TryGetValue("PlayerName", out object attackerNameObj);
        attacker.CustomProperties.TryGetValue("Kingdom", out object attackerKingdomObj);

        string attackerName = (attackerNameObj != null) ? attackerNameObj.ToString() : "<Bilinmiyor>";
        string attackerKingdom = (attackerKingdomObj != null) ? attackerKingdomObj.ToString() : "<Krallık Yok>";

        Debug.Log($"[Spawner] ATTACKER => Name: {attackerName}, Kingdom: {attackerKingdom}");

        if (WarController.Instance != null)
            WarController.Instance.Attacker = $"{attackerName} | Kingdom: {attackerKingdom}";
    }
    else
    {
        Debug.LogWarning("[Spawner] Attacker henüz bulunamadı!");
    }

    // Defender kim?
    Player defender = FindPlayerByRole("defender");
    if (defender != null)
    {
        defender.CustomProperties.TryGetValue("PlayerName", out object defenderNameObj);
        defender.CustomProperties.TryGetValue("Kingdom", out object defenderKingdomObj);

        string defenderName = (defenderNameObj != null) ? defenderNameObj.ToString() : "<Bilinmiyor>";
        string defenderKingdom = (defenderKingdomObj != null) ? defenderKingdomObj.ToString() : "<Krallık Yok>";

        Debug.Log($"[Spawner] DEFENDER => Name: {defenderName}, Kingdom: {defenderKingdom}");

        if (WarController.Instance != null)
            WarController.Instance.Defender = $"{defenderName} | Kingdom: {defenderKingdom}";
    }
    else
    {
        Debug.LogWarning("[Spawner] Defender henüz bulunamadı!");
    }
}


    // -----------------------------------------------------------------------
    // PhotonTransformViewClassic senkronizasyon ayarları
    // -----------------------------------------------------------------------
    private void SetupTransformSync(GameObject obj)
    {
        PhotonView pv = obj.GetComponent<PhotonView>();
        if (pv == null)
        {
            Debug.LogWarning($"[SetupTransformSync] {obj.name} üzerinde PhotonView yok!");
            return;
        }

        PhotonTransformViewClassic transformView = obj.GetComponent<PhotonTransformViewClassic>();
        if (transformView != null)
        {
            transformView.m_PositionModel.SynchronizeEnabled = true;
            transformView.m_RotationModel.SynchronizeEnabled = true;
            Debug.Log($"[SetupTransformSync] {obj.name} => Position/Rotation senkron aktif");
        }
        else
        {
            Debug.LogWarning($"[SetupTransformSync] {obj.name} üzerinde PhotonTransformViewClassic yok!");
        }
    }

    // -----------------------------------------------------------------------
    // Her istemci hangi rolü (attacker/defender) alacak, obje konumu vs.
    // -----------------------------------------------------------------------
    private void AssignAndSpawnPlayerRole()
    {
        Debug.Log($"[Spawner] AssignAndSpawnPlayerRole() => {PhotonNetwork.NickName}");

        // Odadaki oyuncuları gez, eğer bir tane attacker varsa ben defender olayım
        bool attackerExists = false;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.TryGetValue("Role", out object existingRole))
            {
                if (existingRole != null && existingRole.ToString() == "attacker")
                {
                    attackerExists = true;
                    break;
                }
            }
        }

        // Eğer attacker yoksa ben attacker olurum, varsa defender olurum
        string myRole = attackerExists ? "defender" : "attacker";
        Debug.Log($"[Spawner] {PhotonNetwork.NickName} => rol: {myRole}");

        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable { { "Role", myRole } }
        );

        // Seçilen role göre obje spawn
        SpawnPlayer(myRole);
    }

    // -----------------------------------------------------------------------
    // Seçilen role göre objeleri konumlandırıp Ownership veriyoruz
    // -----------------------------------------------------------------------
    private void SpawnPlayer(string role)
    {
        Debug.Log($"[Spawner] SpawnPlayer({role}) -> {PhotonNetwork.NickName}");

        if (role == "attacker")
        {
            // Player (Attacker) Ownership bende
            playerObject.transform.SetPositionAndRotation(attackerSpawnPoint.position, attackerSpawnPoint.rotation);

            photonView.RPC(nameof(SetOwnership), RpcTarget.AllBuffered,
                playerObject.GetComponent<PhotonView>().ViewID,
                PhotonNetwork.LocalPlayer);

            // Enemy sahnede dursun (Ownership yok)
            enemyObject.transform.SetPositionAndRotation(defenderSpawnPoint.position, defenderSpawnPoint.rotation);
        }
        else // defender
        {
            // Enemy (Defender) Ownership bende
            enemyObject.transform.SetPositionAndRotation(defenderSpawnPoint.position, defenderSpawnPoint.rotation);

            photonView.RPC(nameof(SetOwnership), RpcTarget.AllBuffered,
                enemyObject.GetComponent<PhotonView>().ViewID,
                PhotonNetwork.LocalPlayer);

            // Player sahnede dursun (Ownership yok)
            playerObject.transform.SetPositionAndRotation(attackerSpawnPoint.position, attackerSpawnPoint.rotation);
        }
    }

    // -----------------------------------------------------------------------
    // MasterClient: Attacker ölmüş mü kontrol
    // -----------------------------------------------------------------------
    private IEnumerator CheckRespawnRoutine_Attacker()
    {
        Debug.Log("[Spawner] CheckRespawnRoutine_Attacker -> BAŞLADI");

        while (true)
        {
            yield return new WaitForSeconds(2f);

            if (!playerObject.activeInHierarchy) // Player inaktif, demek ki ölmüş
            {
                Debug.Log("[Spawner] Attacker inaktif bulundu, respawn kontrolü yapılıyor...");

                // AllyMinion var mı?
                GameObject[] allyMinions = GameObject.FindGameObjectsWithTag("AllyMinion");
                bool anyAlive = false;
                foreach (var m in allyMinions)
                {
                    if (m.activeInHierarchy) 
                    {
                        anyAlive = true;
                        break;
                    }
                }

                // Minyon biterse coroutine'i durdurmak istemiyorsanız bu satırları yorumlayabilirsiniz
                if (!anyAlive)
                {
                    Debug.LogWarning("[Spawner] AllyMinion kalmadı => Attacker respawn coroutines durduruluyor");
                    yield break;
                }

                // Attacker'ı yeniden doğur
                Player attackerOwner = FindPlayerByRole("attacker");
                int ownerActorNum = (attackerOwner != null) ? attackerOwner.ActorNumber : -1;
                Debug.Log($"[Spawner] Attacker respawn -> RPC_RespawnCharacter, ownerActorNum={ownerActorNum}");

                photonView.RPC(nameof(RPC_RespawnCharacter), RpcTarget.All,
                               "attacker", ownerActorNum);
            }
        }
    }

    // -----------------------------------------------------------------------
    // MasterClient: Defender ölmüş mü kontrol
    // -----------------------------------------------------------------------
    private IEnumerator CheckRespawnRoutine_Defender()
    {
        Debug.Log("[Spawner] CheckRespawnRoutine_Defender -> BAŞLADI");

        while (true)
        {
            yield return new WaitForSeconds(2f);

           if (!enemyObject.activeInHierarchy)
{
    Debug.Log("[Spawner] Defender inaktif bulundu, respawn kontrolü yapılıyor...");

    // [1] İsteğe bağlı minyon kontrolü
    GameObject[] enemyMinions = GameObject.FindGameObjectsWithTag("EnemyMinion");
    bool anyAlive = false;
    foreach (var m in enemyMinions)
    {
        if (m.activeInHierarchy)
        {
            anyAlive = true;
            break;
        }
    }

    // Eğer minyon hiç yoksa coroutine kesilmesi isteniyorsa
    if (!anyAlive)
    {
        Debug.LogWarning("[Spawner] EnemyMinion kalmadı => Defender respawn durduruluyor (yield break).");
        yield break;
    }

    // [2] Kim defender rolünde? Ona göre ownership devredeceğiz
    Player defenderOwner = FindPlayerByRole("defender");
    int ownerActorNum = (defenderOwner != null) ? defenderOwner.ActorNumber : -1;
    Debug.Log($"[Spawner] Defender respawn -> RPC_RespawnCharacter => ownerActorNum={ownerActorNum}");

    // [3] Tüm istemcilerde enemyObject SetActive(true) yapılsın, can sıfırlansın
    photonView.RPC(nameof(RPC_RespawnCharacter), RpcTarget.All, "defender", ownerActorNum);
}

        }
    }

    // -----------------------------------------------------------------------
    // RPC: Herkesin sahnesinde obje aktif edilip can yenilensin
    // MasterClient -> Ownership gerçek sahibine devreder
    // -----------------------------------------------------------------------
    [PunRPC]
    private void RPC_RespawnCharacter(string role, int ownerActorNumber)
    {
        Debug.Log($"[Spawner][RPC_RespawnCharacter] => role={role}, ownerActorNum={ownerActorNumber}, " +
                  $"Nick:{PhotonNetwork.NickName}, Master?:{PhotonNetwork.IsMasterClient}");

        if (role == "attacker")
        {
            // Player yeniden doğsun
            playerObject.transform.SetPositionAndRotation(attackerSpawnPoint.position, attackerSpawnPoint.rotation);
            playerObject.SetActive(true);

            Stats stats = playerObject.GetComponent<Stats>();
            if (stats != null)
            {
                stats.ResetHealthToFull();
                Debug.Log("[Spawner][RPC_RespawnCharacter] Attacker can sıfırlandı.");
            }

            // Ownership devrini sadece MasterClient yapar
            if (PhotonNetwork.IsMasterClient && ownerActorNumber != -1)
            {
                Player realOwner = PhotonNetwork.CurrentRoom.GetPlayer(ownerActorNumber);
                if (realOwner != null)
                {
                    Debug.Log($"[Spawner][RPC_RespawnCharacter] => TransferOwnership -> {realOwner.NickName}");
                    playerObject.GetComponent<PhotonView>().TransferOwnership(realOwner);
                }
            }
        }
        else if (role == "defender")
        {
            // Enemy yeniden doğsun
            enemyObject.transform.SetPositionAndRotation(defenderSpawnPoint.position, defenderSpawnPoint.rotation);
            enemyObject.SetActive(true);

            Stats stats = enemyObject.GetComponent<Stats>();
            if (stats != null)
            {
                stats.ResetHealthToFull();
                Debug.Log("[Spawner][RPC_RespawnCharacter] Defender can sıfırlandı.");
            }

            // Ownership devrini sadece MasterClient yapar
            if (PhotonNetwork.IsMasterClient && ownerActorNumber != -1)
            {
                Player realOwner = PhotonNetwork.CurrentRoom.GetPlayer(ownerActorNumber);
                if (realOwner != null)
                {
                    Debug.Log($"[Spawner][RPC_RespawnCharacter] => TransferOwnership -> {realOwner.NickName}");
                    enemyObject.GetComponent<PhotonView>().TransferOwnership(realOwner);
                }
            }
        }
    }

    // -----------------------------------------------------------------------
    // Rol bazlı player bulma
    // -----------------------------------------------------------------------
    private Player FindPlayerByRole(string role)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.TryGetValue("Role", out object existingRole))
            {
                if (existingRole != null && existingRole.ToString() == role)
                {
                    return p;
                }
            }
        }
        return null;
    }

    // -----------------------------------------------------------------------
    // RPC: (ID, Player) ile Ownership atama
    // -----------------------------------------------------------------------
    [PunRPC]
    private void SetOwnership(int viewID, Player newOwner)
    {
        Debug.Log($"[Spawner][SetOwnership] => viewID={viewID}, newOwner={newOwner?.NickName}");
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null)
        {
            targetView.TransferOwnership(newOwner);
            Debug.Log($"[Spawner][SetOwnership] => {targetView.gameObject.name} => {newOwner.NickName}");
        }
        else
        {
            Debug.LogError($"[Spawner][SetOwnership] PhotonView bulunamadı! ID={viewID}");
        }
    }
}
