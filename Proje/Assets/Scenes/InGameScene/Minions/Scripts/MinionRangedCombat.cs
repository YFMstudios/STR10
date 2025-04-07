using Photon.Pun;
using UnityEngine;

public class MinionRangedCombat : MonoBehaviourPunCallbacks
{
    [Header("Projectile Settings (Koddan Belirliyoruz)")]
    // Burada "Allied" yani dost menzilli minyonun mermisini referanslıyoruz:
    private const string ALLY_PROJECTILE_PATH = "Projectiles/AllyRangedMinionProjectile";

    public Transform projectileSpawnPoint; // Mermi çıkış noktası
    public float attackCooldown = 1.0f;    // Saldırı süresi

    private float attackRange;     
    private float attackDamage;    
    private float lastAttackTime;

    private ObjectiveStats stats;  
    private MinionAI minionAI;     
    private Animator animator;

    private void Start()
    {
        stats = GetComponent<ObjectiveStats>();
        minionAI = GetComponent<MinionAI>();
        animator = GetComponent<Animator>();

        // Bu minyonun menzili ve hasarı
        attackRange = minionAI.stopDistance + 0.5f;
        attackDamage = stats.damage;
    }

    private void Update()
    {
        // Sadece Master Client saldırı hesaplasın
        if (!PhotonNetwork.IsMasterClient) 
        {
            return;
        }

        // Hedef menzil içindeyse ve cooldown dolmuşsa Attack()
        if (Time.time >= lastAttackTime + attackCooldown && IsTargetInRange())
        {
            Attack();
        }
        else if (!IsTargetInRange())
        {
            minionAI.StopCombat();
            animator.SetBool("isAttacking", false);
            animator.SetBool("isWalking", true);
        }
    }

    private bool IsTargetInRange()
    {
        return minionAI.currentTarget != null &&
               Vector3.Distance(transform.position, minionAI.currentTarget.position) <= attackRange;
    }

    private void Attack()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            minionAI.StartCombat();

            animator.SetBool("isAttacking", true);
            animator.SetBool("isWalking", false);

            // DOĞRUDAN Resources içindeki prefab'ı ağ üzerinden oluşturuyoruz
            GameObject projectile = PhotonNetwork.Instantiate(
                ALLY_PROJECTILE_PATH, 
                projectileSpawnPoint.position, 
                Quaternion.identity
            );

            // Mermi script'ine hedef ve hasar bilgisini iletiyoruz
            MinionRangedProjectile projectileScript = projectile.GetComponent<MinionRangedProjectile>();
            projectileScript.SetTarget(minionAI.currentTarget.gameObject, attackDamage);

            // İsterseniz, projectile çarpmasını beklemeden anında hasar da verebilirsiniz:
            if (minionAI.currentTarget.CompareTag("Enemy"))
            {
                Stats enemyStats = minionAI.currentTarget.GetComponent<Stats>();
                enemyStats?.TakeDamage(gameObject, attackDamage);
            }
        }
    }
}
