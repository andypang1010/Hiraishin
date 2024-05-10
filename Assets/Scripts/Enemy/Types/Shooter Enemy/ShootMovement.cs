using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMovement : EnemyMovement
{

    void Update()
    {
        if (vision.playerSeen || hearing.PlayerHeard) {
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, transform.up);
        }
    }
}
