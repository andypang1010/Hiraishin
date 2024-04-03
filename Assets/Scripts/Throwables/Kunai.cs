using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public float damage;
    private Rigidbody rb;

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
        
        // Ignore collisions with player
        else {
            // Stick to collided surface
            rb.isKinematic = true;
            transform.SetParent(other.transform);

            // Disable collisions and rigidbody once collided with other object
            Destroy(rb);
        }
        
    }
}
