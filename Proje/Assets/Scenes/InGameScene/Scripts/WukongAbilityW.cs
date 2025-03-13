using System.Collections;
using UnityEngine;
using UnityEngine.UI;
// Photon eklemeleri
using Photon.Pun;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(ManaSystem))]
[RequireComponent(typeof(PhotonView))]
public class WukongAbilityW : MonoBehaviourPun
{
    [Header("Ability Values")]
    public KeyCode abilityKey = KeyCode.W;  // Yetenek tuşu
    public float cooldown = 18f;           // Cooldown süresi
    public float manaCost = 40f;          // Mana maliyeti
    public float invisibilityDuration = 1.5f; // Görünmezlik süresi

    public GameObject clonePrefab;        // Klon prefab'ı

    [Header("Material")]
    public Material originalMaterial;     // Karakterin orijinal malzemesi
    public Material transparentMaterial;  // Görünmezlik malzemesi

    [Header("UI Elements")]
    public Image abilityImageMain;
    public Image abilityImageGreyed;
    public Text abilityText;
    public Image screenDarkeningOverlay;

    private ManaSystem manaSystem;
    private bool isCooldown = false;
    private float currentCooldown;
    private Renderer[] renderers;     // Çocuğundaki rendererları toplayacağız

    void Awake()
    {
        manaSystem = GetComponent<ManaSystem>();
        CacheRenderers();
        InitializeUI();
    }

    void Update()
    {
        // Sadece local (sahibi olduğumuz) karakterin input'unu dinleriz
        if (!photonView.IsMine) 
        {
            return;
        }

        HandleInput();
        UpdateCooldown();
        UpdateUI();
    }

    // Çocuk objelerdeki Renderer'ları toplayarak malzeme değiştirme işini kolaylaştırır
    private void CacheRenderers()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void HandleInput()
    {
        // Yetenek tuşuna basıldıysa, cooldown yoksa ve mana yetiyorsa
        if (Input.GetKeyDown(abilityKey) && !isCooldown && manaSystem.CanAffordAbility(manaCost))
        {
            ActivateAbility();
        }
    }

    // Yetenek aktif olduğunda, mana düşürme + cooldown + RPC ile görünmezlik + klon oluşturma
    private void ActivateAbility()
    {
        // 1) Mana harca
        manaSystem.UseAbility(manaCost);

        // 2) Cooldown başlat (sadece localde)
        StartCooldown();

        // 3) Klonu ağ üzerinde herkesin görmesi için Instantiate ediyoruz
        if (clonePrefab)
        {
            // PhotonNetwork.Instantiate: Tüm istemcilerde aynı klon oluşturulur
            PhotonNetwork.Instantiate(clonePrefab.name, transform.position, transform.rotation);
        }

        // 4) Karakteri ileri doğru hafif itme (Dash)
        //    Eğer Movement script’iniz transform'u PhotonTransformView ile senkronluyorsa,
        //    sadece localde konumu değiştirmeniz yeterli. Diğer istemciler bu değişikliği görecektir.
        PushPlayerForward();

        // 5) Herkeste görünmezlik efektini tetiklemek istiyorsak, RPC ile malzeme değişimini duyuruyoruz
        photonView.RPC("RPC_BecomeInvisible", RpcTarget.All, invisibilityDuration);

        // (Eğer "ekran karartması" efektini sadece local oyuncu görecekse, burada açabilirsiniz)
        if (screenDarkeningOverlay != null)
        {
            screenDarkeningOverlay.enabled = true; 
        }
    }

    private void PushPlayerForward()
    {
        float pushDistance = 1.0f;
        // Sadece local karakterin konumunu değiştiriyoruz
        transform.position += transform.forward * pushDistance;
    }

    // RPC methodu: Tüm istemcilerde bu obje üzerinde çalışacak; malzemeyi görünmez yapacak, sonra geri alacak
    [PunRPC]
    private void RPC_BecomeInvisible(float duration)
    {
        // Bu karakterin render materyalini görünmez yap
        SetMaterials(transparentMaterial);

        // Her istemcide coroutine başlatıp süre sonunda eski haline dönüyoruz
        StartCoroutine(RevertInvisibility(duration));
    }

    private IEnumerator RevertInvisibility(float duration)
    {
        // Belirli süre bekliyoruz
        yield return new WaitForSeconds(duration);

        // Materyali orijinal haline çevir
        SetMaterials(originalMaterial);

        // Sadece local oyuncu da ekran karartması kapanır
        if (photonView.IsMine && screenDarkeningOverlay != null)
        {
            screenDarkeningOverlay.enabled = false;
        }
    }

    // Tüm Renderer'ların materyalini değiştirir
    private void SetMaterials(Material material)
    {
        foreach (var rend in renderers)
        {
            rend.material = material;
        }
    }

    // Cooldown başlatmak
    private void StartCooldown()
    {
        isCooldown = true;
        currentCooldown = cooldown;
    }

    // Cooldown sayacını düşürmek
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

    // UI Güncellemesi (sadece local player için)
    private void UpdateUI()
    {
        // 2D UI veya benzeri unsurlar genellikle sadece localde gösterilir
        if (!photonView.IsMine)
        {
            return;
        }

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

    // UI başlangıç durumunu ayarlar
    private void InitializeUI()
    {
        if (abilityImageGreyed != null)
        {
            abilityImageGreyed.color = Color.white;
        }
        if (abilityText != null)
        {
            abilityText.text = "";
        }
        if (screenDarkeningOverlay != null)
        {
            screenDarkeningOverlay.enabled = false;
        }
    }
}
