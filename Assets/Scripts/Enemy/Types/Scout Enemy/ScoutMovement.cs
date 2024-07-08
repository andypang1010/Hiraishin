using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutMovement : EnemyMovement
{
    private bool isEvading;
    private bool evadeInitiated;
    private bool isScared;

    protected void Update() 
    {
        if (player.GetComponent<PlayerController>().isDead) 
        {
            return;
        }

        // Check if the player is within evasion distance and evasion has not started yet
        if (ShouldStartEvading())
        {
            StartEvading();
        }

        if (isEvading) 
        {
            print("IS EVADING!");
            if (!evadeInitiated)
            {
                Evade();
                evadeInitiated = true;
                isScared = true;
            }

            if (agent.remainingDistance <= agent.stoppingDistance) {
                StopEvading();
            }
            
            animator.SetBool(isPatrolHash, false);
            animator.SetBool(isSearchHash, false);
            animator.SetBool(isEvadeHash, true);
        }

        else if (isScared) {
            agent.SetDestination(transform.position);

            animator.SetBool(isPatrolHash, false);
            animator.SetBool(isSearchHash, true);
            animator.SetBool(isEvadeHash, false);
        }

        else 
        {
            Patrol();
            animator.SetBool(isPatrolHash, true);
            animator.SetBool(isSearchHash, false);
            animator.SetBool(isEvadeHash, false);
        }

        agent.SetDestination(targetPosition);
    }

    private bool ShouldStartEvading()
    {
        // If the enemy is not evading and has sensed the player
        return vision.PlayerDistance <= data.startEvadeDistance && !isEvading && (vision.playerSeen || hearing.PlayerHeard)

        // Or the enemy is evading and has seen the player (meaning the player is on its path)
        || vision.playerSeen && isEvading && vision.PlayerDistance <= data.startEvadeDistance;
    }

    private void StartEvading()
    {
        isEvading = true;
        evadeInitiated = false; // Reset the flag to allow Evade() to be called once when evasion starts
    }

    private void StopEvading() {
        isEvading = false;
        evadeInitiated = false;
    }
}