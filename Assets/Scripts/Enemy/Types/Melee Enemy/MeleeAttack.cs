using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : MonoBehaviour
{
    [Header("References")]
    public EnemyData data;
    PlayerController player;
    EnemyVision vision;
    NavMeshAgent agent;

    bool canAttack;


    void Start()
    {
        canAttack = true;
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        vision = GetComponent<EnemyVision>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (vision.WithinAttackRadius()
        && canAttack) {
            
            Attack();
        }
    }

    public void Attack() {

        if (player == null) {
            return;
        }

        // Check if player is hit
        if (Physics.Raycast(
            transform.position, 
            (player.transform.position - transform.position).normalized, 
            out RaycastHit hit, 
            data.attackReach)
            
            && hit.transform.gameObject == player.gameObject) {

            print(gameObject.name + " HIT PLAYER");
            player.Die();
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
