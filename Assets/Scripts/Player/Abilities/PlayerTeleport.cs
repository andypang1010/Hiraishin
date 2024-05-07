using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerTeleport : MonoBehaviour
{
    [Header("References")]
    public CanvasScaler canvasScaler;
    public GameObject teleportCrosshair;
    public Volume volume;
    float detectionRadius;
    Vector2 centerPoint;
    RectTransform crosshairRectTransform;
    Image crosshairImage;
    LensDistortion lensDistortion;

    [Header("Settings")]
    public float detectionDistance;
    public Color defaultColor;
    public Color detectedColor;
    public float maxLensDistortion;
    public float distortionSpeed;
    [HideInInspector] public static List<GameObject> teleportables = new List<GameObject>();
    PlayerPickup playerPickup;

    void Start() {
        crosshairRectTransform = teleportCrosshair.GetComponent<RectTransform>();
        crosshairImage = teleportCrosshair.GetComponent<Image>();

        playerPickup = GetComponent<PlayerPickup>();
        
        if (!volume.profile.TryGet(out lensDistortion)) {
            Debug.LogWarning("No Lens Distortion component found on Global Volume");
        }
    }

    void Update()
    {
        // Calculate the radius crosshair and centerpoint of the screen
        detectionRadius = crosshairRectTransform.rect.height / 2f * (Screen.height / canvasScaler.referenceResolution.y);
        centerPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        GameObject closestTarget = null;

        // Iterate through all teleportables
        foreach (GameObject target in teleportables) {

            if (target == playerPickup.heldObj) {
                continue;
            }

            // Convert target's world position to screen position
            Vector2 screenPointPos = Camera.main.WorldToScreenPoint(target.transform.position);

            // If the target position is within the crosshair radius and within 
            if ((Vector2.SqrMagnitude(screenPointPos - centerPoint) <= Mathf.Pow(detectionRadius, 2)
            && Vector3.SqrMagnitude(target.transform.position - transform.position) <= Mathf.Pow(detectionDistance, 2))

            // Alternate check for larger objects/close proximity
            || target.GetComponent<Collider>().Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out _, detectionDistance))
            {

                // if (target.GetComponent<Collider>().Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out _, detectionDistance)) {
                //     print("Raycast found");
                // }

                // else {
                //     print("ScreenPoint found");
                // }

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
            crosshairImage.color = detectedColor;

            // When player wants to teleport
            if (InputController.Instance.GetTeleportDown()) {

                if (closestTarget.layer == LayerMask.NameToLayer("Kunai")) {
                        StartCoroutine(Teleport(closestTarget));
                }

                else if (closestTarget.layer == LayerMask.NameToLayer("Tagged")) {
                    GameObject temp = Instantiate(gameObject);
                    StartCoroutine(SwapLocations(closestTarget, temp));
                }
            }
        }

        else {
            crosshairImage.color = defaultColor;
        }
    }

    void TeleportObjects(GameObject source, GameObject target) {

        if (source.TryGetComponent(out Rigidbody sourceRB)) {
            sourceRB.MovePosition(target.transform.position);

            if (target.TryGetComponent(out Rigidbody targetRB)) {
                sourceRB.velocity = new Vector3(targetRB.velocity.x, 0, targetRB.velocity.z) / targetRB.mass;
            }
        }

        else if (source.TryGetComponent(out NavMeshAgent agent)) {
            agent.Warp(target.transform.position);
        }

        
        
        // Inherit the velocity of moving objects
        // if (target.TryGetComponent(out Rigidbody targetRB) 
        // && source.TryGetComponent(out Rigidbody sourceRB)) {
        //     sourceRB.velocity = new Vector3(targetRB.velocity.x, 0, targetRB.velocity.z) / targetRB.mass;
        // }

        // else if (source.TryGetComponent(out NavMeshAgent agent)) {
        //     agent.Warp(target.transform.position);
        // }

        // else {
        //     source.GetComponent<Rigidbody>().MovePosition(target.transform.position);
        // }
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
        while (lensDistortion.intensity.value < maxLensDistortion) {
            lensDistortion.intensity.value += Time.deltaTime / Time.timeScale * distortionSpeed;
            yield return null;
        }

        UpdateRotation(closestTarget);
        TeleportObjects(gameObject, closestTarget);
        GetComponent<PlayerThrow>().AddKunaiCount(1);
        
        teleportables.Remove(closestTarget);
        Destroy(closestTarget);

        if (lensDistortion.intensity.value > maxLensDistortion) {
            while (lensDistortion.intensity.value > 0) {
                lensDistortion.intensity.value -= Time.deltaTime / Time.timeScale * distortionSpeed;
                yield return null;
            } 
        }
    }

    IEnumerator SwapLocations(GameObject closestTarget, GameObject temp) {
        while (lensDistortion.intensity.value < maxLensDistortion) {
            lensDistortion.intensity.value += Time.deltaTime / Time.timeScale * distortionSpeed;
            yield return null;
        }

        UpdateRotation(closestTarget);
        TeleportObjects(gameObject, closestTarget);
        TeleportObjects(closestTarget, temp);

        Destroy(temp);

        // if (lensDistortion.intensity.value > maxLensDistortion) {
            while (lensDistortion.intensity.value > 0) {
                lensDistortion.intensity.value -= Time.deltaTime / Time.timeScale * distortionSpeed;
                yield return null;
            } 
        // }
    }
}
