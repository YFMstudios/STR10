using UnityEngine;
using Photon.Pun;
using Cinemachine;
using System.Collections;

public class BattleSceneCameraManager : MonoBehaviourPunCallbacks
{
    [Header("Cameras")]
    public CinemachineVirtualCamera attackerVirtualCam;
    public CinemachineVirtualCamera defenderVirtualCam;
    public CinemachineVirtualCamera spectatorVirtualCam;
    public Camera mainCamera;

    // class başına, mevcut değişkenlerin hemen altına EKLE
[Header("Static cams (1‑4)")]
[SerializeField] public CinemachineVirtualCamera[] staticCams;   // 1‑4 için
private bool staticViewActive   = false;   // “ölüm modu” açık mı?
private int  currentStaticIndex = 0;

    // İzleyici kamera kontrolü için değişkenler
    [SerializeField] // Inspector'da görülebilmesi için
    private bool isSpectator = false;

    [SerializeField] // Inspector'da görülebilmesi için
    private int currentSpectatorView = 0; // 0=spectator, 1=attacker, 2=defender

    [SerializeField] // Inspector'da görülebilmesi için
    private Transform attackerTarget;

    [SerializeField] // Inspector'da görülebilmesi için
    private Transform defenderTarget;

    // Debug amaçlı, tuş algılama loglarını ekranda gösterelim
    private float debugMessageTime = 5.0f; // Mesajın ekranda kalma süresi
    private string debugMessage = "";

    void Start()
    {
        // Kameraların kontrolü
        if (attackerVirtualCam == null || defenderVirtualCam == null || spectatorVirtualCam == null || mainCamera == null)
        {
            Debug.LogError("[CameraManager] Kameralar inspector'da atanmamış!");
            return;
        }

        // Başlangıçta tüm kameraları deaktif et
        attackerVirtualCam.gameObject.SetActive(false);
        defenderVirtualCam.gameObject.SetActive(false);
        spectatorVirtualCam.gameObject.SetActive(false);

        // Startup log
        Debug.Log("[CameraManager] Start çağrıldı, kamera yöneticisi başlatıldı.");

        // Başlangıçta durum bilgilerini logla
        LogDebugInfo("Kamera yöneticisi başlatıldı");
    }

    /* --------------------------------------------------------
 *  Update()
 * ------------------------------------------------------ */
void Update()
{
    /* ----------------------------------------------------
     * STATİK KAMERA TUŞLARI  (1‑4)
     * -------------------------------------------------- */
    bool pressed1 = Input.GetKeyDown(KeyCode.Alpha1);
    bool pressed2 = Input.GetKeyDown(KeyCode.Alpha2);
    bool pressed3 = Input.GetKeyDown(KeyCode.Alpha3);
    bool pressed4 = Input.GetKeyDown(KeyCode.Alpha4);

    /* ► İzleyici ise daima, oyuncu ise sadece ölüm modunda */
    bool staticAllowed = isSpectator || staticViewActive;

    if (staticAllowed)
    {
        if (pressed1) ActivateStaticCam(0);
        if (pressed2) ActivateStaticCam(1);
        if (pressed3) ActivateStaticCam(2);
        if (pressed4) ActivateStaticCam(3);
    }

    /* ----------------------------------------------------
     * Q / E  (karakter kameraları)  – yalnızca izleyici
     * -------------------------------------------------- */
    if (isSpectator && !staticViewActive)
    {
        if (Input.GetKeyDown(KeyCode.Q)) SwitchSpectatorCamera(1);
        if (Input.GetKeyDown(KeyCode.E)) SwitchSpectatorCamera(2);
    }

    /* Debug */
    if (Input.GetKeyDown(KeyCode.Space)) LogAllStatus();
}


public void LeaveStaticView()
{
    staticViewActive = false;                     // <─ 1‑4 tuşları artık pasif
    foreach (var cam in staticCams) cam.gameObject.SetActive(false);
    // Karakter/rol kamerası hangisiyse onu açık bırakıyoruz
    if (isSpectator)          spectatorVirtualCam.gameObject.SetActive(true);
    else if (currentSpectatorView == 1) attackerVirtualCam.gameObject.SetActive(true);
    else if (currentSpectatorView == 2) defenderVirtualCam.gameObject.SetActive(true);
}

/* --------------------------------------------------------
 *  Oyuncu/izleyici öldüğünde çağrılır
 * ------------------------------------------------------ */
public void GoToStaticView()
{
    if (staticViewActive) return;          // zaten statik modda

    staticViewActive = true;

    attackerVirtualCam .gameObject.SetActive(false);
    defenderVirtualCam .gameObject.SetActive(false);
    spectatorVirtualCam.gameObject.SetActive(false);

    ActivateStaticCam(0);                  // 1.kameradan başla
    LogDebugInfo("Statik kamera moduna geçildi (1‑4 etkin)");
}

/* --------------------------------------------------------
 *  Statik kamerayı etkinleştir (0‑3)
 * ------------------------------------------------------ */
private void ActivateStaticCam(int idx)
{
    if (idx < 0 || idx >= staticCams.Length) return;

    foreach (var cam in staticCams) cam.gameObject.SetActive(false);
    staticCams[idx].gameObject.SetActive(true);

    currentStaticIndex = idx;
    Debug.Log($"[Camera] Statik Kamera {idx + 1} aktif");
}

private void SelectStaticCam(int idx)
{
    if (idx < 0 || idx >= staticCams.Length)
    {
        Debug.LogWarning($"[CameraManager] Geçersiz statik cam index: {idx}");
        return;
    }

    // tüm sanal kameraları kapat
    attackerVirtualCam.gameObject.SetActive(false);
    defenderVirtualCam.gameObject.SetActive(false);
    spectatorVirtualCam.gameObject.SetActive(false);

    foreach (var c in staticCams) c.gameObject.SetActive(false);

    staticCams[idx].gameObject.SetActive(true);
    currentStaticIndex = idx;
    isSpectator        = true;          // bu görünümde artık serbest izleyici
    currentSpectatorView = 0;

    LogDebugInfo($"Statik kamera {(idx+1)} aktif");
}

    // Debug bilgilerini logla ve ekranda göster
    private void LogDebugInfo(string message)
    {
        debugMessage = message + " | isSpectator: " + isSpectator +
                       " | attacker: " + (attackerTarget != null ? attackerTarget.name : "null") +
                       " | defender: " + (defenderTarget != null ? defenderTarget.name : "null");

        Debug.Log("[CameraManager] " + debugMessage);
        StartCoroutine(ClearDebugMessage());
    }

    private IEnumerator ClearDebugMessage()
    {
        yield return new WaitForSeconds(debugMessageTime);
        debugMessage = "";
    }

    // Ekranda debug bilgilerini göster
    void OnGUI()
    {
        if (!string.IsNullOrEmpty(debugMessage))
        {
            GUI.Box(new Rect(10, 10, Screen.width - 20, 25), "");
            GUI.Label(new Rect(15, 15, Screen.width - 30, 20), debugMessage);
        }

        // Her zaman gösterilecek durum paneli
        string statusText = string.Format("İzleyici: {0} | Mevcut Görünüm: {1} | Q: Attack, E: Defend | Space: Log",
            isSpectator ? "Evet" : "Hayır",
            currentSpectatorView == 0 ? "Spectator" : (currentSpectatorView == 1 ? "Attacker" : "Defender"));

        GUI.Box(new Rect(10, Screen.height - 35, Screen.width - 20, 25), "");
        GUI.Label(new Rect(15, Screen.height - 30, Screen.width - 30, 20), statusText);
    }

    // -----------------------------------------------------------------------
    // İzleyici için kamera değiştirme metodu
    // -----------------------------------------------------------------------
    private void SwitchSpectatorCamera(int cameraIndex)
    {
        Debug.Log($"[CameraManager] SwitchSpectatorCamera({cameraIndex}) çağrıldı, isSpectator: {isSpectator}");

        if (!isSpectator)
        {
            Debug.LogWarning("[CameraManager] İzleyici olmayan bir oyuncu kamera değiştirmeye çalıştı!");
            LogDebugInfo("İzleyici değilsiniz! Kamera değiştirilemez.");
            return;
        }

        currentSpectatorView = cameraIndex;
        LogDebugInfo($"Kamera değiştiriliyor: {cameraIndex}");

        // Tüm kameraları deaktive et
        attackerVirtualCam.gameObject.SetActive(false);
        defenderVirtualCam.gameObject.SetActive(false);
        spectatorVirtualCam.gameObject.SetActive(false);

        switch (cameraIndex)
        {
            case 1: // Attacker kamerası
                if (attackerTarget != null)
                {
                    attackerVirtualCam.gameObject.SetActive(true);
                    attackerVirtualCam.Follow = attackerTarget;
                    attackerVirtualCam.LookAt = attackerTarget;
                    Debug.Log("[CameraManager] İzleyici Attacker kamerasına geçti - Hedef: " + attackerTarget.name);
                    LogDebugInfo("Attacker kamerasına geçildi");
                }
                else
                {
                    Debug.LogWarning("[CameraManager] Attacker hedefi null!");
                    spectatorVirtualCam.gameObject.SetActive(true);
                    currentSpectatorView = 0;
                    LogDebugInfo("Attacker hedefi bulunamadı! Spectator kamerasına dönüldü.");
                }
                break;

            case 2: // Defender kamerası
                if (defenderTarget != null)
                {
                    defenderVirtualCam.gameObject.SetActive(true);
                    defenderVirtualCam.Follow = defenderTarget;
                    defenderVirtualCam.LookAt = defenderTarget;
                    Debug.Log("[CameraManager] İzleyici Defender kamerasına geçti - Hedef: " + defenderTarget.name);
                    LogDebugInfo("Defender kamerasına geçildi");
                }
                else
                {
                    Debug.LogWarning("[CameraManager] Defender hedefi null!");
                    spectatorVirtualCam.gameObject.SetActive(true);
                    currentSpectatorView = 0;
                    LogDebugInfo("Defender hedefi bulunamadı! Spectator kamerasına dönüldü.");
                }
                break;

            default: // Varsayılan izleyici kamerası
                spectatorVirtualCam.gameObject.SetActive(true);
                Debug.Log("[CameraManager] İzleyici spektator kamerasına geçti");
                LogDebugInfo("Spectator kamerasına geçildi");
                break;
        }
    }

    // -----------------------------------------------------------------------
    // İzleyici için özel kamera kurulumu
    // -----------------------------------------------------------------------
    public void SetupCameraForSpectator()
    {
        Debug.Log("[CameraManager] SetupCameraForSpectator() çağrıldı");
        LogDebugInfo("Spectator modu ayarlanıyor");

        isSpectator = true;

        // Önce tüm kameraları deaktive et
        attackerVirtualCam.gameObject.SetActive(false);
        defenderVirtualCam.gameObject.SetActive(false);
        spectatorVirtualCam.gameObject.SetActive(true); // Spectator kamerasını aktif et

        // MainCamera'nın aktif olduğunu garantile
        mainCamera.gameObject.SetActive(true);

        currentSpectatorView = 0;

        Debug.Log("[CameraManager] İzleyici başlangıçta spektatör kamerasını kullanıyor.");
        Debug.Log("[CameraManager] Kamera kontrolleri: Q = Attacker, E = Defender");
        LogDebugInfo("Spectator modunda başlatıldı. Q = Attacker, E = Defender");
    }

    // -----------------------------------------------------------------------
    // Seçilen role göre kamera ayarlamaları yapılır
    // -----------------------------------------------------------------------
    public void SetupCameraForRole(string role, Transform attackerTransform, Transform defenderTransform)
    {
        Debug.Log($"[CameraManager] SetupCameraForRole({role}) çağrıldı");
        LogDebugInfo($"Rol ayarlanıyor: {role}");

        // Hedef referanslarını kaydet
        if (attackerTransform != null)
        {
            attackerTarget = attackerTransform;
            Debug.Log("[CameraManager] Attacker hedefi atandı: " + attackerTransform.name);
        }
        else
        {
            Debug.LogWarning("[CameraManager] Attacker hedefi null!");
        }

        if (defenderTransform != null)
        {
            defenderTarget = defenderTransform;
            Debug.Log("[CameraManager] Defender hedefi atandı: " + defenderTransform.name);
        }
        else
        {
            Debug.LogWarning("[CameraManager] Defender hedefi null!");
        }

        // Önce tüm kameraları deaktive et
        attackerVirtualCam.gameObject.SetActive(false);
        defenderVirtualCam.gameObject.SetActive(false);
        spectatorVirtualCam.gameObject.SetActive(false);

        // Role göre ilgili kamerayı aktifleştir
        if (role == "attacker")
        {
            isSpectator = false;

            // Attacker kamerasını aktifleştir
            attackerVirtualCam.gameObject.SetActive(true);

            // Follow hedefi olarak player objesi ayarla
            if (attackerTarget != null)
            {
                attackerVirtualCam.Follow = attackerTarget;
                attackerVirtualCam.LookAt = attackerTarget;
                Debug.Log("[CameraManager] Attacker kamerası aktifleştirildi");
                LogDebugInfo("Attacker kamerası aktifleştirildi");
            }
            else
            {
                Debug.LogError("[CameraManager] Attacker hedefi null olduğu için kamera takibi kurulamadı!");
                LogDebugInfo("HATA: Attacker hedefi null");
            }
        }
        else if (role == "defender")
        {
            isSpectator = false;

            // Defender kamerasını aktifleştir
            defenderVirtualCam.gameObject.SetActive(true);

            // Follow hedefi olarak enemy objesi ayarla
            if (defenderTarget != null)
            {
                defenderVirtualCam.Follow = defenderTarget;
                defenderVirtualCam.LookAt = defenderTarget;
                Debug.Log("[CameraManager] Defender kamerası aktifleştirildi");
                LogDebugInfo("Defender kamerası aktifleştirildi");
            }
            else
            {
                Debug.LogError("[CameraManager] Defender hedefi null olduğu için kamera takibi kurulamadı!");
                LogDebugInfo("HATA: Defender hedefi null");
            }
        }
        else
        {
            // Spectator kamerasını aktifleştir (oyuncu ne attacker ne de defender)
            isSpectator = true;
            spectatorVirtualCam.gameObject.SetActive(true);
            currentSpectatorView = 0;

            Debug.Log("[CameraManager] Spectator kamerası aktifleştirildi (rol: " + role + ")");
            Debug.Log("[CameraManager] Kamera kontrolleri: Q = Attacker, E = Defender");
            LogDebugInfo("Spectator kamerası aktifleştirildi");
        }

        // MainCamera'nın aktif olduğunu garantile
        mainCamera.gameObject.SetActive(true);
    }

    // -----------------------------------------------------------------------
    // Karakter yeniden doğduğunda kamera takip hedeflerini günceller
    // -----------------------------------------------------------------------
    public void UpdateCameraFollowTarget(string role, Transform target)
    {
        Debug.Log($"[CameraManager] UpdateCameraFollowTarget({role}) çağrıldı");

        if (role == "attacker")
        {
            if (target != null)
            {
                attackerTarget = target;
                Debug.Log("[CameraManager] Attacker hedefi güncellendi: " + target.name);
                LogDebugInfo("Attacker hedefi güncellendi: " + target.name);

                // Attacker kamera güncelleme
                if (attackerVirtualCam.gameObject.activeSelf)
                {
                    attackerVirtualCam.Follow = attackerTarget;
                    attackerVirtualCam.LookAt = attackerTarget;
                    Debug.Log("[CameraManager] Attacker kamera takibi güncellendi");
                }

                // İzleyici modunda ve attacker kamerası kullanılıyorsa takibi güncelle
                if (isSpectator && currentSpectatorView == 1)
                {
                    attackerVirtualCam.Follow = attackerTarget;
                    attackerVirtualCam.LookAt = attackerTarget;
                    Debug.Log("[CameraManager] İzleyici attacker kamera takibi güncellendi");
                }
            }
            else
            {
                Debug.LogError("[CameraManager] UpdateCameraFollowTarget: Attacker hedefi null!");
                LogDebugInfo("HATA: Attacker hedefi null");
            }
        }
        else if (role == "defender")
        {
            if (target != null)
            {
                defenderTarget = target;
                Debug.Log("[CameraManager] Defender hedefi güncellendi: " + target.name);
                LogDebugInfo("Defender hedefi güncellendi: " + target.name);

                // Defender kamera güncelleme
                if (defenderVirtualCam.gameObject.activeSelf)
                {
                    defenderVirtualCam.Follow = defenderTarget;
                    defenderVirtualCam.LookAt = defenderTarget;
                    Debug.Log("[CameraManager] Defender kamera takibi güncellendi");
                }

                // İzleyici modunda ve defender kamerası kullanılıyorsa takibi güncelle
                if (isSpectator && currentSpectatorView == 2)
                {
                    defenderVirtualCam.Follow = defenderTarget;
                    defenderVirtualCam.LookAt = defenderTarget;
                    Debug.Log("[CameraManager] İzleyici defender kamera takibi güncellendi");
                }
            }
            else
            {
                Debug.LogError("[CameraManager] UpdateCameraFollowTarget: Defender hedefi null!");
                LogDebugInfo("HATA: Defender hedefi null");
            }
        }
    }

    // -----------------------------------------------------------------------
    // Mevcut kamera durumunu kontrol etmek için yardımcı metotlar
    // -----------------------------------------------------------------------
    public bool IsSpectator()
    {
        return isSpectator;
    }

    public int GetCurrentSpectatorView()
    {
        return currentSpectatorView;
    }

    // Tüm durum bilgilerini konsola logla
    private void LogAllStatus()
    {
        Debug.Log("------ KAMERA DURUM RAPORU ------");
        Debug.Log($"isSpectator: {isSpectator}");
        Debug.Log($"currentSpectatorView: {currentSpectatorView}");
        Debug.Log($"attackerTarget: {(attackerTarget != null ? attackerTarget.name : "null")}");
        Debug.Log($"defenderTarget: {(defenderTarget != null ? defenderTarget.name : "null")}");
        Debug.Log($"attackerCam aktif: {attackerVirtualCam.gameObject.activeSelf}");
        Debug.Log($"defenderCam aktif: {defenderVirtualCam.gameObject.activeSelf}");
        Debug.Log($"spectatorCam aktif: {spectatorVirtualCam.gameObject.activeSelf}");
        Debug.Log($"mainCamera aktif: {mainCamera.gameObject.activeSelf}");
        Debug.Log("--------------------------------");

        LogDebugInfo("Durum raporu konsola yazıldı. (Space)");
    }

    // -----------------------------------------------------------------------
    // BattleScenePlayerSpawner ile iletişim test fonksiyonu
    // -----------------------------------------------------------------------
    public void TestSpectatorSetup()
    {
        isSpectator = true;
        Debug.Log("[CameraManager] TEST: isSpectator true olarak ayarlandı!");
        LogDebugInfo("TEST: İzleyici modu etkinleştirildi");
    }
}