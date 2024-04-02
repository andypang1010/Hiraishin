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

    // Update is called once per frame
    void OnCollisionEnter(Collision other) {
        if (targetHit) {
            return;
        }
        
        // Ignore collisions with player
        if (!other.gameObject.CompareTag("Player")) {
            targetHit = true;

            // Stick to collided surface
            rb.isKinematic = true;
            transform.SetParent(other.transform);

            // Disable collisions and rigidbody once collided with other object
            GetComponent<BoxCollider>().enabled = false;
            Destroy(rb);
        }
        
    }
}
