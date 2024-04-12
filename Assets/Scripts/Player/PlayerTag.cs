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
    InputController inputController;

    void Start() {
        inputController = GetComponent<InputController>();
    }

    void Update()
    {
        if (inputController.GetTagDown()
            && Physics.Raycast(cam.transform.position, cam.forward, out hit, maxTagDistance) 
            && hit.collider.gameObject.CompareTag("Throwable")
            && hit.collider.gameObject.layer != LayerMask.NameToLayer("Tagged")) {
                holdTime = 0;

                targetObject = hit.collider.gameObject;
            }

        if (inputController.GetTagUp()) {
            holdTime = 0;
        }
   
        if (inputController.GetTagHold()) {
            if (Physics.Raycast(cam.transform.position, cam.forward, out hit, maxTagDistance)
            && hit.collider.gameObject == targetObject) {
                holdTime += Time.deltaTime;

                if (holdTime >= minTagTime) {
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("Tagged");
                    hit.collider.gameObject.GetComponent<Renderer>().material = taggedMaterial;
                    targetObject = null;
                }
            }
        }

    }
}
