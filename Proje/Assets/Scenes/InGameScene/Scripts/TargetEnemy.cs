using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEnemy : MonoBehaviour
{
    // Hedefi tutacak Transform de�i�kenleri
    public Transform target;
    private Transform originalTarget;

    // RigidBody bile�eni ve at�� h�z�
    private Rigidbody theRB;
    public float projectileSpeed;

    // Oyuncu istatistiklerini tutacak Stats bile�eni
    private Stats playerStats;

    // Start fonksiyonu, nesne olu�turuldu�unda ilk �a�r�lan fonksiyon
    void Start()
    {
        // Ba�lang��ta hedefi originalTarget'e kopyala
        originalTarget = target;

        // Player etiketli objenin Stats bile�enini al
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();

        // Nesnenin Rigidbody bile�enini al
        theRB = GetComponent<Rigidbody>();
    }

    // Update fonksiyonu, her frame g�ncellenen fonksiyon
    void Update()
    {
        // E�er halihaz�rda bir hedef varsa
        if (target != null)
        {
            // Hedefe do�ru hareket y�n�
            Vector3 direction = target.position - transform.position;

            // Hareket y�n�n� normalle�tirip at�� h�z�yla �arp ve nesneyi hareket ettir
            theRB.velocity = direction.normalized * projectileSpeed;
        }
        // E�er originalTarget belirlenmi�se (ancak target yoksa)
        else if (originalTarget != null)
        {
            // originalTarget'e do�ru hareket y�n�
            Vector3 direction = originalTarget.position - transform.position;

            // Hareket y�n�n� normalle�tirip at�� h�z�yla �arp ve nesneyi hareket ettir
            theRB.velocity = direction.normalized * projectileSpeed;
        }
        // E�er ne target ne de originalTarget varsa
        else
        {
            // Nesneyi yok et
            Destroy(gameObject);
        }
    }

    // Hedefi ayarlamak i�in kullan�lan fonksiyon
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Nesnenin ba�ka bir collider ile temas etti�inde �a�r�lan fonksiyon
    private void OnTriggerEnter(Collider other)
    {
        // E�er target belirlenmi�se ve di�er nesne target ile ayn� ise
        if (target != null && other.gameObject == target.gameObject)
        {
            // Hedefin Stats bile�enini al ve hedefe zarar ver
            Stats targetStats = target.gameObject.GetComponent<Stats>();
            targetStats?.TakeDamage(target.gameObject, playerStats.damage);

            // Nesneyi yok et
            Destroy(gameObject);
        }
        // E�er originalTarget belirlenmi�se ve di�er nesne originalTarget ile ayn� ise
        else if (originalTarget != null && other.gameObject == originalTarget.gameObject)
        {
            // originalTarget'in Stats bile�enini al ve hedefe zarar ver
            Stats originalTargetStats = originalTarget.gameObject.GetComponent<Stats>();
            originalTargetStats?.TakeDamage(originalTarget.gameObject, playerStats.damage);

            // Nesneyi yok et
            Destroy(gameObject);
        }
    }
}
