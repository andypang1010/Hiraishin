using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTag : MonoBehaviour
{
    public KeyCode tagKey;
    public Transform cam;
    public float maxTagDistance;
    public float tagTime;
    public Material taggedMaterial;
    public float holdTime;
    GameObject targetObject;
    RaycastHit hit;
    void Update()
    {
        if (Input.GetKeyDown(tagKey) 
            && Physics.Raycast(cam.transform.position, cam.forward, out hit, maxTagDistance) 
            && hit.collider.gameObject.CompareTag("Throwable")
            && hit.collider.gameObject.layer != LayerMask.NameToLayer("Tagged")) {
                holdTime = 0;

                targetObject = hit.collider.gameObject;
            }

        if (Input.GetKeyUp(tagKey)) {
            holdTime = 0;
        }
   
        if (Input.GetKey(tagKey)) {
            if (Physics.Raycast(cam.transform.position, cam.forward, out hit, maxTagDistance)
            && hit.collider.gameObject == targetObject) {
                holdTime += Time.deltaTime;

                if (holdTime >= tagTime) {
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("Tagged");
                    hit.collider.gameObject.GetComponent<Renderer>().material = taggedMaterial;
                    targetObject = null;
                }
            }
        }

    }
}
