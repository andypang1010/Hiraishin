using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTag : MonoBehaviour
{
    [Header("Camera")]
    public Transform cam;

    [Header("Settings")]
    public float maxTagDistance;
    public float minTagTime;
    public Material taggedMaterial;
    [HideInInspector] public float holdTime;
    GameObject targetObject;
    RaycastHit hit;

    void Update()
    {
        if (InputController.GetTagDown()
            && Physics.Raycast(cam.transform.position, cam.forward, out hit, maxTagDistance) 
            && hit.collider.gameObject.CompareTag("Throwable")
            && hit.collider.gameObject.layer != LayerMask.NameToLayer("Tagged")) {
                holdTime = 0;

                targetObject = hit.collider.gameObject;
            }

        if (InputController.GetTagUp()) {
            holdTime = 0;
        }
   
        if (InputController.GetTagHold()) {
            if (Physics.Raycast(cam.transform.position, cam.forward, out hit, maxTagDistance)
            && hit.collider.gameObject == targetObject) {
                holdTime += Time.deltaTime;

                if (holdTime >= minTagTime) {
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("Tagged");
                    hit.collider.gameObject.GetComponent<Renderer>().material = taggedMaterial;
                    PlayerTeleport.teleportables.Add(hit.collider.gameObject);
                    targetObject = null;
                }
            }
        }

    }
}
