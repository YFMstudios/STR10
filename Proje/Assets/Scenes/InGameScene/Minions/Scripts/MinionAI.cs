using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    public Transform currentTarget;
    public string enemyTag;
    public string enemyMinionTag;
    public string turretTag;
    public float stopDistance = 2.0f;
    public float aggroRange = 5.0f;
    public float targetSwitchInterval = 2.0f;
    public float rotationSpeed = 5f;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    private NavMeshAgent agent;
    private Animator animator; // Animator bileşeni
    public bool IsInCombat { get; private set; }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Animator bileşeni alınıyor
        InvokeRepeating(nameof(FindAndSetTarget), 0f, targetSwitchInterval);
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            if (!IsInCombat)
            {
                MoveTowardsTarget(); // Hedefe doğru hareket etme
            }
            RotateTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        // Mevcut hedefe doğru hareket etme
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
            StartCombat(); // Hedefe ulaştığında savaşı başlat
        }
    }

    private void RotateTowardsTarget()
    {
        // Hedefe doğru dönme
        Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
        if (directionToTarget != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void StartCombat()
    {
        // Savaşı başlatma
        IsInCombat = true;
        agent.isStopped = true;

        // Saldırı animasyonunu tetikleyin
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        Attack(); // Hedefe ulaştığında saldırıyı başlat
    }

    public void StopCombat()
    {
        // Savaşı durdurma
        IsInCombat = false;
        agent.isStopped = false;

        // Animasyonları sıfırlayın
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
    }

    private void FindAndSetTarget()
    {
        Transform closestEnemy = FindClosestWithTagInRadius(enemyTag, aggroRange);
        Transform closestEnemyMinion = FindClosestWithTagInRadius(enemyMinionTag, aggroRange);
        Transform closestTurret = FindClosestWithTagInRadius(turretTag, aggroRange);

        Transform[] targets = new Transform[] { closestEnemyMinion, closestTurret, closestEnemy };
        currentTarget = GetClosestTarget(targets);
    }

    private Transform GetClosestTarget(Transform[] targets)
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform target in targets)
        {
            if (target != null)
            {
                float distance = Vector3.Distance(currentPosition, target.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
        }

        return closestTarget;
    }

    private Transform FindClosestWithTagInRadius(string tag, float radius)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        return GetClosestObjectInRadius(objects, radius);
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
            if (currentTarget.CompareTag("EnemyMinion"))
            {
                ObjectiveStats minionStats = currentTarget.GetComponent<ObjectiveStats>();
                if (minionStats != null)
                {
                    minionStats.TakeDamage(damage);
                }
            }
            else if (currentTarget.CompareTag("EnemyTurret"))
            {
                ObjectiveStats turretStats = currentTarget.GetComponent<ObjectiveStats>();
                if (turretStats != null)
                {
                    turretStats.TakeDamage(damage);
                }
            }
            else if (currentTarget.CompareTag("Enemy"))
            {
                Stats enemyStats = currentTarget.GetComponent<Stats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(gameObject, damage);
                }
            }
        }
    }
}
