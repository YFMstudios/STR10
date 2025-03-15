using System.Collections;
using UnityEngine;

public class ObjectiveStats2 : MonoBehaviour
{
    [Header("Base Stats")]
    public float health;             // Objektifin sağlık puanı
    public float damage;             // Objektife verilecek hasar miktarı

    // Sağlık çubuğu değişkenleri
    public float damageLerpDuration; // Hasar geçiş süresi
    private float currentHealth;     // Mevcut sağlık
    private float targetHealth;      // Hedeflenen sağlık
    private Coroutine damageCoroutine; // Hasar geçiş işlemi

    private HealthUII healthUII;     // Sağlık arayüzü referansı

    private void Awake()
    {
        healthUII = GetComponent<HealthUII>(); // Sağlık arayüz bileşenini al
        currentHealth = health;                // Mevcut sağlık başlangıçta hedef sağlığa eşitlenir
        targetHealth = health;                 // Hedef sağlık başlangıçta başlangıç sağlığına eşitlenir

        healthUII.Start3DSlider(health);       // 3D sağlık çubuğunu başlatır
    }

    // Hasar alma fonksiyonu
    public void TakeDamage(float damageAmount)
    {
        targetHealth -= damageAmount; // Hedeflenen sağlık miktarını azalt

        if (targetHealth <= 0)        // Eğer sağlık sıfıra ulaşırsa objektif yok edilir
        {
            targetHealth = 0;          // Sağlık sıfıra düşerse sıfır yap
            HandleDeath();            // Ölüm işlemlerini gerçekleştir
        }
        else if (damageCoroutine == null) // Hasar geçiş animasyonu yoksa başlat
        {
            StartLerpHealth();
        }
    }

    // Ölüm işlemlerini gerçekleştiren fonksiyon
    private void HandleDeath()
    {
        if (gameObject.CompareTag("EnemyTurret")) // Eğer objektif bir düşman kule ise
        {
            Destroy(gameObject);   // Kuleyi yok et
            Application.Quit();    // Oyun kapansın
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Oyun editör modunda çalışıyorsa durdur
#endif
        }
        else
        {
            Destroy(gameObject); // Diğer nesneleri yok et
        }
    }

    // Hasar geçişini başlatan fonksiyon
    private void StartLerpHealth()
    {
        if (damageCoroutine == null)
        {
            damageCoroutine = StartCoroutine(LerpHealth()); // Hasar geçiş animasyonunu başlatır
        }
    }

    // Sağlık geçişi animasyonu için coroutine
    private IEnumerator LerpHealth()
    {
        float elapsedTime = 0;
        float initialHealth = currentHealth;

        while (elapsedTime < damageLerpDuration)
        {
            currentHealth = Mathf.Lerp(initialHealth, targetHealth, elapsedTime / damageLerpDuration); // Sağlık geçişi
            UpdateHealthUI(); // UI'yi günceller
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentHealth = targetHealth; // Sağlık geçişi tamamlandığında mevcut sağlık hedef sağlığa eşitlenir
        UpdateHealthUI();              // UI'yi günceller
        damageCoroutine = null;        // Coroutine sonlanır
    }

    // Sağlık arayüzünü günceller
    private void UpdateHealthUI()
    {
        healthUII.Update3DSlider(currentHealth); // Sağlık arayüzünü günceller
    }
}
