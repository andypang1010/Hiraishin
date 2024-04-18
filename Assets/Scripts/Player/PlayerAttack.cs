using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    public float attackCD;
    bool attackReady;
    [Header("Slice")]
    public float sliceForce;
    public Transform[] slicePlanes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputController.GetAttackDown() && attackReady) {
            Attack();

            attackReady = false;
            Invoke(nameof(ResetAttackReady), attackCD);
        }
    }

    void Attack() {
        // TODO: Use OverlapBox to get all gameObjects in range

        // TODO: Select a slice plane randomly from a list

        // TODO: For every gameObject that is an enemy/sliceable, apply slice
    }

    void Slice(GameObject target, Transform slicePlane) {
        SlicedHull hull = target.Slice(slicePlane.position, slicePlane.up);

        if (hull != null) {
            GameObject upperHull = hull.CreateUpperHull(target);
            SetupComponent(upperHull);

            GameObject lowerHull = hull.CreateLowerHull(target);
            SetupComponent(lowerHull);
        }

        Destroy(target);
    }

    void SetupComponent(GameObject slicedObject) {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(sliceForce, slicedObject.transform.position, 1);
    }

    void ResetAttackReady() {
        attackReady = true;
    }
}
