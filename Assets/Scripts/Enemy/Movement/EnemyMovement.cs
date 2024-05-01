using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{    
    [Header("References")]
    public EnemyData data;
    EnemyVision vision;
    EnemyHearing hearing;

    [Header("Navigation")]
    Vector3 targetPosition;
    NavMeshAgent agent;
    GameObject player;

    [Header("Patrol Settings")]
    public List<Transform> patrolPoints;
    int currentPatrolIndex;

    [Header("Detection")]
    bool playerWasHeard;
    float currentSearchTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        vision = GetComponent<EnemyVision>();
        hearing = GetComponent<EnemyHearing>();

        // Copy start point and set as first patrol point
        GameObject patrolPoint0 = Instantiate(new GameObject("Patrol Point 0"), transform.position, transform.rotation);
        patrolPoints.Insert(0, patrolPoint0.transform);
    }

    void Update() {

        if (vision.playerSeen && data.chaseEnabled) {
            agent.isStopped = false;
            Chase();
        }

        else if (data.evadeEnabled && vision.PlayerDistance <= data.startEvadeDistance) {
            agent.isStopped = false;
            Evade();
        }

        else if ((hearing.PlayerHeard || playerWasHeard) && data.searchEnabled) {
            playerWasHeard = true;

            if (Vector3.SqrMagnitude(transform.position - hearing.PlayerLastHeardLocation) <= Mathf.Pow(3f, 2)) {

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

        else if (data.patrolEnabled) {
            agent.isStopped = false;
            Patrol();
        }
        
        agent.SetDestination(targetPosition);
    }


    void Patrol() {
        agent.speed = data.patrolSpeed;
        agent.acceleration = data.patrolSpeed * 1.5f;

        // If enemy is within minReachPatrolDistance of the patrol point, go to the next patrol point
        if (Vector3.SqrMagnitude(targetPosition - transform.position) <= Mathf.Pow(data.minReachPatrolDistance, 2)) {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }

        targetPosition = patrolPoints[currentPatrolIndex].position;
    }

    void Search() {
        agent.speed = data.searchSpeed;
        agent.acceleration = data.searchSpeed * 1.5f;

        targetPosition = hearing.PlayerLastHeardLocation;
    }

    void Chase() {
        agent.speed = data.chaseSpeed;
        agent.acceleration = data.chaseSpeed * 1.5f;

        targetPosition = player.transform.position;
    }

    void LookAround() {
        transform.rotation.Set(
            transform.rotation.x, 
            transform.rotation.y + 10 * Time.deltaTime, 
            transform.rotation.z, 
            transform.rotation.w
        );
    }

    void Evade() {
        agent.speed = data.evadeSpeed;
        agent.acceleration = data.evadeSpeed * 1.5f;

        Vector3 targetDirection = (transform.position - player.transform.position).normalized;
        targetPosition = transform.position + targetDirection * data.evadeSafeDistance;
    }

    void FindNearestPatrolPoint()
    {
        Transform closestPatrolPos = null;
        float closestDistance = float.MaxValue;

        foreach (Transform patrolPos in patrolPoints)
        {
            float distance = Vector3.Distance(patrolPos.position, transform.position);

            if (closestPatrolPos != null)
            {

                // If closer than previous closest distance
                if (distance < closestDistance)
                {

                    // Set as temporary closest patrol point
                    closestPatrolPos = patrolPos;
                    closestDistance = distance;
                }
            }

            else
            {
                closestPatrolPos = patrolPos;
                closestDistance = distance;
            }
        }

        // Go to closest patrol point
        currentPatrolIndex = patrolPoints.IndexOf(closestPatrolPos);
    }
}
