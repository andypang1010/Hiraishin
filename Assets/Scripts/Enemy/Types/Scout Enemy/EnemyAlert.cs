using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAlert : MonoBehaviour
{
    public EnemyData data;
    EnemyVision vision;
    EnemyHearing hearing;

    void Start()
    {
        vision = GetComponent<EnemyVision>();
        hearing = GetComponent<EnemyHearing>();
    }

    void Update()
    {
        if (vision.playerSeen || hearing.PlayerHeard) {

            // Find all nearby enemies
            GameObject[] nearbyEnemies = Physics.OverlapSphere(transform.position, data.alertRadius).
            Select(collider => collider.transform.gameObject.CompareTag("Enemy") ? collider.transform.gameObject : null)
            .ToArray();

            // Alert them of player's position
            foreach (GameObject enemy in nearbyEnemies) {
                
                if (enemy != null && enemy.TryGetComponent(out EnemyVision vision)) {
                    vision.playerSeen = true;
                }
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, data.alertRadius);
    }
}
