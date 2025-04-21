// TimeController.cs
using UnityEngine;

[CreateAssetMenu(fileName = "TimeController", menuName = "ScriptableObjects/Time Controller")]
public class TimeController : ScriptableObject
{
    [Tooltip("Toplam geçen süre (saniye cinsinden)")]
    public float elapsedTime = 0f;
    
    [Tooltip("Timer'ın şu anda çalışıp çalışmadığı")]
    public bool isTimerRunning = false;
    
    // ScriptableObject değerlerini sıfırlamak için metot
    // (Oyun başlatıldığında çağrılabilir)
    public void ResetTimer()
    {
        elapsedTime = 0f;
        isTimerRunning = false;
    }
}
