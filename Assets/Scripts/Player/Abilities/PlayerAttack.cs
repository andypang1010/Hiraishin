using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EzySlice;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    public Transform attackPoint;
    public float attackReach;
    public float attackDistance;
    public float attackCD;
    public float attackForce;
    public VisualEffect sliceVFX;
    [HideInInspector] public bool attackReady { get; private set; }

    readonly float[] attackAngles = {0, 45, 90, 135};

    void Start() {
        attackReady = true;
        attackPoint.localScale = new Vector3(attackDistance, 0.1f, attackReach);
        attackPoint.rotation = Quaternion.Euler(attackPoint.rotation.eulerAngles.x, attackPoint.rotation.eulerAngles.y, attackAngles[Random.Range(0, attackAngles.Length)]);
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

        sliceVFX.Play();

        // Get all targets in range
        GameObject[] targetsInRange = Physics.BoxCastAll(
            attackPoint.position, 
            attackPoint.localScale / 2, 
            Camera.main.transform.forward, 
            attackPoint.rotation, 
            1)
            .Select(hit => hit.collider.gameObject).ToArray();
            
        // GameObject closestTarget = null;
        // float distance = Mathf.Infinity;

        // // Find closest target
        // foreach (GameObject target in targetsInRange) {
        //     if (!target.CompareTag("Enemy")) continue;

        //     if (Vector3.SqrMagnitude(target.transform.position - gameObject.transform.position) < distance) {
        //         closestTarget = target;
        //         distance = Vector3.SqrMagnitude(target.transform.position - gameObject.transform.position);
        //     }
        // }

        // Slice if closest target is not obstructed
        // if (closestTarget != null
        //     && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, attackReach)
        //     && hit.transform.gameObject == closestTarget) {

        //     closestTarget.GetComponent<EnemyController>().Kill(hit.collider.gameObject);
        //     print(hit.collider.gameObject);
        // }

        foreach (GameObject target in targetsInRange)
        {
            if (target.TryGetComponent(out Limb limb)) {
                limb.Dismember();
            }
        }

        // Set up next attack rotation
        attackPoint.rotation = Quaternion.Euler(attackPoint.rotation.eulerAngles.x, attackPoint.rotation.eulerAngles.y, attackAngles[Random.Range(0, attackAngles.Length)]);
        print("Calculated angle: " + new Vector3(attackPoint.rotation.eulerAngles.x, attackPoint.rotation.eulerAngles.y, attackAngles[Random.Range(0, attackAngles.Length)]));
        print("Actual angle: " + attackPoint.rotation.eulerAngles);
    }

    void ResetAttackReady() {
        attackReady = true;
    }
}
