using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform cam;
    public float sensX, sensY;
    public float rotationX, rotationY;
    float currentSensX, currentSensY;


    void Start()
    {
        // Centers and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Time.timeScale != 1) {
            currentSensX = sensX / Time.timeScale;
            currentSensY = sensY / Time.timeScale;
        }

        else {
            currentSensX = sensX;
            currentSensY = sensY;
        }

        // Get mouse input with sensitivity
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * currentSensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * currentSensY;
    
        // Weird but works (DON'T TOUCH)
        rotationY += mouseX;
        rotationX -= mouseY;

        // Clamp y-axis rotation
        rotationX = Math.Clamp(rotationX, -80f, 80f);

        // Update camera rotation
        cam.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        
        // Update player rotation
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
