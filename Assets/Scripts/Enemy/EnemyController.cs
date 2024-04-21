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
    }

    void Update()
    {
        ListenForPlayer();
        LookForPlayer();

        HandleEnemyState();
    }

    void HandleEnemyState() {
        switch(currentState) {

            case EnemyStates.PATROL:
                enemyMovement.Patrol();
                break;

            case EnemyStates.QUESTION:
                enemyMovement.Question();
                break;

            case EnemyStates.CHASE:
                enemyMovement.Chase();
                break;

            case EnemyStates.ATTACK:
                enemyAttack.Attack();
                break;

        }
    }

    void ListenForPlayer() {

        // Check if player is within distance and moving
        if (Vector3.SqrMagnitude(player.transform.position - transform.position) < Mathf.Pow(listenRadius, 2)
        && playerMovement.GetMoveVelocity().magnitude > listenThreshold) {

            currentState = EnemyStates.QUESTION;
            enemyMovement.playerLastHeardPosition = player.transform.position;
        }
    }

    void LookForPlayer() {
        Vector3 playerDirection = player.transform.position - transform.position;
        if (Physics.CheckSphere(transform.position, lookRadius, playerMask)
        && Vector3.Angle(transform.forward, playerDirection) <= lookAngle / 2
        && Physics.Raycast(transform.position, playerDirection, out RaycastHit hit, lookRadius)
        && hit.transform.gameObject == player) {
            currentState = EnemyStates.CHASE;

        }
    }

    public void CheckAttackDistance() {

    }
}

public enum EnemyStates {
    PATROL,
    QUESTION,
    CHASE,
    ATTACK
}