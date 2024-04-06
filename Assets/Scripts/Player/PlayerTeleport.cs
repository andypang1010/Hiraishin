using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform cam;
    public KeyCode teleportModeKey;
    public KeyCode teleportSelectKey;
    public float dilutedTimeScale;
    public float detectionSize;
    public float detectionDistance;

    // public float teleportModeDuration;
    // public float teleportCD;
    public bool inTeleportMode;

    void Update()
    {
        print(gameObject.GetComponent<Rigidbody>().velocity);

        // Enter teleport mode when presses teleportMode key and teleportMode is ready
        if (Input.GetKeyDown(teleportModeKey)) {
            inTeleportMode = !inTeleportMode;
        }

        if (inTeleportMode) {

            // Slow down time
            Time.timeScale = dilutedTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            // If a kunai is found and selectKey pressed
            if (Input.GetKeyDown(teleportSelectKey))
                if (Physics.SphereCast(
                    cam.position, 
                    detectionSize, 
                    cam.forward,
                    out RaycastHit kunaiHit, 
                    detectionDistance, 
                    LayerMask.GetMask("Kunai"))
                ) {
                    Teleport(gameObject, kunaiHit.collider.gameObject);
                    GetComponent<PlayerThrow>().kunaiRemaining++;

                    Destroy(kunaiHit.collider.gameObject);

                    // Revert back to regularMode
                    inTeleportMode = false;
                }

                else if (Physics.SphereCast(
                    cam.position, 
                    detectionSize, 
                    cam.forward,
                    out RaycastHit taggedHit, 
                    detectionDistance, 
                    LayerMask.GetMask("Tagged"))
                ) {
                    GameObject temp = Instantiate(gameObject);
                    Teleport(gameObject, taggedHit.collider.gameObject);
                    Teleport(taggedHit.collider.gameObject, temp);

                    Destroy(temp);
                    
                    // Revert back to regularMode
                    inTeleportMode = false;
                }
        }

        else {
            inTeleportMode = false;

            // Set time to regular scale
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
}

    void Teleport(GameObject source, GameObject target) {

        Rigidbody sourceRB = source.GetComponent<Rigidbody>();
        // Inherit the velocity of moving objects
        if (target.TryGetComponent(out Rigidbody targetRB)) {
            sourceRB.GetComponent<Rigidbody>().velocity = new Vector3(targetRB.velocity.x, 0, targetRB.velocity.z) / targetRB.mass;
        }
        
        sourceRB.MovePosition(target.transform.position + 0.1f * Vector3.up);
    }
}
