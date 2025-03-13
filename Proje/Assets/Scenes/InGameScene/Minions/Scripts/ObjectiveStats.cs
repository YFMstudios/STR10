using System.Collections;
using UnityEngine;
using Photon.Pun;  // <-- Photon eklendi

public class ObjectiveStats : MonoBehaviourPunCallbacks
{
    [Header("Base Stats")]
    public float health;
    public float damage;

    public float damageLerpDuration;
    private float currentHealth;
    private float targetHealth;
    private Coroutine damageCoroutine;

    private float accumulatedDamage = 0; // Biriken hasar

    private HealthUII healthUII;

    // Animator referansı
    private Animator animator;

    [Header("ScriptableObject")]
    public ProgressData progressData;

    private void Awake()
    {
        healthUII = GetComponent<HealthUII>();
        currentHealth = health;
        targetHealth = health;

        healthUII.Start3DSlider(health);

        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Master Client bu metodu doğrudan çağırarak hasar uygular.
    /// Diğer istemciler buraya doğrudan girmeyecek.
    /// </summary>
    public void TakeDamage(float damageAmount)
    {
        // Sadece Master Client bu kodu çalıştırsın
        if (!PhotonNetwork.IsMasterClient) return;

        // Tüm istemcilerde hasar sürecini başlatmak için RPC
        photonView.RPC(nameof(RPC_TakeDamageAll), RpcTarget.All, damageAmount);
    }

    /// <summary>
    /// Tüm istemciler: Hasarı local olarak uygular, Lerp başlatır.
    /// </summary>
    [PunRPC]
    private void RPC_TakeDamageAll(float damageAmount)
    {
        accumulatedDamage += damageAmount;

        // Eğer coroutine çalışmıyorsa başlat
        if (damageCoroutine == null)
        {
            damageCoroutine = StartCoroutine(LerpHealth());
        }
    }

    /// <summary>
    /// Tüm istemciler: Biriken hasarı sağlık çubuğuna animasyonla uygular.
    /// </summary>
    private IEnumerator LerpHealth()
    {
        while (accumulatedDamage > 0)
        {
            float elapsedTime = 0;
            float initialHealth = currentHealth;

            // Hedef sağlık, biriken hasar kadar azalır
            targetHealth -= accumulatedDamage;
            accumulatedDamage = 0; // Biriken hasar sıfırlanır

            // Eğer sağlık 0 (veya altı) olduysa
            if (targetHealth <= 0)
            {
                targetHealth = 0;
                // Sadece Master Client "gerçek ölümü" tetikler
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC(nameof(RPC_HandleDeath), RpcTarget.All);
                }
                break; // Lerp'i de sonlandır
            }

            // Lerp animasyonu
            while (elapsedTime < damageLerpDuration)
            {
                currentHealth = Mathf.Lerp(initialHealth, targetHealth, elapsedTime / damageLerpDuration);
                UpdateHealthUI();
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentHealth = targetHealth;
            UpdateHealthUI();
        }

        damageCoroutine = null;
    }

    /// <summary>
    /// Tüm istemcilerde ölüm animasyonunu oynatır ve Destroy işlemini yapar.
    /// </summary>
    [PunRPC]
    private void RPC_HandleDeath()
    {
        if (animator != null)
        {
            animator.SetTrigger("isDead");
        }

        // Bu obje kuleyse 1 saniye sonra, minyon vs. ise 3 saniye sonra yok ediyoruz
        if (gameObject.CompareTag("EnemyTurret"))
        {
            Destroy(gameObject, 1f);
        }
        else
        {
            Destroy(gameObject, 3f);
        }
    }

    private void UpdateHealthUI()
    {
        healthUII.Update3DSlider(currentHealth);
    }
}
