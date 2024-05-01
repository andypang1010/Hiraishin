using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAlert : MonoBehaviour
{
    public EnemyData data;
    public float alertRadius;
    public EnemyVision vision;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (vision.playerSeen) {

            // Find all nearby enemies
            GameObject[] nearbyEnemies = Physics.OverlapSphere(transform.position, alertRadius).
            Select(collider => collider.gameObject.tag == "Enemy" ? collider.gameObject : null)
            .ToArray();

            // Alert them of player's position
            foreach (GameObject enemy in nearbyEnemies) {
                if (enemy.TryGetComponent(out EnemyVision vision)) {
                    vision.playerSeen = true;
                }
            }
        }
    }
}
