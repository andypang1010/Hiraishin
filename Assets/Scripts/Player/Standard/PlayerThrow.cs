using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [Header("References")]
    public Transform throwPoint;
    public GameObject kunai;

    [Header("Throw")]
    public float kunaiThrowForce;
    public float kunaiUpwardForce;
    public float throwableThrowForce;
    public float throwableUpwardForce;
    public float maxAccurateDistance;
    bool readyToThrow;

    [Header("Settings")]
    public bool kunaiAvailable;
    public int totalKunai;
    public float throwCD;
    [HideInInspector] public int kunaiRemaining { get; private set; }
    PlayerPickup playerPickup;

    void Start()
    {
        playerPickup = GetComponent<PlayerPickup>();
        kunaiRemaining = totalKunai;
        readyToThrow = true;
    }

    void Update() {

        // If the player meets throw conditions
        if (InputController.Instance.GetThrowDown() && readyToThrow && kunaiRemaining > 0) {

            // Throw throwable if it exists
            if (playerPickup.heldObj != null) {

                Throw(playerPickup.heldObj);
                playerPickup.heldObj = null;
            }

            // Throw kunai
            else if (kunaiAvailable) {
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
                GameObject kunai = Instantiate(gameObject, throwPoint.position, Camera.main.transform.rotation);
                PlayerTeleport.teleportables.Add(kunai);

                Rigidbody rb = kunai.GetComponent<Rigidbody>();

                // Calculate default force direction
                forceDirection = (Camera.main.transform.position + maxAccurateDistance * Camera.main.transform.forward - throwPoint.position).normalized;

                // Calculate accurate force direction if in range
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, maxAccurateDistance)) {
                    forceDirection = (hit.point - throwPoint.position).normalized;
                }

                // Propel projectile towards force direction
                rb.AddForce(kunaiThrowForce * forceDirection + kunaiUpwardForce * transform.up, ForceMode.Impulse);

                kunaiRemaining--;

                break;

            case "Throwable":
                // Calculate default force direction
                forceDirection = (Camera.main.transform.position + maxAccurateDistance * Camera.main.transform.forward - playerPickup.throwPoint.position).normalized;

                // Calculate accurate force direction if in range
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxAccurateDistance)) {
                    forceDirection = (hit.point - playerPickup.throwPoint.position).normalized;
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

    
    void OnCollisionEnter(Collision other) {
        if (other.collider.gameObject.tag == "Kunai") {
            PlayerTeleport.teleportables.Remove(other.collider.gameObject);

            Destroy(other.collider.gameObject);

            kunaiRemaining++;
        }
    }

    public void AddKunaiCount(int kunaiCount) {
        kunaiRemaining = Math.Clamp(kunaiRemaining + kunaiCount, 0, totalKunai);
    }
}