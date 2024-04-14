using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("Camera")]
    public Transform cam;
    public Transform heldPoint;

    [Header("Settings")]
    public float maxDistance;
    public float moveForce;
    public float moveDrag;
    [HideInInspector] public GameObject heldObj;

    void Update() {
        if (InputController.GetPickupDown()) {

            // If player already holding an object
            if (heldObj != null) {
                DropObject();
            }

            // If throwable found within range
            if (Physics.Raycast(cam.transform.position, cam.forward, out RaycastHit hit, maxDistance) 
                // && Physics.Raycast(transform.position, Vector3.down, out RaycastHit underHit, 1.2f)
                // && underHit.transform.gameObject != hit.transform.gameObject
                && hit.collider.gameObject.CompareTag("Throwable")) {

                PickUpObject(hit.collider.gameObject);
            }

        }
    }

    void FixedUpdate()
    {

        // Move throwable with player
        if (heldObj != null) {
            MoveObject();
        }        
    }

    void PickUpObject(GameObject pickedObj) {
        // Pick up settings
        if (pickedObj.TryGetComponent(out Rigidbody rb)) {
            rb.useGravity = false;
            rb.freezeRotation = true;
            rb.drag = moveDrag;

            Physics.IgnoreCollision(pickedObj.GetComponent<Collider>(), gameObject.GetComponentInChildren<Collider>(), true);

            pickedObj.transform.SetParent(heldPoint);
            heldObj = pickedObj;
        }
    }

    void MoveObject() {

        // Move object around the heldPoint
        if (Vector3.Distance(heldObj.transform.position, heldPoint.position) > 0f) {
            Vector3 moveDirection = heldPoint.position - heldObj.transform.position;

            heldObj.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }

        // Automatically drop throwable when far away from heldPoint
        if (Vector3.Distance(heldObj.transform.position, heldPoint.position) > maxDistance) {
            DropObject();
        }
    }

    public void DropObject() {
        // Drop settings
        if (heldObj.TryGetComponent(out Rigidbody rb)) {
                rb.useGravity = true;
                rb.freezeRotation = false;
                rb.drag = 0f;

                Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), gameObject.GetComponentInChildren<Collider>(), false);

                heldObj.transform.SetParent(null);
                heldObj = null;
        }
    }
}
