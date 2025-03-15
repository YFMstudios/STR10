using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyPlayerHighlightManager : MonoBehaviour
{
    private Transform highlightedObj;    // Üzerine gelinen objeyi temsil eden transform
    private Transform selectedObj;       // Seçilen objeyi temsil eden transform
    public LayerMask selectableLayer;    // Hover ve seçim işlemlerinin yapılacağı katman maskesi

    private Outline highlightOutline;    // Objeye eklenen kontur bileşeni
    private RaycastHit hit;              // Raycast sonucunu tutacak değişken

    // Update is called once per frame
    void Update()
    {
        HoverHighlight();   // Her güncelleme çerçevesinde hover (üzerine gelme) işlemi yap
    }

    // Fare imleci ile üzerine gelinen objeyi vurgulayan fonksiyon
    public void HoverHighlight()
    {
        if (highlightedObj != null)
        {
            if (highlightOutline != null)
            {
                highlightOutline.enabled = false; // Eğer önceki vurgulanan obje varsa konturunu kapat
            }
            highlightedObj = null; // Önceki objeyi temizle
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Fare imlecinin dünya koordinatlarındaki pozisyonunu al

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, selectableLayer))
        {
            highlightedObj = hit.transform; // Çarpılan objeyi highlightedObj olarak ayarla

            if ((highlightedObj.CompareTag("Player") || highlightedObj.CompareTag("AllyMinion") || highlightedObj.CompareTag("AllyTurret")) && highlightedObj != selectedObj)
            {
                highlightOutline = highlightedObj.GetComponent<Outline>(); // Kontur bileşenini al
                if (highlightOutline != null) // Kontur bileşeni varsa
                {
                    highlightOutline.enabled = true; // Konturu aktif et
                }
            }
            else
            {
                highlightedObj = null; // Eğer obje uygun değilse null olarak ayarla
            }
        }
    }


    // Seçilen objeyi vurgulayan fonksiyon
    public void SelectedHighlight()
    {
        if (highlightedObj != null)
        {
            if (highlightedObj.CompareTag("Player") || highlightedObj.CompareTag("AllyMinion") || highlightedObj.CompareTag("AllyTurret"))
            {
                if (selectedObj != null)
                {
                    selectedObj.GetComponent<Outline>().enabled = false;  // Önceki seçili objenin konturunu kapat
                }

                selectedObj = hit.transform;  // Yeni seçilen objeyi güncelle
                selectedObj.GetComponent<Outline>().enabled = true;  // Yeni seçilen objenin konturunu aç

                highlightOutline.enabled = true;  // Vurgu konturunu aç
                highlightedObj = null;  // Vurgulanmış objeyi temizle
            }
        }
    }

    // Vurgulamayı kaldıran fonksiyon
    public void DeselectHighlight()
    {
        if (selectedObj != null)
        {
            selectedObj.GetComponent<Outline>().enabled = false;  // Seçili objenin konturunu kapat
            selectedObj = null;  // Seçili objeyi temizle
        }
    }
}
