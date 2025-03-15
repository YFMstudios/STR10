using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement)), RequireComponent(typeof(Stats))]
public class RangedCombat : MonoBehaviour
{
    // Ba�l� bile�enler
    private Movement moveScript;    // Hareket script bile�eni
    private Stats stats;            // Oyuncu istatistikleri bile�eni
    private Animator anim;          // Animator bile�eni

    [Header("Target")]
    public GameObject targetEnemy;  // Hedeflenen d��man objesi

    [Header("Ranged Attack Variables")]
    public bool performRangedAttack = true;    // Ranged sald�r� yap�l�yor mu?
    private float attackInterval;               // Sald�r� aral���
    private float nextAttackTime = 0;           // Bir sonraki sald�r� zaman�

    [Header("Projectile Settings")]
    public GameObject attackProjectile;         // Sald�r� projesi prefab�
    public Transform attackSpawnPoint;           // Sald�r� projesi spawn noktas�
    private GameObject spawnedProjectile;       // Olu�turulan sald�r� projesi

    // Start is called before the first frame update
    void Start()
    {
        // Ba�l� bile�enleri al
        moveScript = GetComponent<Movement>();
        stats = GetComponent<Stats>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Sald�r� aral���n� hesapla
        attackInterval = stats.attackSpeed / ((500 + stats.attackSpeed) * 0.01f);

        // Hedef d��man� g�ncelle (Movement scriptinden al�nan)
        targetEnemy = moveScript.targetEnemy;

        // Hedef d��man varsa ve ranged sald�r� yap�l�yorsa ve sald�r� zaman� geldiyse
        if (targetEnemy != null && performRangedAttack && Time.time > nextAttackTime)
        {
            // D��man hareket durma mesafesinde ise
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= moveScript.stoppingDistance)
            {
                // Ranged sald�r� aral���n� ba�lat
                StartCoroutine(RangedAttackInterval());
            }
        }
    }

    // Ranged sald�r� aral���n� y�neten Coroutine
    private IEnumerator RangedAttackInterval()
    {
        performRangedAttack = false;    // Ranged sald�r� yapma iznini kapat

        // Sald�r� animasyonunu tetikle
        anim.SetBool("isAttacking", true);

        // Sald�r� h�z�/Aral�k de�erine g�re bekle
        yield return new WaitForSeconds(attackInterval);

        // E�er hedef d��man hala hayattaysa
        if (targetEnemy == null)
        {
            // Animasyon bool'unu kapat ve tekrar sald�r� yapabilme iznini a�
            anim.SetBool("isAttacking", false);
            performRangedAttack = true;
        }
    }

    // Animasyon eventinde �a�r�lan fonksiyon
    private void RangedAttack()
    {
        // Sald�r� projesini spawn noktas�nda olu�tur
        spawnedProjectile = Instantiate(attackProjectile, attackSpawnPoint.transform.position, attackSpawnPoint.transform.rotation);

        // Sald�r� projesindeki TargetEnemy scriptini al
        TargetEnemy targetEnemyScript = spawnedProjectile.GetComponent<TargetEnemy>();

        // E�er script varsa hedefi ayarla
        if (targetEnemyScript != null)
        {
            targetEnemyScript.SetTarget(targetEnemy.transform);
        }

        // Bir sonraki sald�r� zaman�n� ayarla
        nextAttackTime = Time.time + attackInterval;

        // Animasyon bool'unu kapat ve tekrar sald�r� yapabilme iznini a�
        anim.SetBool("isAttacking", false);
        performRangedAttack = true;
    }
}
