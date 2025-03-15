using UnityEngine;

public class MinionRangedDefence : MonoBehaviour
{
    public GameObject projectilePrefab;       // Projectile prefab (ateslenecek mermi)
    public Transform projectileSpawnPoint;    // Mermi spawn noktası
    public float attackCooldown = 2.0f;       // Saldiri araligi (cooldown)

    private float attackRange;                 // Saldiri menzili
    private float attackDamage;                // Saldiri hasari
    private float lastAttackTime;              // Son saldiri zamani

    private ObjectiveStats stats;              // Obje uzerindeki istatistikler bileşeni
    private MinionAIDefence minionAI;         // Minion hareket ve hedef yönetimi bileşeni

    private void Start()
    {
        // Bilesen referanslarini al
        stats = GetComponent<ObjectiveStats>();
        minionAI = GetComponent<MinionAIDefence>();

        // Saldiri menzilini ve hasarini ayarla
        attackRange = minionAI.stopDistance + 0.5f; // Saldiri menzili
        attackDamage = stats.damage * 0.5f;         // Saldiri hasari
    }

    private void Update()
    {
        // Saldiri cooldown süresi kontrolü
        if (Time.time >= lastAttackTime + attackCooldown && IsTargetInRange())
        {
            Attack(); // Saldiri gerceklestir
        }
        else if (!IsTargetInRange())
        {
            minionAI.StopCombat(); // Hedefte degilse savasi durdur
        }
    }

    private bool IsTargetInRange()
    {
        // Hedefin mevcut olup olmadigini ve hedefin saldiri menzilinde olup olmadigini kontrol et
        return minionAI.currentTarget != null && Vector3.Distance(transform.position, minionAI.currentTarget.position) <= attackRange;
    }

    private void Attack()
    {
        // Eger son saldiri dan sonra cooldown süresi dolmus ise
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time; // Son saldiri zamanini güncelle
            minionAI.StartCombat(); // Minion savasi baslatir

            // ProjectilePrefab'ten yeni bir mermi (projectile) olustur
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            MinionRangedProjectile projectileScript = projectile.GetComponent<MinionRangedProjectile>();
            projectileScript.SetTarget(minionAI.currentTarget.gameObject, attackDamage); // Mermiye hedef ve saldiri hasari ata

            // Eger hedef oyuncuysa hasar verme islemi
            if (minionAI.currentTarget.CompareTag("Player"))
            {
                Stats playerStats = minionAI.currentTarget.GetComponent<Stats>();
                playerStats?.TakeDamage(gameObject, attackDamage); // Null check ile hasar verir
            }
        }
    }
}
