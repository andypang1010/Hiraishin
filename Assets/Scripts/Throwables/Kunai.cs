using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public float damage;
    bool collided;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerThrow>().kunaiRemaining++;
            Destroy(gameObject);
        }
        
        else {
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
}
