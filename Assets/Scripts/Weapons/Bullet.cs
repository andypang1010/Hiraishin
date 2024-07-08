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

        Invoke(nameof(DestroyBullet), 5f);
    }
    
    // Update is called once per frame
    void OnCollisionEnter(Collision other) {
        if (other.transform.gameObject.TryGetComponent(out PlayerController playerController)) {
            // playerController.Shot();
            StartCoroutine(playerController.Die());
        }

        Destroy(gameObject);
        
    }

    void DestroyBullet() {
        Destroy(gameObject);
    }
}
