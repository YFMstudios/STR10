using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(ManaSystem))]
[RequireComponent(typeof(PhotonView))]
public class EzrealAbilityQ : MonoBehaviourPun
{
    [Header("Ability Values")]
    public KeyCode abilityKey = KeyCode.Q;  // Yetenek tuşu
    public float cooldown = 5f;            // Cooldown süresi
    public float manaCost = 30f;          // Mana maliyeti

    [Header("Projectile")]
    public Transform spawnPoint;          // Merminin çıkacağı nokta
    public GameObject skillshotPrefab;    // Mermi prefab (Resources içinde PhotonView'lı)

    [Header("UI Elements 2D")]
    public Image abilityImageMain;
    public Image abilityImageGreyed;
    public Text abilityText;

    [Header("UI Elements 3D")]
    public Canvas abilityCanvas;
    public Image skillshotIndicator;

    private ManaSystem manaSystem;
    private Movement movement;
    private bool isCooldown = false;
    private float currentCooldown;
    private Vector3 aimPosition;

    private Camera mainCamera;
    private Animator anim;

    void Awake()
    {
        CacheComponents();
        InitializeUI();
    }

    void Update()
    {
        // Sadece local oyuncu (IsMine) input alır ve UI kontrol eder
        if (!photonView.IsMine) 
        {
            return; 
        }

        HandleInput();
        UpdateCooldown();
        UpdateUI();

        if (skillshotIndicator != null && skillshotIndicator.enabled)
        {
            UpdateSkillshotIndicator();
        }
    }

    // Sık kullandığımız bileşenleri önceden referans alıyoruz
    private void CacheComponents()
    {
        manaSystem = GetComponent<ManaSystem>();
        movement = GetComponent<Movement>();
        mainCamera = Camera.main;

        if (abilityCanvas == null)
            abilityCanvas = GetComponentInChildren<Canvas>();

        if (skillshotIndicator == null)
            skillshotIndicator = GetComponentInChildren<Image>();

        anim = GetComponent<Animator>();
    }

    // Kullanıcı inputlarını dinleme
    private void HandleInput()
    {
        // Yetenek tuşuna basıldı ve cooldown yok ve yeterli mana varsa
        if (Input.GetKeyDown(abilityKey) && !isCooldown && manaSystem.CanAffordAbility(manaCost))
        {
            EnableAimingMode(true);
        }

        // Skillshot nişangahı açıkken sol tık -> Ateşleme
        if (skillshotIndicator != null && skillshotIndicator.enabled && Input.GetMouseButtonDown(0))
        {
            FireSkillshot();
        }
    }

    // Yetenek ateşlendiğinde
    private void FireSkillshot()
    {
        if (!manaSystem.CanAffordAbility(manaCost))
            return;

        // Hareketi durdur
        movement.StopMovement();

        // Mana düşür
        manaSystem.UseAbility(manaCost);

        // Cooldown başlat
        StartCooldown();

        // Karakteri bakış yönüne döndür
        RotateCharacter();

        // Hedef alma modunu kapat
        EnableAimingMode(false);
        Cursor.visible = true;

        // Animasyon tetikle
        if (anim != null)
        {
            anim.SetTrigger("Ezreal Q");
        }
    }

    /// <summary>
    /// Animasyonda "Attack" anına event ekleyerek bu metodu çağırabilirsiniz.
    /// Mermiyi ağ üzerinde üretir (PhotonNetwork.Instantiate).
    /// </summary>
    public void AimAndFireProjectile()
    {
        if (skillshotPrefab == null || spawnPoint == null) return;

        // Mermiyi herkesin sahnesinde oluşturur
        PhotonNetwork.Instantiate(skillshotPrefab.name, spawnPoint.position, spawnPoint.rotation);

        // Karakter tekrar hareket edebilir
        movement.ResumeMovement();
    }

    // Karakteri farenin gösterdiği yöne çevir
    private void RotateCharacter()
    {
        Vector3 direction = CalculateAimDirection();
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    // Fare raycast'i ile hedef noktayı bul, y'yi sıfırla
    private Vector3 CalculateAimDirection()
    {
        Vector3 direction = (aimPosition - transform.position).normalized;
        direction.y = 0;
        return direction;
    }

    // Cooldown sürecini başlatır
    private void StartCooldown()
    {
        isCooldown = true;
        currentCooldown = cooldown;
    }

    private void UpdateCooldown()
    {
        if (!isCooldown) return;

        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            currentCooldown = 0f;
            isCooldown = false;
        }
    }

    // UI güncelleme (sadece local)
    private void UpdateUI()
    {
        if (abilityImageGreyed != null)
        {
            abilityImageGreyed.color = isCooldown ? Color.grey : Color.white;
            abilityImageGreyed.fillAmount = isCooldown ? currentCooldown / cooldown : 0;
        }

        if (abilityImageMain != null)
        {
            abilityImageMain.color = manaSystem.CanAffordAbility(manaCost) ? Color.white : Color.red;
        }

        if (abilityText != null)
        {
            abilityText.text = isCooldown ? Mathf.Ceil(currentCooldown).ToString() : "";
        }
    }

    // Mermi nişangahı (3D Canvas üzerindeki image) pozisyon & rotasyon güncelle
    private void UpdateSkillshotIndicator()
    {
        if (mainCamera == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            aimPosition = hit.point;
            UpdateIndicatorRotation();
        }
    }

    // Nişangahı aimPosition yönüne döndür
    private void UpdateIndicatorRotation()
    {
        if (abilityCanvas == null) return;

        Quaternion targetRotation = Quaternion.LookRotation(aimPosition - transform.position);
        targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
        abilityCanvas.transform.rotation = targetRotation;
    }

    // Nişangah modunu aç/kapat
    private void EnableAimingMode(bool isEnabled)
    {
        if (abilityCanvas != null)
            abilityCanvas.enabled = isEnabled;

        if (skillshotIndicator != null)
            skillshotIndicator.enabled = isEnabled;

        Cursor.visible = !isEnabled;
    }

    // UI başlangıç ayarları
    private void InitializeUI()
    {
        if (abilityCanvas != null) 
            abilityCanvas.enabled = false;

        if (skillshotIndicator != null)
            skillshotIndicator.enabled = false;

        if (abilityImageGreyed != null)
            abilityImageGreyed.color = Color.white;

        if (abilityText != null)
            abilityText.text = "";
    }
}
