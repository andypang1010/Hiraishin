using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [Header("References")]
    public Transform throwPoint;

    [Header("Kunai")]
    public bool kunaiAvailable;
    public GameObject kunai;
    public int totalKunai;
    public float kunaiThrowForce;
    public float kunaiUpwardForce;
    [HideInInspector] public int kunaiRemaining { get; private set; }

    [Header("Other")]
    public float throwableThrowForce;
    public float throwableUpwardForce;

    [Header("Settings")]
    public float maxAccurateDistance;
    public float throwCD;
    bool readyToThrow;
    PlayerPickup playerPickup;

    void Start()
    {
        playerPickup = GetComponent<PlayerPickup>();
        kunaiRemaining = totalKunai;
        readyToThrow = true;
    }

    void Update() {

        // If the player meets throw conditions
        if (InputController.Instance.GetThrowDown() && readyToThrow) {

            // Throw throwable if it exists
            if (playerPickup.heldObj != null) {

                Throw(playerPickup.heldObj);
                playerPickup.heldObj = null;
            }

            // Throw kunai
            else if (kunaiAvailable && kunaiRemaining > 0) {
                Throw(kunai);
            }
        }
    }

    void Throw(GameObject gameObject) {
        readyToThrow = false;

        // Calculate default force direction
        Vector3 forceDirection = (Camera.main.transform.position + maxAccurateDistance * Camera.main.transform.forward - throwPoint.position).normalized;
        
        // Calculate accurate force direction if in range
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, maxAccurateDistance, ~0, QueryTriggerInteraction.Ignore)) {
            forceDirection = (hit.point - throwPoint.position).normalized;
        }

        // Calculate throw force
        switch (gameObject.GetComponent<Collider>().tag) {
            case "Kunai":

                // Instantiate kunai
                GameObject kunai = Instantiate(gameObject, throwPoint.position, Camera.main.transform.rotation);
                PlayerTeleport.teleportables.Add(kunai);

                // Propel projectile towards force direction
                kunai.GetComponent<Rigidbody>().AddForce(kunaiThrowForce * forceDirection + kunaiUpwardForce * transform.up, ForceMode.Impulse);

                kunaiRemaining--;
                break;

            case "Throwable":

                if (playerPickup.heldObj.TryGetComponent(out Distraction distraction)) {
                    distraction.EnableDistraction();
                }

                // Propel projectile towards force direction
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
