using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : EnemyAttack
{
    public GameObject machete;
    MeleeMovement meleeMovement;
    BoxCollider attackBox;

    int attack1Hash, attack2Hash, attack3Hash;

    int canAttackHash;

    protected override void Start() {
        base.Start();

        meleeMovement = GetComponent<MeleeMovement>();

        attack1Hash = Animator.StringToHash("Attack 1");
        attack2Hash = Animator.StringToHash("Attack 2");
        attack3Hash = Animator.StringToHash("Attack 3");

        canAttackHash = Animator.StringToHash("canAttack");

        foreach (BoxCollider boxCollider in machete.GetComponentsInChildren<BoxCollider>()) {
            if (boxCollider.isTrigger) {
                attackBox = boxCollider;
                break;
            }
        }
    }

    protected override void Update()
    {
        animator.SetBool(canAttackHash, vision.WithinAttackRadius() && canAttack);

        if (vision.WithinAttackRadius()
        && canAttack)
        {
            Attack();
        }

        ToggleAttackHitbox();
    }

    private void ToggleAttackHitbox()
    {
        if (canPlayAttack)
        {
            attackBox.enabled = true;
        }

        else
        {
            attackBox.enabled = false;
        }
    }


    protected void StartAttackAnim() {
        meleeMovement.enabled = false;
        agent.isStopped = true;
    }

    protected void EndAttackAnim() {
        agent.isStopped = false;
        meleeMovement.enabled = true;
        ResetAttack();
    }

    protected void StartAttack() {
        canPlayAttack = true;
    }

    protected void EndAttack() {
        canPlayAttack = false;
    }

    protected override void Attack() {
        switch (Random.Range(1, 3)) {
            case 1:
                animator.Play(attack1Hash);
                break;
            case 2:
                animator.Play(attack2Hash);
                break;
            case 3:
                animator.Play(attack3Hash);
                break;
        }

        canAttack = false;
    }
}
