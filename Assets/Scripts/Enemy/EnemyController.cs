using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("States")]
    public EnemyStates initState;
    public EnemyStates currentState;

    [Header("Senses")]
    public float listenRadius;
    public float listenThreshold;
    public float lookRadius;
    [Range(0, 180)] public float lookAngle;
    public LayerMask playerMask;

    public GameObject player;
    PlayerMovement playerMovement;
    EnemyMovement enemyMovement;
    EnemyAttack enemyAttack;

    void Start()
    {
        currentState = initState;
        player = GameObject.Find("PLAYER");
        playerMovement = player.GetComponent<PlayerMovement>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    void Update()
    {
        if (player != null) {
            ListenForPlayer();
            LookForPlayer();
        }

        HandleEnemyState();
    }

    void HandleEnemyState() {

        if (player == null) {
            currentState = EnemyStates.PATROL;
            enemyMovement.currentPatrolIndex = 0;
            enemyMovement.currentTargetPosition = enemyMovement.patrolPoints[0].position;
            enemyMovement.Patrol();
            return;
        }

        switch(currentState) {

            case EnemyStates.PATROL:
                enemyMovement.Patrol();
                break;

            case EnemyStates.QUESTION:
                enemyMovement.Question();
                break;

            case EnemyStates.CHASE:
                enemyMovement.Chase();

                if (CheckCanAttack()) {
                    enemyAttack.Attack();
                }

                break;
        }
    }

    void ListenForPlayer() {

        // Check if player is within distance and moving
        if (Vector3.SqrMagnitude(player.transform.position - transform.position) < Mathf.Pow(listenRadius, 2)
        && playerMovement.GetMoveVelocity().magnitude > listenThreshold
        && currentState != EnemyStates.CHASE) {

            currentState = EnemyStates.QUESTION;
            enemyMovement.playerLastHeardPosition = player.transform.position;
        }
    }

    void LookForPlayer() {
        Vector3 playerDirection = player.transform.position - transform.position;

        // Check if player is within viewing distance
        if (Physics.CheckSphere(transform.position, lookRadius, playerMask)
        && Vector3.Angle(transform.forward, playerDirection) <= lookAngle / 2
        && Physics.Raycast(transform.position, playerDirection, out RaycastHit hit, lookRadius)
        && hit.transform.gameObject == player) {

            currentState = EnemyStates.CHASE;

        }
    }

    public bool CheckCanAttack() {
        return Vector3.SqrMagnitude(player.transform.position - transform.position) <= Mathf.Pow(enemyAttack.minAttackDistance, 2)
        && enemyAttack.canAttack;
    }
}

public enum EnemyStates {
    PATROL,
    QUESTION,
    CHASE
}