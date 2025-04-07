using Photon.Pun;
using UnityEngine;

public class MinionRangedProjectile : MonoBehaviourPunCallbacks
{
    private GameObject target; 
    private float damage;
    public float speed = 10f;

    public void SetTarget(GameObject newTarget, float attackDamage)
    {
        target = newTarget;
        damage = attackDamage;
    }

    void Update()
    {
        if (target == null)
        {
            // Sadece MasterClient, ağ üzerinden destroy edebilir
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            return;
        }

        MoveTowardsTarget(); 
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            DamageTarget();
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        }
    }

    private void DamageTarget()
    {
        // Yalnızca MasterClient gerçek hasarı uygulasın
        if (PhotonNetwork.IsMasterClient)
        {
            var targetStats = target.GetComponent<ObjectiveStats>();
            if (targetStats != null)
            {
                targetStats.TakeDamage(damage);
            }
        }
    }
}
