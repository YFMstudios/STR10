using Photon.Pun;              // PhotonNetwork, MonoBehaviourPunCallbacks, vb.
using UnityEngine;
using UnityEngine.AI;

public class MinionAIDefence : MonoBehaviourPunCallbacks
{
    [Header("Tags & Targets")]
    public Transform currentTarget;             
    public string enemyMinionTag;               
    public string playerTag;                    

    [Header("Movement & AI")]
    public float stopDistance = 2.0f;           
    public float aggroRange = 5.0f;             
    public float targetSwitchInterval = 2.0f;   
    public float rotationSpeed = 5f;            

    [Header("Combat")]
    private float attackCooldown = 1f;          
    private float lastAttackTime;               
    public bool IsInCombat { get; private set; }

    private NavMeshAgent agent;                 
    private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Hedef belirleme işlemi yalnızca Master Client'ta çalışsın
        if (PhotonNetwork.IsMasterClient)
        {
            InvokeRepeating(nameof(FindAndSetTarget), 0f, targetSwitchInterval);
        }
    }

    private void Update()
    {
        // AI davranışlarını sadece Master Client işlesin
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

            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            agent.isStopped = true;
            Attack(); 
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

        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);

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
        // Master Client en yakın düşmanı arar
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
        // Saldırı ve hasar uygulaması da sadece Master Client'ta yürüsün
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
                if (currentTarget.CompareTag(playerTag))
                {
                    Stats playerStats = currentTarget.GetComponent<Stats>();
                    if (playerStats != null)
                    {
                        playerStats.TakeDamage(gameObject, damage);
                    }
                }
                else if (currentTarget.CompareTag(enemyMinionTag))
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
}
