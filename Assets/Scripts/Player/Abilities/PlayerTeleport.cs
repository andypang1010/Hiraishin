using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    public float teleportCD;
    public bool recycleKunaiEnabled;
    [HideInInspector] public bool teleportReady;
    [HideInInspector] public static List<GameObject> teleportables = new List<GameObject>();
    PlayerPickup playerPickup;

    void Start() {
        crosshairRectTransform = teleportCrosshair.GetComponent<RectTransform>();
        crosshairImage = teleportCrosshair.GetComponent<Image>();

        playerPickup = GetComponent<PlayerPickup>();
        
        if (!volume.profile.TryGet(out lensDistortion)) {
            Debug.LogWarning("No Lens Distortion component found on Global Volume");
        }

        teleportReady = true;
    }

    void Update()
    {
        // Calculate the radius crosshair and centerpoint of the screen
        detectionRadius = crosshairRectTransform.rect.height / 2f * (Screen.height / canvasScaler.referenceResolution.y);
        centerPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        GameObject closestTarget = null;

        // Iterate through all teleportables
        foreach (GameObject target in teleportables) {

            if (target == playerPickup.heldObj || target == null) {
                continue;
            }

            // Convert target's world position to screen position
            Vector2 screenPointPos = Camera.main.WorldToScreenPoint(target.transform.position);

            // If the target position is within the crosshair radius and within 
            if ((Vector2.SqrMagnitude(screenPointPos - centerPoint) <= Mathf.Pow(detectionRadius, 2)
            && Vector3.SqrMagnitude(target.transform.position - transform.position) <= Mathf.Pow(detectionDistance, 2))

            // Alternate check for larger objects/close proximity
            || target.GetComponentsInChildren<Collider>().
            Aggregate(false, (bool sum, Collider collider) => collider.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out _, detectionDistance) || sum))
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
            crosshairImage.color = detectedColor;

            // When player wants to teleport
            if (InputController.Instance.GetTeleportDown() && teleportReady) {

                teleportReady = false;

                if (closestTarget.layer == LayerMask.NameToLayer("Kunai")) {
                    StartCoroutine(Teleport(closestTarget));
                }

                else if (closestTarget.layer == LayerMask.NameToLayer("Tagged")) {
                    GameObject temp = new GameObject();

                    temp.transform.position = gameObject.transform.position;
                    temp.transform.rotation = gameObject.transform.rotation;
                    temp.transform.localScale = gameObject.transform.localScale;

                    StartCoroutine(SwapLocations(closestTarget, temp));
                }

                Invoke(nameof(ResetTeleport), teleportCD);
            }
        }

        else {
            crosshairImage.color = defaultColor;
        }
    }

    void TeleportObjects(GameObject source, GameObject target) {
        // If the source object has a rigidbody (player, other interactables)
        if (source.TryGetComponent(out Rigidbody sourceRB)) {

            if (target.CompareTag("Enemy")) {
                sourceRB.MovePosition(target.transform.position + Vector3.up);
            }
            
            else if (target.CompareTag("Player") && !source.CompareTag("Player")) {
                sourceRB.MovePosition(target.transform.position + Vector3.down);
            }

            else {

                // Crouch if teleporting into a tunnel
                if (sourceRB.TryGetComponent(out PlayerMovement playerMovement)
                && Physics.Raycast(target.transform.position, Vector3.up, playerMovement.playerHeight * 0.5f + 0.5f)) {
                    playerMovement.Crouch();
                    playerMovement.movementState = PlayerMovement.MovementState.CROUCH;
                }

                sourceRB.MovePosition(target.transform.position);
            }

            // Inherit target object velocity
            if (target.TryGetComponent(out Rigidbody targetRB)) {
                sourceRB.velocity = new Vector3(targetRB.velocity.x, 0, targetRB.velocity.z);
            }
        }

        // If the source object is an enemy
        else if (source.TryGetComponent(out NavMeshAgent agent)) {

            agent.enabled = false;
            source.transform.position = target.transform.position + Vector3.down;

            if (NavMesh.SamplePosition(source.transform.position, out _, 1f, NavMesh.AllAreas)) {
                agent.enabled = true;
            }
            else {
                teleportables.Remove(source);
                source.layer = LayerMask.NameToLayer("Enemy");
            }
        }
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

        if (recycleKunaiEnabled){
            GetComponent<PlayerThrow>().AddKunaiCount(1);
        }
        
        teleportables.Remove(closestTarget);
        Destroy(closestTarget);

        while (lensDistortion.intensity.value > 0) {
            lensDistortion.intensity.value -= Time.deltaTime / Time.timeScale * distortionSpeed;
            yield return null;
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

        while (lensDistortion.intensity.value > 0) {
            lensDistortion.intensity.value -= Time.deltaTime / Time.timeScale * distortionSpeed;
            yield return null;
        }
    }

    void OnDestroy() {
        if (lensDistortion != null) {
            lensDistortion.intensity.value = 0;
        }
    }

    void ResetTeleport() {
        teleportReady = true;
    }
}
