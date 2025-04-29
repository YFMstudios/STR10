using System.Collections;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Stats : MonoBehaviourPun
{
    [Header("Base Stats")]
    public float health;
    public float damage;
    public float attackSpeed;

    public float damageLerpDuration;
    private float currentHealth;
    private float targetHealth;
    private Coroutine damageCoroutine;

    private HealthUI healthUI;
    private Health3DBarUpdater health3DUpdater;
    private BattleScenePlayerSpawner spawner;

    // Ölüm işlemi sırasında flag
    private bool isDying = false;

    private void Awake()
    {
        healthUI = GetComponent<HealthUI>();
        health3DUpdater = GetComponent<Health3DBarUpdater>();

        // Spawner referansını Start'ta alacağız (daha güvenli)
        currentHealth = health;
        targetHealth = health;
    }

    private void Start()
    {
        // Spawner'ı Start'ta al (tüm objeler oluşturulduktan sonra)
        spawner = FindObjectOfType<BattleScenePlayerSpawner>();

        // Health bar'ları güncelle
        if (health3DUpdater != null)
        {
            health3DUpdater.SetHealth(currentHealth, health); // 3D bar'ı başlat
        }

        if (healthUI != null && photonView.IsMine)
        {
            healthUI.Update2DSlider(health, currentHealth); // sadece kendi ekranında 2D
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (!photonView.IsMine) return;
        photonView.RPC(nameof(RPC_ApplyDamage), RpcTarget.All, damageAmount);
    }

    public void TakeDamage(GameObject source, float damageAmount)
    {
        photonView.RPC(nameof(RPC_ApplyDamage), RpcTarget.All, damageAmount);
    }

    [PunRPC]
    private void RPC_ApplyDamage(float damageAmount)
    {
        // Eğer zaten ölüyorsa veya aktif değilse, hasar uygulanmaz
        if (!gameObject.activeInHierarchy || isDying)
        {
            Debug.Log($"[Stats] {gameObject.name} hasar almadı: aktif değil veya zaten ölüyor");
            return;
        }

        targetHealth -= damageAmount;
        Debug.Log($"[Stats] {gameObject.name} hasar aldı: {damageAmount}, yeni sağlık: {targetHealth}/{health}");

        if (targetHealth <= 0)
        {
            targetHealth = 0;

            // isDying flag'ini true olarak ayarla
            isDying = true;

            if (CompareTag("Player") || CompareTag("Enemy"))
            {
                HandleCharacterDeath();
            }
            else if (CompareTag("EnemyMinion") || CompareTag("EnemyTurret"))
            {
                var handler = GetComponent<EnemyDeathHandler>();
                if (handler != null)
                {
                    handler.Die();
                }
                else
                {
                    // Direkt deaktivasyon çağrılabilir
                    gameObject.SetActive(false);
                }
            }
        }
        else
        {
            // Sadece aktifse ve ölmek üzere değilse hasar animasyonu göster
            if (damageCoroutine == null && gameObject.activeInHierarchy && !isDying)
            {
                damageCoroutine = StartCoroutine(LerpHealth());
            }
        }
    }

    // Ölüm mantığını ayrı bir metoda taşıyoruz
    private void HandleCharacterDeath()
    {
        Debug.Log($"[Stats] {gameObject.name} öldü!");

        if (CompareTag("Player") && WarController.Instance != null)
        {
            WarController.Instance.playerOlduMu = true;
        }

        if (healthUI != null && photonView.IsMine)
        {
            healthUI.Update2DSlider(health, 0);
        }

        if (health3DUpdater != null)
        {
            health3DUpdater.SetHealth(0, health);
        }

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }

        // Spawn Manager'a bildir (sadece owner için)
        if (spawner != null && photonView.IsMine)
        {
            string role = CompareTag("Player") ? "attacker" : "defender";
            Debug.Log($"[Stats] {gameObject.name} ölümü spawner'a bildiriliyor, role: {role}");
            spawner.NotifyCharacterDied(role);
        }
        else
        {
            Debug.LogWarning($"[Stats] {gameObject.name} ölümü bildirilemedi - spawner: {(spawner == null ? "null" : "not null")}, isMine: {photonView.IsMine}");
        }

        // Objeyi deaktif etme - artık spawner kontrol ediyor
        // İşlem spawner tarafından yapılacak
    }

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

    private void UpdateHealthUI()
    {
        if (photonView.IsMine && healthUI != null)
            healthUI.Update2DSlider(health, currentHealth);

        if (health3DUpdater != null)
            health3DUpdater.SetHealth(currentHealth, health);
    }

    public void ResetHealthToFull()
    {
        // isDying durumunu sıfırla
        isDying = false;

        currentHealth = health;
        targetHealth = health;

        Debug.Log($"[Stats] {gameObject.name} can yenilendi: {currentHealth}/{health}");

        if (photonView.IsMine && healthUI != null)
            healthUI.Update2DSlider(health, currentHealth);

        if (health3DUpdater != null)
            health3DUpdater.SetHealth(currentHealth, health);
    }

    public bool IsDead()
    {
        return targetHealth <= 0;
    }
}