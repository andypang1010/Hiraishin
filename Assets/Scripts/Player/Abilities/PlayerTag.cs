using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTag : MonoBehaviour
{
    [Header("References")]
    public GameObject marker;
    public Material taggedMaterial;

    [Header("Settings")]
    public float maxTagDistance;
    public float minTagTime;
    [HideInInspector] public float holdTime { get; private set; }
    GameObject targetObject;
    RaycastHit hit;

    void Update()
    {
        if (InputController.Instance.GetTagDown()
            && Physics.Raycast(Camera.main.transform.transform.position, Camera.main.transform.forward, out hit, maxTagDistance) 
            && (hit.transform.root.gameObject.CompareTag("Throwable")
            || hit.transform.root.gameObject.CompareTag("Enemy"))
            && hit.collider.gameObject.layer != LayerMask.NameToLayer("Tagged")) {
                holdTime = 0;

                targetObject = hit.transform.root.gameObject;
            }

        if (InputController.Instance.GetTagUp()) {
            holdTime = 0;
        }
   
        if (InputController.Instance.GetTagHold()) {
            if (Physics.Raycast(Camera.main.transform.transform.position, Camera.main.transform.forward, out hit, maxTagDistance)
            && hit.transform.root.gameObject == targetObject) {
                holdTime += Time.deltaTime;

                if (holdTime >= minTagTime) {
                    targetObject.layer = LayerMask.NameToLayer("Tagged");

                    foreach (Transform t in targetObject.transform) 
                    {
                        t.gameObject.layer = LayerMask.NameToLayer("Tagged");
                    }

                    GameObject taggedMarker = Instantiate(marker, targetObject.transform);
                    
                    PlayerTeleport.teleportables.Add(targetObject);
                    targetObject = null;
                }
            }
        }

    }
}
