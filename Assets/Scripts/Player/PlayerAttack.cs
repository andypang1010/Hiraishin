using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EzySlice;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    public Transform attackPoint;
    public float attackReach;
    public float attackDistance;
    public float attackCD;
    public float attackForce;
    [HideInInspector] public bool attackReady {
        get; private set;
    }

    [Header("Sinking")]
    public float startSinkTime;
    public float sinkSpeed;
    public float destroyTime;

    PlayerThrow playerThrow;

    void Start() {
        attackReady = true;
        attackPoint.localScale = new Vector3(attackReach, attackDistance, 0.1f);

        playerThrow = GetComponent<PlayerThrow>();
    }

    void Update()
    {
        if (InputController.Instance.GetAttackDown() && attackReady) {
            Attack();

            attackReady = false;
            Invoke(nameof(ResetAttackReady), attackCD);
        }
    }

    void Attack() {

        // Set up attack rotation
        attackPoint.rotation = Quaternion.Euler(Random.Range(45, 135), 90, 0);

        // Get all targets in range
        GameObject[] targetsInRange = Physics.BoxCastAll(
            attackPoint.position, 
            attackPoint.localScale / 2, 
            Camera.main.transform.forward, 
            attackPoint.rotation, 
            1)
            .Select(hit => hit.collider.gameObject).ToArray();
            
        GameObject closestTarget = null;
        float distance = Mathf.Infinity;

        // Find closest target
        foreach (GameObject target in targetsInRange) {
            if (!target.CompareTag("Enemy")) continue;

            if (Vector3.SqrMagnitude(target.transform.position - gameObject.transform.position) < distance) {
                closestTarget = target;
                distance = Vector3.SqrMagnitude(target.transform.position - gameObject.transform.position);
            }
        }

        // Slice if closest target is not obstructed
        if (closestTarget != null
            && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, attackReach)
            && hit.collider.gameObject == closestTarget) {

            Slice(closestTarget, attackPoint);
        }
    }

    void Slice(GameObject target, Transform slicePlane) {
        SlicedHull hull = target.Slice(slicePlane.position, slicePlane.up, target.GetComponent<Renderer>().material);

        if (hull != null) {

            // Create upper and lower hulls
            GameObject upperHull = hull.CreateUpperHull(target);
            GameObject lowerHull = hull.CreateLowerHull(target);
            SetupComponent(upperHull);
            SetupComponent(lowerHull);
            
            // Automatically collects all kunais on the target
            playerThrow.AddKunaiCount(target.GetComponentsInChildren<Kunai>().Length);

            // Remove target from teleportables list
            PlayerTeleport.teleportables.Remove(target);
            Destroy(target);
        }

    }

    void SetupComponent(GameObject slicedObject) {

        // Adding rigidBody and meshCollider to slicedObject
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(attackForce, slicedObject.transform.position, 1);
        StartCoroutine(Sink(slicedObject));
    }

    IEnumerator Sink(GameObject target)
    {
        yield return new WaitForSeconds(startSinkTime);
        target.GetComponent<MeshCollider>().enabled = false;
        target.GetComponent<Rigidbody>().isKinematic = true;

        // Destroy all kunais on the target
        foreach (Kunai kunai in target.GetComponentsInChildren<Kunai>()) {
            Destroy(kunai.gameObject);
        }
        playerThrow.AddKunaiCount(target.GetComponentsInChildren<Kunai>().Length);

        // Start sinking
        float time = 0;
        while (time < destroyTime)
        {
            target.transform.position = new Vector3(
                target.transform.position.x, 
                target.transform.position.y - sinkSpeed, 
                target.transform.position.z
            );

            time += Time.fixedDeltaTime;
            yield return null;
        }

        Destroy(target);
    }

    void ResetAttackReady() {
        attackReady = true;
    }
}
