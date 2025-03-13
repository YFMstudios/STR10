using System.Collections;
using UnityEngine;
using Photon.Pun; // Photon PUN eklentisi

[RequireComponent(typeof(PhotonView))]
public class Stats : MonoBehaviourPun
{
    [Header("Base Stats")]
    public float health;            // Max Can değeri
    public float damage;            // Verilen hasar miktarı
    public float attackSpeed;       // Saldırı hızı

    // Health Slider Variables
    public float damageLerpDuration;    // Hasar geçiş süresi
    private float currentHealth;        // Mevcut can miktarı
    private float targetHealth;         // Hedeflenen can miktarı
    private Coroutine damageCoroutine;  // Hasar geçiş Coroutine'u

    private HealthUI healthUI;  // Can UI yöneticisi

    private void Awake()
    {
        healthUI = GetComponent<HealthUI>();

        currentHealth = health;
        targetHealth = health;

        if (healthUI != null)
        {
            healthUI.start3DSlider(health);
            healthUI.Update2DSlider(health, currentHealth);
        }
    }

    /// <summary>
    /// (1) Tek parametreli TakeDamage: Sadece hasar miktarını alır.
    /// Harici bir script, "targetStats.TakeDamage(50f);" gibi çağırabilir.
    /// </summary>
    public void TakeDamage(float damageAmount)
    {
        // Eğer bu objenin photonView’ı bize ait değilse (IsMine == false)
        // yine de RPC ile hasarı herkesin görmesini sağlıyoruz.
        if (!photonView.IsMine)
        {
            // Tasarıma göre bir şey yapmak isteyebilirsiniz.
            // Ama en basitinde yine RPC yollayabiliriz. 
        }

        // Tüm istemcilerde "RPC_ApplyDamage" metodunu çağırıyoruz.
        photonView.RPC("RPC_ApplyDamage", RpcTarget.All, damageAmount);
    }

    /// <summary>
    /// (2) İki parametreli TakeDamage: Kaynak (source) + Hasar miktarı.
    /// Bu, Trap veya Projectile vb. yerlerden "TakeDamage(gameObject, 50f);" şeklinde çağrıldığında
    /// Compile hatası almamanızı sağlar. Şu anda “source” parametresini sadece görmezden geliyoruz.
    /// </summary>
    public void TakeDamage(GameObject source, float damageAmount)
    {
        // İsterseniz source parametresini de RPC'ye ekleyip
        // "RPC_ApplyDamageWithSource" gibi bir metot yazabilirsiniz.
        // Şimdilik sadece hasar miktarını yolluyoruz:
        photonView.RPC("RPC_ApplyDamage", RpcTarget.All, damageAmount);
    }

    /// <summary>
    /// Asıl HP düşürme ve ölüm kontrolü bu RPC içinde yapılır;
    /// böylece tüm istemcilerde aynı sonuç oluşur.
    /// </summary>
    [PunRPC]
    private void RPC_ApplyDamage(float damageAmount)
    {
        targetHealth -= damageAmount;

        // Hedef sağlık sıfır veya altına düştü mü?
        if (targetHealth <= 0)
        {
            targetHealth = 0;

            // Player mı, Enemy mi kontrolü yapalım
            if (CompareTag("Player"))
            {
                CheckIfPlayerDead();
            }
            else if (CompareTag("Enemy") || CompareTag("EnemyMinion") || CompareTag("EnemyTurret"))
            {
                var enemyDeathHandler = GetComponent<EnemyDeathHandler>();
                if (enemyDeathHandler != null)
                {
                    enemyDeathHandler.Die();
                }
                else
                {
                    // Photon ile yok etmek
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }

        // Zaten bir damageCoroutine çalışmıyorsa başlat
        if (damageCoroutine == null)
        {
            damageCoroutine = StartCoroutine(LerpHealth());
        }
    }

    // Oyuncunun öldüğünü kontrol eden fonksiyon
    private void CheckIfPlayerDead()
    {
        Debug.Log($"{gameObject.name} öldü!");

        if (healthUI != null)
        {
            // UI'yi sıfırla
            healthUI.Update2DSlider(health, 0);
        }

        // Photon ile yok edelim (tüm istemcilerde silinsin)
        PhotonNetwork.Destroy(gameObject);
    }

    // Hasar geçişini yapan Coroutine
    private IEnumerator LerpHealth()
    {
        float elapsedTime = 0;
        float initialHealth = currentHealth;
        float target = targetHealth;

        while (elapsedTime < damageLerpDuration)
        {
            currentHealth = Mathf.Lerp(initialHealth, target, elapsedTime / damageLerpDuration);
            UpdateHealthUI();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentHealth = target;
        UpdateHealthUI();
        damageCoroutine = null;
    }

    // UI güncelleme işlemi
    private void UpdateHealthUI()
    {
        if (healthUI == null) return;

        healthUI.Update2DSlider(health, currentHealth);
        healthUI.update3DSlider(currentHealth);
    }
}
