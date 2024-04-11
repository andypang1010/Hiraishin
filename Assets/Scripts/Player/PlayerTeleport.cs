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
                UpdateRotation(kunaiHit.collider.gameObject);
                
                Teleport(gameObject, kunaiHit.collider.gameObject);
                GetComponent<PlayerThrow>().kunaiRemaining++;
                
                Destroy(kunaiHit.collider.gameObject);
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
                UpdateRotation(taggedHit.collider.gameObject);
                
                Teleport(gameObject, taggedHit.collider.gameObject);
                Teleport(taggedHit.collider.gameObject, temp);

                Destroy(temp);
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

    void UpdateRotation(GameObject target) {
        Quaternion temp = transform.rotation;

        Vector3 targetEuler = target.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, targetEuler.y, 0);

        // Update camera rotation
        GetComponent<PlayerCamera>().rotationY = targetEuler.y;

        target.transform.rotation = temp;
    }
}
