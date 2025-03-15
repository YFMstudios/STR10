using UnityEngine;

public class MinionMeleeDefenceCombat : MonoBehaviour
{
    private float attackRange = 1.0f;        // Sald�r� menzili
    private float attackDamage = 10.0f;      // Sald�r� hasar�
    public float attackCooldown = 1.0f;      // Sald�r� aral���

    private float lastAttackTime;            // Son sald�r� zaman�
    private bool isAttacking = false;        // Sald�r� durumu

    private ObjectiveStats stats;            // Hedefin istatistik bilgileri
    private MinionAIDefence minionAI;               // Minion AI bile�eni

    private void Start()
    {
        stats = GetComponent<ObjectiveStats>(); // ObjectiveStats bile�eni al�n�r
        minionAI = GetComponent<MinionAIDefence>();   // MinionAI bile�eni al�n�r

        attackRange = minionAI.stopDistance + 0.5f; // Sald�r� menzili, durma mesafesine eklenir
        attackDamage = stats.damage;                // Sald�r� hasar�, ObjectiveStats bile�eninden al�n�r
    }

    private void Update()
    {
        if (isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            isAttacking = false;
            minionAI.StopCombat(); // Sald�r� aral��� sonunda sava�� durdur
        }

        if (minionAI.currentTarget != null && !isAttacking && IsTargetInRange())
        {
            Attack(); // Hedef sald�r� menzilinde ve sald�r� durumunda ise sald�r� yap
        }
    }

    private bool IsTargetInRange()
    {
        float distanceToTarget = Vector3.Distance(transform.position, minionAI.currentTarget.position);
        return distanceToTarget <= attackRange; // Hedef sald�r� menzili i�inde mi kontrol�
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        isAttacking = true;
        minionAI.StartCombat(); // Sald�r�y� ba�lat ve sava� durumunu g�ncelle

        ObjectiveStats targetStats = minionAI.currentTarget.GetComponent<ObjectiveStats>();
        if (targetStats != null)
        {
            targetStats.TakeDamage(attackDamage); // Hedefin can�n� d���r
        }
    }
}