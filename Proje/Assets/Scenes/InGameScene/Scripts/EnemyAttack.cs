using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 2f;      // Sald�r� menzili
    public float attackCooldown = 1f;   // Sald�r� aral���
    public int damage = 10;             // Yap�lacak hasar miktar�
    private float lastAttackTime = -1f; // Son sald�r� zaman�
    private Transform player;           // Oyuncunun transform'u

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;   // Oyuncuyu bul ve referans� ata
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);   // D��man�n oyuncuya olan mesafesi
            Debug.Log("Distance to player for attack: " + distanceToPlayer);                  // Mesafeyi konsola yazd�r

            if (distanceToPlayer <= attackRange)
            {
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    AttackPlayer();                  // Oyuncuya sald�r�
                    lastAttackTime = Time.time;      // Son sald�r� zaman�n� g�ncelle
                    Debug.Log("Attacking player");   // Konsola sald�rd���n� yazd�r
                }
            }
        }
    }

    private void AttackPlayer()
    {
        Debug.Log("Player hit for " + damage + " damage!");   // Oyuncuya hasar verildi�ini konsola yazd�r
        Stats playerStats = player.GetComponent<Stats>();    // Oyuncunun Stats bile�enini al
        if (playerStats != null)
        {
            playerStats.TakeDamage(gameObject, damage);      // Oyuncuya hasar verme i�levini �a��r
        }
    }
}
