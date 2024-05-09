using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : EnemyAttack
{
    protected override void Attack() {

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
            player.Decapacitate();
        }

        else {
            print(gameObject.name + " MISSED");
        }

        // Start attack CD
        canAttack = false;
        Invoke(nameof(ResetAttack), data.attackCD);
    }
}
