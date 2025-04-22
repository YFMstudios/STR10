using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class ScrollLimiter : MonoBehaviour, IScrollHandler, IPointerClickHandler, IDragHandler
{
    public RectTransform content;   // İçerik objesi
    public RectTransform viewport;  // Viewport objesi (görünen alan)

    private ScrollRect scrollRect;
    private float contentHeight, viewportHeight;

    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();

        // Yalnızca dikey scroll olsun
        scrollRect.horizontal = false;
        scrollRect.vertical = true;

        // Kendini devre dışı bırakabilecek otomatik visibility ayarlarını engelle
        if (scrollRect.verticalScrollbar != null)
        {
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
        }
    }

    void Update()
    {
        UpdateContentHeight();
        LimitScrolling();
    }

    void UpdateContentHeight()
    {
        contentHeight = content.rect.height;
        viewportHeight = viewport.rect.height;
    }

    void LimitScrolling()
    {
        if (contentHeight <= viewportHeight)
        {
            scrollRect.verticalNormalizedPosition = 1f;
            scrollRect.vertical = false;
        }
        else
        {
            scrollRect.vertical = true;
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
        }
    }

    public void OnScroll(PointerEventData data)
    {
        if (!scrollRect.vertical) return;

        float scrollSensitivity = 0.1f;
        float scrollDelta = data.scrollDelta.y * scrollSensitivity;

        scrollRect.verticalNormalizedPosition = Mathf.Clamp(
            scrollRect.verticalNormalizedPosition + scrollDelta,
            0f,
            1f
        );
    }

    // SCROLL üzerine tıklama: scroll'u devre dışı bırakmasın
    public void OnPointerClick(PointerEventData eventData)
    {
        // Bilerek boş bırakıldı: hiçbir şey yapılmasın, kapanmasın
    }

    // Scroll bar ya da içerik sürüklenirse kapanmasın
    public void OnDrag(PointerEventData eventData)
    {
        // Bilerek boş: sürükleme scroll'u bozmasın veya devre dışı bırakmasın
    }
}
