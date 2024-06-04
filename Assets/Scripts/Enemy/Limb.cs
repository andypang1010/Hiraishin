using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
    public GameObject currentLimb;

    public void Dismember() {
        if (transform.root.TryGetComponent(out EnemyController enemyController)) {
            enemyController.Die();
            enemyController.enabled = false;
        }

        Destroy(GetComponent<CharacterJoint>());

        if (currentLimb != null) {
            Instantiate(currentLimb, transform.position, transform.rotation);
        }

        transform.localScale = Vector3.zero;
        Destroy(gameObject);
    }
}
