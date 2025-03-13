using System.Collections;
using UnityEngine;
// Photon eklentileri
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(PhotonView))]
public class MeleeCombat : MonoBehaviourPun
{
    private Movement moveScript;        // Hareket script bileşeni
    private Stats stats;                // Oyuncu istatistikleri bileşeni
    private Animator anim;              // Animator bileşeni

    [Header("Target")]
    public GameObject targetEnemy;      // Hedeflenen düşman objesi

    [Header("Melee Attack Variables")]
    public bool performMeleeAttack = true;  // Melee saldırı izni
    private float attackInterval;           // Saldırı aralığı
    private float nextAttackTime = 0;       // Bir sonraki saldırı zamanı

    void Start()
    {
        moveScript = GetComponent<Movement>();   // Hareket script bileşenini al
        stats = GetComponent<Stats>();           // Oyuncu istatistikleri bileşenini al
        anim = GetComponent<Animator>();         // Animator bileşenini al
    }

    void Update()
    {
        // Sadece bu karakterin sahibi (local player) saldırı ve input kodunu çalıştırsın
        if (!photonView.IsMine)
        {
            return; 
        }

        // Saldırı aralığını hesapla (basit bir formül örneği)
        attackInterval = stats.attackSpeed / ((500 + stats.attackSpeed) * 0.01f);

        // Movement scriptinden güncel hedefi al
        targetEnemy = moveScript.targetEnemy;

        // Eğer hedeflenen düşman varsa, melee saldırı zamanını kontrol et
        if (targetEnemy != null && performMeleeAttack && Time.time > nextAttackTime)
        {
            float distance = Vector3.Distance(transform.position, targetEnemy.transform.position);
            // Hedef yeterince yakınsa saldır
            if (distance <= 3.5f)
            {
                StartCoroutine(MeleeAttackInterval());
            }
            else
            {
                // Hedefe yaklaşılacak logiği, Movement scriptinde yer alıyor (NavMeshAgent)
            }
        }
    }

    // Saldırının “aralık” mantığını yöneten coroutine
    private IEnumerator MeleeAttackInterval()
    {
        performMeleeAttack = false; // Şu an saldırı aşamasındayız; yeni saldırı başlamasın

        // Saldırı animasyonunu tetikle (sadece local)
        anim.SetBool("isAttacking", true);

        // Saldırı aralığı kadar bekle
        yield return new WaitForSeconds(attackInterval);

        // Hedef hâlâ var mı? (Bu sırada ölmüş olabilir)
        if (targetEnemy == null)
        {
            // Saldırı animasyonunu bitir ve tekrar saldırıya hazır hale getir
            anim.SetBool("isAttacking", false);
            performMeleeAttack = true;
        }
        // Hedef yaşıyorsa, animasyon event'inde MeleeAttack() çalışacak
        // (örneğin Animasyon klibinizde bir event eklendiğini varsayıyoruz)
    }

    // **Animasyon event'inde** çağrılan saldırı fonksiyonu
    private void MeleeAttack()
    {
        Debug.Log("MeleeAttack çağrıldı! Hedef: " + (targetEnemy ? targetEnemy.name : "NULL"));

        if (targetEnemy != null)
        {
            // Hedefin Stats script'i var mı?
            Stats enemyStats = targetEnemy.GetComponent<Stats>();
            if (enemyStats != null)
            {
                // Burada "TakeDamage" metodunun Photon RPC kullandığını varsayıyoruz.
                // Örneğin Stats.cs içinde:
                //   public void TakeDamage(float dmg) { photonView.RPC("RPC_ApplyDamage", RpcTarget.All, dmg); }
                // gibi bir yapı olabilir.
                enemyStats.TakeDamage(stats.damage); 
            }
            // Yoksa, ObjectiveStats mı?
            else
            {
                ObjectiveStats enemyObjectiveStats = targetEnemy.GetComponent<ObjectiveStats>();
                if (enemyObjectiveStats != null)
                {
                    // ObjectiveStats içinde de benzer şekilde hasarı senkronize etmek isterseniz RPC veya 
                    // master client üzerinden yönetmek gerekebilir.
                    // Basit senaryoda local test için direkt:
                    enemyObjectiveStats.TakeDamage(stats.damage);
                }
            }
        }

        // Bir sonraki saldırı zamanı
        nextAttackTime = Time.time + attackInterval;

        // Animasyon bool'unu kapat
        anim.SetBool("isAttacking", false);

        // Bir sonraki saldırı girişimine hazır
        performMeleeAttack = true;
    }
}
