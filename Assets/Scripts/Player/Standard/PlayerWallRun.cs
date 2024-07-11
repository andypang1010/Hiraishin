using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRun : MonoBehaviour
{
    [Header("References")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    private PlayerMovement playerMovement;
    private Rigidbody rb;

    [Header("Settings")]
    public float wallRunForce;
    public float wallCheckDistance;
    public float minJumpHeight;

    private RaycastHit leftHit, rightHit;
    private bool hasLeft, hasRight;



    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckForWalls();

        if ((hasLeft || hasRight) && InputController.Instance.GetWalkDirection().y > 0 && AboveGround()) {
            if (!playerMovement.wallRunning) {
                StartWallRun();
            }
        }

        else {
            StopWallRun();
        }
    }

    void FixedUpdate() {
        if (playerMovement.wallRunning) {
            ContinueWallRun();
        }
    }

    void CheckForWalls() {
        hasLeft = Physics.Raycast(transform.position, -transform.right, out leftHit, wallCheckDistance, wallLayer);
        hasRight = Physics.Raycast(transform.position, transform.right, out rightHit, wallCheckDistance, wallLayer);
    }

    bool AboveGround() {
        print("true");
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, groundLayer) && !playerMovement.Grounded;
    }

    void StartWallRun() {
        playerMovement.wallRunning = true;
    }

    void ContinueWallRun() {

        // Disable physics and stop vertical movement
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        playerMovement.wallRunning = true;

        // Calculate wall normal and forward vectors
        Vector3 wallNormal = hasRight ? rightHit.normal: leftHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        // Reverse normal if facing away from the normal
        if ((transform.forward - wallForward).magnitude > (transform.forward - -wallForward).magnitude) {
            wallForward = -wallForward;
        }

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        print("adding force");

        // Curved surface wall run
        if (!(hasLeft && InputController.Instance.GetWalkDirection().x > 0) 
        && !(hasRight && InputController.Instance.GetWalkDirection().x < 0)) {
            rb.AddForce(-wallNormal * 100f, ForceMode.Force);
        }
    }

    void StopWallRun() {
        // rb.useGravity = true;
        playerMovement.wallRunning = false;
    }
}
