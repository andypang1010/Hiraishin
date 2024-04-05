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
    
    // Update is called once per frame
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerThrow>().kunaiRemaining++;
            Destroy(gameObject);
        }
        
        else {
            rb.isKinematic = true;
            if (rb != null) Destroy(rb);
            transform.SetParent(other.transform);
        }
        
    }
}
