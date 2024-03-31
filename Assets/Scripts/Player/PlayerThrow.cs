using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform kunaiAttackPoint;
    public GameObject kunai;

    [Header("Settings")]
    public int totalKunai;
    public float throwCD;
    int kunaiRemaining;
    PlayerPickup playerPickup;

    [Header("Throw")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float kunaiThrowForce;
    public float kunaiUpwardForce;
    public float throwableThrowForce;
    public float throwableUpwardForce;
    public float maxDistance;
    bool readyToThrow;

    void Start()
    {
        playerPickup = GetComponent<PlayerPickup>();
        kunaiRemaining = totalKunai;
        readyToThrow = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && kunaiRemaining > 0) {

            // Throw throwable if it's available
            if (playerPickup.heldObj != null) {

                Throw(playerPickup.heldObj);
                playerPickup.heldObj = null;
            }

            // Throw kunai
            else {
                Throw(kunai);
                kunaiRemaining--;
            }
        }
    }

    void Throw(GameObject gameObject) {
        readyToThrow = false;

        // Calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        // Calculate new force direction if within range
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance)) {
            forceDirection = (hit.point - kunaiAttackPoint.position).normalized;
        }

        // Calculate throw force
        switch (gameObject.GetComponent<Collider>().tag) {
            case "Kunai":
                // Instantiate projectile
                GameObject projectile = Instantiate(gameObject, kunaiAttackPoint.position, cam.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance)) {
                    forceDirection = (hit.point - kunaiAttackPoint.position).normalized;
                }

                rb.AddForce(kunaiThrowForce * forceDirection + kunaiUpwardForce * transform.up, ForceMode.Impulse);
                break;

            case "Throwable":
                Rigidbody throwRB = gameObject.GetComponent<Rigidbody>();
                throwRB.useGravity = true;
                throwRB.freezeRotation = false;
                throwRB.drag = 0;
                gameObject.transform.SetParent(null);

                if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance)) {
                    forceDirection = (hit.point - playerPickup.heldPoint.position).normalized;
                }

                throwRB.AddForce(throwableThrowForce * forceDirection + throwableUpwardForce * transform.up, ForceMode.Impulse);
                break;

        }
        
        // Apply force
        Invoke(nameof(ResetThrow), throwCD);
    }

    void ResetThrow() {
        readyToThrow = true;
    }
}
