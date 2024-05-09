using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootAttack : EnemyAttack
{

    [Header("References")]
    public GameObject bullet;

    protected override void Attack() {

        if (player == null) {
            return;
        }

        // TODO: Shoot projectile
        GameObject kunai = Instantiate(gameObject, transform.position, transform.rotation);
        PlayerTeleport.teleportables.Add(kunai);

        Rigidbody rb = kunai.GetComponent<Rigidbody>();

        // TODO: Calculate direction

        // Propel projectile towards force direction
        rb.AddForce(kunaiThrowForce * forceDirection + kunaiUpwardForce * transform.up, ForceMode.Impulse);


        // Start attack CD
        canAttack = false;
        Invoke(nameof(ResetAttack), data.attackCD);
    }
}
