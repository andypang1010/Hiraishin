using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMovement : EnemyMovement
{

    protected int playerHeardHash, playerSeenHash, nearTargetHash, playerReachableHash;

    new void Start() {
        base.Start();

        playerHeardHash = Animator.StringToHash("playerHeard");
        playerSeenHash = Animator.StringToHash("playerSeen");
        nearTargetHash = Animator.StringToHash("nearTarget");
        playerReachableHash = Animator.StringToHash("playerReachable");

        animator.SetBool(Animator.StringToHash("hasWaypoints"), patrolPoints.Count > 1);
    }

    void Update() {
        
        bool closeToHeardTarget = Vector3.Magnitude(transform.position - hearing.PlayerLastHeardLocation) <= data.minTargetDistance;

        // Found a target but can't reach the target
        if ((vision.playerSeen || hearing.PlayerHeard || playerDetected) && !HasValidPathToPlayer()) {
            Search();
        }

        if (vision.playerSeen && HasValidPathToPlayer()) {
            Chase();
        }

        else if ((hearing.PlayerHeard || playerDetected) && !player.GetComponent<PlayerController>().isDead) {
            playerDetected = true;

            if (closeToHeardTarget || !HasValidPathToPlayer()) {
                Search();
            }

            else {
                Investigate();
            }
        }

        else if (patrolPoints.Count > 1) {
            Patrol();
        }

        else {
            Idle();
        }
        
        agent.SetDestination(targetPosition);

        animator.SetBool(playerHeardHash, playerDetected || hearing.PlayerHeard);
        animator.SetBool(playerSeenHash, vision.playerSeen);
        animator.SetBool(nearTargetHash, closeToHeardTarget && currentSearchTime < data.searchDuration);
        animator.SetBool(playerReachableHash, HasValidPathToPlayer());

    }
}
