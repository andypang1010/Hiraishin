using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMovement : EnemyMovement
{

    void Update() {
        print("Agent isStopped?: " + agent.isStopped);

        if (vision.playerSeen) {
            agent.isStopped = false;
            Chase();

            animator.SetBool(isPatrolHash, false);
            animator.SetBool(isSearchHash, false);
            animator.SetBool(isChaseHash, true);
        }

        else if ((hearing.PlayerHeard || playerDetected) && !player.GetComponent<PlayerController>().isDead) {
            playerDetected = true;

            if (Vector3.SqrMagnitude(transform.position - hearing.PlayerLastHeardLocation) <= Mathf.Pow(data.minTargetDistance, 2)) {
                agent.isStopped = true;

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

            Search();

            if (!agent.isStopped) {
                animator.SetBool(isPatrolHash, true);
                animator.SetBool(isSearchHash, false);
                animator.SetBool(isChaseHash, false);
            }

            else {
                animator.SetBool(isPatrolHash, false);
                animator.SetBool(isSearchHash, true);
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
