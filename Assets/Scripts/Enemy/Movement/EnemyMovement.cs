using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{    

    [Header("Navigation")]
    public Vector3 targetPosition;
    NavMeshAgent agent;
    GameObject player;

    [Header("Patrol Settings")]
    public float patrolSpeed;
    public List<Transform> patrolPoints;
    public int currentPatrolIndex;
    public float minReachPatrolDistance;

    [Header("Search Settings")]
    public float searchSpeed;
    public float searchDuration;
    bool playerWasHeard;
    float currentSearchTime;

    [Header("Chase Settings")]
    public float chaseSpeed;

    EnemyVision vision;
    EnemyHearing hearing;

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

        if (vision.PlayerSeen) {
            agent.isStopped = false;
            Chase();
        }

        else if (hearing.PlayerHeard || playerWasHeard) {
            playerWasHeard = true;

            if (Vector3.SqrMagnitude(transform.position - hearing.PlayerLastHeardLocation) <= Mathf.Pow(3f, 2)) {

                if (currentSearchTime >= searchDuration)
                {
                    playerWasHeard = false;
                    currentSearchTime = 0;

                    FindNearestPatrolPosition();

                    return;
                }

                else {
                    agent.isStopped = true;
                    // TODO: LookAround();
                    currentSearchTime += Time.deltaTime;
                }
            }

            else {
                agent.isStopped = false;
                Search();
            }
            
        }

        else {
            agent.isStopped = false;
            Patrol();
        }
        
        agent.SetDestination(targetPosition);
    }


    void Patrol() {
        agent.speed = patrolSpeed;
        agent.acceleration = patrolSpeed;

        // If enemy is within minReachPatrolDistance of the patrol point, go to the next patrol point
        if (Vector3.SqrMagnitude(targetPosition - transform.position) <= Mathf.Pow(minReachPatrolDistance, 2)) {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }

        targetPosition = patrolPoints[currentPatrolIndex].position;
    }

    void Search() {
        agent.speed = searchSpeed;
        agent.acceleration = searchSpeed;

        targetPosition = hearing.PlayerLastHeardLocation;
    }

    void Chase() {
        agent.speed = chaseSpeed;
        agent.acceleration = chaseSpeed;

        targetPosition = player.transform.position;
    }

    void FindNearestPatrolPosition()
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

                    // Set as closest patrol point
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

        // 
        currentPatrolIndex = patrolPoints.IndexOf(closestPatrolPos);
    }
}
