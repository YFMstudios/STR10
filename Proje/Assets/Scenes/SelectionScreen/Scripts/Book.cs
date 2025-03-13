  using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime; // Photon kullanımı için
using Photon.Pun;  // PhotonNetworking için gerekli namespace

public class Book : MonoBehaviour
{
    public GetVariableFromHere getVariable;
    [SerializeField] float pageSpeed = 0.5f;
    [SerializeField] List<Transform> pages;
    int index = -1;
    bool rotate = false;
    [SerializeField] GameObject ButtonLeft;
    [SerializeField] GameObject ButtonRight;
    int k = 0;
    GameObject[] pageObjects = new GameObject[6];
    public int currentSpriteNumber = 1;
    [SerializeField] GameObject SelectButton;

    // Krallık numaralarını saklamak için bir liste
    private List<int> kingdomNumbersInRoom = new List<int>();

    private void Start()
    {
        GetVariableFromHere.currentSpriteNum = currentSpriteNumber;
        ButtonLeft.SetActive(false);
        SelectButton = GameObject.Find("SelectButton");

        // PageSprite2'den başlayarak GameObject'leri bul ve devre dışı bırak
        for (int i = 0; i < pageObjects.Length; i++)
        {
            string objectName = "PageSprite" + (i + 2);
            pageObjects[i] = GameObject.Find(objectName);

            if (pageObjects[i] != null)
            {
                pageObjects[i].SetActive(false);
            }
            else
            {
                Debug.LogError(objectName + " not found!");
            }
        }
        
        // Başlangıçta krallık bilgilerini çek ve listeyi doldur
        FetchRoomDataAndLogKingdoms();
    }

   public void Update()
{
    // Eğer currentSpriteNumber, odadaki herhangi bir krallık numarası ile eşleşirse veya currentSpriteNumber 1 ise SelectButton gizlenir
    if (kingdomNumbersInRoom.Contains(currentSpriteNumber) || currentSpriteNumber == 1)
    {
        SelectButton.SetActive(false);
    }
    else
    {
        SelectButton.SetActive(true);
    }
}


    public void RotateNext()
    {
        currentSpriteNumber++;
        GetVariableFromHere.currentSpriteNum = currentSpriteNumber;
        if (currentSpriteNumber == 7)
        {
            ButtonRight.SetActive(false);
        }

        for (int i = 0; i < pageObjects.Length; i++)
        {
            if (pageObjects[i].name == "PageSprite" + currentSpriteNumber)
            {
                pageObjects[i].SetActive(true);
            }
            else
            {
                pageObjects[i].SetActive(false);
            }
        }

        if (rotate == true)
        {
            return;
        }

        index++;
        float angle = 0;
        ForwardButtonActions();
        pages[index].SetAsLastSibling();
        StartCoroutine(Rotate(angle, true));

        // Sayfa değiştirildiğinde krallık bilgilerini güncelle
        FetchRoomDataAndLogKingdoms();
    }

    public void ForwardButtonActions()
    {
        if (!ButtonLeft.activeInHierarchy)
        {
            ButtonLeft.SetActive(true);
        }

        if (index == pages.Count - 1)
        {
            ButtonRight.SetActive(false);
        }
    }

    public void RotateBack()
    {
        currentSpriteNumber--;
        GetVariableFromHere.currentSpriteNum = currentSpriteNumber;
        for (int i = 0; i < pageObjects.Length; i++)
        {
            if (pageObjects[i].name == "PageSprite" + currentSpriteNumber)
            {
                pageObjects[i].SetActive(true);
            }
            else
            {
                pageObjects[i].SetActive(false);
            }
        }

        if (rotate == true)
        {
            return;
        }

        float angle = 0;
        pages[index].SetAsLastSibling();
        BackButtonActions();
        StartCoroutine(Rotate(angle, false));

        // Sayfa değiştirildiğinde krallık bilgilerini güncelle
        FetchRoomDataAndLogKingdoms();
    }

    public void BackButtonActions()
    {
        if (!ButtonRight.activeInHierarchy)
        {
            ButtonRight.SetActive(true);
        }

        if (index - 1 == -1)
        {
            ButtonLeft.SetActive(false);
        }
    }

    IEnumerator Rotate(float angle, bool forward)
    {
        float value = 0f;
        while (true)
        {
            rotate = true;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            value += Time.deltaTime * pageSpeed;
            pages[index].rotation = Quaternion.Slerp(pages[index].rotation, targetRotation, value);
            float angle1 = Quaternion.Angle(pages[index].rotation, targetRotation);
            if (angle1 < 0.1f)
            {
                if (!forward)
                {
                    index--;
                }
                rotate = false;
                break;
            }
            yield return null;
        }
    }

    // Odadaki oyuncuların krallık bilgilerini listeye eklemek ve log'a yazdırmak için
    private void FetchRoomDataAndLogKingdoms()
    {
        if (PhotonNetwork.InRoom)
        {
            Dictionary<string, int> kingdomCount = new Dictionary<string, int>();
            Dictionary<string, int> kingdomNumbers = new Dictionary<string, int>()
            {
                { "Akhadzria", 2 },
                { "Alfgard", 3 },
                { "Arianopol", 4 },
                { "Dhamuron", 5 },
                { "Lexion", 6 },
                { "Zephyrion", 7 }
            };

            // Listeyi temizle
            kingdomNumbersInRoom.Clear();

            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (player.CustomProperties.ContainsKey("Kingdom"))
                {
                    string playerKingdom = player.CustomProperties["Kingdom"].ToString();

                    if (kingdomCount.ContainsKey(playerKingdom))
                    {
                        kingdomCount[playerKingdom]++;
                    }
                    else
                    {
                        kingdomCount[playerKingdom] = 1;
                    }

                    // Krallık numarasını listeye ekle
                    if (kingdomNumbers.ContainsKey(playerKingdom))
                    {
                        kingdomNumbersInRoom.Add(kingdomNumbers[playerKingdom]);
                    }
                }
                else
                {
                    Debug.LogWarning($"{player.NickName} oyuncusunun krallık bilgisi yok!");
                }
            }

            // Krallık bilgilerini log'a yazdırma
            string kingdomLog = "Odadaki oyuncuların krallık bilgileri ve oyuncu sayıları: ";
            foreach (var kingdom in kingdomCount)
            {
                int kingdomNumber = kingdomNumbers.ContainsKey(kingdom.Key) ? kingdomNumbers[kingdom.Key] : -1;
                kingdomLog += $"{kingdom.Key} (Krallık Numarası: {kingdomNumber}, {kingdom.Value} oyuncu), ";
            }

            Debug.Log(kingdomLog);
        }
        else
        {
            Debug.LogError("Photon: Odaya bağlı değilsiniz!");
        }
    }
}  