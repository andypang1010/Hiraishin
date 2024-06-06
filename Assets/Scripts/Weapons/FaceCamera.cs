using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    GameObject worldSpaceCanvas;
    void Start () 
    {
        worldSpaceCanvas = GameObject.Find("WORLD SPACE CANVAS");
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform, Vector3.up);
        transform.SetParent(worldSpaceCanvas.transform);
    }
}
