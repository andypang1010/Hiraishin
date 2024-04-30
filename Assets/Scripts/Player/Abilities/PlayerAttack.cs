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
    [HideInInspector] public bool attackReady { get; private set; }

    void Start() {
        attackReady = true;
        attackPoint.localScale = new Vector3(attackReach, attackDistance, 0.1f);
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

            closestTarget.GetComponent<EnemyController>().Slice(attackPoint, attackForce);
        }
    }

    void ResetAttackReady() {
        attackReady = true;
    }
}
