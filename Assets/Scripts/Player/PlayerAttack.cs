using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EzySlice;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Camera")]
    public Transform cam;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackReach;
    public float attackDistance;
    public LayerMask attackLayer;
    public float attackCD;
    public float attackForce;
    public bool attackReady;

    [Header("Sinking")]
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
            cam.forward, 
            attackPoint.rotation, 
            1,
            attackLayer).Select(hit => hit.collider.gameObject).ToArray();
            
            GameObject closestTarget = null;
            float distance = Mathf.Infinity;

            // Find closest target
            foreach (GameObject target in targetsInRange) {
                if (Vector3.SqrMagnitude(target.transform.position - gameObject.transform.position) < distance) {
                    closestTarget = target;
                    distance = Vector3.SqrMagnitude(target.transform.position - gameObject.transform.position);
                }
            }

            // Slice if closest target
            if (closestTarget != null) {
                Slice(closestTarget, attackPoint);
            }
    }

    void Slice(GameObject target, Transform slicePlane) {
        SlicedHull hull = target.Slice(slicePlane.position, slicePlane.up, target.GetComponent<Renderer>().material);

        // Create upper and lower hulls
        if (hull != null) {
            GameObject upperHull = hull.CreateUpperHull(target);
            SetupComponent(upperHull);

            GameObject lowerHull = hull.CreateLowerHull(target);
            SetupComponent(lowerHull);
        }

        // Automatically collects all kunais on the target
        playerThrow.kunaiRemaining += target.GetComponentsInChildren<Kunai>().Length;
        Destroy(target);
    }

    void SetupComponent(GameObject slicedObject) {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(attackForce, slicedObject.transform.position, 1);
        StartCoroutine(Sink(slicedObject));
    }

    IEnumerator Sink(GameObject target)
    {
        yield return new WaitForSeconds(attackCD + 0.5f);
        target.GetComponent<Rigidbody>().isKinematic = true;
        target.GetComponent<MeshCollider>().enabled = false;

        float time = 0;
        while (time < destroyTime)
        {
            target.transform.position = new Vector3(
                target.transform.position.x, 
                target.transform.position.y - sinkSpeed, 
                target.transform.position.z
            );

            time += Time.deltaTime;
            yield return null;
        }

        Destroy(target);
    }

    void ResetAttackReady() {
        attackReady = true;
    }
}
