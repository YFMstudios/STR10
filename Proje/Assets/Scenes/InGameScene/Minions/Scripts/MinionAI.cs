using Photon.Pun;               // PhotonNetwork, MonoBehaviourPunCallbacks vb.
using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviourPunCallbacks
{
    [Header("Target & Tags")]
    public Transform currentTarget;
    public string enemyTag;
    public string enemyMinionTag;
    public string turretTag;

    [Header("Movement & AI")]
    public float stopDistance = 2.0f;
    public float aggroRange = 5.0f;
    public float targetSwitchInterval = 2.0f;
    public float rotationSpeed = 5f;

    [Header("Combat")]
    public float attackCooldown = 1f;
    private float lastAttackTime;

    private NavMeshAgent agent;
    private Animator animator;

    public bool IsInCombat { get; private set; }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Belirli aralıklarla hedef arama fonksiyonunu sadece Master Client tetikleyecek
        if (PhotonNetwork.IsMasterClient)
        {
            InvokeRepeating(nameof(FindAndSetTarget), 0f, targetSwitchInterval);
        }
    }

    private void Update()
    {
        // Sadece Master Client yapay zekâ hareketi ve saldırı mantığını işlesin
        if (!PhotonNetwork.IsMasterClient) 
        {
            return; 
        }

        if (currentTarget != null)
        {
            if (!IsInCombat)
            {
                MoveTowardsTarget();
            }
            RotateTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        float distance = Vector3.Distance(transform.position, currentTarget.position);

        if (distance > stopDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(currentTarget.position);

            // "Yürüme" animasyonu
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            // Hedefe ulaştı, dövüş/ saldırı başlasın
            agent.isStopped = true;
            StartCombat();
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
        agent.isStopped = true;

        // "Saldırı" animasyonu
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

        // Hedefe ulaştığı gibi ilk saldırıyı yapabilir
        Attack();
    }

    public void StopCombat()
    {
        IsInCombat = false;
        agent.isStopped = false;

        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
    }

    private void FindAndSetTarget()
    {
        // Master Client tüm düşmanları / minyonları / kuleleri bulup en yakın hedefi seçiyor
        Transform closestEnemy = FindClosestWithTagInRadius(enemyTag, aggroRange);
        Transform closestEnemyMinion = FindClosestWithTagInRadius(enemyMinionTag, aggroRange);
        Transform closestTurret = FindClosestWithTagInRadius(turretTag, aggroRange);

        Transform[] possibleTargets = new Transform[] { closestEnemyMinion, closestTurret, closestEnemy };
        currentTarget = GetClosestTarget(possibleTargets);
    }

    private Transform GetClosestTarget(Transform[] targets)
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform t in targets)
        {
            if (t != null)
            {
                float distance = Vector3.Distance(currentPosition, t.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = t;
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
        // Yalnızca Master Client hasar vermeli (çifte hasar, tutarsızlık olmasın diye)
        if (!PhotonNetwork.IsMasterClient) 
        {
            return;
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            float damage = 2f;

            if (currentTarget != null)
            {
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
                        // Örnek: enemyStats.TakeDamage(gameObject, damage);
                        enemyStats.TakeDamage(gameObject, damage);
                    }
                }
            }
        }
    }
}
