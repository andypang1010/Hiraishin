using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.AI.Navigation;
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
    int isPatrolHash, isSearchHash, isChaseHash, isEvadeHash;

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

        animator.SetBool(isPatrolHash, true);
        animator.SetBool(isSearchHash, false);
        animator.SetBool(isChaseHash, false);
        animator.SetBool(isEvadeHash, false);
    }

    protected void Search() {

        agent.speed = data.searchSpeed;

        targetPosition = hearing.PlayerLastHeardLocation;

        // if (agent.isStopped)

        print(agent.velocity.magnitude);

        if (!agent.isStopped) {
            animator.SetBool(isPatrolHash, true);
            animator.SetBool(isSearchHash, false);
            animator.SetBool(isChaseHash, false);
            animator.SetBool(isEvadeHash, false);
        }

        else {
            animator.SetBool(isPatrolHash, false);
            animator.SetBool(isSearchHash, true);
            animator.SetBool(isChaseHash, false);
            animator.SetBool(isEvadeHash, false);
        }
    }

    protected void Chase() {

        agent.speed = data.chaseSpeed;

        targetPosition = vision.PlayerSeenLocation;

        // TODO: Stop when within certain distance of player to allow for enemy attack

        animator.SetBool(isPatrolHash, false);
        animator.SetBool(isSearchHash, false);
        animator.SetBool(isChaseHash, true);
        animator.SetBool(isEvadeHash, false);
    }
    
    protected void Evade() {

        animator.SetBool(isPatrolHash, false);
        animator.SetBool(isSearchHash, false);
        animator.SetBool(isChaseHash, false);
        animator.SetBool(isEvadeHash, true);

        agent.speed = data.evadeSpeed;

        Vector3 targetDirection = (transform.position - player.transform.position).normalized;

        targetPosition = transform.position + targetDirection * data.evadeSafeDistance;

        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, agent.height * 5f, NavMesh.AllAreas)) {
            targetPosition = hit.position;
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
}
