using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PanelManager : MonoBehaviour
{
    public GameObject buildPanelPrefab; // Panel Prefab'ý
    public GameObject researchPanelPrefab; // Panel Prefab'ý
    public GameObject soldierCreationPanelPrefab;
    public GameObject SoldierHealPanelPrefab;
    public Transform panelContainer; // Vertical Layout Group'un olduðu GameObject

    private List<GameObject> activePanels = new List<GameObject>();
    private List<GameObject> deactivePanels = new List<GameObject>();
    private Dictionary<string, GameObject> panelDictionary = new Dictionary<string, GameObject>();

    

    // Yeni bir panel oluþtur
    public void CreatePanel(string panelId, string panelText, float buildTime,string processType)
    {
        if(processType == "Building")
        {
            GameObject newPanel = Instantiate(buildPanelPrefab, panelContainer);

            Image img = newPanel.transform.Find("ProcessImage").GetComponent<Image>();
            TextMeshProUGUI tmp = newPanel.transform.Find("BuildNameText").GetComponent<TextMeshProUGUI>();
            Image background = newPanel.transform.Find("Background").GetComponent<Image>();
            Sprite sprite = Resources.Load<Sprite>("Buildings/" + panelText);


            if (background == null)
            {
                Debug.Log("Background bulunamadý");
            }
            else
            {
                Image bar = background.transform.Find("Bar").GetComponent<Image>();

                if (bar == null)
                {
                    Debug.Log("Bar bulunamadý");
                }
                else
                {

                    LeanTween.scaleX(bar.gameObject, 1, buildTime).setOnComplete(() => DestroyPanel(panelId));
                }
            }

            if (img != null && tmp != null)
            {
                img.sprite = sprite;
                tmp.text = panelText;
            }

            if (panelDictionary.ContainsKey(panelId))
            {
                Debug.LogWarning($"Panel ID zaten mevcut: {panelId}");
                Destroy(newPanel); // Ayný ID için birden fazla panel oluþmasýný engelle
                return;
            }

            panelDictionary.Add(panelId, newPanel); // Paneli ID ile iliþkilendir

            // Paneli aktifleþtir/deaktif et
            if (OpenProcessPanelWithAnim.isPanelActive)
            {
                activePanels.Add(newPanel);
                Debug.Log($"Panel '{panelId}' aktif.");
            }
            else
            {
                deactivePanels.Add(newPanel);
                newPanel.SetActive(false);
                Debug.Log($"Panel '{panelId}' pasif.");
            }

            Debug.Log($"Aktif Panel Sayýsý: {activePanels.Count}, Deaktif Panel Sayýsý: {deactivePanels.Count}");
        }
       
        else if(processType == "Researching")
        {
            GameObject newPanel = Instantiate(researchPanelPrefab, panelContainer);

            Image img = newPanel.transform.Find("ProcessImage").GetComponent<Image>();
            TextMeshProUGUI tmp = newPanel.transform.Find("ResearchNameText").GetComponent<TextMeshProUGUI>();
            Image background = newPanel.transform.Find("Background").GetComponent<Image>();
            Sprite sprite = Resources.Load<Sprite>("Researches/" + panelId);

            if (background == null)
            {
                Debug.Log("Background bulunamadý");
            }
            else
            {
                Image bar = background.transform.Find("Bar").GetComponent<Image>();

                if (bar == null)
                {
                    Debug.Log("Bar bulunamadý");
                }
                else
                {

                    LeanTween.scaleX(bar.gameObject, 1, buildTime).setOnComplete(() => DestroyPanel(panelId));
                }
            }

            if (img != null && tmp != null)
            {
                img.sprite = sprite;
                tmp.text = panelText;
            }

            if (panelDictionary.ContainsKey(panelId))
            {
                Debug.LogWarning($"Panel ID zaten mevcut: {panelId}");
                Destroy(newPanel); // Ayný ID için birden fazla panel oluþmasýný engelle
                return;
            }

            panelDictionary.Add(panelId, newPanel); // Paneli ID ile iliþkilendir

            // Paneli aktifleþtir/deaktif et
            if (OpenProcessPanelWithAnim.isPanelActive)
            {
                activePanels.Add(newPanel);
                Debug.Log($"Panel '{panelId}' aktif.");
            }
            else
            {
                deactivePanels.Add(newPanel);
                newPanel.SetActive(false);
                Debug.Log($"Panel '{panelId}' pasif.");
            }

            Debug.Log($"Aktif Panel Sayýsý: {activePanels.Count}, Deaktif Panel Sayýsý: {deactivePanels.Count}");
        }

        else if(processType == "SoldierCreation")
        {
            GameObject newPanel = Instantiate(soldierCreationPanelPrefab, panelContainer);

            Image img = newPanel.transform.Find("ProcessImage").GetComponent<Image>();
            TextMeshProUGUI tmp = newPanel.transform.Find("SoldierAmountText").GetComponent<TextMeshProUGUI>();
            Image background = newPanel.transform.Find("Background").GetComponent<Image>();
            Sprite sprite = Resources.Load<Sprite>("Military/SoldierCreation");

            if (background == null)
            {
                Debug.Log("Background bulunamadý");
            }
            else
            {
                Image bar = background.transform.Find("Bar").GetComponent<Image>();

                if (bar == null)
                {
                    Debug.Log("Bar bulunamadý");
                }
                else
                {

                    LeanTween.scaleX(bar.gameObject, 1, buildTime).setOnComplete(() => DestroyPanel(panelId));
                }
            }

            if (img != null && tmp != null)
            {
                img.sprite = sprite;
                tmp.text = panelText;
            }

            if (panelDictionary.ContainsKey(panelId))
            {
                Debug.LogWarning($"Panel ID zaten mevcut: {panelId}");
                Destroy(newPanel); // Ayný ID için birden fazla panel oluþmasýný engelle
                return;
            }

            panelDictionary.Add(panelId, newPanel); // Paneli ID ile iliþkilendir

            // Paneli aktifleþtir/deaktif et
            if (OpenProcessPanelWithAnim.isPanelActive)
            {
                activePanels.Add(newPanel);
                Debug.Log($"Panel '{panelId}' aktif.");
            }
            else
            {
                deactivePanels.Add(newPanel);
                newPanel.SetActive(false);
                Debug.Log($"Panel '{panelId}' pasif.");
            }

            Debug.Log($"Aktif Panel Sayýsý: {activePanels.Count}, Deaktif Panel Sayýsý: {deactivePanels.Count}");
        }

        else if (processType == "HealSoldier")
        {
            GameObject newPanel = Instantiate(SoldierHealPanelPrefab, panelContainer);

            Image img = newPanel.transform.Find("ProcessImage").GetComponent<Image>();
            TextMeshProUGUI tmp = newPanel.transform.Find("HealedSoldierAmountText").GetComponent<TextMeshProUGUI>();
            Image background = newPanel.transform.Find("Background").GetComponent<Image>();
            Sprite sprite = Resources.Load<Sprite>("Military/HealSoldier");

            if (background == null)
            {
                Debug.Log("Background bulunamadý");
            }
            else
            {
                Image bar = background.transform.Find("Bar").GetComponent<Image>();

                if (bar == null)
                {
                    Debug.Log("Bar bulunamadý");
                }
                else
                {

                    LeanTween.scaleX(bar.gameObject, 1, buildTime).setOnComplete(() => DestroyPanel(panelId));
                }
            }

            if (img != null && tmp != null)
            {
                img.sprite = sprite;
                tmp.text = panelText;
            }

            if (panelDictionary.ContainsKey(panelId))
            {
                Debug.LogWarning($"Panel ID zaten mevcut: {panelId}");
                Destroy(newPanel); // Ayný ID için birden fazla panel oluþmasýný engelle
                return;
            }

            panelDictionary.Add(panelId, newPanel); // Paneli ID ile iliþkilendir

            // Paneli aktifleþtir/deaktif et
            if (OpenProcessPanelWithAnim.isPanelActive)
            {
                activePanels.Add(newPanel);
                Debug.Log($"Panel '{panelId}' aktif.");
            }
            else
            {
                deactivePanels.Add(newPanel);
                newPanel.SetActive(false);
                Debug.Log($"Panel '{panelId}' pasif.");
            }

            Debug.Log($"Aktif Panel Sayýsý: {activePanels.Count}, Deaktif Panel Sayýsý: {deactivePanels.Count}");
        }
    }

    // Paneli yok et
    public void DestroyPanel(string panelId)
    {
        if (panelDictionary.ContainsKey(panelId))
        {
            GameObject panelToRemove = panelDictionary[panelId];

            if (activePanels.Contains(panelToRemove))
                activePanels.Remove(panelToRemove);

            if (deactivePanels.Contains(panelToRemove))
                deactivePanels.Remove(panelToRemove);

            Destroy(panelToRemove);
            panelDictionary.Remove(panelId);

            Debug.Log($"Panel yok edildi: {panelId}");
        }
        else
        {
            Debug.LogWarning($"Panel bulunamadý: {panelId}");
        }
    }

    // Panellerin durumunu deðiþtir
    public void ChangeStatus()
    {
        Debug.Log("Panellerin durumunu deðiþtiriyorum...");
        Debug.Log($"Aktif Panel Sayýsý: {activePanels.Count}, Deaktif Panel Sayýsý: {deactivePanels.Count}");

        if (OpenProcessPanelWithAnim.isPanelActive)
        {
            foreach (GameObject panel in deactivePanels.ToArray())
            {
                deactivePanels.Remove(panel);
                activePanels.Add(panel);
                panel.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject panel in activePanels.ToArray())
            {
                activePanels.Remove(panel);
                deactivePanels.Add(panel);
                panel.SetActive(false);
            }
        }

        Debug.Log($"Güncel Aktif Panel Sayýsý: {activePanels.Count}, Deaktif Panel Sayýsý: {deactivePanels.Count}");
    }

    // Paneli güncelle (örneðin panel textini deðiþtir)
    public void UpdatePanel(string panelId, string newText)
    {
        if (panelDictionary.ContainsKey(panelId))
        {
            GameObject panelToUpdate = panelDictionary[panelId];
            Text panelText = panelToUpdate.GetComponentInChildren<Text>();
            if (panelText != null)
            {
                panelText.text = newText;
                Debug.Log($"Panel güncellendi: {panelId}");
            }
        }
        else
        {
            Debug.LogWarning($"Güncelleme yapýlamadý, panel bulunamadý: {panelId}");
        }
    }
}
