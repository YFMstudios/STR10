using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Photon eklentileri
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
public class Movement : MonoBehaviourPun
{
    public NavMeshAgent agent;                  // NavMeshAgent bileşeni
    public float rotateSpeedMovement = 0.05f;   // Hareket sırasında dönüş hızı
    private float rotateVelocity;               // Dönüş hızı için geçici değişken

    public Animator anim;                       // Animator bileşeni
    float motionSmoothTime = 0.1f;              // Animasyon geçiş süresi

    [Header("Enemy Targeting")]
    public GameObject targetEnemy;              // Hedeflenen düşman objesi
    public float stoppingDistance;              // Durdurma mesafesi
    private HighlightManager hmScript;          // HighlightManager bileşeni

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        hmScript = GetComponent<HighlightManager>();

        // PhotonTransformView'u kullanarak, Inspector üzerinde
        // "Synchronize Position" ve "Synchronize Rotation" ayarlarını aktif etmeyi unutma.
    }

    void Update()
    {
        // 1) Sadece local oyuncu kendi karakterini kontrol edecek
        if (!photonView.IsMine)
        {
            // Başkası tarafından kontrol edilen karakter
            // Hareket girişi almayacak, animasyon parametrelerini de local girdiyle değiştirmeyecek
            return;
        }

        // 2) Animasyonları güncelle (local)
        UpdateAnimation();

        // 3) Hareket ve hedefleme işlemlerini yönet (local)
        HandleMovement();
    }

    /// <summary>
    /// Animator parametrelerini günceller.
    /// Eğer animasyonu da diğer istemcilere senkronize etmek istiyorsan,
    /// PhotonAnimatorView veya kendi OnPhotonSerializeView (anim parametresi gönderimi) kullanabilirsin.
    /// </summary>
    private void UpdateAnimation()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("Speed", speedPercent, motionSmoothTime, Time.deltaTime);
    }

    /// <summary>
    /// Oyuncunun mouse input’unu alarak zemine tıklama veya düşman seçme gibi işlemleri yapar.
    /// </summary>
    private void HandleMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Sağ tık yapıldı, yere veya düşmana tıklanmış olabilir.
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    MoveToPosition(hit.point);
                }
                else if (hit.collider.CompareTag("Enemy") ||
                         hit.collider.CompareTag("EnemyMinion") ||
                         hit.collider.CompareTag("EnemyTurret"))
                {
                    MoveTowardsEnemy(hit.collider.gameObject);
                }
            }
        }

        // Eğer bir düşman hedeflenmişse, sürekli ona doğru hareket etmeye devam eder
        if (targetEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, targetEnemy.transform.position);
            if (distance > stoppingDistance)
            {
                agent.SetDestination(targetEnemy.transform.position);
            }
        }
    }

    /// <summary>
    /// Belirli bir noktaya hareket eder.
    /// </summary>
    /// <param name="position">Hareket edilecek hedef konum</param>
    public void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);
        agent.stoppingDistance = 0; // Serbest hareket

        SetRotation(position);

        // Eğer önceden seçili bir düşman varsa deselect yap
        if (targetEnemy != null)
        {
            hmScript.DeselectHighlight();
            targetEnemy = null;
        }
        else if (position.y >= 0.1f)
        {
            targetEnemy = null;
        }
    }

    /// <summary>
    /// Düşmana doğru hareket etmeyi sağlar.
    /// </summary>
    /// <param name="enemy">Hedeflenen düşman GameObject'i</param>
    public void MoveTowardsEnemy(GameObject enemy)
    {
        targetEnemy = enemy;
        agent.SetDestination(targetEnemy.transform.position);
        agent.stoppingDistance = stoppingDistance;

        SetRotation(targetEnemy.transform.position);
        hmScript.SelectedHighlight();
    }

    /// <summary>
    /// Karakterin, verilen konuma doğru bakış rotasyonunu ayarlar.
    /// </summary>
    /// <param name="lookAtPosition">Bakılacak nokta</param>
    private void SetRotation(Vector3 lookAtPosition)
    {
        Quaternion rotationToLookAt = Quaternion.LookRotation(lookAtPosition - transform.position);
        float rotationY = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            rotationToLookAt.eulerAngles.y,
            ref rotateVelocity,
            rotateSpeedMovement * (Time.deltaTime * 5)
        );

        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }

    /// <summary>
    /// Hareketi durdurur (NavMeshAgent’i bloke eder).
    /// </summary>
    public void StopMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Hareketi tekrar başlatır.
    /// </summary>
    public void ResumeMovement()
    {
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }
}
