using UnityEngine;

public class MinionRangedProjectile : MonoBehaviour
{
    private GameObject target; // Hedef nesnesi
    private float damage;      // Zarar miktarý
    public float speed = 10f;  // Hýz

    // Hedef ve saldýrý hasarý ayarlamak için kullanýlýr
    public void SetTarget(GameObject newTarget, float attackDamage)
    {
        target = newTarget;
        damage = attackDamage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Hedef yoksa projeyi yok et ve fonksiyondan çýk
            return;
        }

        MoveTowardsTarget(); // Hedefe doðru hareket et
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = target.transform.position - transform.position; // Hedefe doðru vektör
        float distanceThisFrame = speed * Time.deltaTime; // Bu frame'deki hareket mesafesi

        if (direction.magnitude <= distanceThisFrame) // Hedefe ulaþýldýysa
        {
            DamageTarget(); // Hedefe zarar ver
            Destroy(gameObject); // Projeyi yok et
        }
        else
        {
            transform.Translate(direction.normalized * distanceThisFrame, Space.World); // Hedefe doðru hareket et
        }
    }

    private void DamageTarget()
    {
        ObjectiveStats targetStats = target.GetComponent<ObjectiveStats>(); // Hedefin istatistikler bileþeni
        if (targetStats != null)
        {
            targetStats.TakeDamage(damage); // Hedefe zarar ver
        }
    }
}
