using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float detectionRadius = 10f;   // Alg�lama yar��ap�
    private Transform player;             // Oyuncunun transform'u
    private NavMeshAgent navMeshAgent;    // NavMeshAgent bile�eni

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;   // Oyuncuyu bul ve referans� ata
        navMeshAgent = GetComponent<NavMeshAgent>();                      // NavMeshAgent bile�enini al
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);  // D��man�n oyuncuya olan mesafesi
            Debug.Log("Distance to player: " + distanceToPlayer);                            // Mesafeyi konsola yazd�r

            if (distanceToPlayer <= detectionRadius)
            {
                navMeshAgent.SetDestination(player.position);   // Oyuncuya do�ru hareket et
                Debug.Log("Moving towards player");             // Konsola hareket etti�ini yazd�r
            }
        }
    }
}
