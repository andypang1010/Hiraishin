using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machete : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        print(other.transform.parent.gameObject);

        if (other.transform.parent.gameObject.CompareTag("Player")) {
            other.transform.parent.gameObject.GetComponent<PlayerController>().Decapacitate();
        }
    }
}