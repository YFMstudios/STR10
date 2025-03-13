using Photon.Pun;
using UnityEngine;

public class MinionMeleeDefenceCombat : MonoBehaviourPunCallbacks
{
    private float attackRange = 1.0f;        
    private float attackDamage = 10.0f;      
    public float attackCooldown = 1.0f;      

    private float lastAttackTime;            
    private bool isAttacking = false;        

    private ObjectiveStats stats;            
    private MinionAIDefence minionAI;               

    private void Start()
    {
        stats = GetComponent<ObjectiveStats>();
        minionAI = GetComponent<MinionAIDefence>();

        attackRange = minionAI.stopDistance + 0.5f; 
        attackDamage = stats.damage;
    }

    private void Update()
    {
        // Saldırı mantığı sadece Master Client'ta işlesin
        if (!PhotonNetwork.IsMasterClient) return;

        if (isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            isAttacking = false;
            minionAI.StopCombat();
        }

        if (minionAI.currentTarget != null && !isAttacking && IsTargetInRange())
        {
            Attack();
        }
    }

    private bool IsTargetInRange()
    {
        float distanceToTarget = Vector3.Distance(transform.position, minionAI.currentTarget.position);
        return distanceToTarget <= attackRange;
    }

    private void Attack()
    {
        // Tekrar kontrol edelim
        if (!PhotonNetwork.IsMasterClient) return;

        lastAttackTime = Time.time;
        isAttacking = true;
        minionAI.StartCombat();

        ObjectiveStats targetStats = minionAI.currentTarget.GetComponent<ObjectiveStats>();
        if (targetStats != null)
        {
            targetStats.TakeDamage(attackDamage);
        }
    }
}
