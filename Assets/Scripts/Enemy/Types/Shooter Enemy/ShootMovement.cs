using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ShootMovement : EnemyMovement
{
    public float maxRotationSpeed = 90f;
    public Rig playerTracking;

    void Update()
    {
        if (player.GetComponent<PlayerController>().isDead) {
            return;
        }

        if (vision.playerSeen || hearing.PlayerHeard) {
            playerDetected = true;
        }

        if (playerDetected) {
            playerTracking.weight = 1;

            // Calculate the angular distance between the current and target rotation
            Vector3 playerDirection = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
            float angle = Quaternion.Angle(transform.rotation, targetRotation);

            // Calculate the maximum possible rotation step for this frame
            float maxRotationStep = maxRotationSpeed * Time.deltaTime;

            // Interpolate between the current and target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Mathf.Min(1f, maxRotationStep / angle));
        }

        else {
            playerTracking.weight = 0;
        }
    }
}
