using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPlayerMovement : MonoBehaviour
{
    public NavMeshAgent agent;                  // NavMeshAgent bileşeni
    public float rotateSpeedMovement = 0.05f;   // Hareket sırasında dönüş hızı
    private float rotateVelocity;               // Dönüş hızı için geçici değişken

    public Animator anim;                       // Animator bileşeni
    float motionSmoothTime = 0.1f;              // Animasyon geçiş süresi

    [Header("Enemy Targeting")]
    public GameObject targetEnemy;              // Hedeflenen düşman objesi
    public float stoppingDistance;              // Durdurma mesafesi
    private EnemyPlayerHighlightManager hmScript;          // HighlightManager bileşeni

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();  // NavMeshAgent bileşenini al
        hmScript = GetComponent<EnemyPlayerHighlightManager>();      // HighlightManager bileşenini al
    }

    // Update is called once per frame
    void Update()
    {
        Animation();    // Animasyonları güncelle
        Move();         // Hareketi yönet
    }

    // Animasyonu güncelleyen fonksiyon
    public void Animation()
    {
        float speed = agent.velocity.magnitude / agent.speed;  // Hızı hesapla
        anim.SetFloat("Speed", speed, motionSmoothTime, Time.deltaTime);  // Animasyon parametresini güncelle
    }

    // Hareketi yöneten fonksiyon
    public void Move()
    {
        if (Input.GetMouseButtonDown(1))    // Fare sağ tuşa basıldıysa
        {
            RaycastHit hit;

            // Fare pozisyonundan Raycast yaparak nesneyi tespit et
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Ground")   // Eğer tıklanan zemin ise
                {
                    MoveToPosition(hit.point);      // Belirlenen noktaya hareket et
                }
                else if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("AllyMinion") || hit.collider.CompareTag("AllyTurret"))
                {
                    MoveTowardsEnemy(hit.collider.gameObject);
                }

            }
        }

        // Eğer hedeflenen düşman varsa
        if (targetEnemy != null)
        {
            // Oyuncu düşmandan belirli bir mesafede ise
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) > stoppingDistance)
            {
                agent.SetDestination(targetEnemy.transform.position);  // Düşmanın konumuna git
            }
        }
    }

    // Belirli bir noktaya hareket etmeyi sağlayan fonksiyon
    public void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);         // Belirlenen noktaya git
        agent.stoppingDistance = 0;             // Durma mesafesini sıfırla

        Rotation(position);                     // Rotasyonu ayarla

        // Eğer hedeflenen düşman varsa ve seçiliyse, vurguyu kaldır
        if (targetEnemy != null)
        {
            hmScript.DeselectHighlight();
            targetEnemy = null;
        }
        // Aksi takdirde, hedef düşmanı sıfırla
        else if (position.y >= 0.1f)
        {
            targetEnemy = null;
        }
    }

    // Düşmana doğru hareket etmeyi sağlayan fonksiyon
    public void MoveTowardsEnemy(GameObject enemy)
    {
        targetEnemy = enemy;                    // Hedeflenen düşmanı ayarla
        agent.SetDestination(targetEnemy.transform.position);  // Düşmanın konumuna git
        agent.stoppingDistance = stoppingDistance;  // Durma mesafesini ayarla

        Rotation(targetEnemy.transform.position);  // Rotasyonu ayarla
        hmScript.SelectedHighlight();            // Düşmana vurgu ekle
    }

    // Belirli bir noktaya doğru rotasyonu ayarlayan fonksiyon
    public void Rotation(Vector3 lookAtPosition)
    {
        Quaternion rotationToLookAt = Quaternion.LookRotation(lookAtPosition - transform.position);  // Belirli noktaya bakacak rotasyonu hesapla
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y,
            ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));  // Y ekseninde dönüşü pürüzsüz yap

        transform.eulerAngles = new Vector3(0, rotationY, 0);  // Rotasyonu uygula
    }

    //NEWLY ADDED
    public void StopMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true; // Stop the NavMeshAgent from moving
            agent.velocity = Vector3.zero; // Immediately stop any current movement
        }
    }

    public void ResumeMovement()
    {
        if (agent != null)
        {
            agent.isStopped = false; // Allow the NavMeshAgent to move again
        }
    }
}
