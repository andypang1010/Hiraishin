using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore;

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
        GameObject patrolPoint0 = new GameObject(gameObject.name + ": Waypoint 0");
        patrolPoint0.transform.position = transform.position;
        patrolPoint0.transform.rotation = transform.rotation;

        patrolPoint0.transform.SetParent(patrolPoints[0].parent);
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

        if (player.GetComponent<PlayerMovement>().isWallRunning 
        && NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 5f, LayerMask.GetMask("Ground"))) {
            targetPosition = hit.position;
        }
    }

    protected void Chase() {

        agent.speed = data.chaseSpeed;
        targetPosition = vision.PlayerSeenLocation;

        if (player.GetComponent<PlayerMovement>().isWallRunning 
        && NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 5f, LayerMask.GetMask("Ground"))) {
            targetPosition = hit.position;
        }
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

    public void TurnToTarget(Vector3 targetPos) {
        // Calculate the angular distance between the current and target rotation
        Vector3 targetDirection = targetPos - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        float angle = Quaternion.Angle(transform.rotation, targetRotation);

        // Calculate the maximum possible rotation step for this frame
        float maxRotationStep = data.maxRotationSpeed * Time.deltaTime;

        // Interpolate between the current and target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Mathf.Min(1f, maxRotationStep / angle));
    }

    // Check if there is a valid path to the player
    protected bool HasValidPathToPlayer()
    {
        NavMeshPath path = new NavMeshPath();
        Vector3 playerPosition = player.transform.position;

        // Calculate the path to the player
        if (agent.CalculatePath(playerPosition, path))
        {
            // Check if the path status is complete
            return path.status == NavMeshPathStatus.PathComplete;
        }

        return false;
    }

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.magenta;
    //     Gizmos.DrawCube(targetPosition, 1 * Vector3.one);
    // }
}
