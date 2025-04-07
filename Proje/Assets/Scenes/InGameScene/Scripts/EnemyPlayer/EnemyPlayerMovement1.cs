using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class EnemyPlayerMovement : MonoBehaviourPun
{
    public NavMeshAgent agent;
    public float rotateSpeedMovement = 0.05f;
    private float rotateVelocity;

    public Animator anim;
    float motionSmoothTime = 0.1f;

    [Header("Enemy Targeting")]
    public GameObject targetEnemy;
    public float stoppingDistance;
    private EnemyPlayerHighlightManager hmScript;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        hmScript = GetComponent<EnemyPlayerHighlightManager>();
        
        // Eğer bu karakter bana ait değilse:
        if (!photonView.IsMine)
        {
            // Eski yaklaşım “agent.enabled = false;” olabilir.
            // Ama eğer uzaktaki hareketleri de göreceksek, agent açık kalabilir.
            // Tercihe göre kapatabilir veya açık bırakabilirsiniz:
            // agent.enabled = false;
            return;
        }

        // Bu noktada karakter bana ait; agent'ı etkin kılıyoruz
        agent.enabled = true;

        // --- ÖNEMLİ KISIM: Karakter NavMesh üzerinde mi? ---
        // Bazen spawn noktası çok kenarda veya hafif havada kalırsa agent navmesh’e oturmuyor.
        // Bu yüzden yakın bir konumda NavMesh varsa oraya warp ediyoruz.
        if (!agent.isOnNavMesh)
        {
            // 5f yarıçap kadar alanda en yakın navmesh noktasını bulmaya çalış
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
            else
            {
                Debug.LogError("NavMesh üzerinde yer bulunamadı! Spawn noktası geçersiz olabilir.");
            }
        }
    }

    void Update()
    {
        // Agent bana ait değilse, input kodları devre dışı
        if (!photonView.IsMine) return;

        Animation();
        Move();
    }

    public void Animation()
    {
        float speed = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("Speed", speed, motionSmoothTime, Time.deltaTime);
    }

    public void Move()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    MoveToPosition(hit.point);
                }
                else if (hit.collider.CompareTag("Player") ||
                         hit.collider.CompareTag("AllyMinion") ||
                         hit.collider.CompareTag("AllyTurret"))
                {
                    MoveTowardsEnemy(hit.collider.gameObject);
                }
            }
        }

        if (targetEnemy != null)
        {
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) > stoppingDistance)
            {
                agent.SetDestination(targetEnemy.transform.position);
            }
        }
    }

    public void MoveToPosition(Vector3 position)
    {
        // Agent navmesh üzerinde mi kontrol edebiliriz:
        if (!agent.isOnNavMesh) return;

        agent.SetDestination(position);
        agent.stoppingDistance = 0;
        Rotation(position);

        if (targetEnemy != null)
        {
            hmScript.DeselectHighlight();
            targetEnemy = null;
        }
    }

    public void MoveTowardsEnemy(GameObject enemy)
    {
        if (!agent.isOnNavMesh) return;

        targetEnemy = enemy;
        agent.SetDestination(targetEnemy.transform.position);
        agent.stoppingDistance = stoppingDistance;
        Rotation(targetEnemy.transform.position);
        hmScript.SelectedHighlight();
    }

    public void Rotation(Vector3 lookAtPosition)
    {
        Quaternion rotationToLookAt = Quaternion.LookRotation(lookAtPosition - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                                                rotationToLookAt.eulerAngles.y,
                                                ref rotateVelocity,
                                                rotateSpeedMovement * (Time.deltaTime * 5));

        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }

    public void StopMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
    }

    public void ResumeMovement()
    {
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }
}
