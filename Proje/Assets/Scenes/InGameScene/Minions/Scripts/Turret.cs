using UnityEngine;
using System.Linq;

public class Turret : MonoBehaviour
{
    [Header("Turret Settings")]
    public float attackRange = 5f;          // Saldırı menzili
    public float attackCooldown = 2f;       // Saldırı aralığı
    public GameObject projectilePrefab;     // Mermi prefab'ı
    public Transform spawnPoint;            // Mermi çıkış noktası
    public LineRenderer lineRenderer;       // Hedefe çizgi çizmek için LineRenderer
    public float attackDamage = 10f;        // Saldırı hasarı

    private float nextAttackTime = 0f;      // Bir sonraki saldırı zamanı
    private GameObject currentTarget;       // Mevcut hedef
    private string[] targetTags;            // Taretin hedef alacağı tag'ler

    private void Start()
    {
        // Taretin etiketine göre hedef tag'lerini belirle
        if (CompareTag("AllyTurret"))
        {
            targetTags = new string[] { "EnemyMinion", "Player" }; // Düşman minion ve player hedef
        }
        else
        {
            targetTags = new string[] { "AllyMinion", "Player" };  // Kendi minionlar ve player hedef
        }
    }

    private void Update()
    {
        // Hedef yoksa veya menzil dışındaysa yeni bir hedef bul
        if (currentTarget == null || !IsTargetInRange(currentTarget))
        {
            currentTarget = FindClosestTargetInRange();
        }

        // Çizgiyi güncelle
        UpdateLineToCurrentTarget();

        // Saldırı zamanı geldiyse ve hedef varsa saldır
        if (Time.time >= nextAttackTime && currentTarget != null)
        {
            AttackCurrentTarget();
            nextAttackTime = Time.time + attackCooldown;  // Saldırı bekleme süresini ayarla
        }
    }

    // Hedefin menzil içinde olup olmadığını kontrol eder
    private bool IsTargetInRange(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position) <= attackRange;
    }

    // Menzildeki en yakın hedefi bulur
    private GameObject FindClosestTargetInRange()
    {
        // Menzil içindeki hedefleri bul
        var hitColliders = Physics.OverlapSphere(transform.position, attackRange)
            .Where(collider => targetTags.Contains(collider.gameObject.tag)) // Hedefin tag'leri uygun mu kontrol et
            .ToArray();

        GameObject closestTarget = null;
        float closestDistance = float.MaxValue;

        // En yakın hedefi bul
        foreach (var collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = collider.gameObject;
            }
        }

        return closestTarget;
    }

    // LineRenderer ile hedefe çizgi çizer
    private void UpdateLineToCurrentTarget()
    {
        if (currentTarget != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, spawnPoint.position);                // Çizginin başlangıç noktası
            lineRenderer.SetPosition(1, currentTarget.transform.position);   // Çizginin bitiş noktası
        }
        else
        {
            lineRenderer.enabled = false; // Hedef yoksa çizgiyi kapat
        }
    }

    // Mevcut hedefe saldırı yapar
    private void AttackCurrentTarget()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity); // Mermi üret
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.Seek(currentTarget.transform, attackDamage);  // Hedefe yönlendir ve hasarı uygula
    }
}
