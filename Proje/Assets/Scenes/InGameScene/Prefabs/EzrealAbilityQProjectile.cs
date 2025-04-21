using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class EzrealAbilityQProjectile : MonoBehaviourPun
{
    [Header("Projectile Settings")]
    public float  speed       = 10f;
    public float  maxDistance = 30f;
    public float  damage      = 50f;
    public LayerMask hitLayers;

    private Vector3 spawnPos;
    private bool    markedForDestroy;

    /*──────────────────────────────────────────────────────────*/
    void Start()
    {
        spawnPos = transform.position;
        if (TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;
    }

    void Update()
    {
        if (photonView.IsMine)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (!markedForDestroy &&
            (transform.position - spawnPos).sqrMagnitude >= maxDistance * maxDistance)
        {
            NetworkDestroy();
        }
    }

    /*──────────────────────── ÇARPIŞMA ───────────────────────*/
    void OnTriggerEnter(Collider other)
    {
        if (markedForDestroy) return;

        if (((1 << other.gameObject.layer) & hitLayers.value) == 0) return;

        /* Stats */
        if (other.TryGetComponent<Stats>(out Stats stats))
        {
            stats.photonView.RPC("RPC_ApplyDamage", RpcTarget.AllBuffered, damage);
        }
        /* ObjectiveStats */
        else if (other.TryGetComponent<ObjectiveStats>(out ObjectiveStats objStats))
        {
            objStats.photonView.RPC("RPC_ApplyDamage", RpcTarget.AllBuffered, damage);
        }

        NetworkDestroy();
    }

    /*──────────────────────── YOK ETME ───────────────────────*/
    private void NetworkDestroy()
    {
        if (markedForDestroy) return;
        markedForDestroy = true;

        if (photonView.IsMine || PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(gameObject);
    }
}
