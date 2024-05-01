using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearing : MonoBehaviour
{
    GameObject player;
    PlayerMovement playerMovement;

    [Header("Listen Settings")]
    public float listenRadius;
    public float movementThreshold;
    public bool PlayerHeard;
    public Vector3 PlayerLastHeardLocation { get; private set; }

    void Start() {
        PlayerHeard = false;

        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (playerMovement.movementState == PlayerMovement.MovementState.CROUCH) {
            return;
        }

        // Check if player is within distance and moving
        if (Vector3.SqrMagnitude(player.transform.position - transform.position) <= Mathf.Pow(listenRadius, 2)
        && playerMovement.GetMoveVelocity().magnitude >= movementThreshold) {

            PlayerHeard = true;
            PlayerLastHeardLocation = player.transform.position;
        }
        
        else {
            PlayerHeard = false;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, listenRadius);
    }
}
