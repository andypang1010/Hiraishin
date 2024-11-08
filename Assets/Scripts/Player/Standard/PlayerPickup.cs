using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [Header("References")]
    public Transform throwPoint;

    [Header("Settings")]
    public float maxDistance;
    public float moveForce;
    public float moveDrag;
    [HideInInspector] public GameObject heldObj;

    void Update() {
        if (InputController.Instance.GetPickupDown()) {

            // If player already holding an object
            if (heldObj != null) {
                DropObject();
            }

            // If throwable found within range
            if (Physics.Raycast(Camera.main.transform.transform.position, Camera.main.transform.forward, out RaycastHit hit, maxDistance))
            {
                switch (hit.collider.gameObject.tag) {
                    case "Throwable":
                        PickUpObject(hit.collider.gameObject);
                        break;
                    case "Interactable":
                        Interactables interactable = hit.collider.gameObject.GetComponent<Interactables>();
                        if (interactable.isActivated) {
                            
                        }
                        hit.collider.gameObject.GetComponent<Interactables>().OnInteract();
                        break;
                    default:
                        Debug.LogWarning("Trying to pick up " + hit.collider.gameObject + ", of tag " + hit.collider.gameObject.tag);
                        break;
                }
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

            pickedObj.transform.SetParent(throwPoint);
            heldObj = pickedObj;
        }
    }

    void MoveObject() {

        // Move object around the heldPoint
        if (Vector3.Distance(heldObj.transform.position, throwPoint.position) > 0f) {
            Vector3 moveDirection = throwPoint.position - heldObj.transform.position;

            heldObj.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }

        // Automatically drop throwable when far away from heldPoint
        if (Vector3.Distance(heldObj.transform.position, throwPoint.position) > maxDistance) {
            DropObject();
        }
    }

    public void DropObject() {
        // Drop settings
        if (heldObj != null && heldObj.TryGetComponent(out Rigidbody rb)) {
                rb.useGravity = true;
                rb.freezeRotation = false;
                rb.drag = 0f;

                Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), gameObject.GetComponentInChildren<Collider>(), false);

                heldObj.transform.SetParent(null);
                heldObj = null;
        }
    }

    private void OnDestroy() {
        DropObject();
    }
}
