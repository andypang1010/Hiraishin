using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : EnemyAttack
{
    public BoxCollider machete;
    protected override void Update() {
        if (vision.WithinAttackRadius()
        && canAttack) {
            animator.Play(attackHash);
        }

        if (canPlayAttack) {
            machete.enabled = true;
        }

        else {
            machete.enabled = false;
        }
    }

    
    protected void StartAttackAnim() {
        agent.isStopped = true;
    }

    protected void EndAttackAnim() {
        agent.isStopped = false;
    }

    protected void StartAttack() {
        canPlayAttack = true;
    }

    protected void EndAttack() {
        canPlayAttack = false;
    }
}
