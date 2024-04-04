using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public KeyCode teleportModeKey;
    public KeyCode teleportSelectKey;
    public float dilutedTimeScale;
    public float detectionSize;
    public float detectionDistance;
    public bool inTeleportMode;
    public Transform cam;
    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

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
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            // If a kunai is found and selectKey pressed
            if (Input.GetKeyDown(teleportSelectKey)) {
                if (Physics.SphereCast(
                    cam.position, 
                    detectionSize, 
                    cam.forward,
                    out RaycastHit hit, 
                    detectionDistance, 
                    LayerMask.GetMask("Kunai", "Tagged")
                )) {

                    GameObject target = hit.collider.gameObject;
                    print(target + " found");

                    // Teleport to new position (Do not call transform.position directly on RigidBodies!!!)
                    rb.MovePosition(target.transform.position + 0.1f * Vector3.up);

                    // Inherit the velocity of in-air kunai
                    if (target.TryGetComponent(out Rigidbody targetRB)) {
                        GetComponent<Rigidbody>().velocity = new Vector3(targetRB.velocity.x, 0, targetRB.velocity.z);
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

            Time.fixedDeltaTime = 0.02f;
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(cam.position + cam.forward * detectionDistance, detectionSize);
    }
}
