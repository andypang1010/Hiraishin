using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCanvasUIManager : MonoBehaviour
{
    public Transform target;
    GameObject worldSpaceCanvas;

    void Start () 
    {
        worldSpaceCanvas = GameObject.Find("WORLD SPACE CANVAS");
    }

    void Update()
    {
        if (target == null) {
            Destroy(gameObject);
        }

        else {
            transform.position = target.position;
        }

        transform.LookAt(Camera.main.transform, Vector3.up);
        transform.SetParent(worldSpaceCanvas.transform);
    }
}
