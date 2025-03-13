using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Photon eklentileri
using Photon.Pun;

public class HighlightManager : MonoBehaviourPun
{
    private Transform highlightedObj;    // Üzerine gelinen objeyi temsil eden transform
    private Transform selectedObj;       // Seçilen objeyi temsil eden transform
    public LayerMask selectableLayer;    // Hover ve seçim işlemlerinin yapılacağı katman maskesi

    private Outline highlightOutline;    // Objeye eklenen kontur bileşeni
    private RaycastHit hit;              // Raycast sonucunu tutacak değişken

    void Update()
    {
        // Sadece bu karakterin sahibi isek mouse üzerinden highlight yapalım
        if (!photonView.IsMine)
        {
            return; 
        }

        HoverHighlight(); // Her karede hover (üzerine gelme) işlemi yap
    }

    // Fare imleci ile üzerine gelinen objeyi vurgulayan fonksiyon
    public void HoverHighlight()
    {
        // Eski highlighted objeyi temizle
        if (highlightedObj != null)
        {
            if (highlightOutline != null)
            {
                highlightOutline.enabled = false; // Önceki vurgulanan objenin konturunu kapat
            }
            highlightedObj = null;
        }

        // Fare imlecinden dünya koordinatlarında bir ray oluştur
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // UI üzerinde misin kontrolü (EventSystem) + katman maskesi
        // Not: Burada Physics.Raycast(...) için en fazla mesafe eklemek istersen, son param olarak Mathf.Infinity kullanabilirsin.
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, Mathf.Infinity, selectableLayer))
        {
            highlightedObj = hit.transform; // Çarpılan objeyi highlightedObj olarak ayarla

            // Eğer düşman türünde ve şu an seçili objeden farklıysa
            if ((highlightedObj.CompareTag("Enemy") || 
                 highlightedObj.CompareTag("EnemyMinion") || 
                 highlightedObj.CompareTag("EnemyTurret")) 
                && highlightedObj != selectedObj)
            {
                highlightOutline = highlightedObj.GetComponent<Outline>(); 
                if (highlightOutline != null)
                {
                    highlightOutline.enabled = true; // Konturu aktif et
                }
            }
            else
            {
                highlightedObj = null;
            }
        }
    }

    // Seçilen objeyi vurgulayan fonksiyon
    public void SelectedHighlight()
    {
        // Eğer şu an bir objeyi 'highlight' ediyorsak
        if (highlightedObj != null)
        {
            if (highlightedObj.CompareTag("Enemy") || 
                highlightedObj.CompareTag("EnemyMinion") || 
                highlightedObj.CompareTag("EnemyTurret"))
            {
                // Daha önce seçili bir obje varsa konturunu kapat
                if (selectedObj != null && selectedObj.GetComponent<Outline>() != null)
                {
                    selectedObj.GetComponent<Outline>().enabled = false;
                }

                // Yeni seçili objeyi güncelle
                selectedObj = hit.transform;

                // Onun konturunu aktif et
                if (selectedObj.GetComponent<Outline>() != null)
                {
                    selectedObj.GetComponent<Outline>().enabled = true;
                }

                // highlightOutline da aktif kalsın
                if (highlightOutline != null)
                {
                    highlightOutline.enabled = true;
                }

                // Geçici highlight objesini sıfırla (artık selection aşamasına geçti)
                highlightedObj = null;
            }
        }
    }

    // Vurgulamayı kaldıran fonksiyon
    public void DeselectHighlight()
    {
        if (selectedObj != null)
        {
            var outline = selectedObj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
            selectedObj = null;
        }
    }
}
