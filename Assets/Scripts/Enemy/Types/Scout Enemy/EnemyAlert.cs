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
        List<EnemyVision> nearbyEnemies = Physics.OverlapSphere(transform.position, data.alertRadius)
            .Where(collider => collider.transform.root.TryGetComponent(out EnemyVision _))
            .Select(collider => collider.transform.root.GetComponent<EnemyVision>())
            .ToArray().Distinct().ToList();

        nearbyEnemies.Remove(GetComponent<EnemyVision>());

        // Alert them of player's position
        foreach (EnemyVision vision in nearbyEnemies) {
            vision.playerSeen = true;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, data.alertRadius);
    }
}
