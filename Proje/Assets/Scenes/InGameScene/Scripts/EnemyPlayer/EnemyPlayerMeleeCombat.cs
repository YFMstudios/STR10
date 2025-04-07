using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(EnemyPlayerMovement)), RequireComponent(typeof(Stats)), RequireComponent(typeof(PhotonView))]
public class EnemyPlayerMeleeCombat : MonoBehaviourPun
{
    private EnemyPlayerMovement moveScript;
    private Stats stats;
    private Animator anim;

    [Header("Target")]
    public GameObject targetEnemy;

    [Header("Melee Attack Variables")]
    public bool performMeleeAttack = true;
    private float attackInterval;
    private float nextAttackTime = 0;

    void Start()
    {
        moveScript = GetComponent<EnemyPlayerMovement>();
        stats = GetComponent<Stats>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        attackInterval = stats.attackSpeed / ((500 + stats.attackSpeed) * 0.01f);
        targetEnemy = moveScript.targetEnemy;

        if (targetEnemy != null && performMeleeAttack && Time.time > nextAttackTime)
        {
            float distance = Vector3.Distance(transform.position, targetEnemy.transform.position);

            if (distance <= 3.5f)
            {
                StartCoroutine(MeleeAttackInterval());
            }
        }
    }

    private IEnumerator MeleeAttackInterval()
    {
        performMeleeAttack = false;
        anim.SetBool("isAttacking", true);

        yield return new WaitForSeconds(attackInterval);

        if (targetEnemy == null)
        {
            anim.SetBool("isAttacking", false);
            performMeleeAttack = true;
        }
    }

    // Animasyon event'inde çağrılır
    private void MeleeAttack()
    {
        if (!photonView.IsMine)
            return;

        if (targetEnemy != null)
        {
            Stats enemyStats = targetEnemy.GetComponent<Stats>();
            if (enemyStats != null)
            {
                // Lokal TakeDamage çağırarak, Stats içindeki RPC_ApplyDamage tetiklenir.
                enemyStats.TakeDamage(stats.damage);
            }
            else
            {
                ObjectiveStats enemyObjectiveStats = targetEnemy.GetComponent<ObjectiveStats>();
                if (enemyObjectiveStats != null)
                {
                    enemyObjectiveStats.TakeDamage(stats.damage);
                }
            }
        }

        nextAttackTime = Time.time + attackInterval;
        anim.SetBool("isAttacking", false);
        performMeleeAttack = true;
    }
}
