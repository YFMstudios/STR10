using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    Transform cameraTransform;  // Kamera transformu

    // Baþlangýçta kamera transformunu al
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Her frame'de kanvasýn daima kameraya doðru bakmasýný saðla
    void LateUpdate()
    {
        transform.LookAt(transform.position + cameraTransform.rotation * -Vector3.forward, cameraTransform.rotation * Vector3.up);
    }
}
