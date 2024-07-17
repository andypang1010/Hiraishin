using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Interactables : MonoBehaviour
{
    [Header("Activation")]
    public bool isActivated;

    [Header("Floating")]
    public bool floatEnabled;
    public float amplitude;
    public float frequency;
    Vector3 startPos;

    protected virtual void Start() {
        if (floatEnabled) {
            startPos = transform.position;
        }
    }

    protected virtual void Update() {
        if (floatEnabled) {
            Float();
        }
    }

    public abstract void OnInteract();

    void Float() {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
