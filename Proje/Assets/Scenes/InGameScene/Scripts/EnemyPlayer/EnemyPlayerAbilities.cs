using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPlayerAbilities : MonoBehaviour
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

    public EnemyPlayerManaSystem manaSystem;

    void Start()
    {
        manaSystem = GetComponent<EnemyPlayerManaSystem>();

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

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Ability1Input();
        Ability2Input();

        AbilityCooldown(ability1Cooldown, abilityManaCost, ref currentAbility1Cooldown, ref isAbility1Cooldown, abilityImage1, abilityText1);
        AbilityCooldown(ability2Cooldown, ability2ManaCost, ref currentAbility2Cooldown, ref isAbility2Cooldown, abilityImage2, abilityText2);

        Ability1Canvas();
        Ability2Canvas();


    }

    private void Ability1Canvas()
    {
        if (ability1Skillshot.enabled)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            Quaternion ab1Canvas = Quaternion.LookRotation(position - transform.position);
            ab1Canvas.eulerAngles = new Vector3(0, ab1Canvas.eulerAngles.y, ab1Canvas.eulerAngles.z);
            ability1Canvas.transform.rotation = Quaternion.Lerp(ab1Canvas, ability1Canvas.transform.rotation, 0);
        }
    }

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

        var newKitPos = transform.position + hitPosDir * distance;
        ability2Canvas.transform.position = newKitPos;
    }

    private void Ability1Input()
    {
        if (Input.GetKeyDown(ability1Key) && !isAbility1Cooldown)
        {
            ability1Canvas.enabled = true;
            ability1Skillshot.enabled = true;

            ability2Canvas.enabled = false;
            ability2RangeIndicator.enabled = false;

            Cursor.visible = true;
        }

        if (ability1Skillshot.enabled && Input.GetMouseButtonDown(0))
        {
            isAbility1Cooldown = true;
            currentAbility1Cooldown = ability1Cooldown;

            // Hangi durumda mana harcaması yapılacak, bu işlem artık EzrealAbilityQ scriptinde olacak
        }
    }

    private Coroutine ability2TimeoutCoroutine;

    private void Ability2Input()
    {
        if (Input.GetKeyDown(ability2Key) && !isAbility2Cooldown && manaSystem.CanAffordAbility(ability2ManaCost))
        {
            ability2Canvas.enabled = true;
            ability2RangeIndicator.enabled = true;
            Cursor.visible = true;

            // Eğer daha önce başlatılmış bir coroutine varsa durdur
            if (ability2TimeoutCoroutine != null)
            {
                StopCoroutine(ability2TimeoutCoroutine);
            }

            // 7 saniye içinde hamle yapılmazsa kapat
            ability2TimeoutCoroutine = StartCoroutine(Ability2Timeout());
        }

        if (ability2Canvas.enabled && Input.GetMouseButtonDown(0))
        {
            isAbility2Cooldown = true;
            currentAbility2Cooldown = ability2Cooldown;

            float abilityRadius = maxAbility2Distance / 2;
            Collider[] hitColliders = Physics.OverlapSphere(position, abilityRadius);

            foreach (var hitCollider in hitColliders)
            {
                ObjectiveStats enemyStats = hitCollider.GetComponent<ObjectiveStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(65);
                }
            }

            CloseAbility2();
        }
    }

    // **7 saniye bekler, eğer süre dolarsa kapanır**
    private IEnumerator Ability2Timeout()
    {
        yield return new WaitForSeconds(7f);

        // Eğer yetenek hala açıksa kapat
        if (ability2Canvas.enabled)
        {
            CloseAbility2();
        }
    }

    // **Ability2’yi kapatma fonksiyonu**
    private void CloseAbility2()
    {
        ability2Canvas.enabled = false;
        ability2RangeIndicator.enabled = false;

        // Coroutine’i sıfırla
        ability2TimeoutCoroutine = null;
    }




    private void AbilityCooldown(float abilityCooldown, float abilityManaCost, ref float currentCooldown, ref bool isCooldown, Image skillImage, Text skillText)
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
            if (manaSystem.CanAffordAbility(abilityManaCost))
            {
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
                if (skillImage != null)
                {
                    skillImage.color = Color.blue;
                }
            }
        }
    }
}
