using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ShootMovement : EnemyMovement
{
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

            TurnToTarget(player.transform.position);
        }

        else {
            playerTracking.weight = 0;
        }
    }
}
