using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public KeyCode teleportModeKey;
    public KeyCode teleportSelectKey;
    public float dilutedTimeScale;
    public float detectionDistance;
    public bool inTeleportMode;
    public Transform cam;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(cam.position, cam.position + cam.forward * detectionDistance, Color.green);

        // Toggle between teleportMode and regularMode
        if (Input.GetKeyDown(teleportModeKey)) {
            inTeleportMode = !inTeleportMode;
        }

        if (inTeleportMode) {

            // Slow down time
            Time.timeScale = dilutedTimeScale;

            // If a kunai is found and selectKey pressed
            if (Input.GetKeyDown(teleportSelectKey)) {
                if (Physics.SphereCast(
                    cam.position, 
                    0.2f, 
                    cam.forward,
                    out RaycastHit hit, 
                    detectionDistance, 
                    LayerMask.GetMask("Kunai", "Tagged")
                )) {

                    GameObject target = hit.collider.gameObject;
                    print(target + " found");

                    // Teleport to new position
                    transform.position = target.transform.position + 0.1f * Vector3.up;
                    print(target.transform.position);

                    // Inherit the velocity of in-air kunai
                    if (target.TryGetComponent(out Rigidbody rb)) {
                        GetComponent<Rigidbody>().velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                    }

                    GetComponent<PlayerThrow>().kunaiRemaining++;

                    // Revert back to regularMode
                    inTeleportMode = false;

                    // Remove target from level
                    Destroy(target);
                }
            }
        }

        else {

            // Set time to regular scale
            Time.timeScale = 1f;
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(cam.position + cam.forward * 1, 0.2f);
    }
}
