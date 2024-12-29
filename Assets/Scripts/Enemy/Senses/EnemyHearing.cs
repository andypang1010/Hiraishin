using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyHearing : MonoBehaviour
{
    [Header("References")]
    public EnemyData data;
    GameObject player;
    PlayerMovement playerMovement;

    [Header("Detection")]
    public bool PlayerHeard { get ; private set; }
    public Vector3 PlayerLastHeardLocation { get; private set; }
    private Vector3 ClosestDistractionHeardLocation;
    public bool hasDistraction;

    void Start() {
        PlayerHeard = false;

        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (player.GetComponent<PlayerController>().isDead
        || (player.TryGetComponent(out PlayerBulletTime playerBulletTime) && playerBulletTime.inBulletTime)) {
            PlayerHeard = false;
            return;
        }

        // Check if player is within certain radius and is heard
        if (PlayerIsHeard(data.listenWalkRadius, data.walkAcceptedStates) || PlayerIsHeard(data.listenSprintRadius, data.sprintAcceptedStates)

        // Check if player is not crouching or not in air
        && !(playerMovement.movementState == PlayerMovement.MovementState.CROUCH && playerMovement.movementState == PlayerMovement.MovementState.AIR)
        
        // Check if player is within peripheral hearing radius
        || (Vector3.SqrMagnitude(player.transform.position - transform.position) <= Mathf.Pow(data.peripheralRadius, 2))) {

            SetPlayerHeardLocation(player.transform.position);
        }

        else if (DistractionIsHeard()) {
            SetPlayerHeardLocation(ClosestDistractionHeardLocation);
        }
        
        else {
            PlayerHeard = false;
        }
    }

    bool PlayerIsHeard(float radius, PlayerMovement.MovementState[] movementStates) {
        // Check player is inside radius
        return Vector3.SqrMagnitude(player.transform.position - transform.position) <= Mathf.Pow(radius, 2)

        // Check player is in one of the accepted states
        && Array.IndexOf(movementStates, playerMovement.GetMovementState()) != -1;
    }

    bool DistractionIsHeard() {
        List<Distraction> distractions = Physics.OverlapSphere(transform.position, data.listenSprintRadius)
            .Select(collider => collider.GetComponent<Distraction>()) // Get the Distraction component
            .Where(distraction => distraction != null && distraction.isActive) // Filter only active distractions
            .OrderBy(distraction => Vector3.Distance(transform.position, distraction.transform.position))
            .ToArray().Distinct().ToList();

        if (distractions.Count > 0) {
            ClosestDistractionHeardLocation = distractions[0].transform.position;
            return true;
        }
        
        ClosestDistractionHeardLocation = Vector3.zero;
        return false;
    }

    public void SetPlayerHeardLocation(Vector3 position) {
        PlayerHeard = true;
        PlayerLastHeardLocation = position;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, data.listenSprintRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, data.listenWalkRadius);
    }
}
