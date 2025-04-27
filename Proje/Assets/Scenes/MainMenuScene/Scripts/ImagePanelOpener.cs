using UnityEngine;
using UnityEngine.EventSystems;

public class ImagePanelOpener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Panel;

    void Start()
    {
        Panel.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Panel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Panel.SetActive(false);
    }
}
