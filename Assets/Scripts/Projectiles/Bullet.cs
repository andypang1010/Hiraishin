using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        Invoke(nameof(UseGravity), 0.15f);
    }
    
    // Update is called once per frame
    void OnCollisionEnter(Collision other) {
        if (other.transform.gameObject.TryGetComponent(out PlayerController playerController)) {
            playerController.Shot();
        }

        Destroy(rb);
        transform.SetParent(other.transform);
        
    }

    void UseGravity() {
        if (rb != null) {
            rb.useGravity = true;
        }
    }
}
