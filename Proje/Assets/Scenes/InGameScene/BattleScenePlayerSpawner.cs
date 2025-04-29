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

    [Header("Respawn Settings")]
    public float respawnDelay = 5f;

    public SoldierController soldierManager;
    public BattleSceneCameraManager cameraManager;

    // İzleyici kontrolü için değişken
    private bool isSpectator = false;

    // Respawn kontrolü için değişkenler
    private bool isRespawningAttacker = false;
    private bool isRespawningDefender = false;

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

        if (playerObject == null || enemyObject == null)
        {
            Debug.LogError("[Spawner] Player veya Enemy objeleri atanmamış!");
            return;
        }

        if (cameraManager == null)
        {
            Debug.LogError("[Spawner] Kamera yöneticisi atanmamış!");
            return;
        }

        // Transform senkron ayarları
        SetupTransformSync(playerObject);
        SetupTransformSync(enemyObject);

        // İlk spawn için objeleri aktif et
        playerObject.SetActive(true);
        enemyObject.SetActive(true);

        // Photon'dan Role'ü kontrol edip spawn ve kamera ayarlarını yap
        HandlePlayerRoleFromPhoton();
    }

    // Photon'dan role bilgisini alıp gerekli işlemleri yapan metod
    private void HandlePlayerRoleFromPhoton()
    {
        // Photon'dan Role özelliğini al
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Role", out object roleObj))
        {
            string playerRole = roleObj.ToString();

            // SoldierController'a role bilgisini aktar
            if (soldierManager != null)
            {
                soldierManager.PlayerRole = playerRole;
            }

            // Role göre spawn ve kamera ayarlarını yap
            if (playerRole == "spectator")
            {
                isSpectator = true;
                SpawnSpectator();
                cameraManager.SetupCameraForRole(playerRole, playerObject.transform, enemyObject.transform);
            }
            else
            {
                isSpectator = false;
                SpawnPlayer(playerRole);
                cameraManager.SetupCameraForRole(playerRole, playerObject.transform, enemyObject.transform);
            }
        }
        else
        {
            Debug.LogWarning("[Spawner] Photon'da Role property'si bulunamadı! Yeni rol ataması yapılıyor...");
            AssignAndSpawnPlayerRole();
        }
    }

    // Sadece spectator rolü için metod
    private void SpawnSpectator()
    {
        // playerObject ve enemyObject'in konumlarını ayarla (oyuncunun görebilmesi için)
        playerObject.transform.SetPositionAndRotation(attackerSpawnPoint.position, attackerSpawnPoint.rotation);
        enemyObject.transform.SetPositionAndRotation(defenderSpawnPoint.position, defenderSpawnPoint.rotation);
    }

    // PhotonTransformViewClassic senkronizasyon ayarları
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
            transformView.m_PositionModel.InterpolateOption = PhotonTransformViewPositionModel.InterpolateOptions.EstimatedSpeed;
            transformView.m_PositionModel.ExtrapolateOption = PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues;

            transformView.m_RotationModel.SynchronizeEnabled = true;
            transformView.m_RotationModel.InterpolateOption = PhotonTransformViewRotationModel.InterpolateOptions.Lerp;
        }
        else
        {
            Debug.LogWarning($"[SetupTransformSync] {obj.name} üzerinde PhotonTransformViewClassic yok!");
        }
    }

    // Her istemci hangi rolü (attacker/defender) alacak
    private void AssignAndSpawnPlayerRole()
    {
        // Odadaki oyuncu sayısını kontrol et
        if (PhotonNetwork.PlayerList.Length > 2)
        {
            bool attackerExists = false;
            bool defenderExists = false;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.CustomProperties.TryGetValue("Role", out object existingRole))
                {
                    if (existingRole.ToString() == "attacker")
                    {
                        attackerExists = true;
                    }
                    else if (existingRole.ToString() == "defender")
                    {
                        defenderExists = true;
                    }
                }
            }

            // Eğer hem attacker hem defender varsa, bu oyuncu spectator olmalı
            if (attackerExists && defenderExists)
            {
                string spectatorRole = "spectator";

                // Photon'a role bilgisini gönder
                PhotonNetwork.LocalPlayer.SetCustomProperties(
                    new ExitGames.Client.Photon.Hashtable { { "Role", spectatorRole } }
                );

                // SoldierController'a role bilgisini aktar
                if (soldierManager != null)
                {
                    soldierManager.PlayerRole = spectatorRole;
                }

                isSpectator = true;
                SpawnSpectator();
                cameraManager.SetupCameraForRole(spectatorRole, playerObject.transform, enemyObject.transform);
                return;
            }
        }

        // Normal rol atama süreci (attacker veya defender)
        bool attackerAlreadyExists = false;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.TryGetValue("Role", out object existingRole))
            {
                if (existingRole.ToString() == "attacker")
                {
                    attackerAlreadyExists = true;
                    break;
                }
            }
        }

        string roleToAssign = attackerAlreadyExists ? "defender" : "attacker";

        // Photon'a role bilgisini gönder
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable { { "Role", roleToAssign } }
        );

        // SoldierController'a role bilgisini aktar
        if (soldierManager != null)
        {
            soldierManager.PlayerRole = roleToAssign;
        }

        SpawnPlayer(roleToAssign);
        cameraManager.SetupCameraForRole(roleToAssign, playerObject.transform, enemyObject.transform);
    }

    // Seçilen role göre objeleri konumlandırıp Ownership veriyoruz
    private void SpawnPlayer(string role)
    {
        if (role == "attacker")
        {
            // Player (Attacker) Ownership bende
            playerObject.transform.SetPositionAndRotation(attackerSpawnPoint.position, attackerSpawnPoint.rotation);

            // DÜZELTME: Ownership'i BufferedAll ile yolla ki, sonradan bağlanan oyuncular da durumu görsün
            photonView.RPC(nameof(SetOwnership), RpcTarget.AllBuffered,
                playerObject.GetComponent<PhotonView>().ViewID,
                PhotonNetwork.LocalPlayer);

            // Enemy sahnede dursun (Ownership yok)
            enemyObject.transform.SetPositionAndRotation(defenderSpawnPoint.position, defenderSpawnPoint.rotation);

            // DÜZELTME: Tüm oyunculara pozisyonun değiştiğini bildir
            photonView.RPC(nameof(SyncPositions), RpcTarget.Others);
        }
        else if (role == "defender")
        {
            // Enemy (Defender) Ownership bende
            enemyObject.transform.SetPositionAndRotation(defenderSpawnPoint.position, defenderSpawnPoint.rotation);

            // DÜZELTME: Ownership'i BufferedAll ile yolla ki, sonradan bağlanan oyuncular da durumu görsün
            photonView.RPC(nameof(SetOwnership), RpcTarget.AllBuffered,
                enemyObject.GetComponent<PhotonView>().ViewID,
                PhotonNetwork.LocalPlayer);

            // Player sahnede dursun (Ownership yok)
            playerObject.transform.SetPositionAndRotation(attackerSpawnPoint.position, attackerSpawnPoint.rotation);

            // DÜZELTME: Tüm oyunculara pozisyonun değiştiğini bildir
            photonView.RPC(nameof(SyncPositions), RpcTarget.Others);
        }
        else
        {
            Debug.LogWarning($"[Spawner] Geçersiz rol ({role}) için SpawnPlayer çağrıldı!");
        }
    }

    // Tüm oyuncularda karakter konumlarını senkronize et
    [PunRPC]
    private void SyncPositions()
    {
        Debug.Log("[Spawner] SyncPositions RPC çağrıldı - tüm pozisyonlar senkronize ediliyor");

        // Her iki karakteri görünür yap ve konumları ayarla
        if (!playerObject.activeInHierarchy)
            playerObject.SetActive(true);

        if (!enemyObject.activeInHierarchy)
            enemyObject.SetActive(true);

        playerObject.transform.SetPositionAndRotation(attackerSpawnPoint.position, attackerSpawnPoint.rotation);
        enemyObject.transform.SetPositionAndRotation(defenderSpawnPoint.position, defenderSpawnPoint.rotation);
    }

    // Karakter öldüğünde Stats.cs tarafından çağrılan metot
    public void NotifyCharacterDied(string role)
    {
        Debug.Log($"[Spawner] NotifyCharacterDied({role}) çağrıldı.");

        if ((role == "attacker" && isRespawningAttacker) ||
            (role == "defender" && isRespawningDefender))
        {
            Debug.Log($"[Spawner] {role} zaten respawn ediliyor, işlem atlanıyor.");
            return;
        }

        // Durumu burada güncelleyelim
        if (role == "attacker")
        {
            isRespawningAttacker = true;

            // Attacker'ı deaktif et (diğer oyuncular da görsün)
            playerObject.SetActive(false);
            photonView.RPC(nameof(DeactivateCharacter), RpcTarget.Others, "attacker");
        }
        else if (role == "defender")
        {
            isRespawningDefender = true;

            // Defender'ı deaktif et (diğer oyuncular da görsün)
            enemyObject.SetActive(false);
            photonView.RPC(nameof(DeactivateCharacter), RpcTarget.Others, "defender");
        }

        // Karakterlerin durumunu kontrol et ve ekrana yazdır
        Debug.Log($"[Spawner] Character statuses: Attacker active={playerObject.activeInHierarchy}, Defender active={enemyObject.activeInHierarchy}");

        // YENI YÖNTEM: RespawnManager kullan!
        // Respawn Manager'a görev ver (eğer varsa)
        RespawnManager manager = RespawnManager.Instance;

        if (manager != null)
        {
            Debug.Log($"[Spawner] {role} için respawn işi RespawnManager'a verildi.");

            int ownerActorNum = -1;
            if (role == "attacker")
            {
                Player owner = FindPlayerByRole("attacker");
                ownerActorNum = (owner != null) ? owner.ActorNumber : -1;
            }
            else if (role == "defender")
            {
                Player owner = FindPlayerByRole("defender");
                ownerActorNum = (owner != null) ? owner.ActorNumber : -1;
            }

            manager.ScheduleRespawn(role, respawnDelay, ownerActorNum);
        }
        else
        {
            Debug.LogError("[Spawner] RespawnManager bulunamadı! RPC yöntemi deneniyor...");

            // Eski RPC yöntemi yedek olarak kalsın
            Debug.Log($"[Spawner] {role} için yeniden doğma RPC'si çağrılıyor...");
            photonView.RPC(nameof(RPC_StartRespawnTimer), RpcTarget.All, role);
        }
    }

    // Tüm oyuncularda belirtilen karakteri deaktif et
    [PunRPC]
    private void DeactivateCharacter(string role)
    {
        Debug.Log($"[Spawner] DeactivateCharacter RPC çağrıldı: {role}");

        if (role == "attacker")
        {
            playerObject.SetActive(false);
        }
        else if (role == "defender")
        {
            enemyObject.SetActive(false);
        }
    }

    // Ölüm gerçekleşince tüm clientlerde respawn zamanlayıcısı başlatır
    [PunRPC]
    private void RPC_StartRespawnTimer(string role)
    {
        Debug.Log($"[Spawner] RPC_StartRespawnTimer({role}) başlatıldı. {respawnDelay} saniye beklenecek.");

        try
        {
            // Respawn işlemini başlat
            StartCoroutine(RespawnAfterDelay(role));
            Debug.Log("[Spawner] Coroutine başarıyla başlatıldı!");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Spawner] RPC_StartRespawnTimer'da exception: {ex.Message}");

            // Exception durumunda 0.5 saniye sonra doğrudan respawn et
            // RespawnManager başarısız olursa kullanılır
            StartCoroutine(EmergencyRespawn(role, 0.5f));
        }
    }

    // Acil durum respawn (exception veya hata durumları için)
    private IEnumerator EmergencyRespawn(string role, float delay)
    {
        Debug.Log($"[Spawner] EmergencyRespawn bekliyor: {delay}s");
        yield return new WaitForSeconds(delay);

        int ownerActorNum = -1;
        if (role == "attacker")
        {
            Player owner = FindPlayerByRole("attacker");
            ownerActorNum = (owner != null) ? owner.ActorNumber : -1;
        }
        else if (role == "defender")
        {
            Player owner = FindPlayerByRole("defender");
            ownerActorNum = (owner != null) ? owner.ActorNumber : -1;
        }

        ForceRespawnCharacter(role, ownerActorNum);
    }

    // Belirli süre sonra karakteri yeniden doğur
    private IEnumerator RespawnAfterDelay(string role)
    {
        Debug.Log($"[Spawner] RespawnAfterDelay coroutine başladı, role={role}");

        // Tam olarak belirtilen süre kadar bekle
        yield return new WaitForSeconds(respawnDelay);

        Debug.Log($"[Spawner] {respawnDelay} saniye geçti, {role} yeniden doğuyor...");

        try
        {
            // Her client kendi karakterini respawn edebilsin
            if (role == "attacker")
            {
                Player attackerOwner = FindPlayerByRole("attacker");
                int ownerActorNum = (attackerOwner != null) ? attackerOwner.ActorNumber : -1;

                Debug.Log($"[Spawner] Attacker respawn RPC çağrılıyor, ownerActorNum={ownerActorNum}");

                // Önce yerel olarak respawn et
                ForceRespawnCharacter(role, ownerActorNum);

                // Sonra tüm istemcilere respawn komutunu gönder 
                photonView.RPC(nameof(RPC_RespawnCharacter), RpcTarget.Others, role, ownerActorNum);
            }
            else if (role == "defender")
            {
                Player defenderOwner = FindPlayerByRole("defender");
                int ownerActorNum = (defenderOwner != null) ? defenderOwner.ActorNumber : -1;

                Debug.Log($"[Spawner] Defender respawn RPC çağrılıyor, ownerActorNum={ownerActorNum}");

                // Önce yerel olarak respawn et
                ForceRespawnCharacter(role, ownerActorNum);

                // Sonra tüm istemcilere respawn komutunu gönder
                photonView.RPC(nameof(RPC_RespawnCharacter), RpcTarget.Others, role, ownerActorNum);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Spawner] RespawnAfterDelay'de exception: {ex.Message}");

            // Exception durumunda RespawnManager'a bildir
            RespawnManager manager = RespawnManager.Instance;

            if (manager != null)
            {
                Debug.Log($"[Spawner] Exception sonrası respawn işi RespawnManager'a verildi.");

                int ownerActorNum = -1;
                if (role == "attacker")
                {
                    Player owner = FindPlayerByRole("attacker");
                    ownerActorNum = (owner != null) ? owner.ActorNumber : -1;
                }
                else if (role == "defender")
                {
                    Player owner = FindPlayerByRole("defender");
                    ownerActorNum = (owner != null) ? owner.ActorNumber : -1;
                }

                // Hemen respawn yap
                manager.ScheduleRespawn(role, 0.1f, ownerActorNum);
            }
        }
    }

    // RPC: Karakter yeniden doğma - RespawnManager'dan da çağrılabilir
    [PunRPC]
    private void RPC_RespawnCharacter(string role, int ownerActorNumber)
    {
        Debug.Log($"[Spawner][RPC_RespawnCharacter] => role={role}, ownerActorNum={ownerActorNumber}");
        ForceRespawnCharacter(role, ownerActorNumber);
    }

    // RespawnManager veya RPC tarafından çağrılabilir - İmzayı değiştirme (RespawnManager ile uyumluluk için)
    public void ForceRespawnCharacter(string role, int ownerActorNumber = -1)
    {
        Debug.Log($"[Spawner][ForceRespawnCharacter] => role={role}, ownerActorNum={ownerActorNumber}");

        if (role == "attacker")
        {
            Debug.Log("[Spawner] Attacker yeniden doğuyor!");

            // Player yeniden doğsun
            playerObject.transform.SetPositionAndRotation(attackerSpawnPoint.position, attackerSpawnPoint.rotation);

            // Aktif hale getir - BU ÇOK ÖNEMLİ!
            playerObject.SetActive(true);

            Stats stats = playerObject.GetComponent<Stats>();
            if (stats != null)
            {
                stats.ResetHealthToFull();
                Debug.Log("[Spawner] Attacker can yenilendi!");
            }
            else
            {
                Debug.LogError("[Spawner] Attacker Stats komponenti bulunamadı!");
            }

            // Ownership devrini sadece MasterClient yapar
            if (PhotonNetwork.IsMasterClient && ownerActorNumber != -1)
            {
                Player realOwner = PhotonNetwork.CurrentRoom.GetPlayer(ownerActorNumber);
                if (realOwner != null)
                {
                    playerObject.GetComponent<PhotonView>().TransferOwnership(realOwner);
                    Debug.Log($"[Spawner] Attacker ownership transferi: ActorNum={ownerActorNumber}");
                }
                else
                {
                    Debug.LogError($"[Spawner] ActorNumber={ownerActorNumber} için Player bulunamadı!");
                }
            }

            // Yeniden doğduktan sonra kamera takiplerini güncelle
            if (cameraManager != null)
            {
                cameraManager.UpdateCameraFollowTarget(role, playerObject.transform);
                Debug.Log("[Spawner] Attacker için kamera güncellendi");
            }

            // Respawn durumunu güncelle
            isRespawningAttacker = false;
            Debug.Log("[Spawner] Attacker respawn process tamamlandı");

            // Tüm oyunculara bildir
            photonView.RPC(nameof(SyncPositions), RpcTarget.Others);
        }
        else if (role == "defender")
        {
            Debug.Log("[Spawner] Defender yeniden doğuyor!");

            // Enemy yeniden doğsun
            enemyObject.transform.SetPositionAndRotation(defenderSpawnPoint.position, defenderSpawnPoint.rotation);

            // Aktiflik kontrolü
            Debug.Log($"[Spawner] Defender aktivasyon öncesi: {enemyObject.activeInHierarchy}");

            // KRITIK FIX: Aktif hale getir - BU ÇOK ÖNEMLİ!
            enemyObject.SetActive(true);

            Debug.Log($"[Spawner] Defender aktivasyon sonrası: {enemyObject.activeInHierarchy}");

            Stats stats = enemyObject.GetComponent<Stats>();
            if (stats != null)
            {
                stats.ResetHealthToFull();
                Debug.Log("[Spawner] Defender can yenilendi!");
            }
            else
            {
                Debug.LogError("[Spawner] Defender Stats komponenti bulunamadı!");
            }

            // Ownership devrini sadece MasterClient yapar
            if (PhotonNetwork.IsMasterClient && ownerActorNumber != -1)
            {
                Player realOwner = PhotonNetwork.CurrentRoom.GetPlayer(ownerActorNumber);
                if (realOwner != null)
                {
                    enemyObject.GetComponent<PhotonView>().TransferOwnership(realOwner);
                    Debug.Log($"[Spawner] Defender ownership transferi: ActorNum={ownerActorNumber}");
                }
                else
                {
                    Debug.LogError($"[Spawner] ActorNumber={ownerActorNumber} için Player bulunamadı!");
                }
            }

            // Yeniden doğduktan sonra kamera takiplerini güncelle
            if (cameraManager != null)
            {
                cameraManager.UpdateCameraFollowTarget(role, enemyObject.transform);
                Debug.Log("[Spawner] Defender için kamera güncellendi");
            }

            // Respawn durumunu güncelle
            isRespawningDefender = false;
            Debug.Log("[Spawner] Defender respawn process tamamlandı");

            // Tüm oyunculara bildir
            photonView.RPC(nameof(SyncPositions), RpcTarget.Others);
        }
    }

    // Rol bazlı player bulma
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

    // RPC: (ID, Player) ile Ownership atama
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
            Debug.LogError($"[Spawner][SetOwnership] PhotonView bulunamadı! ID={viewID}");
        }
    }

    // Photon Player Properties değişimini takip etmek için override
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        // Eğer değişen özellik "Role" ise ve bu yerel oyuncudan geliyorsa kamera ve spawn ayarlarını güncelle
        if (changedProps.ContainsKey("Role") && targetPlayer.IsLocal)
        {
            string newRole = changedProps["Role"].ToString();

            // SoldierController'a role bilgisini aktar
            if (soldierManager != null)
            {
                soldierManager.PlayerRole = newRole;
            }

            // Role göre spawn ve kamera ayarlarını güncelle
            if (newRole == "spectator")
            {
                isSpectator = true;
                SpawnSpectator();
                cameraManager.SetupCameraForRole(newRole, playerObject.transform, enemyObject.transform);
            }
            else
            {
                isSpectator = false;
                SpawnPlayer(newRole);
                cameraManager.SetupCameraForRole(newRole, playerObject.transform, enemyObject.transform);
            }
        }
    }

    // Yeni bir oyuncu bağlandığında tam bir senkronizasyon yap
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Debug.Log($"[Spawner] Yeni oyuncu katıldı: {GetPlayerName(newPlayer)}");

        // Yeni oyuncuya mevcut durumu bildir
        photonView.RPC(nameof(SyncPositions), RpcTarget.All);
    }
}