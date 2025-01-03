using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void OnCollisionEnter(Collision other) {
        Destroy(rb);
        transform.SetParent(other.transform);
        
    }

    void UseGravity() {
        if (rb != null) {
            rb.useGravity = true;
        }
    }
}
