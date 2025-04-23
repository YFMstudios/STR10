using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Abilities : MonoBehaviourPun
{

    [Header("Ability 1 Projectile")]
public Transform ability1SpawnPoint;          // Q_SpawnPoint
public GameObject ability1ProjectilePrefab;   // Ezreal_Q_Projectile
private Animator anim;                        // Animator referansı

    public enum CharacterType { Player, Enemy }
    public CharacterType characterType; // **SADECE INSPECTOR'DAN AYARLANACAK**

    [Header("Ability 1")]
    public Image abilityImage1;
    public Text abilityText1;
    public KeyCode ability1Key;
    public float ability1Cooldown = 5;
    public float abilityManaCost = 30;
    public Canvas ability1Canvas;
    public Image ability1Skillshot;

    [Header("Ability 2")]
    public Image abilityImage2;
    public Text abilityText2;
    public KeyCode ability2Key;
    public float ability2Cooldown = 7;
    public float ability2ManaCost = 30;
    public Canvas ability2Canvas;
    public Image ability2RangeIndicator;
    public float maxAbility2Distance = 7;

    private bool isAbility1Cooldown = false;
    private bool isAbility2Cooldown = false;

    private float currentAbility1Cooldown;
    private float currentAbility2Cooldown;

    private Vector3 position;
    private RaycastHit hit;
    private Ray ray;

    public ManaSystem manaSystem;
    private Coroutine ability2TimeoutCoroutine;

    void Start()
    {

        anim = GetComponent<Animator>();

        manaSystem = GetComponent<ManaSystem>();

        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
        abilityText1.text = "abilityText1";
        abilityText2.text = "abilityText2";

        // Skillshot Image bulma
        if (ability1Skillshot == null)
        {
            ability1Skillshot = GetComponentInChildren<Canvas>(true)
                ?.transform.Find("Skillshot Image")?.GetComponent<Image>();
            if (ability1Skillshot != null)
                ability1Skillshot.enabled = false;
            else
                Debug.LogError("Skillshot Image bulunamadı!");
        }
        else
        {
            ability1Skillshot.enabled = false;
        }

        // Ability2RangeIndicator bulma
        if (ability2RangeIndicator == null)
        {
            ability2RangeIndicator = GetComponentInChildren<Canvas>(true)
                ?.transform.Find("Ability2RangeIndicator")?.GetComponent<Image>();
            if (ability2RangeIndicator != null)
                ability2RangeIndicator.enabled = false;
            else
                Debug.LogError("Ability2RangeIndicator bulunamadı!");
        }
        else
        {
            ability2RangeIndicator.enabled = false;
        }

        // Ability1Canvas bulma
        if (ability1Canvas == null)
        {
            ability1Canvas = GetComponentInChildren<Canvas>(true)
                ?.transform.Find("Ability1Canvas")?.GetComponent<Canvas>();
            if (ability1Canvas != null)
                ability1Canvas.enabled = false;
            else
                Debug.LogError("Ability1Canvas bulunamadı!");
        }
        else
        {
            ability1Canvas.enabled = false;
        }

        // Ability2Canvas bulma
        if (ability2Canvas == null)
        {
            ability2Canvas = GetComponentInChildren<Canvas>(true)
                ?.transform.Find("Ability2Canvas")?.GetComponent<Canvas>();
            if (ability2Canvas != null)
                ability2Canvas.enabled = false;
            else
                Debug.LogError("Ability2Canvas bulunamadı!");
        }
        else
        {
            ability2Canvas.enabled = false;
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Input ve Canvas işlemleri
        Ability1Input();
        Ability2Input();

        // Cooldown işlemleri
        AbilityCooldown(ability1Cooldown, abilityManaCost,
                        ref currentAbility1Cooldown, ref isAbility1Cooldown,
                        abilityImage1, abilityText1);

        AbilityCooldown(ability2Cooldown, ability2ManaCost,
                        ref currentAbility2Cooldown, ref isAbility2Cooldown,
                        abilityImage2, abilityText2);

        // Canvasları takip ettir
        Ability1Canvas();
        Ability2Canvas();
    }

    #region ABILITY 1
    private void Ability1Canvas()
    {
        if (ability1Skillshot.enabled)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                position = hit.point;
            }

            Quaternion ab1CanvasRot = Quaternion.LookRotation(position - transform.position);
            ab1CanvasRot.eulerAngles = new Vector3(0, ab1CanvasRot.eulerAngles.y, 0);
            ability1Canvas.transform.rotation = ab1CanvasRot;
        }
    }

    private void Ability1Input()
    {
        // Yeterli mana + cooldown'da değilsek, skillshot modunu aç
        if (Input.GetKeyDown(ability1Key) 
            && !isAbility1Cooldown 
            && manaSystem.CanAffordAbility(abilityManaCost))
        {
            ability1Canvas.enabled = true;
            ability1Skillshot.enabled = true;

            // Diğer yetenek penceresi kapansın
            ability2Canvas.enabled = false;
            ability2RangeIndicator.enabled = false;

            Cursor.visible = true;
        }

        // Skillshot aktifken sol tık -> Yetenek kullan
        if (ability1Skillshot.enabled && Input.GetMouseButtonDown(0))
{
    // 1) Cooldown & mana
    isAbility1Cooldown = true;
    currentAbility1Cooldown = ability1Cooldown;
    manaSystem.UseAbility(abilityManaCost);

    // 2) Karakteri fare yönüne çevir
    Vector3 dir = (position - transform.position).normalized;
    dir.y = 0;
    if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);

    // 3) ANİMASYONU TETİKLE
    if (anim != null) anim.SetTrigger("Ezreal Q Projectile");   // Animator’da aynı adda Trigger olmalı

    // 4) PROJECTILE OLUŞTUR
    if (ability1ProjectilePrefab != null && ability1SpawnPoint != null)
        PhotonNetwork.Instantiate(ability1ProjectilePrefab.name,
                                  ability1SpawnPoint.position,
                                  ability1SpawnPoint.rotation);

    // 5) Canvas kapat
    ability1Canvas.enabled = false;
    ability1Skillshot.enabled = false;
}

    }

    /*
    // Örnek RPC
    [PunRPC]
    private void RPC_Ability1Effect(Vector3 castPosition)
    {
        // Projectile veya AoE hasar
    }
    */
    #endregion

    #region ABILITY 2
    private void Ability2Canvas()
    {
        int layerMask = ~LayerMask.GetMask("Player");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                position = hit.point;
            }
        }

        var hitPosDir = (hit.point - transform.position).normalized;
        float distance = Vector3.Distance(hit.point, transform.position);
        distance = Mathf.Min(distance, maxAbility2Distance);

        var newPos = transform.position + hitPosDir * distance;
        ability2Canvas.transform.position = newPos;
    }

    private void Ability2Input()
    {
        if (Input.GetKeyDown(ability2Key)
            && !isAbility2Cooldown
            && manaSystem.CanAffordAbility(ability2ManaCost))
        {
            ability2Canvas.enabled = true;
            ability2RangeIndicator.enabled = true;
            Cursor.visible = true;

            // Eski coroutine varsa iptal
            if (ability2TimeoutCoroutine != null)
            {
                StopCoroutine(ability2TimeoutCoroutine);
            }

            // 7 sn içinde hamle gelmezse iptal
            ability2TimeoutCoroutine = StartCoroutine(Ability2Timeout());
        }

        // Canvas açıkken sol tık -> Yeteneği kullan
        if (ability2Canvas.enabled && Input.GetMouseButtonDown(0))
        {
            isAbility2Cooldown = true;
            currentAbility2Cooldown = ability2Cooldown;

            // Mana düş
            manaSystem.UseAbility(ability2ManaCost);

            // Hasarı veya etkiyi herkese yolla
            photonView.RPC("RPC_Ability2Damage", RpcTarget.All, position);

            // Canvas kapat
            CloseAbility2();
        }
    }

    private IEnumerator Ability2Timeout()
    {
        yield return new WaitForSeconds(7f);

        if (ability2Canvas.enabled)
        {
            CloseAbility2();
        }
    }

    private void CloseAbility2()
    {
        ability2Canvas.enabled = false;
        ability2RangeIndicator.enabled = false;
        ability2TimeoutCoroutine = null;
    }

    // Tüm istemcilerde çalışır
   [PunRPC]
private void RPC_Ability2Damage(Vector3 abilityCenter)
{
    float abilityRadius = maxAbility2Distance / 2;
    // Tüm layer’ları taramak isterseniz mask’i ~0 yapabilirsiniz, 
    // yoksa karakter katmanınızı da eklerseniz performans artar.
    Collider[] hitColliders = Physics.OverlapSphere(abilityCenter, abilityRadius);

    foreach (var col in hitColliders)
    {
        // 1) Öncelikle minyon, tuzak vb. için ObjectiveStats
        var objStats = col.GetComponentInParent<ObjectiveStats>();
        if (objStats != null)
        {
            objStats.TakeDamage(65);
            continue;
        }

        // 2) Karakterler için Stats (Player/Enemy)
        var charStats = col.GetComponentInParent<Stats>();
        if (charStats != null)
        {
            // Stats.TakeDamage içinde photonView.IsMine kontrolü var,
            // bu yüzden her şey doğru şekilde senkronize olur.
            charStats.TakeDamage(65f);
        }
    }
}

    #endregion

    #region COOLDOWN
    private void AbilityCooldown(float abilityCooldown, float abilityManaCost,
                                 ref float currentCooldown, ref bool isCooldown,
                                 Image skillImage, Text skillText)
    {
        if (isCooldown)
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0f)
            {
                isCooldown = false;
                currentCooldown = 0;
            }

            if (skillImage != null)
            {
                skillImage.color = Color.grey;
                skillImage.fillAmount = 1;
            }

            if (skillText != null)
            {
                skillText.text = Mathf.Ceil(currentCooldown).ToString();
            }
        }
        else
        {
            // Cooldown yokken mana durumunu kontrol
            if (manaSystem.CanAffordAbility(abilityManaCost))
            {
                // Yeterli mana varsa
                if (skillImage != null)
                {
                    skillImage.color = Color.grey;
                    skillImage.fillAmount = 0;
                }
                if (skillText != null)
                {
                    skillText.text = "";
                }
            }
            else
            {
                // Mana yoksa renk başka
                if (skillImage != null)
                {
                    skillImage.color = Color.blue;
                }
            }
        }
    }
    #endregion
}
