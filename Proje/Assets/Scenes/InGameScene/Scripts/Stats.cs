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

    private void Awake()
    {
        healthUI = GetComponent<HealthUI>();
        health3DUpdater = GetComponent<Health3DBarUpdater>();

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
        if (!gameObject.activeInHierarchy) return;

        targetHealth -= damageAmount;

        if (targetHealth <= 0)
        {
            targetHealth = 0;

            if (CompareTag("Player") || CompareTag("Enemy"))
            {
                CheckIfCharacterDead();
            }
            else if (CompareTag("EnemyMinion") || CompareTag("EnemyTurret"))
            {
                var handler = GetComponent<EnemyDeathHandler>();
                if (handler != null) handler.Die();
                else StartCoroutine(DeactivateAfterDelay());
            }
        }

        if (damageCoroutine == null && gameObject.activeInHierarchy)
        {
            damageCoroutine = StartCoroutine(LerpHealth());
        }
    }

    private void CheckIfCharacterDead()
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

        StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
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
