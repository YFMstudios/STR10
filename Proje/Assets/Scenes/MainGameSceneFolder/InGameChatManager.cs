using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class InGameChatManager : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    public TMP_InputField chatInputField; // Kullanıcının mesaj yazdığı alan
    [SerializeField] private Button sendChatButton; // Gönder butonu
    [SerializeField] private Transform chatMessageContainer; // ScrollView'in Content kısmı
    [SerializeField] private GameObject chatMessagePrefab; // Mesaj prefabı

    private void Start()
    {
        if (sendChatButton != null)
        {
            sendChatButton.onClick.AddListener(SendChatMessage);
        }
        else
        {
            Debug.LogError("Send Chat Button is not assigned in the Inspector.");
        }

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void SendChatMessage()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Not connected to Photon. Cannot send in-game messages.");
            return;
        }

        if (chatInputField == null || string.IsNullOrEmpty(chatInputField.text))
        {
            Debug.LogWarning("Chat Input Field is null or empty.");
            return;
        }

        string chatMessage = chatInputField.text;

        // Kullanıcı adını Photon'dan al
        string playerName = PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PlayerName")
            ? PhotonNetwork.LocalPlayer.CustomProperties["PlayerName"].ToString()
            : null;

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("PlayerName is not set in Photon Custom Properties.");
            return;
        }

        photonView.RPC("BroadcastInGameMessage", RpcTarget.All, playerName, chatMessage, System.DateTime.Now.ToString("HH:mm"));

        chatInputField.text = string.Empty;
    }

    [PunRPC]
    public void BroadcastInGameMessage(string playerName, string chatMessage, string timestamp)
    {
        if (chatMessagePrefab == null || chatMessageContainer == null)
        {
            Debug.LogError("Chat Message Prefab or Chat Message Container is not assigned.");
            return;
        }

        // Content alanının pivotunu ve anchor'larını ayarla
        RectTransform contentRect = chatMessageContainer.GetComponent<RectTransform>();
        contentRect.pivot = new Vector2(0.5f, 1f);
        contentRect.anchorMin = new Vector2(0f, 1f);
        contentRect.anchorMax = new Vector2(1f, 1f);

        // Prefab oluştur
        GameObject newChatMessage = Instantiate(chatMessagePrefab, chatMessageContainer);
        newChatMessage.SetActive(true);

        // Mesaj içeriğini ve saatini ayarla
        TMP_Text chatMessageText = newChatMessage.GetComponentInChildren<TMP_Text>();
        if (chatMessageText != null)
        {
            chatMessageText.text = $"{timestamp} - {playerName}: {chatMessage}";
        }
        else
        {
            Debug.LogError("Chat Message Prefab does not have a TMP_Text component.");
        }

        // Mesaj prefabının RectTransform'unu ayarla
        RectTransform newChatMessageRect = newChatMessage.GetComponent<RectTransform>();
        newChatMessageRect.pivot = new Vector2(0.5f, 1f);
        newChatMessageRect.anchorMin = new Vector2(0f, 1f);
        newChatMessageRect.anchorMax = new Vector2(1f, 1f);

        // Mesajın yatayda tam genişlemesi için offset'leri ayarla
        newChatMessageRect.offsetMin = new Vector2(0f, newChatMessageRect.offsetMin.y);
        newChatMessageRect.offsetMax = new Vector2(0f, newChatMessageRect.offsetMax.y);

        // Yeni mesajın konumunu ayarla
        if (chatMessageContainer.childCount > 1)
        {
            // Bir önceki mesajın RectTransform'unu al
            RectTransform lastMessageRect = chatMessageContainer.GetChild(chatMessageContainer.childCount - 2).GetComponent<RectTransform>();

            // Yeni mesajı bir önceki mesajın altına hizala
            float newY = lastMessageRect.anchoredPosition.y - lastMessageRect.sizeDelta.y - 10; // 10 birim boşluk
            newChatMessageRect.anchoredPosition = new Vector2(0, newY);
        }
        else
        {
            // İlk mesaj için en üst pozisyonu ayarla
            newChatMessageRect.anchoredPosition = new Vector2(0, 0);
        }

        // Content alanının boyutunu güncelle
        AdjustChatContentSize();

        // Scroll pozisyonunu koru
        StartCoroutine(MaintainChatScrollPosition());
    }

    private void AdjustChatContentSize()
    {
        RectTransform contentRect = chatMessageContainer.GetComponent<RectTransform>();

        float totalHeight = 0f;
        for (int i = 0; i < chatMessageContainer.childCount; i++)
        {
            RectTransform childRect = chatMessageContainer.GetChild(i).GetComponent<RectTransform>();
            totalHeight += childRect.sizeDelta.y;
            if (i > 0)
            {
                totalHeight += 10; // Mesajlar arasındaki boşluk
            }
        }

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, Mathf.Abs(totalHeight));
    }

    private IEnumerator MaintainChatScrollPosition()
    {
        yield return new WaitForEndOfFrame();

        ScrollRect scrollRect = chatMessageContainer.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            // Scroll pozisyonunu en alta ayarla
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}
