using UnityEngine;

public class PhotonPropertiesUpdater : MonoBehaviour
{
    public KaynakYoneticisi kaynakYoneticisi;

    void Update()
    {
        kaynakYoneticisi.TrySync(); // Sadece ihtiyaç varsa sync yap!
    }
}
