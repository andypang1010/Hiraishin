using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAttack : MonoBehaviour
{
    [Header("References")]
    public EnemyData data;
    protected PlayerController player;
    protected EnemyVision vision;

    protected bool canAttack;

    protected bool canPlayAttack;

    protected NavMeshAgent agent;

    protected Animator animator;

    protected int attackHash;


    protected virtual void Start()
    {
        canAttack = true;
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        vision = GetComponent<EnemyVision>();

        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
        attackHash = Animator.StringToHash("Attack");
    }

    protected abstract void Update();

    protected abstract void Attack();

    protected void ResetAttack() {
        canAttack = true;
    }
}
