using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public KeyCode teleportModeKey;
    public KeyCode selectKey;
    public float dilutedTimeScale;
    public bool inTeleportMode;
    public Camera cam;
    PlayerThrow playerThrow;

    void Start() {
        playerThrow = GetComponent<PlayerThrow>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.cyan);

        if (Input.GetKeyDown(teleportModeKey)) {
            inTeleportMode = !inTeleportMode;
        }

        if (inTeleportMode) {
            Time.timeScale = dilutedTimeScale;
            // Time.fixedDeltaTime = 0.005f;

            Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit debugHit, 70f, LayerMask.GetMask("Kunai"));

            if (Input.GetKeyDown(selectKey) 
            && Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, 70f, LayerMask.GetMask("Kunai"))) {

                hit.transform.SetParent(null);

                transform.position = hit.transform.position;
                Destroy(hit.transform.gameObject);
                playerThrow.kunaiRemaining++;

                inTeleportMode = false;
            }
        }

        else {
            Time.timeScale = 1f;
            // Time.fixedDeltaTime = 0.02f;
        }
    }
}
