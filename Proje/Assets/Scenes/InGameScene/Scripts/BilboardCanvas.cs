using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilboardCanvas : MonoBehaviour
{
    Transform cameraTransform; // Kameran�n transform bilgisi

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform; // Sahnedeki ana kameran�n transformunu al
    }

    // Update is called once per frame
    void Update()
    {
        // Canvas'in her zaman kameraya do�ru bakmas�n� sa�lar
        // transform.LookAt ile Canvas'in z ekseni kameraya do�ru y�nlendirilir
        // cameraTransform.rotation * -Vector3.forward, kameran�n bak�� y�n�n�n tersi y�nde bir vekt�r olu�turur
        // cameraTransform.rotation * Vector3.up, kameran�n yukar� do�ru bak�� y�n�n� belirler
        transform.LookAt(transform.position + cameraTransform.rotation * -Vector3.forward, cameraTransform.rotation * Vector3.up);
    }
}
