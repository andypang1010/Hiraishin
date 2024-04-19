using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    bool collided;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void OnCollisionEnter(Collision other) {
        if (!collided && rb != null) {
            rb.isKinematic = true;
            collided = true;
        }
        
        else {
            Destroy(rb);
        }
            
        transform.SetParent(other.transform);
        
    }
}
