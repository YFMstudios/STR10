using UnityEngine;

public class TrapController : MonoBehaviour
{
    public int damageAmount = 50;       // Vereceği hasar miktarı
    public GameObject trapVisual;       // Tuzağın görsel objesi (visual kısmını bu objeye bağla)

    public float activationDistance = 2f; // Mesafe kontrolü için
    private bool isActive = false;      // Tuzak başta aktif değil
    private float activationCooldown = 0.5f; // Tekrar tetiklenmesin diye cooldown süresi
    private float lastActivatedTime = -1f;

    private Transform playerTransform; // Oyuncunun transformunu takip etmek için

    private void Start()
    {
        if (trapVisual != null)
        {
            trapVisual.SetActive(false); // Oyunun başında tuzağın görselini kapat
        }

        // Oyuncuyu sahnede bul ve transformunu al
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        // Player ve AllyMinion'lar için mesafe kontrolü yap
        if (!isActive)
        {
            // Player mesafesi
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float playerDistance = Vector3.Distance(transform.position, player.transform.position);
                if (playerDistance <= activationDistance && Time.time > lastActivatedTime + activationCooldown)
                {
                    ActivateTrap(); // Player tuzağa yaklaşınca aktive et
                }
            }

            // AllyMinionlar için mesafe kontrolü
            GameObject[] allyMinions = GameObject.FindGameObjectsWithTag("AllyMinion");
            foreach (GameObject allyMinion in allyMinions)
            {
                float minionDistance = Vector3.Distance(transform.position, allyMinion.transform.position);
                if (minionDistance <= activationDistance && Time.time > lastActivatedTime + activationCooldown)
                {
                    ActivateTrap(); // AllyMinion tuzağa yaklaşınca aktive et
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
            trapVisual.SetActive(true); // Tuzağı görünür yap
            Debug.Log("Tuzak aktifleştirildi ve görseli görünür oldu!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Eğer tuzak aktifleşmişse ve oyuncu/minion temasa geçerse hasar ver
        if (isActive)
        {
            if (other.CompareTag("Player") || other.CompareTag("AllyMinion"))
            {
                // Player için Stats
                if (other.CompareTag("Player"))
                {
                    var stats = other.GetComponent<Stats>();
                    if (stats != null)
                    {
                        stats.TakeDamage(gameObject, damageAmount); // Player için hasar verme
                        Debug.Log("Player hasara uğradı: " + damageAmount);
                    }
                }

                // Minion için ObjectiveStats
                if (other.CompareTag("AllyMinion"))
                {
                    var objStats = other.GetComponent<ObjectiveStats>();
                    if (objStats != null)
                    {
                        objStats.TakeDamage(damageAmount); // Minion için hasar verme
                        Debug.Log("AllyMinion hasara uğradı: " + damageAmount);
                    }
                }
            }
        }
    }

    public void DeactivateTrap()
    {
        if (trapVisual != null)
        {
            trapVisual.SetActive(false); // Tuzağın görselini tekrar gizle
            isActive = false;
            Debug.Log("Tuzak kapatıldı!");
        }
    }
}
