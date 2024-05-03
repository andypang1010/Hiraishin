using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutMovement : EnemyMovement
{
    bool isEvading;

    void Update() {

        // Start evading when player is too close and haven't started evading
        if (vision.PlayerDistance <= data.startEvadeDistance && !isEvading) {

            isEvading = true;
        }

        else if (isEvading) {

            if (vision.PlayerDistance >= data.evadeSafeDistance) {
                isEvading = false;
                return;
            }

            Evade();
        }

        else if (vision.playerSeen) {
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, transform.up);
        }

        else {
            Patrol();
        }
        
        agent.SetDestination(targetPosition);
    }
}
