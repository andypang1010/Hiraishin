using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMovement : EnemyMovement
{

    void Update()
    {
        if (player == null) {
            return;
        }

        if (vision.playerSeen || hearing.PlayerHeard) {
            Vector3 playerDirection = player.transform.position - transform.position;
            Quaternion temp = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDirection), 50 * Time.deltaTime);

            transform.rotation = Quaternion.Euler(transform.rotation.x, temp.eulerAngles.y, transform.rotation.z);
        }
    }
}
