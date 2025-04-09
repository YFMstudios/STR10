using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera cmVirtualCam;  // Sahnedeki vcam referansı
    // Main Camera üzerinde CinemachineBrain olduğunu varsayıyoruz
    // Ayrı bir "mainCamera" bileşenine genelde ihtiyaç olmaz.

    private bool isActive = true;

    void Start()
    {
        // Oyun başlar başlamaz vcam aktif olsun (priority veya gameObject ile)
        cmVirtualCam.gameObject.SetActive(true);
        // Priority'yi de isterseniz yükseltebilirsiniz
        cmVirtualCam.Priority = 10;
    }

    void Update()
    {
        // Space'e basıldığında vcam'i aç/kapa yapmak isterseniz:
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isActive = !isActive;
            cmVirtualCam.gameObject.SetActive(isActive);

            // Dilerseniz Priority'yi de 0/10 arası değiştirebilirsiniz
            // cmVirtualCam.Priority = isActive ? 10 : 0;
        }
    }
}
