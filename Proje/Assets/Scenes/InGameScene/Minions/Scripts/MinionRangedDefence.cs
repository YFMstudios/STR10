using Photon.Pun;
using UnityEngine;

public class MinionRangedDefence : MonoBehaviourPunCallbacks
{
    [Header("Projectile Settings (Koddan Belirliyoruz)")]
    // Burada enemy menzilli minyonun mermisini referanslıyoruz:
    private const string ENEMY_PROJECTILE_PATH = "Projectiles/EnemyRangedMinionProjectile";

    public Transform projectileSpawnPoint;    
    public float attackCooldown = 2.0f;       

    private float attackRange;
    private float attackDamage;
    private float lastAttackTime;

    private ObjectiveStats stats;
    private MinionAIDefence minionAI;

    private void Start()
    {
        stats = GetComponent<ObjectiveStats>();
        minionAI = GetComponent<MinionAIDefence>();

        // Örnek: hasarı %50'si olsun
        attackRange = minionAI.stopDistance + 0.5f;
        attackDamage = stats.damage * 0.5f;  
    }

    private void Update()
    {
        // Yalnızca Master Client saldırı
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (Time.time >= lastAttackTime + attackCooldown && IsTargetInRange())
        {
            Attack();
        }
        else if (!IsTargetInRange())
        {
            minionAI.StopCombat();
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

            // Mermiyi ağ üzerinden oluştur
            GameObject projectile = PhotonNetwork.Instantiate(
                ENEMY_PROJECTILE_PATH,
                projectileSpawnPoint.position,
                Quaternion.identity
            );

            // Mermi script'ine hedef ve hasarı ilet
            MinionRangedProjectile projectileScript = projectile.GetComponent<MinionRangedProjectile>();
            projectileScript.SetTarget(minionAI.currentTarget.gameObject, attackDamage);

            // Örneğin hedef "Player" ise anında hasar verebilirsiniz (isteğe bağlı):
            if (minionAI.currentTarget.CompareTag("Player"))
            {
                Stats playerStats = minionAI.currentTarget.GetComponent<Stats>();
                playerStats?.TakeDamage(gameObject, attackDamage);
            }
        }
    }
}
