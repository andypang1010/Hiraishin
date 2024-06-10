using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTag : MonoBehaviour
{
    [Header("Settings")]
    public float maxTagDistance;
    public float minTagTime;
    public Material taggedMaterial;
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
                    hit.transform.gameObject.layer = LayerMask.NameToLayer("Tagged");

                    foreach (Transform t in targetObject.transform) 
                    {
                        t.gameObject.layer = LayerMask.NameToLayer("Tagged");
                    }

                    // if (targetObject.TryGetComponent(out Renderer renderer)) {
                    //     renderer.material = taggedMaterial;
                    // }

                    // else {
                    //     Renderer[] renderers = targetObject.GetComponentsInChildren<Renderer>();
                    //     foreach (Renderer r in renderers)
                    //     {
                    //         r.material = taggedMaterial;
                    //     }
                    // }
                    
                    PlayerTeleport.teleportables.Add(targetObject);
                    targetObject = null;
                }
            }
        }

    }
}
