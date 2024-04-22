using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerTeleport : MonoBehaviour
{
    [Header("References")]
    public CanvasScaler canvasScaler;
    public RectTransform crosshair;
    public Volume volume;
    float maxDetectionSize;
    Vector2 centerPoint;
    LensDistortion lensDistortion;

    [Header("Settings")]
    public float detectionDistance;
    public float maxLensDistortion;
    public float distortionSpeed;
    public static List<GameObject> teleportables = new List<GameObject>();
    PlayerPickup playerPickup;
    Camera cameraObj;

    void Start() {
        playerPickup = GetComponent<PlayerPickup>();
        cameraObj = Camera.main.transform.gameObject.GetComponent<Camera>();
        
        if (!volume.profile.TryGet(out lensDistortion)) {
            Debug.LogWarning("No Lens Distortion component found on Global Volume");
        }
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
                || target.GetComponent<Collider>().Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out _, 2f))
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
                        StartCoroutine(Teleport(closestTarget));
                    }

                    else if (closestTarget.layer == LayerMask.NameToLayer("Tagged"))
                    {
                        GameObject temp = Instantiate(gameObject);
                        StartCoroutine(SwapLocations(closestTarget, temp));
                    }
            }
        }
    }

    void TeleportObjects(GameObject source, GameObject target) {

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

    IEnumerator Teleport(GameObject closestTarget) {
        while (lensDistortion.intensity.value < 0.5) {
            lensDistortion.intensity.value += Time.deltaTime * 2;
            yield return null;
        }

        UpdateRotation(closestTarget);
        TeleportObjects(gameObject, closestTarget);
        GetComponent<PlayerThrow>().AddKunaiCount(1);
        
        teleportables.Remove(closestTarget);
        Destroy(closestTarget);

        if (lensDistortion.intensity.value >0.5) {
            while (lensDistortion.intensity.value > 0) {
                lensDistortion.intensity.value -= Time.deltaTime * 2;
                yield return null;
            } 
        }
    }

    IEnumerator SwapLocations(GameObject closestTarget, GameObject temp) {
        while (lensDistortion.intensity.value < maxLensDistortion) {
            lensDistortion.intensity.value += Time.deltaTime * distortionSpeed;
            yield return null;
        }

        UpdateRotation(closestTarget);
        TeleportObjects(gameObject, closestTarget);
        TeleportObjects(closestTarget, temp);

        Destroy(temp);

        if (lensDistortion.intensity.value > maxLensDistortion) {
            while (lensDistortion.intensity.value > 0) {
                lensDistortion.intensity.value -= Time.deltaTime * distortionSpeed;
                yield return null;
            } 
        }
    }
}
