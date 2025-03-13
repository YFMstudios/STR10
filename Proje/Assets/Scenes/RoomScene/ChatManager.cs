using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    public TMP_InputField messageInputField; // Kullanıcının mesaj yazdığı alan
    [SerializeField] private Button sendButton; // Gönder butonu
    [SerializeField] private Transform messageContainer; // ScrollView'in Content kısmı
    [SerializeField] private GameObject messagePrefab; // Mesaj prefabı

    private void Start()
    {
        if (sendButton != null)
        {
            sendButton.onClick.AddListener(SendMessage);
        }
        else
        {
            Debug.LogError("Send Button is not assigned in the Inspector.");
        }

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void SendMessage()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Not connected to Photon. Cannot send messages.");
            return;
        }

        if (messageInputField == null || string.IsNullOrEmpty(messageInputField.text))
        {
            Debug.LogWarning("Message Input Field is null or empty.");
            return;
        }

        string message = messageInputField.text;

        // Kullanıcı adını Photon'dan al
        string senderName = PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PlayerName")
            ? PhotonNetwork.LocalPlayer.CustomProperties["PlayerName"].ToString()
            : null;

        if (string.IsNullOrEmpty(senderName))
        {
            Debug.LogError("PlayerName is not set in Photon Custom Properties.");
            return;
        }

        photonView.RPC("BroadcastMessage", RpcTarget.All, senderName, message, System.DateTime.Now.ToString("HH:mm"));

        messageInputField.text = string.Empty;
    }

    [PunRPC]
    public void BroadcastMessage(string senderName, string message, string timestamp)
    {
        if (messagePrefab == null || messageContainer == null)
        {
            Debug.LogError("Message Prefab or Message Container is not assigned.");
            return;
        }

        // Content alanının pivotunu ve anchor'larını ayarla
        RectTransform contentRect = messageContainer.GetComponent<RectTransform>();
        contentRect.pivot = new Vector2(0.5f, 1f);
        contentRect.anchorMin = new Vector2(0f, 1f);
        contentRect.anchorMax = new Vector2(1f, 1f);

        // Prefab oluştur
        GameObject newMessage = Instantiate(messagePrefab, messageContainer);
        newMessage.SetActive(true);

        // Mesaj içeriğini ve saatini ayarla
        TMP_Text messageText = newMessage.GetComponentInChildren<TMP_Text>();
        if (messageText != null)
        {
            messageText.text = $"{timestamp} - {senderName}: {message}";
        }
        else
        {
            Debug.LogError("Message Prefab does not have a TMP_Text component.");
        }

        // Mesaj prefabının RectTransform'unu ayarla
        RectTransform newMessageRect = newMessage.GetComponent<RectTransform>();
        newMessageRect.pivot = new Vector2(0.5f, 1f);
        newMessageRect.anchorMin = new Vector2(0f, 1f);
        newMessageRect.anchorMax = new Vector2(1f, 1f);

        // Mesajın yatayda tam genişlemesi için offset'leri ayarla
        newMessageRect.offsetMin = new Vector2(0f, newMessageRect.offsetMin.y);
        newMessageRect.offsetMax = new Vector2(0f, newMessageRect.offsetMax.y);

        // Yeni mesajın konumunu ayarla
        if (messageContainer.childCount > 1)
        {
            // Bir önceki mesajın RectTransform'unu al
            RectTransform lastMessageRect = messageContainer.GetChild(messageContainer.childCount - 2).GetComponent<RectTransform>();

            // Yeni mesajı bir önceki mesajın altına hizala
            float newY = lastMessageRect.anchoredPosition.y - lastMessageRect.sizeDelta.y - 10; // 10 birim boşluk
            newMessageRect.anchoredPosition = new Vector2(0, newY);
        }
        else
        {
            // İlk mesaj için en üst pozisyonu ayarla
            newMessageRect.anchoredPosition = new Vector2(0, 0);
        }

        // Content alanının boyutunu güncelle
        AdjustContentSize();

        // Scroll pozisyonunu koru
        StartCoroutine(MaintainScrollPosition());
    }

    private void AdjustContentSize()
    {
        RectTransform contentRect = messageContainer.GetComponent<RectTransform>();

        float totalHeight = 0f;
        for (int i = 0; i < messageContainer.childCount; i++)
        {
            RectTransform childRect = messageContainer.GetChild(i).GetComponent<RectTransform>();
            totalHeight += childRect.sizeDelta.y;
            if (i > 0)
            {
                totalHeight += 10; // Mesajlar arasındaki boşluk
            }
        }

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, Mathf.Abs(totalHeight));
    }

    private IEnumerator MaintainScrollPosition()
    {
        yield return new WaitForEndOfFrame();

        ScrollRect scrollRect = messageContainer.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            // Scroll pozisyonunu en alta ayarla
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}
