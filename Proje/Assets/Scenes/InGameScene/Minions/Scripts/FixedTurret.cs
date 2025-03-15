using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FixedTurret : MonoBehaviour
{
    private void Start()
    {
        // Kuleyi Kinematik yap
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }
}

