using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputController))]
public class PlayerCamera : MonoBehaviour
{
    public Camera cam;
    public float sensX, sensY;

    private float rotationX, rotationY;
    InputController inputController;

    void Start()
    {
        // Centers and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputController = GetComponent<InputController>();
    }

    void Update()
    {

        // Get mouse input with sensitivity

        Vector2 lookDirection = inputController.GetLookDirection();

        float mouseX = lookDirection.x * Time.fixedDeltaTime * sensX;
        float mouseY = lookDirection.y * Time.fixedDeltaTime * sensY;
    
        // Weird but works (DON'T TOUCH)
        rotationY += mouseX;
        rotationX -= mouseY;

        // Clamp y-axis rotation
        rotationX = Math.Clamp(rotationX, -80f, 80f);

        // Update camera rotation
        cam.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        
        // Update player rotation
        this.transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
