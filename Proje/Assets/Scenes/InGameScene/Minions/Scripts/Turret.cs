using Photon.Pun;
using System.Linq;
using UnityEngine;

public class Turret : MonoBehaviourPunCallbacks
{
    [Header("Turret Settings")]
    public float attackRange = 5f;          // Saldırı menzili
    public float attackCooldown = 2f;       // Saldırı aralığı
    public Transform spawnPoint;            // Mermi çıkış noktası
    public LineRenderer lineRenderer;       // Hedefe çizgi çizmek için LineRenderer
    public float attackDamage = 10f;        // Saldırı hasarı

    private float nextAttackTime = 0f;      // Bir sonraki saldırı zamanı
    private GameObject currentTarget;       // Mevcut hedef
    private string[] targetTags;            // Hedef alınacak tag'ler

    // Sabit: Turret'in fırlattığı mermi prefab'ının Resources altındaki yolu
    private const string TURRET_PROJECTILE_PATH = "Projectiles/TurretProjectile";

    private void Start()
    {
        // Turret etiketine göre hedef tag'leri ayarlanır
        if (CompareTag("AllyTurret"))
        {
            targetTags = new string[] { "EnemyMinion", "Player" };
        }
        else
        {
            targetTags = new string[] { "AllyMinion", "Player" };
        }
    }

    private void Update()
    {
        // Saldırı kararları yalnızca MasterClient tarafından verilsin
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (currentTarget == null || !IsTargetInRange(currentTarget))
        {
            currentTarget = FindClosestTargetInRange();
        }

        UpdateLineToCurrentTarget();

        if (Time.time >= nextAttackTime && currentTarget != null)
        {
            AttackCurrentTarget();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private bool IsTargetInRange(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position) <= attackRange;
    }

    private GameObject FindClosestTargetInRange()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, attackRange)
            .Where(collider => targetTags.Contains(collider.gameObject.tag))
            .ToArray();

        GameObject closestTarget = null;
        float closestDistance = float.MaxValue;

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

    private void UpdateLineToCurrentTarget()
    {
        if (currentTarget != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, spawnPoint.position);
            lineRenderer.SetPosition(1, currentTarget.transform.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void AttackCurrentTarget()
    {
        // Turret oklarını ağ üzerinden oluşturuyor (yalnızca MasterClient çalıştırır)
        GameObject projectileGO = PhotonNetwork.Instantiate(
            TURRET_PROJECTILE_PATH,
            spawnPoint.position,
            Quaternion.identity
        );

        // Mermi script'inden hedefi ve hasarı gönderiyoruz
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.Seek(currentTarget.transform, attackDamage);
    }
}
