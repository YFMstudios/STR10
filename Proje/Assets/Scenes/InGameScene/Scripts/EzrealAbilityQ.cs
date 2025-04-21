using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(ManaSystem))]
[RequireComponent(typeof(PhotonView))]
public class EzrealAbilityQ : MonoBehaviourPun
{
    /*────────── AYARLAR ──────────*/
    [Header("Ability Values")]
    public KeyCode abilityKey = KeyCode.Q;
    public float   cooldown   = 5f;
    public float   manaCost   = 30f;

    [Header("Projectile")]
    public Transform  spawnPoint;        // Her prefab için farklı referans
    public GameObject skillshotPrefab;   // Her prefab için farklı mermi

    [Header("UI Elements 2D")]
    public Image abilityImageMain;
    public Image abilityImageGreyed;
    public Text  abilityText;

    [Header("UI Elements 3D")]
    public Canvas abilityCanvas;
    public Image  skillshotIndicator;

    /*────────── DAHİLİ ──────────*/
    private ManaSystem manaSystem;
    private Movement   movement;
    private Animator   anim;
    private Camera     mainCamera;

    private bool  isCooldown;
    private float currentCooldown;
    private Vector3 aimPosition;

    /*────────────────────────────*/
    private void Awake()
    {
        CacheComponents();
        InitializeUI();
    }

    private void Update()
    {
        /*–– Sadece KENDİ karakterin input alır ––*/
        if (!photonView.IsMine) return;

        HandleInput();
        UpdateCooldown();
        UpdateUI();

        if (skillshotIndicator != null && skillshotIndicator.enabled)
            UpdateSkillshotIndicator();
    }

    /*────────── INPUT & CAST ──────────*/
    private void HandleInput()
    {
        if (Input.GetKeyDown(abilityKey) && ReadyToCast())
            EnableAimingMode(true);

        if (skillshotIndicator != null && skillshotIndicator.enabled && Input.GetMouseButtonDown(0))
            FireSkillshot();
    }

    private bool ReadyToCast() =>
        !isCooldown && manaSystem.CanAffordAbility(manaCost);

    private void FireSkillshot()
    {
        if (!manaSystem.CanAffordAbility(manaCost)) return;

        movement.StopMovement();
        manaSystem.UseAbility(manaCost);
        StartCooldown();
        RotateCharacter();

        EnableAimingMode(false);
        Cursor.visible = true;

        if (anim != null) anim.SetTrigger("Ezreal Q");
    }

    /// <summary>Animasyon event’i “vur” anında bu metodu çağırmalı.</summary>
    public void AimAndFireProjectile()
    {
        if (skillshotPrefab == null || spawnPoint == null) return;

        PhotonNetwork.Instantiate(skillshotPrefab.name,
                                  spawnPoint.position,
                                  spawnPoint.rotation);

        movement.ResumeMovement();
    }

    /*────────── COOLDOWN ──────────*/
    private void StartCooldown()
    {
        isCooldown      = true;
        currentCooldown = cooldown;
    }
    private void UpdateCooldown()
    {
        if (!isCooldown) return;

        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            currentCooldown = 0f;
            isCooldown      = false;
        }
    }

    /*────────── UI / AIMING ───────*/
    private void UpdateUI()
    {
        if (abilityImageGreyed != null)
        {
            abilityImageGreyed.color      = isCooldown ? Color.grey : Color.white;
            abilityImageGreyed.fillAmount = isCooldown ? currentCooldown / cooldown : 0;
        }
        if (abilityImageMain != null)
            abilityImageMain.color = manaSystem.CanAffordAbility(manaCost) ? Color.white : Color.red;

        if (abilityText != null)
            abilityText.text = isCooldown ? Mathf.Ceil(currentCooldown).ToString() : "";
    }

    private void UpdateSkillshotIndicator()
    {
        if (mainCamera == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            aimPosition = hit.point;
            Quaternion r = Quaternion.LookRotation((aimPosition - transform.position).normalized);
            abilityCanvas.transform.rotation = Quaternion.Euler(0, r.eulerAngles.y, r.eulerAngles.z);
        }
    }

    private void EnableAimingMode(bool state)
    {
        if (abilityCanvas      != null) abilityCanvas.enabled      = state;
        if (skillshotIndicator != null) skillshotIndicator.enabled = state;
        Cursor.visible = !state;
    }

    private void RotateCharacter()
    {
        Vector3 dir = aimPosition - transform.position;
        dir.y = 0f;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    /*────────── YARDIMCI ──────────*/
    private void CacheComponents()
    {
        manaSystem  = GetComponent<ManaSystem>();
        movement    = GetComponent<Movement>();
        anim        = GetComponent<Animator>();
        mainCamera  = Camera.main;

        if (abilityCanvas      == null) abilityCanvas      = GetComponentInChildren<Canvas>();
        if (skillshotIndicator == null) skillshotIndicator = abilityCanvas?.GetComponentInChildren<Image>();
    }

    private void InitializeUI()
    {
        if (abilityCanvas      != null) abilityCanvas.enabled      = false;
        if (skillshotIndicator != null) skillshotIndicator.enabled = false;
        if (abilityImageGreyed != null) abilityImageGreyed.color   = Color.white;
        if (abilityText        != null) abilityText.text           = "";
    }
}
