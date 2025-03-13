using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Photon
using Photon.Pun;
using Photon.Realtime;

public class Abilities : MonoBehaviourPun
{
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

    // Ability2'yi iptal etmek için Coroutine referansı
    private Coroutine ability2TimeoutCoroutine;

    void Start()
    {
        manaSystem = GetComponent<ManaSystem>();

        // Başlangıçta UI göstergeleri sıfır veya boş
        abilityImage1.fillAmount = 0;
        abilityImage2.fillAmount = 0;
        abilityText1.text = "";
        abilityText2.text = "";

        ability1Skillshot.enabled = false;
        ability2RangeIndicator.enabled = false;

        ability1Canvas.enabled = false;
        ability2Canvas.enabled = false;
    }

    void Update()
    {
        // Photon: Sadece local player (IsMine) bu scripti tam anlamıyla kontrol etsin
        if (!photonView.IsMine)
        {
            return; 
        }

        // Ekranda farenin olduğu yeri ray'e çevir
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Yetenek girişi (Input) ve kullanımlarını dinle
        Ability1Input();
        Ability2Input();

        // Cooldown güncellemelerini yap
        AbilityCooldown(ability1Cooldown, abilityManaCost, 
                        ref currentAbility1Cooldown, ref isAbility1Cooldown, 
                        abilityImage1, abilityText1);

        AbilityCooldown(ability2Cooldown, ability2ManaCost, 
                        ref currentAbility2Cooldown, ref isAbility2Cooldown, 
                        abilityImage2, abilityText2);

        // Canvas’ların (skillshot gösterge vs.) konumunu-görünüşünü güncelle
        Ability1Canvas();
        Ability2Canvas();
    }

    #region ABILITY 1
    private void Ability1Canvas()
    {
        // Ability1 skillshot gösterimi açık ise, mouse'u takip ettiriyoruz
        if (ability1Skillshot.enabled)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                position = hit.point;
            }

            Quaternion ab1CanvasRot = Quaternion.LookRotation(position - transform.position);
            ab1CanvasRot.eulerAngles = new Vector3(0, ab1CanvasRot.eulerAngles.y, ab1CanvasRot.eulerAngles.z);
            ability1Canvas.transform.rotation = ab1CanvasRot;
        }
    }

    private void Ability1Input()
    {
        // Ability1 tuşuna basıldıysa ve cooldown yoksa
        if (Input.GetKeyDown(ability1Key) && !isAbility1Cooldown)
        {
            // Skillshot çizgilerini göster
            ability1Canvas.enabled = true;
            ability1Skillshot.enabled = true;

            // Diğer ability canvas'ı kapat
            ability2Canvas.enabled = false;
            ability2RangeIndicator.enabled = false;

            // Mouse görünür olsun
            Cursor.visible = true;
        }

        // Skillshot modu açıkken sol tık yapınca yeteneği "onayla"
        if (ability1Skillshot.enabled && Input.GetMouseButtonDown(0))
        {
            // Cooldown başlat (yerel)
            isAbility1Cooldown = true;
            currentAbility1Cooldown = ability1Cooldown;

            // Mana düşürmek istersen bu noktada yapabilirsin
            // manaSystem.UseMana(abilityManaCost);

            // Bu yetenekle ilgili hasar veya görsel efekti
            // Tüm oyuncular görebilsin diye, RPC ile paylaşabilirsiniz.
            // photonView.RPC("RPC_Ability1Effect", RpcTarget.All, position);

            // Geçici olarak skill UI'ını kapatalım
            ability1Canvas.enabled = false;
            ability1Skillshot.enabled = false;
        }
    }

    /*
    // Örneğin Ability1'de hasar veya efekt oluşturmak için bir RPC
    [PunRPC]
    private void RPC_Ability1Effect(Vector3 castPosition)
    {
        // Burada Projectile veya görsel efekt instantiate edebilir,
        // Yere AoE hasarı verebilirsin vb.
    }
    */
    #endregion

    #region ABILITY 2
    private void Ability2Canvas()
    {
        // Menzil göstergesini ayarlamak için
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
        // Ability2 tuşuna basıldıysa, cooldown yoksa ve mana yeterliyse
        if (Input.GetKeyDown(ability2Key) && !isAbility2Cooldown && manaSystem.CanAffordAbility(ability2ManaCost))
        {
            ability2Canvas.enabled = true;
            ability2RangeIndicator.enabled = true;
            Cursor.visible = true;

            // Eski coroutine varsa iptal et
            if (ability2TimeoutCoroutine != null)
            {
                StopCoroutine(ability2TimeoutCoroutine);
            }

            // 7 sn içinde hamle gelmezse, iptal et
            ability2TimeoutCoroutine = StartCoroutine(Ability2Timeout());
        }

        // Canvas açıkken sol tık -> Yeteneği kullan
        if (ability2Canvas.enabled && Input.GetMouseButtonDown(0))
        {
            // Local cooldown başlat
            isAbility2Cooldown = true;
            currentAbility2Cooldown = ability2Cooldown;

            // Mana düşürülebilir
            // manaSystem.UseMana(ability2ManaCost);

            // **Hasar veya etkiyi herkesin görmesi için bir RPC yolluyoruz**
            photonView.RPC("RPC_Ability2Damage", RpcTarget.All, position);

            // Canvas'ı kapat
            CloseAbility2();
        }
    }

    // 7 saniye bekler, hâlâ kullanılmamışsa iptal
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

    // Bu RPC, tüm istemcilerde çalışır
    [PunRPC]
    private void RPC_Ability2Damage(Vector3 abilityCenter)
    {
        float abilityRadius = maxAbility2Distance / 2;
        Collider[] hitColliders = Physics.OverlapSphere(abilityCenter, abilityRadius);

        foreach (var hitCollider in hitColliders)
        {
            ObjectiveStats enemyStats = hitCollider.GetComponent<ObjectiveStats>();
            if (enemyStats != null)
            {
                // Hasarı tüm istemcilerde uygular
                enemyStats.TakeDamage(65);
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
            // Cooldown yokken mana durumunu kontrol ediyoruz
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
                // Mana yoksa renk farklı olabilir
                if (skillImage != null)
                {
                    skillImage.color = Color.blue;
                }
            }
        }
    }
    #endregion
}
