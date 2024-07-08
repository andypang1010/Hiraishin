using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootAttack : EnemyAttack
{

    [Header("References")]
    public GameObject bulletRef;
    public Transform shootPoint;
    public float shootForce, shootUpwardForce;

    protected override void Update() {
        if (vision.WithinAttackRadius()
        && canAttack) {

            canAttack = false;

            animator.Play(attackHash);
        }
    }

    protected override void Attack() {

        print("Attacked");

        if (player.GetComponent<PlayerController>().isDead) {
            return;
        }

        // Propel projectile towards force direction
        Rigidbody bulletRB = Instantiate(bulletRef, shootPoint.position, shootPoint.rotation).GetComponent<Rigidbody>();
        bulletRB.AddForce(shootForce * shootPoint.forward + shootUpwardForce * shootPoint.up, ForceMode.Impulse);
    }
}
