using System.Collections;
using UnityEngine;
using Photon.Pun;

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
    private Animator animator;

    [Header("ScriptableObject")]
    public ProgressData progressData;

    private void Awake()
    {
        healthUII = GetComponent<HealthUII>();
        currentHealth = health;
        targetHealth = health;

        if (healthUII != null)
            healthUII.Start3DSlider(health);

        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damageAmount)
    {
        if (!PhotonNetwork.IsMasterClient || !gameObject.activeInHierarchy)
            return;

        photonView.RPC(nameof(RPC_TakeDamageAll), RpcTarget.All, damageAmount);
    }

    [PunRPC]
    private void RPC_TakeDamageAll(float damageAmount)
    {
        if (!gameObject.activeInHierarchy) return;

        accumulatedDamage += damageAmount;

        if (damageCoroutine == null)
            damageCoroutine = StartCoroutine(LerpHealth());
    }

    private IEnumerator LerpHealth()
    {
        while (accumulatedDamage > 0)
        {
            float elapsedTime = 0;
            float initialHealth = currentHealth;

            targetHealth -= accumulatedDamage;
            accumulatedDamage = 0;

            if (targetHealth <= 0)
            {
                targetHealth = 0;
                if (PhotonNetwork.IsMasterClient)
                    photonView.RPC(nameof(RPC_HandleDeath), RpcTarget.All);
                break;
            }

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

    [PunRPC]
    private void RPC_HandleDeath()
    {
        if (animator != null)
            animator.SetTrigger("isDead");

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }

        float delay = gameObject.CompareTag("EnemyTurret") ? 1f : 3f;
        Invoke(nameof(DeactivateObject), delay);
    }

    private void DeactivateObject()
    {
        gameObject.SetActive(false);
    }

    private void UpdateHealthUI()
    {
        if (healthUII != null)
            healthUII.Update3DSlider(currentHealth);
    }
}
