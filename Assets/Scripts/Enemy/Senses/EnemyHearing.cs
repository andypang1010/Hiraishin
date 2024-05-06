using System.Collections;
using System.Collections.Generic;
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

    void Start() {
        PlayerHeard = false;

        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {

        // Check if player is within distance and moving or not crouching
        if (Vector3.SqrMagnitude(player.transform.position - transform.position) <= Mathf.Pow(data.listenRadius, 2)
        && playerMovement.GetMoveVelocity().magnitude >= data.movementThreshold
        && playerMovement.movementState != PlayerMovement.MovementState.CROUCH) {

            PlayerHeard = true;
            PlayerLastHeardLocation = player.transform.position;
        }
        
        else {
            PlayerHeard = false;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.listenRadius);
    }
}
