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
        if (vision.PlayerDistance <= data.startEvadeDistance && !isEvading && (vision.playerSeen || hearing.PlayerHeard)) {
            isEvading = true;
        }

        if (isEvading) {
            Evade();

            animator.SetBool(isPatrolHash, false);
            animator.SetBool(isEvadeHash, true);
        }

        else {
            Patrol();

            animator.SetBool(isPatrolHash, true);
            animator.SetBool(isEvadeHash, false);
        }
        
        agent.SetDestination(targetPosition);
    }
}
