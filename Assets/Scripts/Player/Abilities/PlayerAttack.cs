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
        attackPoint.localScale = new Vector3(attackDistance, attackPoint.localScale.y, attackReach);
        attackPoint.rotation = Quaternion.Euler(attackPoint.rotation.eulerAngles.x, attackPoint.rotation.eulerAngles.y, attackAngles[Random.Range(0, attackAngles.Length)]);
    }

    void Update()
    {
        if (InputController.Instance.GetAttackDown() && attackReady) {
            Attack();

            // Set up next attack rotation
            attackPoint.rotation = Quaternion.Euler(attackPoint.rotation.eulerAngles.x, attackPoint.rotation.eulerAngles.y, attackAngles[Random.Range(0, attackAngles.Length)]);

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
            0.25f,
            LayerMask.GetMask("Enemy", "Bullet"))
            .Select(hit => hit.collider.gameObject).ToArray();

        foreach (GameObject target in targetsInRange)
        {
            if (target.TryGetComponent(out Bullet _)) {
                Destroy(target);
            }

            else if (target.TryGetComponent(out Limb limb)) {
                limb.Dismember();
                limb.GetComponent<Rigidbody>().AddExplosionForce(attackForce, limb.transform.position, 1f);
            }

            else if (target.transform.root.TryGetComponent(out EnemyController enemyController)) {
                enemyController.Die();
            }
        }
    }

    void ResetAttackReady() {
        attackReady = true;
    }
}
