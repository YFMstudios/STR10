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

    private void Awake()
    {
        healthUI = GetComponent<HealthUI>();
        currentHealth = health;
        targetHealth = health;

        if (healthUI != null)
        {
            healthUI.Start3DSlider(health);
            healthUI.Update2DSlider(health, currentHealth);
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

    // 🔽 BURADA photonView.IsMine KOYMA! HERKES ÇALIŞTIRMALI
    if (damageCoroutine == null && gameObject.activeInHierarchy)
    {
        damageCoroutine = StartCoroutine(LerpHealth());
    }
}


    private void CheckIfCharacterDead()
    {
        Debug.Log($"{gameObject.name} öldü!");
        
        // WarController'a haber ver
        if (CompareTag("Player"))
        {
            if (WarController.Instance != null)
                WarController.Instance.playerOlduMu = true;
        }

        if (healthUI != null)
        {
            healthUI.Update2DSlider(health, 0);
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
        gameObject.SetActive(false); // BattleScenePlayerSpawner buradan tekrar doğuracak
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
    if (healthUI == null) return;

    // Bu oyuncunun kendisi mi?
    if ((CompareTag("Player") || CompareTag("Enemy")) && photonView.IsMine)
    {
        healthUI.Update2DSlider(health, currentHealth);
    }

    // 3D bar herkes için güncellenir
    healthUI.Update3DSlider(currentHealth);
}


    public void ResetHealthToFull()
    {
        currentHealth = health;
        targetHealth = health;

        if (healthUI != null)
        {
            healthUI.Update2DSlider(health, currentHealth);
            healthUI.Update3DSlider(currentHealth);
        }
    }

    // Minyonların hedef alabilmesi için dışarıdan kontrol imkanı
    public bool IsDead()
    {
        return targetHealth <= 0;
    }
}
