using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("References")]
    public EnemyData data;
    GameObject player;
    EnemyVision vision;

    bool canAttack;


    void Start()
    {
        canAttack = true;
        
        player = GameObject.FindGameObjectWithTag("Player");
        vision = GetComponent<EnemyVision>();
    }

    void Update() {
        if (vision.playerSeen 
        && vision.PlayerDistance <= data.attackReach * 1.5f
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
            data.attackReach)
            
            && hit.transform.gameObject == player) {

            print(gameObject.name + " HIT PLAYER");
            hit.transform.gameObject.GetComponent<PlayerController>().Die();
        }

        else {
            print(gameObject.name + " MISSED");
        }


        // Start attack CD
        canAttack = false;
        Invoke(nameof(ResetAttack), data.attackCD);
    }

    void ResetAttack() {
        canAttack = true;
    }
}
