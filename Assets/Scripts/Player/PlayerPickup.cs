using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform cam;
    public Transform heldPoint;
    public KeyCode pickUpKey;
    public GameObject heldObj;
    public float maxDistance;
    public float moveForce;

    void Update()
    {

        // Move throwable with player
        if (heldObj != null) {
            MoveObject();
        }

        if (Input.GetKeyDown(pickUpKey)) {

            // If player already holding an object
            if (heldObj != null) {
                DropObject();
            }

            // If throwable found within range
            if (Physics.Raycast(cam.transform.position, cam.forward, out RaycastHit hit, maxDistance) 

                // Alternative approach to prevent flying with throwable underneath (raycast below and disable pickup if throwable is underneath player)
                // && Physics.Raycast(transform.position, Vector3.down, out RaycastHit underHit, 1.2f)
                // && underHit.transform.gameObject != hit.transform.gameObject
                && hit.transform.gameObject.CompareTag("Throwable")) {

                PickUpObject(hit.transform.gameObject);
            }

        }
    }

    void PickUpObject(GameObject pickedObj) {

        // Disable collider to prevent player flying on top of throwable when picking up
        if (pickedObj.TryGetComponent<Collider>(out Collider collider)) {
            collider.enabled = false;
        }

        // Pick up settings
        if (pickedObj.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
            rb.useGravity = false;
            rb.freezeRotation = true;
            rb.drag = 10f;

            pickedObj.transform.SetParent(heldPoint);
            heldObj = pickedObj;
        }
    }

    void MoveObject() {

        // Move object around the heldPoint
        if (Vector3.Distance(heldObj.transform.position, heldPoint.position) > 0.1f) {
            Vector3 moveDirection = heldPoint.position - heldObj.transform.position;

            heldObj.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }

        // Re-enable colliders once at heldPoint
        else {
            heldObj.GetComponent<Collider>().enabled = true;
        }

        // Automatically drop throwable when far away from heldPoint
        if (Vector3.Distance(heldObj.transform.position, heldPoint.position) > 2f) {
            DropObject();
        }
    }

    public void DropObject() {

        // Re-enable colliders
        if (heldObj.TryGetComponent<Collider>(out Collider collider)) {
            collider.enabled = true;
        }

        // Drop settings
        if (heldObj.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
                rb.useGravity = true;
                rb.freezeRotation = false;
                rb.drag = 0f;

                heldObj.transform.SetParent(null);
                heldObj = null;
            }
        }
}
