using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyStates {
        PATROL,
        QUESTION,
        CHASE
    }

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

        if (playerMovement.movementState == PlayerMovement.MovementState.CROUCH) {
            return;
        }

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

    bool CheckCanAttack() {
        return Vector3.SqrMagnitude(player.transform.position - transform.position) <= Mathf.Pow(enemyAttack.minAttackDistance, 2)
        && enemyAttack.canAttack;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, listenRadius);
        
        void DrawArc(float startAngle, float endAngle, 
        Vector3 position, Quaternion orientation, float radius, 
        Color color, bool drawChord = false, bool drawSector = false, 
        int arcSegments = 32)
        {

            float arcSpan = Mathf.DeltaAngle(startAngle, endAngle);
        
            // Since Mathf.DeltaAngle returns a signed angle of the shortest path between two angles, it 
            // is necessary to offset it by 360.0 degrees to get a positive value
            if (arcSpan <= 0)
            {
                arcSpan += 360.0f;
            }
        
            // angle step is calculated by dividing the arc span by number of approximation segments
            float angleStep = (arcSpan / arcSegments) * Mathf.Deg2Rad;
            float stepOffset = startAngle * Mathf.Deg2Rad;
        
            // stepStart, stepEnd, lineStart and lineEnd variables are declared outside of the following for loop
            float stepStart = 0.0f;
            float stepEnd = 0.0f;
            Vector3 lineStart = Vector3.zero;
            Vector3 lineEnd = Vector3.zero;
        
            // arcStart and arcEnd need to be stored to be able to draw segment chord
            Vector3 arcStart = Vector3.zero;
            Vector3 arcEnd = Vector3.zero;
        
            // arcOrigin represents an origin of a circle which defines the arc
            Vector3 arcOrigin = position;
        
            for (int i = 0; i < arcSegments; i++)
            {
                // Calculate approximation segment start and end, and offset them by start angle
                stepStart = angleStep * i + stepOffset;
                stepEnd = angleStep * (i + 1) + stepOffset;
        
                lineStart.x = Mathf.Cos(stepStart);
                lineStart.z = Mathf.Sin(stepStart);
                lineStart.y = 0.0f;
        
                lineEnd.x = Mathf.Cos(stepEnd);
                lineEnd.z = Mathf.Sin(stepEnd);
                lineEnd.y = 0.0f;
        
                // Results are multiplied so they match the desired radius
                lineStart *= radius;
                lineEnd *= radius;
        
                // Results are multiplied by the orientation quaternion to rotate them 
                // since this operation is not commutative, result needs to be
                // reassigned, instead of using multiplication assignment operator (*=)
                lineStart = orientation * lineStart;
                lineEnd = orientation * lineEnd;
        
                // Results are offset by the desired position/origin 
                lineStart += position;
                lineEnd += position;
        
                // If this is the first iteration, set the chordStart
                if (i == 0)
                {
                    arcStart = lineStart;
                }
        
                // If this is the last iteration, set the chordEnd
                if(i == arcSegments - 1)
                {
                    arcEnd = lineEnd;
                }
        
                Debug.DrawLine(lineStart, lineEnd, color);
            }
        
            if (drawChord)
            {
                Debug.DrawLine(arcStart, arcEnd, color);
            }
            if (drawSector)
            {
                Debug.DrawLine(arcStart, arcOrigin, color);
                Debug.DrawLine(arcEnd, arcOrigin, color);
            }
        }

        DrawArc(-lookAngle, lookAngle, transform.position, Quaternion.LookRotation(-transform.right), lookRadius, Color.red, false, true);
    }

    public void Attacked() {

    }
}
