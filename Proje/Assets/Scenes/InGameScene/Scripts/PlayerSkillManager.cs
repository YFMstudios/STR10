using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public LayerMask attackableLayers; // Hangi katmanlar hedef olarak seçilebilir?
    public float attackRange = 15f; // Maksimum saldırı mesafesi
    public Transform playerTransform; // Oyuncunun pozisyon referansı
    private Animator playerAnimator; // Animator referansı

    void Start()
    {
        // Animator bileşenini almak
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol tık yapıldığında
        {
            HandleMouseClick();
        }
    }

    void HandleMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackRange, attackableLayers))
        {
            GameObject target = hit.collider.gameObject;

            // Hedefin özelliklerini kontrol edelim
            if (target.CompareTag("Enemy") || target.CompareTag("EnemyMinion"))
            {
                // Hedefe saldırı işlemini başlat
                AttackTarget(target);
            }
        }
    }

    void AttackTarget(GameObject target)
    {
        // Hedefin sağlık bileşenini alalım
        ObjectiveStats targetStats = target.GetComponent<ObjectiveStats>();

        if (targetStats != null)
        {
            // Hedefin sağlık puanını azalt
            targetStats.TakeDamage(50); // Saldırı hasarı
            Debug.Log($"Attacked {target.name} and dealt damage.");
        }

        // Saldırı animasyonunu tetikle
        PlayAttackAnimation(target.transform.position);
    }

    void PlayAttackAnimation(Vector3 targetPosition)
    {
        // Animator üzerinden saldırı animasyonunu tetikleyelim
        if (playerAnimator != null)
        {
            // Örneğin 'Attack' trigger'ını tetikle
            playerAnimator.SetTrigger("Attack");
            Debug.Log("Playing attack animation!");
        }
    }
}
