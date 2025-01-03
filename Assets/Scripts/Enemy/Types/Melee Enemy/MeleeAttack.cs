using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : EnemyAttack
{
    public GameObject machete;
    MeleeMovement meleeMovement;
    BoxCollider attackBox;

    protected override void Start() {
        base.Start();

        meleeMovement = GetComponent<MeleeMovement>();

        foreach (BoxCollider boxCollider in machete.GetComponentsInChildren<BoxCollider>()) {
            if (boxCollider.isTrigger) {
                attackBox = boxCollider;
                break;
            }
        }
    }
    protected override void Update() {
        if (vision.WithinAttackRadius()
        && canAttack) {

            canAttack = false;

            meleeMovement.TurnToTarget(player.transform.position);
            animator.Play(attackHash);
        }

        if (canPlayAttack) {
            attackBox.enabled = true;
        }

        else {
            attackBox.enabled = false;
        }
    }


    
    protected void StartAttackAnim() {
        meleeMovement.enabled = false;
        agent.isStopped = true;
    }

    protected void EndAttackAnim() {
        meleeMovement.enabled = true;
        agent.isStopped = false;
        ResetAttack();
    }

    protected void StartAttack() {
        canPlayAttack = true;
    }

    protected void EndAttack() {
        canPlayAttack = false;
    }

    protected override void Attack() {
        
    }
}
