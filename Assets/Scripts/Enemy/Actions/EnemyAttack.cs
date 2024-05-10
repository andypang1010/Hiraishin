using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("References")]
    public EnemyData data;
    protected PlayerController player;
    protected EnemyVision vision;

    protected bool canAttack;


    protected void Start()
    {
        canAttack = true;
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        vision = GetComponent<EnemyVision>();
    }

    protected virtual void Update() {
        if (vision.WithinAttackRadius()
        && canAttack) {
            
            Attack();
        }
    }

    protected virtual void Attack() {

    }

    protected void ResetAttack() {
        canAttack = true;
    }
}
