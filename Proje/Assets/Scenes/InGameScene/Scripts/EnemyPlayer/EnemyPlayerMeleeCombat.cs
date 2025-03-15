using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement)), RequireComponent(typeof(Stats))]
public class EnemyPlayerMeleeCombat : MonoBehaviour
{
    private EnemyPlayerMovement moveScript;        // Hareket script bileşeni
    private EnemyPlayerStats stats;                // Oyuncu istatistikleri bileşeni
    private Animator anim;              // Animator bileşeni

    [Header("Target")]
    public GameObject targetEnemy;      // Hedeflenen düşman objesi

    [Header("Melee Attack Variables")]
    public bool performMeleeAttack = true;  // Melee saldırı yapılıyor mu?
    private float attackInterval;           // Saldırı aralığı
    private float nextAttackTime = 0;       // Bir sonraki saldırı zamanı

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<EnemyPlayerMovement>();   // Hareket script bileşenini al
        stats = GetComponent<EnemyPlayerStats>();           // Oyuncu istatistikleri bileşenini al
        anim = GetComponent<Animator>();         // Animator bileşenini al
    }

    // Update is called once per frame
    void Update()
    {
        attackInterval = stats.attackSpeed / ((500 + stats.attackSpeed) * 0.01f);  // Saldırı aralığını hesapla

        targetEnemy = moveScript.targetEnemy;  // Hedef düşmanı güncelle (Movement scriptinden alınan)

        if (targetEnemy != null)
        {
            Debug.Log("Target enemy found: " + targetEnemy.name); // Hedef doğru şekilde alınıyor mu kontrol et
        }
        else
        {
            Debug.LogWarning("Target enemy is null!");
        }

        // Eğer hedeflenen düşman varsa, melee saldırı yapılabiliyorsa ve saldırı zamanı geldiyse
        if (targetEnemy != null && performMeleeAttack && Time.time > nextAttackTime)
        {
            // Mesafeyi hesapla
            float distance = Vector3.Distance(transform.position, targetEnemy.transform.position);

            // Debug: Mesafeyi yazdır
            Debug.Log("Mesafe: " + distance + " StoppingDistance: 4");

            // Eğer mesafe 4'e eşit ya da küçükse
            if (distance <= 3.5f)  // Mesafeyi 4 olarak belirledik
            {
                Debug.Log("Yeterli mesafeye gelindi, saldırı başlayacak.");
                StartCoroutine(MeleeAttackInterval());
            }
            else
            {
                Debug.Log("Hedefe yaklaşılacak. Mesafe: " + distance);
            }
        }
    }




    // Melee saldırı aralığını yöneten Coroutine
    private IEnumerator MeleeAttackInterval()
    {
        performMeleeAttack = false;    // Melee saldırı yapma iznini kapat

        // Saldırı animasyonunu tetikle
        anim.SetBool("isAttacking", true);

        // Saldırı hızı/Aralık değerine göre bekle
        yield return new WaitForSeconds(attackInterval);

        // Eğer hedef düşman hala hayattaysa
        if (targetEnemy == null)
        {
            // Animasyon bool'unu kapat ve tekrar saldırı yapabilme iznini aç
            anim.SetBool("isAttacking", false);
            performMeleeAttack = true;
        }
    }

    // Animasyon eventinde çağrılan fonksiyon
    private void MeleeAttack()
    {
        Debug.Log("MeleeAttack çağrıldı! Hedef: " + targetEnemy.name);

        if (targetEnemy != null)
        {
            // Eğer hedef düşman bir minyon veya başka bir oyuncu ise
            EnemyPlayerStats enemyStats = targetEnemy.GetComponent<EnemyPlayerStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(gameObject, stats.damage);  // Kendi hasarını düşmana uygula
            }
            // Eğer hedef bir minyon ise, ObjectiveStats kontrol et
            else
            {
                ObjectiveStats enemyObjectiveStats = targetEnemy.GetComponent<ObjectiveStats>();
                if (enemyObjectiveStats != null)
                {
                    enemyObjectiveStats.TakeDamage(stats.damage);  // Minyon veya başka bir objeye hasar uygula
                }
            }
        }

        // Saldırı aralığını güncelle
        nextAttackTime = Time.time + attackInterval;

        // Saldırı animasyonunu durdur
        anim.SetBool("isAttacking", false);

        // Bir sonraki saldırıya hazır ol
        performMeleeAttack = true;
    }

}
