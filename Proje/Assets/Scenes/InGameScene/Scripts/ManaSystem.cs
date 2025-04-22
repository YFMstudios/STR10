using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class ManaSystem : MonoBehaviourPun, IPunObservable
{
    [Header("Mana Stats")]
    public float maxMana = 100f;
    public float startingMana = 100f;
    public float manaRegenRate = 5f;

    [Header("UI References")]
    public Slider manaBar2d;
    public Text manaText2d;

    private Mana3DBarUpdater mana3DUpdater;
    private float currentMana;

    private void Start()
    {
        mana3DUpdater = GetComponent<Mana3DBarUpdater>();

        if (photonView.IsMine)
            currentMana = startingMana;

        if (!CompareTag("Player") && !CompareTag("Enemy"))
        {
            if (manaBar2d != null) manaBar2d.gameObject.SetActive(false);
            if (manaText2d != null) manaText2d.gameObject.SetActive(false);
        }
        else if (photonView.IsMine && manaBar2d != null)
        {
            manaBar2d.maxValue = 1f;
            manaBar2d.value = currentMana / maxMana;
        }

        UpdateManaUI();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            RegenerateMana();
        }
    }

    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
            UpdateManaUI();
        }
    }

    public bool CanAffordAbility(float abilityCost)
    {
        return currentMana >= abilityCost;
    }

    public void UseAbility(float abilityCost)
    {
        if (!photonView.IsMine) return;

        currentMana -= abilityCost;
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
        UpdateManaUI();
    }

    private void UpdateManaUI()
    {
        // ✅ 3D bar HERKES için güncellenir
        if (mana3DUpdater != null)
            mana3DUpdater.SetMana(currentMana, maxMana);

        // ✅ 2D bar SADECE yerel oyuncuda çalışır
        if (photonView.IsMine)
        {
            if (manaBar2d != null)
                manaBar2d.value = currentMana / maxMana;

            if (manaText2d != null)
                manaText2d.text = $"{Mathf.RoundToInt(currentMana)} / {maxMana}";
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentMana);
        }
        else
        {
            currentMana = (float)stream.ReceiveNext();
            UpdateManaUI();
        }
    }
}
