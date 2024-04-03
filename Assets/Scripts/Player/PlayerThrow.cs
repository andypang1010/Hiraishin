using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputController))]
public class PlayerThrow : MonoBehaviour
{
    [Header("References")]
    InputController inputController;
    PlayerTeleport playerTeleport;
    public Transform cam;
    public Transform kunaiAttackPoint;
    public GameObject kunai;

    [Header("Settings")]
    public int totalKunai;
    public float throwCD;
    public int kunaiRemaining;
    PlayerPickup playerPickup;

    [Header("Throw")]
    public float kunaiThrowForce;
    public float kunaiUpwardForce;
    public float throwableThrowForce;
    public float throwableUpwardForce;
    public float maxDistance;
    bool readyToThrow;

    void Start()
    {
        playerPickup = GetComponent<PlayerPickup>();
        playerTeleport = GetComponent<PlayerTeleport>();
        inputController = GetComponent<InputController>();
        kunaiRemaining = totalKunai;
        readyToThrow = true;
    }

    void FixedUpdate()
    {

        // If the player meets throw conditions
        if (inputController.GetThrow() && !playerTeleport.inTeleportMode && readyToThrow && kunaiRemaining > 0) {

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

        // Calculate default force direction
        Vector3 forceDirection = (cam.position + maxDistance * cam.forward - kunaiAttackPoint.position).normalized;

        // Calculate accurate force direction if in range
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxDistance)) {
            forceDirection = (hit.point - kunaiAttackPoint.position).normalized;
        }

        // Calculate throw force
        switch (gameObject.GetComponent<Collider>().tag) {
            case "Kunai":

                // Instantiate kunai
                GameObject kunai = Instantiate(gameObject, kunaiAttackPoint.position, cam.rotation);
                Rigidbody rb = kunai.GetComponent<Rigidbody>();

                // Propel projectile towards force direction
                rb.AddForce(kunaiThrowForce * forceDirection + kunaiUpwardForce * transform.up, ForceMode.Impulse);

                kunaiRemaining--;

                break;

            case "Throwable":

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
