using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTag : MonoBehaviour
{
    [Header("References")]
    public GameObject marker;
    public GameObject teleportCrosshair;
    public Sprite tagCrosshair;
    Sprite defaultCrosshair;

    [Header("Settings")]
    public float maxTagDistance;
    public float minTagTime;
    [HideInInspector] public float holdTime { get; private set; }
    GameObject targetObject;
    RaycastHit hit;

    private void Start() {
        defaultCrosshair = teleportCrosshair.GetComponent<Image>().sprite;
    }

    void Update()
    {
        if (Physics.Raycast(Camera.main.transform.transform.position, Camera.main.transform.forward, out hit, maxTagDistance, ~LayerMask.GetMask("Tagged"), QueryTriggerInteraction.Ignore) 
            && (hit.transform.root.gameObject.CompareTag("Throwable")
            || hit.transform.root.gameObject.CompareTag("Enemy"))) {
                teleportCrosshair.GetComponent<Image>().sprite = tagCrosshair;

                print("Tag Distance to Target: " + hit.distance);

                if (InputController.Instance.GetTagDown()) {
                    holdTime = 0;
                    targetObject = hit.transform.root.gameObject;
                }
        }

        else {
            teleportCrosshair.GetComponent<Image>().sprite = defaultCrosshair;
        }

        if (InputController.Instance.GetTagUp()) {
            holdTime = 0;
        }
   
        if (InputController.Instance.GetTagHold()) {
            if (Physics.Raycast(Camera.main.transform.transform.position, Camera.main.transform.forward, out hit, maxTagDistance,  ~LayerMask.GetMask("Tagged"), QueryTriggerInteraction.Ignore)
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
