using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTeleport : MonoBehaviour
{
    [Header("Camera")]
    public Transform cam;
    public CanvasScaler canvasScaler;
    public RectTransform crosshair;
    float maxDetectionSize;
    Vector2 centerPoint;

    [Header("Settings")]
    public float detectionDistance;
    public static List<GameObject> teleportables = new List<GameObject>();
    PlayerPickup playerPickup;
    Camera cameraObj;

    void Start() {
        playerPickup = GetComponent<PlayerPickup>();
        cameraObj = cam.gameObject.GetComponent<Camera>();
    }

    void Update()
    {

        // Prevent player from teleporting to tagged heldObjects
        if (playerPickup.heldObj != null 
            && playerPickup.heldObj.layer == LayerMask.NameToLayer("Tagged")) {
            return;
        }

        // When player wants to teleport
        if (InputController.Instance.GetTeleportDown()) {

            // Calculate the radius crosshair and centerpoint of the screen
            maxDetectionSize = (float) Math.Pow(crosshair.rect.height / 2.5f * (Screen.height / canvasScaler.referenceResolution.y), 2);
            centerPoint = new Vector2(Screen.width / 2, Screen.height / 2);

            GameObject closestTarget = null;

            // Iterate through all teleportables
            foreach (GameObject target in teleportables) {

                // Convert target's world position to screen position
                Vector2 screenPointPos = cameraObj.WorldToScreenPoint(target.transform.position);

                // If the target position is within the crosshair radius
                if (Vector2.SqrMagnitude(screenPointPos - centerPoint) < maxDetectionSize

                // Alternate check for close proximity
                || target.GetComponent<Collider>().Raycast(new Ray(cam.position, cam.forward), out _, 2f))
                {
                    if (closestTarget == null) {
                        closestTarget = target;
                    }

                    // If the new target is closer 
                    else if (Vector3.SqrMagnitude(target.transform.position - transform.position) 
                        < Vector3.SqrMagnitude(closestTarget.transform.position - transform.position)) {
                        closestTarget = target;
                    }
                }
            }

            if (closestTarget != null) {
                if (closestTarget.layer == LayerMask.NameToLayer("Kunai"))
                    {
                        UpdateRotation(closestTarget);

                        Teleport(gameObject, closestTarget);
                        GetComponent<PlayerThrow>().kunaiRemaining++;

                        teleportables.Remove(closestTarget);
                        Destroy(closestTarget);
                    }

                    else if (closestTarget.layer == LayerMask.NameToLayer("Tagged"))
                    {
                        GameObject temp = Instantiate(gameObject);
                        UpdateRotation(closestTarget);

                        Teleport(gameObject, closestTarget);
                        Teleport(closestTarget, temp);

                        Destroy(temp);
                    }
            }
        }
    }

    void Teleport(GameObject source, GameObject target) {

        Rigidbody sourceRB = source.GetComponent<Rigidbody>();

        // Inherit the velocity of moving objects
        if (target.TryGetComponent(out Rigidbody targetRB)) {
            sourceRB.GetComponent<Rigidbody>().velocity = new Vector3(targetRB.velocity.x, 0, targetRB.velocity.z) / targetRB.mass;
        }
        
        sourceRB.MovePosition(target.transform.position + 0.1f * Vector3.up);
    }

    void UpdateRotation(GameObject target) {
        Quaternion temp = transform.rotation;

        Vector3 targetEuler = target.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, targetEuler.y, 0);

        // Update camera rotation
        GetComponent<PlayerCamera>().rotationY = targetEuler.y;

        target.transform.rotation = temp;
    }
}
