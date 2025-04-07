using Photon.Pun;    // <-- Photon kütüphanesi
using UnityEngine;

public class MinionMeleeCombat : MonoBehaviourPunCallbacks
{
    private float attackRange = 1.0f;        
    private float attackDamage = 10.0f;      
    public float attackCooldown = 1.0f;      

    private float lastAttackTime;            
    private bool isAttacking = false;        

    private ObjectiveStats stats;            
    private MinionAI minionAI;               

    private void Start()
    {
        stats = GetComponent<ObjectiveStats>();  
        minionAI = GetComponent<MinionAI>();     

        // Saldırı menzilini, minyonun stopDistance'ına göre ayarla
        attackRange = minionAI.stopDistance + 1.5f; 
        // Saldırı hasarını ObjectiveStats'tan çek
        attackDamage = stats.damage;              
    }

    private void Update()
    {
        // Tüm saldırı kontrolü yalnızca Master Client'ta çalışsın
        if (!PhotonNetwork.IsMasterClient) return;

        // Saldırı bitti mi?
        if (isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            isAttacking = false;
            minionAI.StopCombat();
        }

        // Menzil içinde hedef varsa saldıralım
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
        // Master Client kontrolü (bir kez daha)
        if (!PhotonNetwork.IsMasterClient) return;

        lastAttackTime = Time.time;
        isAttacking = true;
        minionAI.StartCombat(); // Animasyonu vs. başlat

        // Hedefe hasar ver
        ObjectiveStats targetStats = minionAI.currentTarget.GetComponent<ObjectiveStats>();
        if (targetStats != null)
        {
            targetStats.TakeDamage(attackDamage); 
        }
    }
}
