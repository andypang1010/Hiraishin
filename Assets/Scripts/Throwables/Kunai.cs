using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public float damage;
    private Rigidbody rb;
    private bool targetHit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        gameObject.layer = LayerMask.NameToLayer("Kunai");
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
            other.gameObject.GetComponent<PlayerThrow>().kunaiRemaining++;
        }

        // if (targetHit) {
        //     return;
        // }
        
        // Ignore collisions with player
        else if (!other.gameObject.CompareTag("Player")) {
            targetHit = true;

            // Stick to collided surface
            rb.isKinematic = true;
            transform.SetParent(other.transform);

            // Disable collisions and rigidbody once collided with other object
            Destroy(rb);
        }
        
    }
}
