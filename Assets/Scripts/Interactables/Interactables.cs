using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactables : MonoBehaviour
{
    Vector3 startPos;

    [Header("Floating")]
    public bool floatEnabled;
    public float amplitude;
    public float frequency;

    protected void Start() {
        startPos = transform.position;
    }

    protected void Update() {
        if (floatEnabled) {
            print("Float Enabled");
            Float();
        }
    }

    public abstract void OnInteract();

    void Float() {
        // Calculate the new Y position using a sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        print("Floating: " + newY);

        // Set the new position
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
