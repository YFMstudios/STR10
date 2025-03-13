using UnityEngine;

public class SecondScreenController : MonoBehaviour
{
    public void MuteMusic()
    {
        if (Audio.Instance != null)
        {
            Debug.Log("Sesi kapatma işlemi başlıyor.");
            Audio.Instance.MuteSound(); // Sesi kapat
        }
        else
        {
            Debug.LogError("Audio.Instance bulunamadı!");
        }
    }

    public void UnmuteMusic()
    {
        if (Audio.Instance != null)
        {
            Debug.Log("Sesi açma işlemi başlıyor.");
            Audio.Instance.UnmuteSound(); // Sesi aç
        }
        else
        {
            Debug.LogError("Audio.Instance bulunamadı!");
        }
    }
}
