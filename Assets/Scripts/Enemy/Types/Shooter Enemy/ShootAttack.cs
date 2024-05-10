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
        && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, data.attackReach)
        && hit.transform.gameObject.CompareTag("Player")
        && canAttack) {
            
            Attack();
        }
    }

    protected override void Attack() {

        if (player == null) {
            return;
        }

        // Propel projectile towards force direction
        Rigidbody bulletRB = Instantiate(bulletRef, shootPoint.position, shootPoint.rotation).GetComponent<Rigidbody>();
        bulletRB.AddForce(shootForce * shootPoint.forward + shootUpwardForce * shootPoint.up, ForceMode.Impulse);


        // Start attack CD
        canAttack = false;
        Invoke(nameof(ResetAttack), data.attackCD);
    }
}
