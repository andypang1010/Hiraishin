using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Distraction : MonoBehaviour
{
    bool isEnabled = false;
    public bool isActive;
    // public float distractRadius;
    public float deactivateTime;

    private void OnCollisionEnter(Collision other) {
        if (isEnabled && other.gameObject.name != "PLAYER") {
            ActivateDistraction();
            Invoke(nameof(DeactivateDistraction), deactivateTime);
        }
    }

    public void EnableDistraction() {
        isEnabled = true;
    }

    public void ActivateDistraction()
    {
        isActive = true;
    }

    void DeactivateDistraction() {
        isActive = false;
    }
}
