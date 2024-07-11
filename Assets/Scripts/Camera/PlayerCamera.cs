using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{
    [Header("References")]
    public HeadBob headBob;
    public Transform cameraHolder;
    public Camera mainCamera, uiCamera;

    [Header("Sensitivity")]
    public float sensX;
    public float sensY;
    [HideInInspector] public float rotationX, rotationY;
    float currentSensX, currentSensY;

    void Start()
    {
        // Centers and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentSensX = sensX;
        currentSensY = sensY;

        mainCamera = Camera.main;
        uiCamera = GameObject.Find("UI Camera").GetComponent<Camera>();

        // Disable HeadBob first to ensure camera is working correctly
        // headBob.enabled = false;

        // StartCoroutine(EnableHeadBob());
    }

    void Update()
    {
        // headBob.enabled = true;

        if (Time.timeScale != 1) {
            currentSensX = sensX / Time.timeScale;
            currentSensY = sensY / Time.timeScale;
        }

        else {
            currentSensX = sensX;
            currentSensY = sensY;
        }

        Vector2 lookDirection = InputController.Instance.GetLookDirection();

        // Get mouse input with sensitivity
        float mouseX = lookDirection.x * Time.fixedDeltaTime * currentSensX;
        float mouseY = lookDirection.y * Time.fixedDeltaTime * currentSensY;
    
        // Weird but works (DON'T TOUCH)
        rotationY += mouseX;
        rotationX -= mouseY;

        // Clamp y-axis rotation
        rotationX = Math.Clamp(rotationX, -80f, 80f);

        // Update camera rotation
        // Camera.main.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        // GameObject.Find("UI Camera").transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

        cameraHolder.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        
        // Update player rotation
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    IEnumerator EnableHeadBob() {
        headBob.enabled = false;

        yield return null;
        headBob.enabled = true;
    }

    public void DoFieldOfView(float endValue) {
        mainCamera.DOFieldOfView(endValue, 0.25f);
        uiCamera.GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt) {
        mainCamera.transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
        uiCamera.transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}
