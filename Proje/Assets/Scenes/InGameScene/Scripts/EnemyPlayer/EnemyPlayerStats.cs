using System.Collections;
using UnityEngine;

public class EnemyPlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float health;            // Can değeri
    public float damage;            // Verilen hasar miktarı
    public float attackSpeed;       // Saldırı hızı

    // Health Slider Variables
    public float damageLerpDuration;    // Hasar geçiş süresi
    private float currentHealth;        // Mevcut can miktarı
    private float targetHealth;         // Hedeflenen can miktarı
    private Coroutine damageCoroutine;  // Hasar geçiş Coroutine'u

    private EnemyPlayerHealthUI healthUI;  // Can UI yöneticisi

    private void Awake()
    {
        healthUI = GetComponent<EnemyPlayerHealthUI>();  // HealthUI bileşenini alır
        currentHealth = health;               // Mevcut canı başlatır
        targetHealth = health;                // Hedef canı başlatır

        healthUI.start3DSlider(health);       // 3D slider'ı başlatır
        healthUI.Update2DSlider(health, currentHealth);  // 2D slider'ı günceller
    }

    // Hasar alma işlemi
    public void TakeDamage(GameObject source, float damageAmount)
    {
        targetHealth -= damageAmount;  // Hedef canı hasar miktarı kadar azaltır

        if (targetHealth <= 0)  // Can sıfıra ulaşırsa
        {
            targetHealth = 0;

            if (gameObject.CompareTag("EnemyPlayer"))
            {
                CheckIfPlayerDead();  // Oyuncu ölümü kontrolü
            }
            else if (gameObject.CompareTag("Player") || gameObject.CompareTag("AllyMinion") || gameObject.CompareTag("AllyTurret"))
            {
                // Düşman veya turret öldüğünde
                var enemyDeathHandler = GetComponent<EnemyDeathHandler>();
                if (enemyDeathHandler != null)
                {
                    enemyDeathHandler.Die();  // Ölüm işlemi düşman yönetim scriptinde yapılır
                }
                else
                {
                    Destroy(gameObject);  // Eğer EnemyDeathHandler yoksa direkt düşmanı veya turret'i yok et
                }
            }
        }

        // Hasar geçiş Coroutine'u başlatır
        if (damageCoroutine == null)
        {
            damageCoroutine = StartCoroutine(LerpHealth());
        }
    }

    // Oyuncunun öldüğünü kontrol eden fonksiyon
    private void CheckIfPlayerDead()
    {
        Debug.Log($"{gameObject.name} öldü!");  // Ölüm mesajı
        healthUI.Update2DSlider(health, 0);     // Canı sıfıra ayarlar
        Destroy(gameObject);  // Oyuncuyu yok eder
    }

    // Hasar geçişini yapan Coroutine
    private IEnumerator LerpHealth()
    {
        float elapsedTime = 0;
        float initialHealth = currentHealth;
        float target = targetHealth;

        // Belirlenen sürede hasar geçişini yapar
        while (elapsedTime < damageLerpDuration)
        {
            currentHealth = Mathf.Lerp(initialHealth, target, elapsedTime / damageLerpDuration);  // Can geçişini yapar
            UpdateHealthUI();   // UI'yi günceller
            elapsedTime += Time.deltaTime;
            yield return null;  // Sonraki frame'i bekler
        }

        currentHealth = target;  // Canı hedefe ayarlar
        UpdateHealthUI();        // UI'yi günceller
        damageCoroutine = null;  // Coroutine'u sıfırlar
    }

    // UI güncelleme işlemi
    private void UpdateHealthUI()
    {
        healthUI.Update2DSlider(health, currentHealth);  // 2D slider'ı günceller
        healthUI.update3DSlider(currentHealth);          // 3D slider'ı günceller
    }
}
