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
            print("PLAYER PICKUP: HELDOBJ NOT NULL");
            MoveObj();
        }

        if (Input.GetKeyDown(pickUpKey)) {

            // If player already holding an object
            if (heldObj == null) {

                // If throwable found within range
                if (Physics.Raycast(cam.transform.position, cam.forward, out RaycastHit hit, maxDistance) 
                    && hit.transform.gameObject.CompareTag("Throwable")) {

                    PickUpObj(hit.transform.gameObject);
                    print("PICKED UP OBJ");
                }
            }

            else {
                DropObj();
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
        heldObj.transform.position = heldPoint.position;
    }

    void DropObj() {
        if (heldObj.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
            rb.useGravity = true;
            rb.freezeRotation = false;
            rb.drag = 0f;

            heldObj.transform.SetParent(heldPoint);
            heldObj = null;
        }
    }
}
