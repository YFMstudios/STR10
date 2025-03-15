using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;    // MainMenuVoice AudioSource'u buraya bağlayacağız
    public GameObject voiceOffButton;  // VoiceOffButton referansı
    public GameObject voiceOnButton;   // VoiceOnButton referansı
    private bool isMuted = false;      // Sesin kapalı olup olmadığını kontrol eder
    private float previousVolume;      // Ses kapatılmadan önceki değeri saklar

    void Start()
    {
        // Sesin kapatılmadan önceki varsayılan değeri sakla
        previousVolume = audioSource.volume;

        // İlk başta butonların her ikisini de aktif tutalım, ses açık olsun
        voiceOffButton.SetActive(true);
        voiceOnButton.SetActive(true);
    }

    // Ses kapatma fonksiyonu (Voice Off butonuna bağlanacak)
    public void MuteSound()
    {
        if (audioSource != null)
        {
            previousVolume = audioSource.volume; // Mevcut sesi sakla
            audioSource.volume = 0; // Sesi kapat
            isMuted = true;
        }
        else
        {
            Debug.LogWarning("AudioSource bulunamadı.");
        }
    }

    // Ses açma fonksiyonu (Voice On butonuna bağlanacak)
    public void UnmuteSound()
    {
        if (audioSource != null)
        {
            audioSource.volume = previousVolume; // Önceki sesi geri getir
            isMuted = false;
        }
        else
        {
            Debug.LogWarning("AudioSource bulunamadı.");
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // MainMenuVoice objesinin yok edilmesini engeller
    }
}
