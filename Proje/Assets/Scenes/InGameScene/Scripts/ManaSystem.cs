using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class ManaSystem : MonoBehaviourPun, IPunObservable
{
    [Header("Mana Stats")]
    public float maxMana = 100f;         // Maksimum mana
    public float startingMana = 100f;    // Başlangıç manası
    public float manaRegenRate = 5f;     // Saniyelik mana yenileme hızı

    [Header("UI References")]
    public Slider manaBar2d;     // 2D UI için mana çubuğu (yalnızca local)
    public Slider manaBar3d;     // 3D UI için mana çubuğu (herkes görebilir)
    public Text manaText2d;      // 2D UI için mana metni (yalnızca local)

    private float currentMana;   // Şu anki mana

    private void Start()
    {
        // Sadece local player “startingMana” ayarlayacak.
        // Remote oyuncular OnPhotonSerializeView’den değer alacak.
        if (photonView.IsMine)
        {
            currentMana = startingMana;
        }

        UpdateManaUI();
    }

    private void Update()
    {
        // Mana yenileme işlemini sadece local player yapar
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

    /// <summary>
    /// Yetenek maliyetine yetecek mana var mı?
    /// Bu kontrol de local player tarafından yapılır.
    /// </summary>
    public bool CanAffordAbility(float abilityCost)
    {
        return currentMana >= abilityCost;
    }

    /// <summary>
    /// Bir yetenek kullandığımızda local player mana düşürüyor.
    /// </summary>
    public void UseAbility(float abilityCost)
    {
        if (!photonView.IsMine) return; // Sadece local karakter mana harcar

        currentMana -= abilityCost;
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);

        UpdateManaUI();
    }

    /// <summary>
    /// 2D/3D mana barlarını günceller.
    /// 2D bar sadece local player için, 3D bar ise herkes için.
    /// </summary>
    private void UpdateManaUI()
    {
        // 3D bar (herkes görsün)
        if (manaBar3d != null)
        {
            manaBar3d.value = currentMana / maxMana;
        }

        // Sadece local player ise 2D barı güncelle
        if (photonView.IsMine && gameObject.CompareTag("Player"))
        {
            if (manaBar2d != null)
            {
                manaBar2d.value = currentMana / maxMana;
            }
            if (manaText2d != null)
            {
                manaText2d.text = Mathf.RoundToInt(currentMana) + " / " + maxMana;
            }
        }
    }

    /// <summary>
    /// Photon'un serialize metodu: Her karede veri senkronu yaparız.
    /// Local player “currentMana” bilgisini yazar, remote oyuncular okur.
    /// </summary>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // Eğer local player isek, manamızı gönderiyoruz
        {
            stream.SendNext(currentMana);
        }
        else // Remote player isek, mana değerini alıyoruz
        {
            currentMana = (float)stream.ReceiveNext();
            UpdateManaUI();
        }
    }
}
