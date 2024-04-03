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
        if (other.gameObject.CompareTag("Player")) {
            print("collided with player");
            transform.SetParent(null);

            Destroy(gameObject);
            other.gameObject.GetComponent<PlayerThrow>().kunaiRemaining++;
        }
        
        else {
            print("COLLIDED WITH SOMETHING ELSE");
            if (rb != null) Destroy(rb);
            transform.SetParent(other.transform);
        }
        
    }
}
