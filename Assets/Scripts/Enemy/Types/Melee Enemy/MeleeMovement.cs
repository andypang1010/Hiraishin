using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMovement : EnemyMovement
{

    void Update() {

        if (vision.playerSeen) {
            print("CHASING");
            agent.isStopped = false;
            Chase();
        }

        else if (hearing.PlayerHeard || playerWasHeard) {
            print("SEARCHING");
            playerWasHeard = true;

            if (Vector3.SqrMagnitude(transform.position - hearing.PlayerLastHeardLocation) <= Mathf.Pow(data.minTargetDistance, 2)) {

                if (currentSearchTime >= data.searchDuration)
                {
                    playerWasHeard = false;
                    currentSearchTime = 0;

                    FindNearestPatrolPoint();

                    return;
                }

                else {
                    agent.isStopped = true;
                    LookAround();
                    currentSearchTime += Time.deltaTime;
                }
            }

            else {
                agent.isStopped = false;
                Search();
            }
            
        }

        else {
            print("PATROLLING");
            agent.isStopped = false;
            Patrol();
        }
        
        agent.SetDestination(targetPosition);
        print(targetPosition);
    }
}
