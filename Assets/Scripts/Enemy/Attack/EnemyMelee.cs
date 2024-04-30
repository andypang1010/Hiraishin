using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [Header("Melee Settings")]
    public float attackCD;
    public float attackReach;
    public bool canAttack;

    GameObject player;
    EnemyVision vision;

    void Start()
    {
        canAttack = true;
        
        player = GameObject.FindGameObjectWithTag("Player");
        vision = GetComponent<EnemyVision>();
    }

    void Update() {
        if (vision.PlayerSeen 
        && vision.PlayerDistance <= attackReach * 1.5f
        && canAttack) {
            Attack();
        }
    }

    public void Attack() {

        // Check if player is within attack range
        if (Physics.Raycast(
            transform.position, 
            (player.transform.position - transform.position).normalized, 
            out RaycastHit hit, 
            attackReach)
            
            && hit.transform.gameObject == player) {

            print(gameObject.name + " HIT PLAYER");
            hit.transform.gameObject.GetComponent<PlayerController>().Die();
        }

        else {
            print(gameObject.name + " MISSED");
        }


        // Start attack CD
        canAttack = false;
        Invoke(nameof(ResetAttack), attackCD);
    }

    void ResetAttack() {
        canAttack = true;
    }
}
