using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machete : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.isTrigger || other.transform.parent.gameObject == null) {
            return;
        }

        if (other.transform.parent.gameObject.CompareTag("Player")) {
            StartCoroutine(other.transform.parent.gameObject.GetComponent<PlayerController>().Die());
        }
    }
}
