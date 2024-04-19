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
        if (InputController.Instance.GetTagDown()
            && Physics.Raycast(cam.transform.position, cam.forward, out hit, maxTagDistance) 
            && (hit.collider.gameObject.CompareTag("Throwable")
            || hit.collider.gameObject.CompareTag("Enemy"))
            && hit.collider.gameObject.layer != LayerMask.NameToLayer("Tagged")) {
                holdTime = 0;

                targetObject = hit.collider.gameObject;
            }

        if (InputController.Instance.GetTagUp()) {
            holdTime = 0;
        }
   
        if (InputController.Instance.GetTagHold()) {
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
