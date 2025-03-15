using UnityEngine;

public class MinionRangedCombat : MonoBehaviour
{
    public GameObject projectilePrefab;        // Ateşlenecek mermi prefab'ı
    public Transform projectileSpawnPoint;     // Mermi spawn noktası
    public float attackCooldown = 1.0f;        // Saldırı aralığı (cooldown)

    private float attackRange;                 // Saldırı menzili
    private float attackDamage;                // Saldırı hasarı
    private float lastAttackTime;              // Son saldırı zamanı

    private ObjectiveStats stats;              // Obje üzerindeki istatistikler bileşeni
    private MinionAI minionAI;                 // Minion hareket ve hedef yönetimi bileşeni
    private Animator animator;                 // Animator bileşeni

    private void Start()
    {
        stats = GetComponent<ObjectiveStats>();        // Obje üzerindeki istatistikler bileşeni alınır
        minionAI = GetComponent<MinionAI>();           // Minion hareket ve hedef yönetimi bileşeni alınır
        animator = GetComponent<Animator>();           // Animator bileşeni alınır

        attackRange = minionAI.stopDistance + 0.5f;     // Saldırı menzili minion hareket bileşeninden alınır
        attackDamage = stats.damage;                   // Saldırı hasarı istatistikler bileşeninden alınır
    }

    private void Update()
    {
        if (Time.time >= lastAttackTime + attackCooldown && IsTargetInRange())
        {
            Attack(); // Saldırı gerçekleştir
        }
        else if (!IsTargetInRange())
        {
            minionAI.StopCombat(); // Hedefte değilse savaşı durdur
            animator.SetBool("isAttacking", false); // Saldırı animasyonunu durdur
            animator.SetBool("isWalking", true);   // Yürüme animasyonunu başlat
        }
    }

    private bool IsTargetInRange()
    {
        // Minion'un hedefi var mı ve hedef minion'un saldırı menzilinde mi kontrol eder
        return minionAI.currentTarget != null &&
               Vector3.Distance(transform.position, minionAI.currentTarget.position) <= attackRange;
    }

    private void Attack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time; // Son saldırı zamanını güncelle
            minionAI.StartCombat();    // Minion savaşı başlatır

            // Saldırı animasyonunu tetikle
            animator.SetBool("isAttacking", true);
            animator.SetBool("isWalking", false);

            // ProjectilePrefab'ten yeni bir mermi (projectile) oluştur
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            MinionRangedProjectile projectileScript = projectile.GetComponent<MinionRangedProjectile>();
            projectileScript.SetTarget(minionAI.currentTarget.gameObject, attackDamage); // Mermiye hedef ve saldırı hasarı ata

            // Eğer hedef oyuncuysa hasar verme işlemi
            if (minionAI.currentTarget.CompareTag("Enemy"))
            {
                Stats playerStats = minionAI.currentTarget.GetComponent<Stats>();
                playerStats?.TakeDamage(gameObject, attackDamage); // Null check ile hasar verir
            }
        }
    }
}
