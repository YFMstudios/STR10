using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using Cinemachine; // Cinemachine kütüphanesi için eklendi

public class BattleScenePlayerSpawner : MonoBehaviourPunCallbacks
{
    [Header("Spawn Points")]
    public Transform attackerSpawnPoint;
    public Transform defenderSpawnPoint;

    [Header("Character Objects")]
    public GameObject playerObject; // Attacker
    public GameObject enemyObject;  // Defender

    [Header("Cameras")]
    public CinemachineVirtualCamera attackerVirtualCam;
    public CinemachineVirtualCamera defenderVirtualCam;
    public CinemachineVirtualCamera spectatorVirtualCam;
    public Camera mainCamera;

    public SoldierController soldierManager;

    // İzleyici kamera kontrolü için değişkenler
    private bool isSpectator = false;
    private int currentSpectatorView = 0; // 0=spectator, 1=attacker, 2=defender

    void Awake()
    {
        if (soldierManager != null)
        {
            soldierManager.setBattleScenePlayerSpawner(this);
        }
        else
        {
            Debug.LogError("[BattleScenePlayerSpawner] SoldierManager atanmadı!");
        }
    }

    // Oyuncudan PlayerName özelliğini çekmek için yardımcı metod
    private string GetPlayerName(Player player)
    {
        if (player != null && player.CustomProperties.TryGetValue("PlayerName", out object playerNameObj))
        {
            return playerNameObj.ToString();
        }
        return "Bilinmeyen Oyuncu";
    }

    void Start()
    {
        // SoldierManager bağlantıları
        if (soldierManager != null)
        {
            MinionSpawner minionSpawner = FindObjectOfType<MinionSpawner>();
            EnemyMinionSpawner enemyMinionSpawner = FindObjectOfType<EnemyMinionSpawner>();

            if (minionSpawner != null)
                minionSpawner.soldierManager = soldierManager;

            if (enemyMinionSpawner != null)
                enemyMinionSpawner.soldierManager = soldierManager;
        }

        // Her istemci (Master veya değil) kod buradan geçer
        string playerName = GetPlayerName(PhotonNetwork.LocalPlayer);
        Debug.Log($"[Spawner] Start() -> PlayerName:{playerName}, IsMaster?: {PhotonNetwork.IsMasterClient}");

        if (playerObject == null || enemyObject == null)
        {
            Debug.LogError("[Spawner] Player veya Enemy objeleri inspector'da atamayı unutma!");
            return;
        }

        // Kameraların kontrolü
        if (attackerVirtualCam == null || defenderVirtualCam == null || spectatorVirtualCam == null || mainCamera == null)
        {
            Debug.LogError("[Spawner] Kameralar inspector'da atanmamış!");
            return;
        }

        // (1) Transform senkron ayarları (Opsiyonel, Inspector'dan da yapabilirsiniz)
        SetupTransformSync(playerObject);
        SetupTransformSync(enemyObject);

        // (2) İlk spawn için ikisini de aktif edelim
        playerObject.SetActive(true);
        enemyObject.SetActive(true);

        // (3) Photon'dan Role'ü kontrol edip spawn ve kamera ayarlarını yap
        HandlePlayerRoleFromPhoton();

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

        // Role işlemleri tamamlandıktan sonra bilgileri yazdır
        StartCoroutine(DelayedPrintInfo());
    }

    // Photon'dan role bilgisini alıp gerekli işlemleri yapan yeni metod
    private void HandlePlayerRoleFromPhoton()
    {
        // Photon'dan Role özelliğini al
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Role", out object roleObj))
        {
            string playerRole = roleObj.ToString();
            string playerName = GetPlayerName(PhotonNetwork.LocalPlayer);
            Debug.Log($"[Spawner] HandlePlayerRoleFromPhoton -> Photon'dan alınan rol: {playerRole}, Oyuncu: {playerName}");

            // SoldierController'a role bilgisini aktar
            if (soldierManager != null)
            {
                soldierManager.PlayerRole = playerRole;
                Debug.Log($"[Spawner] PlayerRole SoldierController'a set edildi: {playerRole}");
            }

            // Role göre spawn ve kamera ayarlarını yap
            if (playerRole == "spectator")
            {
                isSpectator = true;
                SetupCameraForSpectator();
                Debug.Log("[Spawner] İzleyici (spectator) modu aktifleştirildi");
            }
            else
            {
                SpawnPlayer(playerRole);
                SetupCameraForRole(playerRole);
                Debug.Log($"[Spawner] Oyuncu {playerRole} olarak spawn edildi ve kamerası ayarlandı");
            }
        }
        else
        {
            Debug.LogWarning("[Spawner] Photon'da Role property'si bulunamadı! Yeni rol ataması yapılıyor...");
            AssignAndSpawnPlayerRole(); // Eski rol atama mekanizmasını yedek olarak çağır
        }
    }

    void Update()
    {
        // Eğer izleyici rolündeyse, kamera değiştirme kontrollerini kontrol et
        if (isSpectator)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) // 1 tuşu - Attacker kamerası
            {
                SwitchSpectatorCamera(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) // 2 tuşu - Defender kamerası
            {
                SwitchSpectatorCamera(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0)) // 0 tuşu - Spectator kamerası
            {
                SwitchSpectatorCamera(0);
            }
        }
    }

    // İzleyici için kamera değiştirme metodu
    private void SwitchSpectatorCamera(int cameraIndex)
    {
        if (!isSpectator) return;

        currentSpectatorView = cameraIndex;
        Debug.Log($"[Spawner] İzleyici kamera değiştirdi: {cameraIndex}");

        // Tüm kameraları deaktive et
        attackerVirtualCam.gameObject.SetActive(false);
        defenderVirtualCam.gameObject.SetActive(false);
        spectatorVirtualCam.gameObject.SetActive(false);

        switch (cameraIndex)
        {
            case 1: // Attacker kamerası
                attackerVirtualCam.gameObject.SetActive(true);
                attackerVirtualCam.Follow = playerObject.transform;
                attackerVirtualCam.LookAt = playerObject.transform;
                Debug.Log("[Spawner] İzleyici Attacker kamerasına geçti");
                break;

            case 2: // Defender kamerası
                defenderVirtualCam.gameObject.SetActive(true);
                defenderVirtualCam.Follow = enemyObject.transform;
                defenderVirtualCam.LookAt = enemyObject.transform;
                Debug.Log("[Spawner] İzleyici Defender kamerasına geçti");
                break;

            default: // Varsayılan izleyici kamerası
                spectatorVirtualCam.gameObject.SetActive(true);
                Debug.Log("[Spawner] İzleyici spektator kamerasına geçti");
                break;
        }
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

        // İzleyicileri kontrol et ve say
        int spectatorCount = 0;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.TryGetValue("Role", out object roleObj) && roleObj.ToString() == "spectator")
            {
                spectatorCount++;
            }
        }

        Debug.Log($"[Spawner] Toplam izleyici sayısı: {spectatorCount}");
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
        string playerName = GetPlayerName(PhotonNetwork.LocalPlayer);
        Debug.Log($"[Spawner] AssignAndSpawnPlayerRole() => Oyuncu: {playerName}");

        // Odadaki oyuncu sayısını kontrol et
        if (PhotonNetwork.PlayerList.Length > 2)
        {
            // İlk iki oyuncu attacker ve defender olduğu için, 
            // üçüncü ve sonraki oyuncular izleyici olacak
            bool attackerExists = false;
            bool defenderExists = false;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.CustomProperties.TryGetValue("Role", out object existingRole))
                {
                    if (existingRole != null && existingRole.ToString() == "attacker")
                    {
                        attackerExists = true;
                    }
                    else if (existingRole != null && existingRole.ToString() == "defender")
                    {
                        defenderExists = true;
                    }
                }
            }

            // Eğer hem attacker hem defender varsa, bu oyuncu spectator olmalı
            if (attackerExists && defenderExists)
            {
                string spectatorRole = "spectator";
                Debug.Log($"[Spawner] {playerName} => rol: {spectatorRole} (izleyici)");

                // Photon'a role bilgisini gönder
                PhotonNetwork.LocalPlayer.SetCustomProperties(
                    new ExitGames.Client.Photon.Hashtable { { "Role", spectatorRole } }
                );

                // SoldierController'a role bilgisini aktar
                if (soldierManager != null)
                {
                    soldierManager.PlayerRole = spectatorRole;
                    Debug.Log("[Spawner] PlayerRole SoldierController'a set edildi: " + spectatorRole);
                }

                isSpectator = true; // İzleyici modunu aktifleştir
                SetupCameraForSpectator(); // İzleyici kamera kurulumu

                Debug.Log("[Spawner] Oyuncu Photon custom property olarak 'spectator' rolü atandı");
                return;
            }
        }

        // Normal rol atama süreci (attacker veya defender)
        bool attackerAlreadyExists = false;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.TryGetValue("Role", out object existingRole))
            {
                if (existingRole != null && existingRole.ToString() == "attacker")
                {
                    attackerAlreadyExists = true;
                    break;
                }
            }
        }

        string roleToAssign = attackerAlreadyExists ? "defender" : "attacker";
        Debug.Log($"[Spawner] {playerName} => rol: {roleToAssign}");

        // Photon'a role bilgisini gönder
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable { { "Role", roleToAssign } }
        );

        // SoldierController'a role bilgisini aktar
        if (soldierManager != null)
        {
            soldierManager.PlayerRole = roleToAssign;
            Debug.Log("[Spawner] PlayerRole SoldierController'a set edildi: " + roleToAssign);
        }

        SpawnPlayer(roleToAssign);
        SetupCameraForRole(roleToAssign);

        Debug.Log($"[Spawner] Oyuncu Photon custom property olarak '{roleToAssign}' rolü atandı");
    }

    // -----------------------------------------------------------------------
    // İzleyici için özel kamera kurulumu
    // -----------------------------------------------------------------------
    private void SetupCameraForSpectator()
    {
        string playerName = GetPlayerName(PhotonNetwork.LocalPlayer);
        Debug.Log($"[Spawner] SetupCameraForSpectator() -> Oyuncu: {playerName}");

        // Önce tüm kameraları deaktive et
        attackerVirtualCam.gameObject.SetActive(false);
        defenderVirtualCam.gameObject.SetActive(false);
        spectatorVirtualCam.gameObject.SetActive(true); // Spectator kamerasını aktif et

        // MainCamera'nın aktif olduğunu garantile
        mainCamera.gameObject.SetActive(true);

        Debug.Log("[Spawner] İzleyici başlangıçta spektatör kamerasını kullanıyor. 0-1-2 tuşları ile değiştirebilir.");
    }

    // -----------------------------------------------------------------------
    // Seçilen role göre kamera ayarlamaları yapılır
    // -----------------------------------------------------------------------
    private void SetupCameraForRole(string role)
    {
        string playerName = GetPlayerName(PhotonNetwork.LocalPlayer);
        Debug.Log($"[Spawner] SetupCameraForRole({role}) -> Oyuncu: {playerName}");

        // Önce tüm kameraları deaktive et
        attackerVirtualCam.gameObject.SetActive(false);
        defenderVirtualCam.gameObject.SetActive(false);
        spectatorVirtualCam.gameObject.SetActive(false);

        // Role göre ilgili kamerayı aktifleştir
        if (role == "attacker")
        {
            // Attacker kamerasını aktifleştir
            attackerVirtualCam.gameObject.SetActive(true);

            // Follow hedefi olarak player objesi ayarla
            attackerVirtualCam.Follow = playerObject.transform;
            attackerVirtualCam.LookAt = playerObject.transform;

            Debug.Log("[Spawner] Attacker kamerası aktifleştirildi");
        }
        else if (role == "defender")
        {
            // Defender kamerasını aktifleştir
            defenderVirtualCam.gameObject.SetActive(true);

            // Follow hedefi olarak enemy objesi ayarla
            defenderVirtualCam.Follow = enemyObject.transform;
            defenderVirtualCam.LookAt = enemyObject.transform;

            Debug.Log("[Spawner] Defender kamerası aktifleştirildi");
        }
        else
        {
            // Spectator kamerasını aktifleştir (oyuncu ne attacker ne de defender)
            spectatorVirtualCam.gameObject.SetActive(true);
            Debug.Log("[Spawner] Spectator kamerası aktifleştirildi (rol: " + role + ")");
        }

        // MainCamera'nın aktif olduğunu garantile
        mainCamera.gameObject.SetActive(true);
    }

    // -----------------------------------------------------------------------
    // Seçilen role göre objeleri konumlandırıp Ownership veriyoruz
    // -----------------------------------------------------------------------
    private void SpawnPlayer(string role)
    {
        string playerName = GetPlayerName(PhotonNetwork.LocalPlayer);
        Debug.Log($"[Spawner] SpawnPlayer({role}) -> Oyuncu: {playerName}");

        if (role == "attacker")
        {
            // Player (Attacker) Ownership bende
            playerObject.transform.SetPositionAndRotation(attackerSpawnPoint.position, attackerSpawnPoint.rotation);

            photonView.RPC(nameof(SetOwnership), RpcTarget.AllBuffered,
                playerObject.GetComponent<PhotonView>().ViewID,
                PhotonNetwork.LocalPlayer);

            // Enemy sahnede dursun (Ownership yok)
            enemyObject.transform.SetPositionAndRotation(defenderSpawnPoint.position, defenderSpawnPoint.rotation);

            Debug.Log("[Spawner] Oyuncu attackerSpawnPoint'e konumlandırıldı ve ownership verildi");
        }
        else if (role == "defender")
        {
            // Enemy (Defender) Ownership bende
            enemyObject.transform.SetPositionAndRotation(defenderSpawnPoint.position, defenderSpawnPoint.rotation);

            photonView.RPC(nameof(SetOwnership), RpcTarget.AllBuffered,
                enemyObject.GetComponent<PhotonView>().ViewID,
                PhotonNetwork.LocalPlayer);

            // Player sahnede dursun (Ownership yok)
            playerObject.transform.SetPositionAndRotation(attackerSpawnPoint.position, attackerSpawnPoint.rotation);

            Debug.Log("[Spawner] Oyuncu defenderSpawnPoint'e konumlandırıldı ve ownership verildi");
        }
        else
        {
            Debug.LogWarning($"[Spawner] Geçersiz rol ({role}) için SpawnPlayer çağrıldı!");
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
                string attackerName = GetPlayerName(attackerOwner);
                Debug.Log($"[Spawner] Attacker ({attackerName}) respawn -> RPC_RespawnCharacter, ownerActorNum={ownerActorNum}");

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
                string defenderName = GetPlayerName(defenderOwner);
                Debug.Log($"[Spawner] Defender ({defenderName}) respawn -> RPC_RespawnCharacter => ownerActorNum={ownerActorNum}");

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
        string playerName = GetPlayerName(PhotonNetwork.LocalPlayer);
        Debug.Log($"[Spawner][RPC_RespawnCharacter] => role={role}, ownerActorNum={ownerActorNumber}, " +
                  $"PlayerName:{playerName}, Master?:{PhotonNetwork.IsMasterClient}");

        // Photon custom property'den oyuncunun rolünü al
        string playerRole = "";
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Role", out object roleObj))
        {
            playerRole = roleObj.ToString();
        }

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
                    string ownerName = GetPlayerName(realOwner);
                    Debug.Log($"[Spawner][RPC_RespawnCharacter] => TransferOwnership -> {ownerName}");
                    playerObject.GetComponent<PhotonView>().TransferOwnership(realOwner);
                }
            }

            // Yeniden doğduktan sonra ilgili kişinin kamera takibini düzelt
            if (playerRole == "attacker")
            {
                attackerVirtualCam.Follow = playerObject.transform;
                attackerVirtualCam.LookAt = playerObject.transform;
                Debug.Log("[Spawner][RPC_RespawnCharacter] Attacker kamera takibi güncellendi");
            }

            // İzleyici modunda ve attacker kamerası kullanılıyorsa takibi güncelle
            if (isSpectator && currentSpectatorView == 1)
            {
                attackerVirtualCam.Follow = playerObject.transform;
                attackerVirtualCam.LookAt = playerObject.transform;
                Debug.Log("[Spawner][RPC_RespawnCharacter] İzleyici attacker kamera takibi güncellendi");
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
                    string ownerName = GetPlayerName(realOwner);
                    Debug.Log($"[Spawner][RPC_RespawnCharacter] => TransferOwnership -> {ownerName}");
                    enemyObject.GetComponent<PhotonView>().TransferOwnership(realOwner);
                }
            }

            // Yeniden doğduktan sonra ilgili kişinin kamera takibini düzelt
            if (playerRole == "defender")
            {
                defenderVirtualCam.Follow = enemyObject.transform;
                defenderVirtualCam.LookAt = enemyObject.transform;
                Debug.Log("[Spawner][RPC_RespawnCharacter] Defender kamera takibi güncellendi");
            }

            // İzleyici modunda ve defender kamerası kullanılıyorsa takibi güncelle
            if (isSpectator && currentSpectatorView == 2)
            {
                defenderVirtualCam.Follow = enemyObject.transform;
                defenderVirtualCam.LookAt = enemyObject.transform;
                Debug.Log("[Spawner][RPC_RespawnCharacter] İzleyici defender kamera takibi güncellendi");
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
        string ownerName = GetPlayerName(newOwner);
        Debug.Log($"[Spawner][SetOwnership] => viewID={viewID}, newOwner={ownerName}");
        
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null)
        {
            targetView.TransferOwnership(newOwner);
            Debug.Log($"[Spawner][SetOwnership] => {targetView.gameObject.name} => {ownerName}");
        }
        else
        {
            Debug.LogError($"[Spawner][SetOwnership] PhotonView bulunamadı! ID={viewID}");
        }
    }

    // -----------------------------------------------------------------------
    // Photon Player Properties değişimini takip etmek için override
    // -----------------------------------------------------------------------
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        // Eğer değişen özellik "Role" ise ve bu yerel oyuncudan geliyorsa kamera ve spawn ayarlarını güncelle
        if (changedProps.ContainsKey("Role") && targetPlayer.IsLocal)
        {
            string newRole = changedProps["Role"].ToString();
            string playerName = GetPlayerName(targetPlayer);
            Debug.Log($"[Spawner] OnPlayerPropertiesUpdate -> Rol değişimi algılandı: {newRole}, Oyuncu: {playerName}");

            // SoldierController'a role bilgisini aktar
            if (soldierManager != null)
            {
                soldierManager.PlayerRole = newRole;
                Debug.Log($"[Spawner] PlayerRole SoldierController'a güncellendi: {newRole}");
            }

            // Role göre spawn ve kamera ayarlarını güncelle
            if (newRole == "spectator")
            {
                isSpectator = true;
                SetupCameraForSpectator();
            }
            else
            {
                isSpectator = false;
                SpawnPlayer(newRole);
                SetupCameraForRole(newRole);
            }
        }
    }



}