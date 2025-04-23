using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class NonTransparentRaycast : MonoBehaviour, ICanvasRaycastFilter
{
    private Image image;
    private Texture2D texture;

    [Range(0f, 1f)]
    public float alphaThreshold = 0.1f; // bu eşik değerin altındaki alfa değerleri tıklanmaz

    void Awake()
    {
        image = GetComponent<Image>();
        Sprite sprite = image.sprite;
        if (sprite.texture.isReadable)
        {
            texture = sprite.texture;
        }
        else
        {
            Debug.LogWarning($"Texture '{sprite.texture.name}' is not readable. Make sure 'Read/Write Enabled' is checked in import settings.");
        }
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (texture == null)
            return true; // readable değilse tıklanabilir kabul et

        RectTransform rectTransform = image.rectTransform;
        Vector2 localPoint;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out localPoint))
            return false;

        Rect rect = rectTransform.rect;
        float x = (localPoint.x - rect.x) / rect.width;
        float y = (localPoint.y - rect.y) / rect.height;

        if (x < 0f || x > 1f || y < 0f || y > 1f)
            return false;

        // Sprite içindeki doğru tex koordinatını al
        int texX = Mathf.FloorToInt(x * texture.width);
        int texY = Mathf.FloorToInt(y * texture.height);

        try
        {
            Color pixel = texture.GetPixel(texX, texY);
            return pixel.a >= alphaThreshold;
        }
        catch
        {
            return false;
        }
    }
}