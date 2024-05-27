using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMovement : EnemyMovement
{

    void Update() {

        if (vision.playerSeen) {
            print("In CHASE!");
            Chase();
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
            }
            
        }

        else {
                        print("In PATROL!");
            Patrol();
        }
        
        agent.SetDestination(targetPosition);
    }
}
