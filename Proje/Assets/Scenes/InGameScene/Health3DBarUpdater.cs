using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Health3DBarUpdater : MonoBehaviourPun, IPunObservable
{
    [Header("3D Health Bar")]
    public Slider health3DSlider;

    private float currentHealth = 100f;
    private float maxHealth = 100f;

    private void Start()
    {
        // 3D bar HERKES tarafından görülecek, gizleme yok
        if (health3DSlider != null)
        {
            health3DSlider.maxValue = maxHealth;
            health3DSlider.value = currentHealth;
        }
    }

    public void SetHealth(float current, float max)
    {
        currentHealth = current;
        maxHealth = max;
        UpdateHealth3DBar();
    }

    private void UpdateHealth3DBar()
    {
        if (health3DSlider != null)
        {
            health3DSlider.maxValue = maxHealth;
            health3DSlider.value = currentHealth;
        }
    }

    // Photon senkronizasyonu (herkes görecek)
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
            stream.SendNext(maxHealth);
        }
        else
        {
            currentHealth = (float)stream.ReceiveNext();
            maxHealth = (float)stream.ReceiveNext();
            UpdateHealth3DBar();
        }
    }
}
