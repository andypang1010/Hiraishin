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

    void Start() {
        playerPickup = GetComponent<PlayerPickup>();
    }

    void Update()
    {

        // Prevent player from teleporting to tagged heldObjects
        if (playerPickup.heldObj != null 
            && playerPickup.heldObj.layer == LayerMask.NameToLayer("Tagged")) {
            return;
        }


        // When player wants to teleport
        if (InputController.GetTeleportDown()) {

            foreach (GameObject target in teleportables) {
                maxDetectionSize = crosshair.rect.height / 2.5f * (Screen.height / canvasScaler.referenceResolution.y);
                centerPoint = new Vector2(Screen.width / 2, Screen.height / 2);
                Vector2 screenPointPos = cam.gameObject.GetComponent<Camera>().WorldToScreenPoint(target.transform.position);
                
                if (Vector2.Distance(screenPointPos, centerPoint) < maxDetectionSize
                || target.GetComponent<Collider>().Raycast(new Ray(cam.position, cam.forward), out RaycastHit hit, 2f)) {

                    print(target.GetComponent<Collider>().Raycast(new Ray(cam.position, cam.forward), out hit, 2f));

                    if (target.layer == LayerMask.NameToLayer("Kunai")) {
                        UpdateRotation(target);

                        Teleport(gameObject, target);
                        GetComponent<PlayerThrow>().kunaiRemaining++;

                        teleportables.Remove(target);
                        Destroy(target);
                        break;
                    }

                    else if (target.layer == LayerMask.NameToLayer("Tagged")) {
                        GameObject temp = Instantiate(gameObject);
                        UpdateRotation(target);
                
                        Teleport(gameObject, target);
                        Teleport(target, temp);

                        Destroy(temp);
                        break;
                    }
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
