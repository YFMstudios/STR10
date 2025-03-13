using UnityEngine;
using UnityEngine.UI;

public class NoRaycastText : MonoBehaviour, ICanvasRaycastFilter
{
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        // Her zaman false dönerse, bu nesne raycast tarafýndan hedeflenmez.
        return false;
    }
}
