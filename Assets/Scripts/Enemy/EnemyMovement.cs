using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{    
    [Header("Navigation")]
    public Vector3 currentTargetPosition;
    NavMeshAgent agent;

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

    EnemyController enemyController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyController = GetComponent<EnemyController>();

        // Copy start point and set as first patrol point
        GameObject patrolPoint0 = Instantiate(new GameObject("Patrol Point 0"), transform.position, transform.rotation);
        patrolPoints.Insert(0, patrolPoint0.transform);
    }

    public void Patrol() {
        agent.speed = patrolSpeed;
        agent.acceleration = patrolSpeed;

        // If enemy is within minReachPatrolDistance of the patrol point, go to the next patrol point
        if (Vector3.SqrMagnitude(currentTargetPosition - transform.position) <= Mathf.Pow(minReachPatrolDistance, 2)) {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }

        currentTargetPosition = patrolPoints[currentPatrolIndex].position;
        agent.SetDestination(currentTargetPosition);
    }

    public void Question() {
        agent.speed = questionSpeed;
        agent.acceleration = questionSpeed;

        currentTargetPosition = playerLastHeardPosition;
        agent.SetDestination(currentTargetPosition);
    }

    public void Chase() {
        agent.speed = chaseSpeed;
        agent.acceleration = chaseSpeed;

        currentTargetPosition = enemyController.player.transform.position;
        agent.SetDestination(currentTargetPosition);
    }
}
