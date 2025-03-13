using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro bileşeni için gerekli

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText; // TextMeshProUGUI doğru sınıf
    private float elapsedTime; // Geçen süre
    private bool isTimerRunning = false; // Zamanlayıcı kontrolü

    void Start()
    {
        // Zamanlayıcıyı başlat
        elapsedTime = 0f;
        isTimerRunning = true;

        // UI'ı hemen güncelle
        timerText.text = "00:00:00"; 
    }

    void Update()
    {
        if (isTimerRunning)
        {
            // Geçen süreyi arttır
            elapsedTime += Time.deltaTime;

            // Saniye, dakika ve saat hesaplamaları
            int hours = Mathf.FloorToInt(elapsedTime / 3600); // 1 saat = 3600 saniye
            int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60); // 1 dakika = 60 saniye
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            // TextMeshPro UI'ını güncelle
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
    }

    public void StopTimer()
    {
        // Zamanlayıcıyı durdur
        isTimerRunning = false;

        // Zamanı sıfırla ve UI'ı güncelle
        elapsedTime = 0f;
        timerText.text = "00:00:00";
    }
}
