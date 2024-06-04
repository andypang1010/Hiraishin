using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutMovement : EnemyMovement
{
    bool isEvading;

    void Update() {

        if (player == null) {
            return;
        }

        // Start evading when player is too close and haven't started evading
        if (vision.PlayerDistance <= data.startEvadeDistance && !isEvading && vision.playerSeen) {
            isEvading = true;
        }

        else if (isEvading) {
            Evade();
        }

        else {
            Patrol();
        }
        
        agent.SetDestination(targetPosition);
    }
}
