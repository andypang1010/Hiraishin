using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX, sensY;
    public Transform orientation;

    private float rotationX, rotationY;

    void Start()
    {
        // Centers and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        // Get mouse input with sensitivity
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;


        // Weird but works (DON'T TOUCH)
        rotationY += mouseX;
        rotationX -= mouseY;

        // Update camera rotation
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        
        // Update player rotation
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
