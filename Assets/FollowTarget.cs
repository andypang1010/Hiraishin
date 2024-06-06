using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void Update()
    {
        if (target == null) {
            Destroy(gameObject);
        }

        else {
            transform.position = target.position;
        }
    }
}
