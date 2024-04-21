using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack")]
    public float attackCD;
    public float minAttackDistance;
    public float attackReach;
    public bool canAttack;
    public float cameraExplosionForce;

    EnemyController enemyController;
    EnemyMovement enemyMovement;
    GameObject playerCam;

    void Start()
    {
        canAttack = true;
        enemyController = GetComponent<EnemyController>();
        enemyMovement = GetComponent<EnemyMovement>();

        playerCam = Camera.main.transform.parent.gameObject;
    }

    public void Attack() {

        if (Physics.Raycast(transform.position, (enemyController.player.transform.position - transform.position).normalized, out RaycastHit hit, attackReach)
        && hit.transform.gameObject == enemyController.player) {

            print(gameObject.name + " HIT PLAYER");

            // Add rigidbody and collider to camera
            Rigidbody rb = Camera.main.gameObject.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.AddForce(cameraExplosionForce * Camera.main.transform.up, ForceMode.Impulse);

            // Destroy player gameObject
            Destroy(enemyController.player);


            // Remove HeadBob and MoveCamera
            playerCam.GetComponent<HeadBob>().enabled = false;
            playerCam.GetComponent<MoveCamera>().enabled = false;


            Camera.main.gameObject.AddComponent<SphereCollider>();

        }

        else {
            print(gameObject.name + " MISSED");
        }

        canAttack = false;
        Invoke(nameof(ResetAttack), attackCD);
    }

    void ResetAttack() {
        canAttack = true;
    }
}
