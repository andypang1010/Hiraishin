using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public KeyCode teleportModeKey;
    public KeyCode selectKey;
    public float dilutedTimeScale;
    public float detectionDistance;
    public bool inTeleportMode;
    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        // Toggle between teleportMode and regularMode
        if (Input.GetKeyDown(teleportModeKey)) {
            inTeleportMode = !inTeleportMode;
        }

        if (inTeleportMode) {

            // Slow down time
            Time.timeScale = dilutedTimeScale;

            // If a kunai is found and selectKey pressed
            if (Input.GetKeyDown(selectKey) 
            && Physics.Raycast(
                cam.transform.position, 
                cam.transform.forward, 
                out RaycastHit hit, 
                detectionDistance, 
                LayerMask.GetMask("Kunai", "Tagged")
            )) {

                GameObject target = hit.collider.gameObject;
                print("Target 1: " + target);

                // Inherit the velocity of in-air kunai
                if (target.TryGetComponent(out Rigidbody rb)) {
                    GetComponent<Rigidbody>().velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                }

                // Teleport to new position
                transform.position = target.transform.position + 0.1f * transform.up;

                Destroy(target);
                GetComponent<PlayerThrow>().kunaiRemaining++;

                // Revert back to regularMode
                inTeleportMode = false;
            }
        }

        else {

            // Set time to regular scale
            Time.timeScale = 1f;
        }
    }
}
