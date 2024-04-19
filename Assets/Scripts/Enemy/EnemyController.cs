using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Navigation")]
    public GameObject player;
    public LayerMask playerMask;
    public EnemyStates initState;
    public EnemyStates currentState;
    public Vector3 currentTargetPosition;
    NavMeshAgent agent;
    PlayerMovement playerMovement;

    [Header("Senses")]
    public float listenRadius;
    public float lookRadius;

    [Header("Patrol")]
    public float patrolSpeed;
    public List<Transform> patrolPoints;
    public int currentPatrolIndex = 0;
    public float minReachPatrolDistance;

    [Header("Question")]
    public float questionSpeed;
    public Vector3 playerLastHeardPosition;

    [Header("Chase")]
    public float chaseSpeed;
    
    [Header("Attack")]
    public float attackCD;
    public float minAttackDistance;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerMovement = GetComponent<PlayerMovement>();
        currentState = initState;

        patrolPoints.Insert(0, transform);
    }

    // Update is called once per frame
    void Update()
    {
        ListenForPlayer();
        LookForPlayer();
        HandleMovementState();

        agent.SetDestination(currentTargetPosition);
    }

    void ListenForPlayer() {

        // Check if player is within distance, not crouching, and moving
        if (Vector3.SqrMagnitude(player.transform.position - transform.position) < Mathf.Pow(listenRadius, 2)
        && playerMovement.movementState != PlayerMovement.MovementState.CROUCH
        && playerMovement.GetMoveVelocity().magnitude > 0.3f) {

            currentState = EnemyStates.QUESTION;
            playerLastHeardPosition = player.transform.position;
        }
    }

    void LookForPlayer() {
        if (Physics.CheckSphere(transform.position, lookRadius, playerMask)) {
            currentState = EnemyStates.CHASE;

        }
    }

    void AttackPlayer() {

    }

    void HandleMovementState() {
        switch(currentState) {

            case EnemyStates.PATROL:
                agent.speed = patrolSpeed;

                // If enemy is within minReachPatrolDistance of the patrol point, go to the next patrol point
                if (Vector3.SqrMagnitude(currentTargetPosition - transform.position) < Mathf.Pow(minReachPatrolDistance, 2)) {
                    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                }

                currentTargetPosition = patrolPoints[currentPatrolIndex].position;

                break;

            case EnemyStates.QUESTION:
                agent.speed = questionSpeed;

                currentTargetPosition = playerLastHeardPosition;
                break;

            case EnemyStates.CHASE:
                agent.speed = chaseSpeed;

                currentTargetPosition = player.transform.position;
                break;

            case EnemyStates.ATTACK:
                agent.speed = 0;
                break;

        }
    }

    public enum EnemyStates {
        PATROL,
        QUESTION,
        CHASE,
        ATTACK
    }
}
