using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutMovement : EnemyMovement
{

    bool isEvading;

    void Update() {

        if (vision.playerSeen && !playerDetected) {
            playerDetected = true;
        }

        if (vision.PlayerDistance <= data.startEvadeDistance && !isEvading) {

            isEvading = true;
            Evade();
        }

        else if (isEvading) {
            Evade();

            if (vision.PlayerDistance >= data.evadeSafeDistance) {
                isEvading = false;
            }
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
