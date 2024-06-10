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
        if (player == null) {
            PlayerHeard = false;
            return;
        }

        // Check if player is within listenRadius and moving
        if ((Vector3.SqrMagnitude(player.transform.position - transform.position) <= Mathf.Pow(data.listenRadius, 2)
        && playerMovement.GetMoveVelocity().magnitude >= data.movementThreshold

        // Check if player is not crouching
        && playerMovement.movementState != PlayerMovement.MovementState.CROUCH)
        
        // Check if player is within peripheral hearing radius
        || (Vector3.SqrMagnitude(player.transform.position - transform.position) <= Mathf.Pow(data.peripheralRadius, 2))) {

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
