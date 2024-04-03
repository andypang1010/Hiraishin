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
            // transform.SetParent(null);

            other.gameObject.GetComponent<PlayerThrow>().kunaiRemaining++;
            Destroy(gameObject);
        }
        
        else {
            if (rb != null) Destroy(rb);
            transform.SetParent(other.transform);
        }
        
    }
}
