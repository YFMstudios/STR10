using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Mana3DBarUpdater : MonoBehaviourPun, IPunObservable
{
    [Header("3D Mana Bar")]
    public Slider mana3DSlider;

    private float currentMana = 100f;
    private float maxMana = 100f;

    private void Start()
    {
        // 3D bar HERKES tarafından görüleceği için gizleme yok
        if (mana3DSlider != null)
        {
            mana3DSlider.maxValue = maxMana;
            mana3DSlider.value = currentMana;
        }
    }

    public void SetMana(float current, float max)
    {
        currentMana = current;
        maxMana = max;
        UpdateMana3DBar();
    }

    private void UpdateMana3DBar()
    {
        if (mana3DSlider != null)
        {
            mana3DSlider.maxValue = maxMana;
            mana3DSlider.value = currentMana;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentMana);
            stream.SendNext(maxMana);
        }
        else
        {
            currentMana = (float)stream.ReceiveNext();
            maxMana = (float)stream.ReceiveNext();
            UpdateMana3DBar();
        }
    }
}
