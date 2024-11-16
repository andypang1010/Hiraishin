using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ShootMovement : EnemyMovement
{
    public Rig playerTracking;

    new void Start() {
        base.Start();

        MultiAimConstraint constraint = GetComponentInChildren<MultiAimConstraint>();
        WeightedTransformArray data = constraint.data.sourceObjects;
        data.Clear();
        data.Add(new WeightedTransform(player.transform, 1));
        constraint.data.sourceObjects = data;

        transform.root.GetComponent<RigBuilder>().Build();
    }

    void Update()
    {
        if (player.GetComponent<PlayerController>().isDead) {
            return;
        }

        if (vision.playerSeen || hearing.PlayerHeard) {
            playerDetected = true;
        }

        if (playerDetected) {
            playerTracking.weight = 1;

            TurnToTarget(player.transform.position);
        }

        else {
            playerTracking.weight = 0;
        }
    }
}
