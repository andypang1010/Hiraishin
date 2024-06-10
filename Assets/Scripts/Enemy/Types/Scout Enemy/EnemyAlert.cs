using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAlert : MonoBehaviour
{
    public EnemyData data;
    EnemyVision vision;
    EnemyHearing hearing;

    NavMeshAgent agent;
    bool alertPlayed;
    Animator animator;
    int alertHash;

    void Start()
    {
        vision = GetComponent<EnemyVision>();
        hearing = GetComponent<EnemyHearing>();

        agent = GetComponent<NavMeshAgent>();
        alertPlayed = false;

        animator = GetComponent<Animator>();
        alertHash = Animator.StringToHash("Alert");
    }

    void Update()
    {
        if ((vision.playerSeen || hearing.PlayerHeard) && !alertPlayed) {

            animator.Play(alertHash);
            alertPlayed = true;
        }
    }

    public void StartAlert() {
        agent.isStopped = true;
    }

    public void EndAlert() {
        agent.isStopped = false;
    }

    public void Alert() {
        // Find all nearby enemies
        GameObject[] nearbyEnemies = Physics.OverlapSphere(transform.position, data.alertRadius).
        Select(collider => collider.transform.gameObject.CompareTag("Enemy") ? collider.transform.gameObject : null)
        .ToArray();

        // Alert them of player's position
        foreach (GameObject enemy in nearbyEnemies) {

            print(enemy);
            
            if (enemy != null && enemy.TryGetComponent(out EnemyVision vision)) {
                vision.playerSeen = true;
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, data.alertRadius);
    }
}
