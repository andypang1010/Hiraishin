using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Distraction : MonoBehaviour
{
    public bool isActive;
    // public float distractRadius;
    public float deactivateTime;

    private void OnCollisionEnter(Collision other) {
        Invoke(nameof(DeactivateDistraction), deactivateTime);
    }

    public void ActivateDistraction()
    {
        isActive = true;
    }

    void DeactivateDistraction() {
        isActive = false;
    }
}
