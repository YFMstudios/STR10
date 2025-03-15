using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera cmVirtualCam;   // Sanal kamera referansý
    public Camera mainCamera;                       // Birincil kamera referansý

    private bool usingVirtualCam = true;            // Sanal kamera kullanýlýyor mu?

    // Update is called once per frame
    void Update()
    {
        // Uzay tuþuna basýldýðýnda kamera modunu deðiþtir
        if (Input.GetKeyDown(KeyCode.Space))
        {
            usingVirtualCam = !usingVirtualCam;  // Kamera modunu tersine çevir

            if (usingVirtualCam)
            {
                cmVirtualCam.gameObject.SetActive(true);   // Sanal kamerayý etkinleþtir
                mainCamera.gameObject.SetActive(false);    // Birincil kamerayý devre dýþý býrak
            }
            else
            {
                cmVirtualCam.gameObject.SetActive(false);  // Sanal kamerayý devre dýþý býrak
                mainCamera.gameObject.SetActive(true);     // Birincil kamerayý etkinleþtir
            }
        }

        // Eðer birincil kamera kullanýlýyorsa, fare pozisyonuna baðlý olarak kamerayý hareket ettir
        if (!usingVirtualCam)
        {
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;

            // Fare ekranýn sol kenarýna yakýnsa, kamerayý sola doðru hareket ettir
            if (x < 10)
            {
                mainCamera.transform.position -= Vector3.left * Time.deltaTime * 10f;
            }
            // Fare ekranýn sað kenarýna yakýnsa, kamerayý saða doðru hareket ettir
            else if (x > Screen.width - 10)
            {
                mainCamera.transform.position -= Vector3.right * Time.deltaTime * 10f;
            }
            // Fare ekranýn alt kenarýna yakýnsa, kamerayý geriye doðru hareket ettir
            if (y < 10)
            {
                mainCamera.transform.position -= Vector3.back * Time.deltaTime * 10f;
            }
            // Fare ekranýn üst kenarýna yakýnsa, kamerayý ileriye doðru hareket ettir
            else if (y > Screen.height - 10)
            {
                mainCamera.transform.position -= Vector3.forward * Time.deltaTime * 10f;
            }
        }
    }
}
