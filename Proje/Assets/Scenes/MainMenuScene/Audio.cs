using UnityEngine;

public class Audio : MonoBehaviour
{
    private static Audio instance;

    public static Audio Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Audio instance'ına erişilemiyor. AudioManager sahnede mevcut mu?");
            }
            return instance;
        }
    }

    private AudioSource audioSource;

    void Awake()
    {
        // Eğer başka bir instance yoksa bu instance'i kullan
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Root GameObject üzerinde çalışır
            audioSource = GetComponent<AudioSource>(); // AudioSource'u al

            if (audioSource == null)
            {
                Debug.LogError("AudioSource bulunamadı!");
            }
        }
        else
        {
            Destroy(gameObject);  // Eğer başka bir instance varsa bu objeyi yok et
        }
    }

    // Ses açma fonksiyonu
    public void UnmuteSound()
    {
        if (audioSource != null)
        {
            audioSource.volume = 1; // Ses seviyesini aç
            Debug.Log("Ses açıldı.");
        }
    }

    // Ses kapama fonksiyonu
    public void MuteSound()
    {
        if (audioSource != null)
        {
            audioSource.volume = 0; // Sesi kapat
            Debug.Log("Ses kapatıldı.");
        }
    }
}
