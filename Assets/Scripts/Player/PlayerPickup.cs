using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform cam;
    public Transform heldPoint;
    public KeyCode pickUpKey;
    public float maxDistance;
    public float moveForce;
    public float moveDrag;
    [HideInInspector] public GameObject heldObj;

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

            // If throwable found within range and is not directly below the player (preventing flying with throwable)
            if (Physics.Raycast(cam.transform.position, cam.forward, out RaycastHit hit, maxDistance) 
                && Physics.Raycast(transform.position, Vector3.down, out RaycastHit underHit, 1.2f)
                && underHit.transform.gameObject != hit.transform.gameObject
                && hit.transform.gameObject.CompareTag("Throwable")) {

                PickUpObject(hit.transform.gameObject);
            }

        }
    }

    void PickUpObject(GameObject pickedObj) {
        // Pick up settings
        if (pickedObj.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
            rb.useGravity = false;
            rb.freezeRotation = true;
            rb.drag = moveDrag;

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
        if (Vector3.Distance(heldObj.transform.position, heldPoint.position) > 2f) {
            DropObject();
        }
    }

    public void DropObject() {
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
