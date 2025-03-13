using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;  // Projenin hızı
    private Transform target;  // Projenin hedefi
    private float damage;      // Vereceği hasar

    // Hedef ve hasar değerini belirlemek için kullanılan metot
    public void Seek(Transform _target, float _damage)
    {
        target = _target;
        damage = _damage;
    }

    private void Update()
    {
        // Hedef yoksa en yakın hedefi bul
        if (target == null)
        {
            target = FindClosestTarget();
            if (target == null)
            {
                Destroy(gameObject); // Hedef bulunamazsa projeyi yok et
                return;
            }
        }

        Vector3 direction = target.position - transform.position; // Hedefe yön
        float distanceThisFrame = speed * Time.deltaTime;          // O karede alınacak mesafe

        // Eğer hedefe ulaşıldıysa, hedefe vur
        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // Hedefe doğru hareket et ve hedefe bak
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    // Hedef vurulduğunda yapılacak işlemler
    private void HitTarget()
    {
        // Öncelikli olarak ObjectiveStats kontrolü (minion ve kule)
        ObjectiveStats objectiveStats = target.GetComponent<ObjectiveStats>();
        if (objectiveStats != null)
        {
            objectiveStats.TakeDamage(damage); // Hedef minion veya kule ise hasar uygula
        }
        else
        {
            // Eğer ObjectiveStats yoksa Stats kontrol et (player)
            Stats stats = target.GetComponent<Stats>();
            if (stats != null)
            {
                stats.TakeDamage(gameObject, damage); // Hedef player ise hasar uygula
            }
        }

        Destroy(gameObject); // Hedef vurulduktan sonra projeyi yok et
    }

    // En yakın hedefi bulur
    private Transform FindClosestTarget()
    {
        // Tüm potansiyel hedefleri (minion, kule ve player) al
        ObjectiveStats[] objectiveStats = FindObjectsOfType<ObjectiveStats>();
        Stats[] playerStats = FindObjectsOfType<Stats>();

        Transform closestTarget = null;
        float shortestDistance = Mathf.Infinity;

        // Minion ve kuleler için hedef araması yap
        foreach (ObjectiveStats obj in objectiveStats)
        {
            float distanceToTarget = Vector3.Distance(transform.position, obj.transform.position);
            if (distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                closestTarget = obj.transform;
            }
        }

        // Player için hedef araması yap
        foreach (Stats player in playerStats)
        {
            float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                closestTarget = player.transform;
            }
        }

        return closestTarget;
    }

    // Çarpışma algılayıcı (hedefle çarpışmayı algılar)
    private void OnTriggerEnter(Collider other)
    {
        // Eğer çarpılan nesne hedefse, hedefi vur
        if (other.transform == target)
        {
            HitTarget();
        }
    }
}
