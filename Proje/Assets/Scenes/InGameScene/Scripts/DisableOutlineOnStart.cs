using UnityEngine;

public class DisableOutlineOnStart : MonoBehaviour
{
    private float delay = 0.01f;   // Devre dýþý býrakma gecikme süresi
    private Outline outline;       // Outline bileþeni referansý

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();   // Nesnenin Outline bileþenini al
        Invoke("DisableOutline", delay);      // Belirli bir süre sonra DisableOutline iþlevini çaðýr
    }

    private void DisableOutline()
    {
        outline.enabled = false;   // Outline bileþenini devre dýþý býrak
    }
}
