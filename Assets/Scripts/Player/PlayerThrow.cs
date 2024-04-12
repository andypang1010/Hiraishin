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
    public KeyCode throwKey;
    public int totalKunai;
    public float throwCD;
    public int kunaiRemaining;
    PlayerPickup playerPickup;

    [Header("Throw")]
    public float kunaiThrowForce;
    public float kunaiUpwardForce;
    public float throwableThrowForce;
    public float throwableUpwardForce;
    public float maxAccurateDistance;
    public bool readyToThrow;

    bool throwConditions;

    void Start()
    {
        playerPickup = GetComponent<PlayerPickup>();
        kunaiRemaining = totalKunai;
        readyToThrow = true;
    }

    void Update() {

        // If the player meets throw conditions
        if (Input.GetKeyDown(throwKey) && readyToThrow && kunaiRemaining > 0) {

            // Throw throwable if it exists
            if (playerPickup.heldObj != null) {

                Throw(playerPickup.heldObj);
                playerPickup.heldObj = null;
            }

            // Throw kunai
            else {
                Throw(kunai);
            }
        }
    }

    void Throw(GameObject gameObject) {
        readyToThrow = false;

        Vector3 forceDirection = new Vector3();
        
        // Calculate throw force
        switch (gameObject.GetComponent<Collider>().tag) {
            case "Kunai":

                // Instantiate kunai
                GameObject kunai = Instantiate(gameObject, kunaiAttackPoint.position, cam.rotation);
                Rigidbody rb = kunai.GetComponent<Rigidbody>();

                // Calculate default force direction
                forceDirection = (cam.position + maxAccurateDistance * cam.forward - kunaiAttackPoint.position).normalized;

                // Calculate accurate force direction if in range
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxAccurateDistance)) {
                    forceDirection = (hit.point - kunaiAttackPoint.position).normalized;
                }

                // Propel projectile towards force direction
                rb.AddForce(kunaiThrowForce * forceDirection + kunaiUpwardForce * transform.up, ForceMode.Impulse);

                kunaiRemaining--;

                break;

            case "Throwable":
                // Calculate default force direction
                forceDirection = (cam.position + maxAccurateDistance * cam.forward - playerPickup.heldPoint.position).normalized;

                // Calculate accurate force direction if in range
                if (Physics.Raycast(cam.position, cam.forward, out hit, maxAccurateDistance)) {
                    forceDirection = (hit.point - playerPickup.heldPoint.position).normalized;
                }

                playerPickup.heldObj.GetComponent<Rigidbody>().AddForce(throwableThrowForce * forceDirection + throwableUpwardForce * transform.up, ForceMode.Impulse);
                playerPickup.DropObject();
                break;

        }

        // Apply force
        Invoke(nameof(ResetThrow), throwCD);
    }

    void ResetThrow() {
        readyToThrow = true;
    }
}
