using UnityEngine;
using UnityEngine.AI;

public class MinionAIDefence : MonoBehaviour
{
    public Transform currentTarget;             // Mevcut hedef
    public string enemyMinionTag;               // Düşman minion etiketi
    public string playerTag;                    // Oyuncu etiketi
    public float stopDistance = 2.0f;           // Hedefe yaklaşma mesafesi
    public float aggroRange = 5.0f;             // Hedef arama menzili
    public float targetSwitchInterval = 2.0f;   // Hedef değiştirme aralığı
    public float rotationSpeed = 5f;            // Hedefe dönme hızı

    private NavMeshAgent agent;                 // NavMesh Agent bileşeni
    private Animator animator;
    public bool IsInCombat { get; private set; } // Savaş durumu kontrolü

    private float attackCooldown = 1f;          // Saldırı için bekleme süresi
    private float lastAttackTime;               // Son saldırı zamanı

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMesh Agent bileşeni alınır
        animator = GetComponent<Animator>(); // Animator bileşeni alınıyor
        InvokeRepeating(nameof(FindAndSetTarget), 0f, targetSwitchInterval); // Hedef belirleme metodu belirli aralıklarla çağrılır
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            if (!IsInCombat)
            {
                MoveTowardsTarget(); // Hedefe doğru hareket et
            }
            RotateTowardsTarget(); // Hedefe doğru dön
        }
    }

    private void MoveTowardsTarget()
    {
        if (Vector3.Distance(transform.position, currentTarget.position) > stopDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(currentTarget.position);

            // Yürüme animasyonunu tetikleyin
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            agent.isStopped = true;
            Attack(); // Hedefe ulaşıldığında saldır
        }
    }

    private void RotateTowardsTarget()
    {
        Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
        if (directionToTarget != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void StartCombat()
    {
        IsInCombat = true;
        agent.isStopped = true; // Savaş durumunda hareket durdurulur

        // Saldırı animasyonunu tetikleyin
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        Attack(); // Hedefe ulaştığında saldırıyı başlat
    }

    public void StopCombat()
    {
        IsInCombat = false;
        agent.isStopped = false; // Savaş bittiğinde hareket devam eder

        // Animasyonları sıfırlayın
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
    }

    private void FindAndSetTarget()
    {
        Transform closestEnemyMinion = FindClosestWithTagInRadius(enemyMinionTag, aggroRange);
        Transform closestPlayer = FindClosestWithTag(playerTag);

        // En yakın hedefi seç
        if (closestEnemyMinion != null && closestPlayer != null)
        {
            float distanceToEnemyMinion = Vector3.Distance(transform.position, closestEnemyMinion.position);
            float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);
            currentTarget = distanceToEnemyMinion < distanceToPlayer ? closestEnemyMinion : closestPlayer;
        }
        else
        {
            currentTarget = closestEnemyMinion ?? closestPlayer;
        }
    }

    private Transform FindClosestWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        return GetClosestObject(objects);
    }

    private Transform FindClosestWithTagInRadius(string tag, float radius)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        return GetClosestObjectInRadius(objects, radius);
    }

    private Transform GetClosestObject(GameObject[] objects)
    {
        float closestDistance = Mathf.Infinity;
        Transform closestObject = null;
        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(currentPosition, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj.transform;
            }
        }

        return closestObject;
    }

    private Transform GetClosestObjectInRadius(GameObject[] objects, float radius)
    {
        float closestDistance = Mathf.Infinity;
        Transform closestObject = null;
        Vector3 currentPosition = transform.position;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(currentPosition, obj.transform.position);
            if (distance < closestDistance && distance <= radius)
            {
                closestDistance = distance;
                closestObject = obj.transform;
            }
        }

        return closestObject;
    }

    private void Attack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            float damage = 2f;
            if (currentTarget.CompareTag(playerTag)) // Eğer hedef oyuncuysa
            {
                Stats playerStats = currentTarget.GetComponent<Stats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(gameObject, damage);
                }
            }
            else if (currentTarget.CompareTag(enemyMinionTag)) // Eğer hedef düşman minion ise
            {
                ObjectiveStats minionStats = currentTarget.GetComponent<ObjectiveStats>();
                if (minionStats != null)
                {
                    minionStats.TakeDamage(damage);
                }
            }
        }
    }
}
