using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform cam;
    public KeyCode teleportSelectKey;
    public float detectionSize;
    public float detectionDistance;
    PlayerBulletTime playerBulletTime;

    void Start() {
        playerBulletTime = GetComponent<PlayerBulletTime>();
    }

    void Update()
    {
        // If a kunai is found and selectKey pressed
        if (Input.GetKeyDown(teleportSelectKey)) {
            if (Physics.SphereCast(
                cam.position, 
                detectionSize, 
                cam.forward,
                out RaycastHit kunaiHit, 
                detectionDistance, 
                LayerMask.GetMask("Kunai"))
            ) {
                Teleport(gameObject, kunaiHit.collider.gameObject);
                GetComponent<PlayerThrow>().kunaiRemaining++;

                Destroy(kunaiHit.collider.gameObject);
                
                if (playerBulletTime.inBulletTime) {
                    playerBulletTime.SetCooldown();
                }
            }

            else if (Physics.SphereCast(
                cam.position, 
                detectionSize, 
                cam.forward,
                out RaycastHit taggedHit, 
                detectionDistance, 
                LayerMask.GetMask("Tagged"))
            ) {
                GameObject temp = Instantiate(gameObject);
                Teleport(gameObject, taggedHit.collider.gameObject);
                Teleport(taggedHit.collider.gameObject, temp);

                Destroy(temp);

                if (playerBulletTime.inBulletTime) {
                    playerBulletTime.SetCooldown();
                }
            }
        }
    }

    void Teleport(GameObject source, GameObject target) {

        Rigidbody sourceRB = source.GetComponent<Rigidbody>();
        // Inherit the velocity of moving objects
        if (target.TryGetComponent(out Rigidbody targetRB)) {
            sourceRB.GetComponent<Rigidbody>().velocity = new Vector3(targetRB.velocity.x, 0, targetRB.velocity.z) / targetRB.mass;
        }
        
        sourceRB.MovePosition(target.transform.position + 0.1f * Vector3.up);
    }
}
