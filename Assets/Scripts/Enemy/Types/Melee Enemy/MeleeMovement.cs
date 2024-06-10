using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMovement : EnemyMovement
{

    void Update() {

        if (vision.playerSeen) {
            Chase();

            animator.SetBool(isPatrolHash, false);
            animator.SetBool(isSearchHash, false);
            animator.SetBool(isChaseHash, true);
        }

        else if ((hearing.PlayerHeard || playerDetected) && player != null) {
            playerDetected = true;

            if (Vector3.SqrMagnitude(transform.position - hearing.PlayerLastHeardLocation) <= Mathf.Pow(data.minTargetDistance, 2)) {

                if (currentSearchTime >= data.searchDuration)
                {
                    playerDetected = false;
                    currentSearchTime = 0;

                    FindNearestPatrolPoint();

                    return;
                }

                else {
                    currentSearchTime += Time.deltaTime;
                }
            }

            else {
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
            
        }

        else {
            Patrol();

            animator.SetBool(isPatrolHash, true);
            animator.SetBool(isSearchHash, false);
            animator.SetBool(isChaseHash, false);
        }
        
        agent.SetDestination(targetPosition);
    }
}
