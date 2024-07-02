using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyMovement : MonoBehaviour
{    
    [Header("References")]
    public EnemyData data;
    protected EnemyVision vision;
    protected EnemyHearing hearing;

    [Header("Navigation")]
    protected Vector3 targetPosition;
    protected NavMeshAgent agent;
    protected GameObject player;

    [Header("Patrol Settings")]
    public List<Transform> patrolPoints;
    protected int currentPatrolIndex;

    [Header("Detection")]
    protected bool playerDetected;
    protected float currentSearchTime;


    protected Animator animator;
    protected int isPatrolHash, isSearchHash, isChaseHash, isEvadeHash;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        vision = GetComponent<EnemyVision>();
        hearing = GetComponent<EnemyHearing>();

        animator = GetComponent<Animator>();

        isPatrolHash = Animator.StringToHash("isPatrol");
        isSearchHash = Animator.StringToHash("isSearch");
        isChaseHash = Animator.StringToHash("isChase");
        isEvadeHash = Animator.StringToHash("isEvade");

        // Copy start point and set as first patrol point
        GameObject patrolPoint0 = new GameObject("Patrol Point 0");
        patrolPoint0.transform.position = transform.position;
        patrolPoint0.transform.rotation = transform.rotation;
        patrolPoints.Insert(0, patrolPoint0.transform);
    }

    protected void Patrol() {

        agent.speed = data.patrolSpeed;

        // If enemy is within minReachPatrolDistance of the patrol point, go to the next patrol point
        if (Vector3.SqrMagnitude(targetPosition - transform.position) <= Mathf.Pow(data.minTargetDistance, 2)) {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }

        targetPosition = patrolPoints[currentPatrolIndex].position;
    }

    protected void Search() {

        agent.speed = data.searchSpeed;

        targetPosition = hearing.PlayerLastHeardLocation;
    }

    protected void Chase() {

        agent.speed = data.chaseSpeed;

        targetPosition = vision.PlayerSeenLocation;

        print("Has Path?: " + agent.hasPath);
    }
    
    protected void Evade() {

        agent.speed = data.evadeSpeed;

        Vector3 targetDirection = (transform.position - player.transform.position).normalized;

        targetPosition = transform.position + targetDirection * data.evadeSafeDistance;

        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, agent.height * 10f, NavMesh.AllAreas)) {
            targetPosition = hit.position;
        }

        else
        {
            // Retry mechanism to find an arbitrary valid position on the NavMesh
            bool validPositionFound = false;
            for (int i = 0; i < 10; i++)
            {
                // Generate a random angle and direction
                float randomAngle = UnityEngine.Random.Range(0f, 360f);
                Vector3 randomDirection = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 0, Mathf.Sin(randomAngle * Mathf.Deg2Rad));

                // Calculate a new target position based on the random direction
                Vector3 retryPosition = player.transform.position + randomDirection * data.evadeSafeDistance;

                // Validate the new target position on the NavMesh
                if (NavMesh.SamplePosition(retryPosition, out hit, agent.height * 10f, NavMesh.AllAreas))
                {
                    targetPosition = hit.position;
                    validPositionFound = true;
                    break;
                }
            }

            if (!validPositionFound)
            {
                targetPosition = patrolPoints[0].position;
            }
        }

    }

    protected void FindNearestPatrolPoint()
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

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.magenta;
    //     Gizmos.DrawCube(targetPosition, 1 * Vector3.one);
    // }
}
