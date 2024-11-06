using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMovement : EnemyMovement
{

    void Update() {

        if (vision.playerSeen) {
            if (!HasValidPathToPlayer()) {

                print("Can't reach player");
                agent.isStopped = true;

                animator.SetBool(isPatrolHash, false);
                animator.SetBool(isSearchHash, true);
                animator.SetBool(isChaseHash, false);
                return;
            }

            else {
                agent.isStopped = false;

                animator.SetBool(isPatrolHash, false);
                animator.SetBool(isSearchHash, false);
                animator.SetBool(isChaseHash, true);

                Chase();
            }

        }

        else if ((hearing.PlayerHeard || playerDetected) && !player.GetComponent<PlayerController>().isDead) {
            playerDetected = true;

            // TurnToTarget(hearing.PlayerLastHeardLocation);

            if (Vector3.SqrMagnitude(transform.position - hearing.PlayerLastHeardLocation) <= Mathf.Pow(data.minTargetDistance, 2)) {
                agent.isStopped = true;

                animator.SetBool(isPatrolHash, false);
                animator.SetBool(isSearchHash, true);
                animator.SetBool(isChaseHash, false);

                if (currentSearchTime >= data.searchDuration)
                {
                    playerDetected = false;
                    currentSearchTime = 0;
                    agent.isStopped = false;

                    FindNearestPatrolPoint();

                    return;
                }

                else {
                    currentSearchTime += Time.deltaTime;
                }
            }

            else {
                agent.isStopped = false;
                Search();
                
                animator.SetBool(isPatrolHash, true);
                animator.SetBool(isSearchHash, false);
                animator.SetBool(isChaseHash, false);
            }
        }

        else if (patrolPoints.Count > 1) {
            agent.isStopped = false;
            Patrol();

            animator.SetBool(isPatrolHash, true);
            animator.SetBool(isSearchHash, false);
            animator.SetBool(isChaseHash, false);
        }

        else {
            agent.isStopped = true;
            
            animator.SetBool(isPatrolHash, false);
            animator.SetBool(isSearchHash, false);
            animator.SetBool(isChaseHash, false);
        }
        
        agent.SetDestination(targetPosition);
    }
}
