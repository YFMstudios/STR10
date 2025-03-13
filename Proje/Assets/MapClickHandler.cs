using UnityEngine;
using UnityEngine.UI;

public class MapClickHandler : MonoBehaviour
{
    // Image bileþenini referans almak için bir deðiþken oluþturuyoruz.
    private Image imageComponent;

    void Start()
    {
        // GameObject üzerindeki Image bileþenini alýyoruz.
        imageComponent = GetComponent<Image>();

        // Image'in alphaHitTestMinimumThreshold özelliðini ayarlýyoruz.
        if (imageComponent != null)
        {
            // 0.1 deðeri, %10 þeffaflýk seviyesinden düþük bölgelerin týklanamaz olmasýný saðlar.
            imageComponent.alphaHitTestMinimumThreshold = 0.1f;
        }
    }
}
