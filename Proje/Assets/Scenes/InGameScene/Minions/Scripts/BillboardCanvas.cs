using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    Transform cameraTransform;  // Kamera transformu

    // Ba�lang��ta kamera transformunu al
 void Start()
{
    if (Camera.main != null)
        cameraTransform = Camera.main.transform;
    else
        Debug.LogError("MainCamera bulunamadı! Lütfen kameranın etiketini 'MainCamera' yapın.");
}

void LateUpdate()
{
    if (cameraTransform != null)
        transform.LookAt(transform.position + cameraTransform.rotation * -Vector3.forward, cameraTransform.rotation * Vector3.up);
}

}
