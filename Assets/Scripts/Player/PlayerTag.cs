using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTag : MonoBehaviour
{
    public KeyCode tagKey;
    public Transform cam;
    public float maxTagDistance;
    public float holdTime = 5f;
    public Material taggedMaterial;
    float startTime;
    float timer;
    GameObject targetObject;
    RaycastHit hit;
    void Update()
    {
        if (Input.GetKeyDown(tagKey) 
            && Physics.Raycast(cam.transform.position, cam.forward, out hit, maxTagDistance) 
            && hit.collider.gameObject.CompareTag("Throwable")
            && hit.collider.gameObject.layer != LayerMask.NameToLayer("Tagged")) {

                targetObject = hit.collider.gameObject;

                startTime = Time.time;
                timer = startTime;
            }
   
        if (Input.GetKey(tagKey)) {
            timer += Time.deltaTime;

            if (startTime + holdTime <= Time.time
            && Physics.Raycast(cam.transform.position, cam.forward, out hit, maxTagDistance)
            && hit.collider.gameObject.Equals(targetObject)) {

                hit.collider.gameObject.layer = LayerMask.NameToLayer("Tagged");
                hit.collider.gameObject.GetComponent<Renderer>().material = taggedMaterial;
                targetObject = null;
            }
        }
    }
}
