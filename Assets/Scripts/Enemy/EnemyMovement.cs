using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{    

    [Header("Navigation")]
    public Vector3 targetPosition;
    NavMeshAgent agent;
    GameObject player;
    NavMeshPath path;
    Vector3[] waypoints;
    float moveSpeed;

    [Header("Patrol Settings")]
    public float patrolSpeed;
    public List<Transform> patrolPoints;
    public int currentPatrolIndex = 0;
    public float minReachPatrolDistance;

    [Header("Question Settings")]
    public float questionSpeed;

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
            Chase();
        }

        else if (hearing.PlayerHeard) {
            Question();
        }

        else {
            Patrol();
        }
    }

    void FixedUpdate() {
        Move();
    }

    public void Patrol() {
        moveSpeed = patrolSpeed;

        // If enemy is within minReachPatrolDistance of the patrol point, go to the next patrol point
        if (Vector3.SqrMagnitude(targetPosition - transform.position) <= Mathf.Pow(minReachPatrolDistance, 2)) {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }

        targetPosition = patrolPoints[currentPatrolIndex].position;
    }

    public void Question() {
        moveSpeed = questionSpeed;

        targetPosition = hearing.PlayerLastHeardLocation;
    }

    public void Chase() {
        moveSpeed = chaseSpeed;

        targetPosition = player.transform.position;
    }

    void Move() {
        // if (agent.CalculatePath(currentTargetPosition, path)) {
        //     print("Path found");
        //     waypoints = path.corners;
        // }

        // TODO: Do something similar to PlayerMovement

        agent.speed = moveSpeed;
        agent.acceleration = moveSpeed;
        agent.SetDestination(targetPosition);
    }
}
