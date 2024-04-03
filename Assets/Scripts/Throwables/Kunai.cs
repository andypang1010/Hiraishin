using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public float damage;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision other) {
        print("Collided with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player")) {
            transform.SetParent(null);

            Destroy(gameObject);
            other.gameObject.GetComponent<PlayerThrow>().kunaiRemaining++;
        }
        
        else {
            if (rb != null) Destroy(rb);
            transform.SetParent(other.transform);
        }
        
    }
}
