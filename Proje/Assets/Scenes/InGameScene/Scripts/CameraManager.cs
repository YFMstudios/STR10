using UnityEngine;
using Cinemachine;

public class MultiCharacterCameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera character1VCam;
    [SerializeField] private CinemachineVirtualCamera character2VCam;

  [Header("Main Camera Virtual Camera (Kullanılmayacak)")]
[SerializeField] private CinemachineVirtualCamera mainVCam;

void Start()
{
    // Main Camera üzerindeki virtual camera'yı pasif hale getir.
    if (mainVCam != null)
    {
        mainVCam.gameObject.SetActive(false);
    }
    
    // Oyuna başladığında oyuncu kendi karakterini seçiyor.
    // Burada örneğin varsayılan olarak karakter 1 seçilmiş durumda.
    ActivateCharacterCamera(1);
}

// Dışarıdan çağırılarak aktif karakterin virtual camera'sını belirle.
public void ActivateCharacterCamera(int characterNumber)
{
    // Önce tüm karakter virtual cameralarını devre dışı bırak.
    if (character1VCam != null)
    {
        character1VCam.gameObject.SetActive(false);
    }
    if (character2VCam != null)
    {
        character2VCam.gameObject.SetActive(false);
    }

    // Seçilen karakterin virtual camera'sını aktif et.
    if (characterNumber == 1)
    {
        if (character1VCam != null)
        {
            character1VCam.gameObject.SetActive(true);
        }
    }
    else if (characterNumber == 2)
    {
        if (character2VCam != null)
        {
            character2VCam.gameObject.SetActive(true);
        }
    }
}

}
