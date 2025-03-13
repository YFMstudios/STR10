using UnityEngine;

public class TrapController : MonoBehaviour
{
    public int damageAmount = 15;       // Vereceği hasar miktarı
    public GameObject trapVisual;       // Tuzak görseli
    public float activationDistance = 2f; // Aktifleştirme mesafesi
    private bool isActive = false;
    private float activationCooldown = 0.5f;
    private float lastActivatedTime = -1f;

    private Transform playerTransform;

    private void Start()
    {
        if (trapVisual != null)
        {
            trapVisual.SetActive(false);
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (!isActive)
        {
            // Player kontrolü
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float playerDistance = Vector3.Distance(transform.position, player.transform.position);
                if (playerDistance <= activationDistance && Time.time > lastActivatedTime + activationCooldown)
                {
                    ActivateTrap();
                }
            }

            // AllyMinion kontrolü
            GameObject[] allyMinions = GameObject.FindGameObjectsWithTag("AllyMinion");
            foreach (GameObject allyMinion in allyMinions)
            {
                float minionDistance = Vector3.Distance(transform.position, allyMinion.transform.position);
                if (minionDistance <= activationDistance && Time.time > lastActivatedTime + activationCooldown)
                {
                    ActivateTrap();
                }
            }
        }
    }

    private void ActivateTrap()
    {
        isActive = true;
        lastActivatedTime = Time.time;
        if (trapVisual != null)
        {
            trapVisual.SetActive(true);
            Debug.Log("Tuzak aktifleştirildi ve görseli aktif!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Sadece MasterClient hasar uygulasın
        if (!Photon.Pun.PhotonNetwork.IsMasterClient) return;

        if (isActive)
        {
            if (other.CompareTag("Player") || other.CompareTag("AllyMinion"))
            {
                if (other.CompareTag("Player"))
                {
                    var stats = other.GetComponent<Stats>();
                    if (stats != null)
                    {
                        stats.TakeDamage(gameObject, damageAmount);
                        Debug.Log("Player hasarı: " + damageAmount);
                    }
                }
                if (other.CompareTag("AllyMinion"))
                {
                    var objStats = other.GetComponent<ObjectiveStats>();
                    if (objStats != null)
                    {
                        objStats.TakeDamage(damageAmount);
                        Debug.Log("AllyMinion hasarı: " + damageAmount);
                    }
                }
            }
        }
    }

    public void DeactivateTrap()
    {
        if (trapVisual != null)
        {
            trapVisual.SetActive(false);
            isActive = false;
            Debug.Log("Tuzak kapatıldı!");
        }
    }
}
