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
        spawner = FindObjectOfType<BattleScenePlayerSpawner>();

        currentHealth = health;
        targetHealth = health;

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
        if (!gameObject.activeInHierarchy || isDying) return;

        targetHealth -= damageAmount;

        if (targetHealth <= 0)
        {
            targetHealth = 0;

            if (CompareTag("Player") || CompareTag("Enemy"))
            {
                // isDying flag'ini true olarak ayarla
                isDying = true;
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
                    // Coroutine kullanmadan doğrudan deaktif edebiliriz ya da güvenli şekilde Coroutine başlatabiliriz
                    if (gameObject.activeInHierarchy)
                    {
                        StartCoroutine(DeactivateAfterDelay());
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }

        // Sadece aktifse ve ölmek üzere değilse hasar animasyonu göster
        if (damageCoroutine == null && gameObject.activeInHierarchy && !isDying)
        {
            damageCoroutine = StartCoroutine(LerpHealth());
        }
    }

    // Ölüm mantığını ayrı bir metoda taşıyoruz
    private void HandleCharacterDeath()
    {
        Debug.Log($"{gameObject.name} öldü!");

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

        // Önce BattleScenePlayerSpawner'a bildiriyoruz - obje hala aktifken
        if (spawner != null && photonView.IsMine)
        {
            string role = CompareTag("Player") ? "attacker" : "defender";
            spawner.NotifyCharacterDied(role);

            // Deactivate After spawner notification
            // NOT: Burada 0 saniye bekleyerek birkaç frame geçmesini sağlıyoruz
            // Bu, spawner'ın RPC işlemlerini tamamlaması için zaman tanır
            StartCoroutine(DeactivateAfterDelay(0.5f));
        }
        else
        {
            // Eğer spawner yoksa veya photonView.IsMine değilse, normal olarak deaktif et
            StartCoroutine(DeactivateAfterDelay());
        }
    }

    // Parametreli versiyonu da ekleyelim
    private IEnumerator DeactivateAfterDelay(float delay = 3f)
    {
        yield return new WaitForSeconds(delay);

        // isDying durumunu sıfırla, böylece respawn olduğunda bu kontroller çalışabilir
        isDying = false;

        // Eğer hala aktifse deaktif et
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
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