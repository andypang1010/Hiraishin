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
            MoveObj();
        }

        if (Input.GetKeyDown(pickUpKey)) {

            // If player already holding an object
            if (heldObj != null) {
                DropObj();
            }

            // If throwable found within range
            if (Physics.Raycast(cam.transform.position, cam.forward, out RaycastHit hit, maxDistance) 
                && hit.transform.gameObject.CompareTag("Throwable")) {

                PickUpObj(hit.transform.gameObject);
            }

        }
    }

    void PickUpObj(GameObject pickedObj) {
        if (pickedObj.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
            rb.useGravity = false;
            rb.freezeRotation = true;
            rb.drag = 10f;

            pickedObj.transform.SetParent(heldPoint);
            heldObj = pickedObj;
        }
    }

    void MoveObj() {
        if (Vector3.Distance(heldObj.transform.position, heldPoint.position) > 0f) {
            Vector3 moveDirection = heldPoint.position - heldObj.transform.position;

            heldObj.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }

        else if (Vector3.Distance(heldObj.transform.position, heldPoint.position) > 10f) {
            DropObj();
        }

        // heldObj.transform.position = heldPoint.position;
    }

    void DropObj() {
        SetObjAsActive();
    }

    public void SetObjAsActive() {
        if (heldObj.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
                rb.useGravity = true;
                rb.freezeRotation = false;
                rb.drag = 0f;

                heldObj.transform.SetParent(null);
                heldObj = null;
            }
        }
}
