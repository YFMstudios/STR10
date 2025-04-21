using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
public class Movement : MonoBehaviourPun
{
    [Header("NavMesh & Anim")]
    public NavMeshAgent agent;
    public Animator     anim;
    public float rotateSpeedMovement = 0.05f;
    private float rotateVelocity;
    private const float motionSmoothTime = 0.1f;

    [Header("Enemy Targeting")]
    public GameObject targetEnemy;
    public float stoppingDistance = 1.5f;
    private HighlightManager hmScript;   // opsiyonel

    /*──────────────────────────────────────────────────────────*/
    private void Awake()
    {
        // Bileşen önbelleği
        agent = GetComponent<NavMeshAgent>();
        anim  = GetComponent<Animator>();

        if (!TryGetComponent(out hmScript))
            Debug.LogWarning($"{name}: HighlightManager yok – vurgulama pasif.");
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        UpdateAnimation();
        HandleMovement();
    }

    /*───────── ANİMASYON ─────────*/
    private void UpdateAnimation()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("Speed", speedPercent, motionSmoothTime, Time.deltaTime);
    }

    /*───────── GİRİŞ & HEDEFLEME ─────────*/
    private void HandleMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
                                out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Ground"))
                    MoveToPosition(hit.point);
                else if (hit.collider.CompareTag("Enemy") ||
                         hit.collider.CompareTag("EnemyMinion") ||
                         hit.collider.CompareTag("EnemyTurret"))
                    MoveTowardsEnemy(hit.collider.gameObject);
            }
        }

        // Hedefe otomatik koş
        if (targetEnemy != null)
        {
            float dist = Vector3.Distance(transform.position, targetEnemy.transform.position);
            if (dist > stoppingDistance)
                agent.SetDestination(targetEnemy.transform.position);
        }
    }

    /*───────── SERBEST HAREKET ─────────*/
    public void MoveToPosition(Vector3 position)
    {
        agent.stoppingDistance = 0;
        agent.SetDestination(position);
        SetRotation(position);

        if (targetEnemy != null)
        {
            if (hmScript) hmScript.DeselectHighlight();
            targetEnemy = null;
        }
    }

    /*───────── DÜŞMANA KOŞ ─────────*/
    public void MoveTowardsEnemy(GameObject enemy)
    {
        targetEnemy = enemy;
        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(enemy.transform.position);
        SetRotation(enemy.transform.position);

        if (hmScript) hmScript.SelectedHighlight();   // NULL kontrolü
    }

    /*───────── ROTASYON ─────────*/
    private void SetRotation(Vector3 lookAtPos)
    {
        Quaternion rot = Quaternion.LookRotation(lookAtPos - transform.position);
        float y = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                                        rot.eulerAngles.y,
                                        ref rotateVelocity,
                                        rotateSpeedMovement * Time.deltaTime * 5f);
        transform.rotation = Quaternion.Euler(0, y, 0);
    }

    /*───────── DIŞ ARAYÜZ ─────────*/
    public void StopMovement()
    {
        if (agent)
        {
            agent.isStopped = true;
            agent.velocity  = Vector3.zero;
        }
    }
    public void ResumeMovement()
    {
        if (agent) agent.isStopped = false;
    }
}
