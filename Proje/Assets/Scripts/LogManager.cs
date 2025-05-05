using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LogManager : MonoBehaviour
{
    // Singleton desenini kullanarak her yerden erişim sağlayacağız
    public static LogManager Instance;

    // Log panelindeki TextMeshPro bileşeni
    public TextMeshProUGUI logText;

    // Log paneli GameObject'i
    public GameObject logPanel;

    // Logların ekranda kalma süresi (saniye)
    public float logSuresi = 5f;

    // Aynı logları önlemek için son log mesajı
    private string sonLogMesaji = "";

    // Otomatik temizleme için coroutine
    private Coroutine temizlemeCoroutine;

    private void Awake()
    {
        // Singleton oluşturma
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Panel başlangıçta kapalı olsun
        if (logPanel != null)
        {
            logPanel.SetActive(false);
        }
    }

    // Yeni log ekleme fonksiyonu
    public void LogEkle(string mesaj)
    {
        // Panel yoksa işlem yapma
        if (logPanel == null || logText == null)
            return;

        // Aynı mesaj arka arkaya gelirse tekrar gösterme
        if (mesaj == sonLogMesaji)
            return;

        sonLogMesaji = mesaj;

        // Log panelini göster
        logPanel.SetActive(true);

        // Metni güncelle - tarih olmadan direkt mesajı yazıyoruz
        logText.text = mesaj;

        // Eğer halihazırda bir temizleme coroutine'i çalışıyorsa durdur
        if (temizlemeCoroutine != null)
        {
            StopCoroutine(temizlemeCoroutine);
        }

        // Yeni temizleme coroutine'i başlat
        temizlemeCoroutine = StartCoroutine(OtomatikTemizle());
    }

    // Hata logları için
    public void HataLogEkle(string mesaj)
    {
        LogEkle("<color=red>HATA: " + mesaj + "</color>");
    }

    // Uyarı logları için
    public void UyariLogEkle(string mesaj)
    {
        LogEkle("<color=yellow>UYARI: " + mesaj + "</color>");
    }

    // Başarı logları için (yeşil renk)
    public void BasariLogEkle(string mesaj)
    {
        LogEkle("<color=green>BAŞARI: " + mesaj + "</color>");
    }

    // Logları belirli bir süre sonra otomatik temizleyen fonksiyon
    private IEnumerator OtomatikTemizle()
    {
        // Belirtilen süre kadar bekle
        yield return new WaitForSeconds(logSuresi);

        // Log panelini gizle
        logPanel.SetActive(false);

        // Log metnini temizle
        logText.text = "";

        // Son log mesajını sıfırla (böylece aynı mesaj tekrar gösterilebilir)
        sonLogMesaji = "";

        temizlemeCoroutine = null;
    }
}