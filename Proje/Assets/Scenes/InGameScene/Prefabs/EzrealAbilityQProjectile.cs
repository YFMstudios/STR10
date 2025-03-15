using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class EzrealAbilityQProjectile : MonoBehaviourPun
{
    [Header("Projectile Settings")]
    public float speed = 10f;         // Merminin hızı
    public float maxDistance = 100f;  // Max mesafe
    public float damage = 50f;        // Hasar
    public LayerMask hitLayers;       // Hangi layer'lara çarpacak?

    private Vector3 startPosition;    // Başlangıç konumu

    void Start()
    {
        startPosition = transform.position;

        // Rigidbody varsa, kinematic yap
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
    }

    void Update()
    {
        // Sadece sahibi (IsMine) mermiyi hareket ettirsin
        if (!photonView.IsMine) return;

        MoveProjectile();
        CheckDistanceTravelled();
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void CheckDistanceTravelled()
    {
        float traveled = Vector3.Distance(startPosition, transform.position);
        if (traveled >= maxDistance)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    // OnTriggerEnter tetiklenirse hasar ver ve yok ol
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        // Sadece tanımladığımız layer'lara değerse
        if (((1 << other.gameObject.layer) & hitLayers.value) == 0)
        {
            return; // Bu layer'a çarpmayı umursamıyoruz
        }

        // "Stats" script'i var mı?
        Stats targetStats = other.GetComponent<Stats>();
        if (targetStats != null)
        {
            targetStats.TakeDamage(damage);
        }
        else
        {
            // "ObjectiveStats" script'i var mı?
            ObjectiveStats targetObjStats = other.GetComponent<ObjectiveStats>();
            if (targetObjStats != null)
            {
                targetObjStats.TakeDamage(damage);
            }
        }

        // Herkeste mermiyi yok et
        PhotonNetwork.Destroy(gameObject);
    }
}
