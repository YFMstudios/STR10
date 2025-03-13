using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class ExperienceSystem : MonoBehaviourPun, IPunObservable
{
    [Header("Experience Stats")]
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToLevelUp = 100;
    public int expIncreaseFactor = 2;

    [Header("UI References")]
    public Slider expBarSlider;  // Deneyim çubuğu (3D veya 2D olabilir)
    public Text levelText2D;     // 2D seviye metni (yalnızca local)
    public Text levelText3D;     // 3D seviye metni (herkes görsün)

    void Update()
    {
        // Her çerçevede UI güncellemesi yapalım (veriler OnPhotonSerializeView ile senkron oluyor)
        UpdateUI();
    }

    /// <summary>
    /// Düşmanı yenince, istediğimiz XP miktarını kazandırır.
    /// </summary>
    public void GainExperienceFromEnemy(int amount)
    {
        // Sadece kendi karakterimiz isek, local xp artırma işlemini başlatırız
        if (!photonView.IsMine) 
            return;

        // Tüm istemcilerde XP güncellensin diye RPC iletmeyi tercih edebilirsiniz. 
        // Veya sadece localde XP’yi arttırıp OnPhotonSerializeView ile senkron da edebilirsiniz.
        // Burada örnek olarak direkt localde arttırıp, OnPhotonSerializeView ile paylaşacağız:
        GainExperience(amount);
    }

    /// <summary>
    /// Asıl XP arttırma mantığı (local).
    /// </summary>
    private void GainExperience(int amount)
    {
        currentExp += amount;

        // Seviye atlama kontrolü
        while (currentExp >= expToLevelUp)
        {
            LevelUp();
        }
        // UpdateUI();  // Local UI anında güncellenir, 
        // ancak OnPhotonSerializeView ile 3D kopyalar da senkron olacak.
    }

    /// <summary>
    /// Seviye atlama mantığı
    /// </summary>
    private void LevelUp()
    {
        currentLevel++;
        currentExp -= expToLevelUp;
        expToLevelUp *= expIncreaseFactor;
    }

    /// <summary>
    /// Her çerçevede slider ve seviye metnini günceller.
    /// (currentExp, currentLevel vb. OnPhotonSerializeView ile senkronize edilir.)
    /// </summary>
    private void UpdateUI()
    {
        // Experience Bar
        if (expBarSlider != null)
        {
            expBarSlider.maxValue = expToLevelUp;
            expBarSlider.value = currentExp;
        }

        // 2D Level text (yalnızca local player)
        if (levelText2D != null && photonView.IsMine)
        {
            levelText2D.text = currentLevel.ToString();
        }

        // 3D Level text (herkes görebilir)
        // Eğer sadece local oyuncu üzerinde 3D text görmek istiyorsanız, 
        // burada da photonView.IsMine kontrolü yapabilirsiniz.
        if (levelText3D != null)
        {
            levelText3D.text = currentLevel.ToString();
        }
    }

    /// <summary>
    /// OnPhotonSerializeView ile currentExp, currentLevel, expToLevelUp değerlerini 
    /// ağ üzerinde senkronize ediyoruz.
    /// Local player yazıyor (IsWriting), diğerleri okuyor (IsReading).
    /// </summary>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) 
        {
            // Local player verileri gönderir
            stream.SendNext(currentLevel);
            stream.SendNext(currentExp);
            stream.SendNext(expToLevelUp);
        }
        else
        {
            // Remote player verileri alır
            currentLevel = (int)stream.ReceiveNext();
            currentExp = (int)stream.ReceiveNext();
            expToLevelUp = (int)stream.ReceiveNext();
        }
    }
}
