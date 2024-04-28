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
        if (InputManager.Instance.TagDown
            && Physics.Raycast(Camera.main.transform.transform.position, Camera.main.transform.forward, out hit, maxTagDistance) 
            && (hit.collider.gameObject.CompareTag("Throwable")
            || hit.collider.gameObject.CompareTag("Enemy"))
            && hit.collider.gameObject.layer != LayerMask.NameToLayer("Tagged")) {
                holdTime = 0;

                targetObject = hit.collider.gameObject;
            }

        if (InputManager.Instance.TagUp) {
            holdTime = 0;
        }
   
        if (InputManager.Instance.TagHold) {
            if (Physics.Raycast(Camera.main.transform.transform.position, Camera.main.transform.forward, out hit, maxTagDistance)
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
