using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRun : MonoBehaviour
{
    [Header("References")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public HeadBob headBob;
    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;
    private Rigidbody rb;

    [Header("Settings")]
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallCheckDistance;
    public float minJumpHeight;
    public float cameraTilt;

    [Header("Exiting Wall")]
    public float exitWallTime;
    private bool exitingWall;
    private float exitWallTimer;


    private RaycastHit leftHit, rightHit;
    private bool hasLeft, hasRight;



    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCamera = GetComponent<PlayerCamera>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckForWalls();

        // Wall run
        if ((hasLeft || hasRight) && InputController.Instance.GetWalkDirection().y > 0 && InputController.Instance.GetSprint() && AboveGround() && !exitingWall) {
            if (!playerMovement.wallRunning) {
                StartWallRun();
            }

            // Wall jump
            if (InputController.Instance.GetJumpDown()) {
                WallJump();
            }
        }

        // Exit wall run when wall jumping
        else if (exitingWall) {
            if (playerMovement.wallRunning) {
                StopWallRun();
            }

            if (exitWallTimer > 0) {
                exitWallTimer -= Time.deltaTime / Time.timeScale;
            }

            if (exitWallTimer <= 0) {
                exitingWall = false;
            }
        }

        // Do nothing
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
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, groundLayer) && !playerMovement.Grounded;
    }

    void StartWallRun() {
        playerMovement.wallRunning = true;
        headBob.enabled = false;

        playerCamera.DoFieldOfView(90f);

        if (hasLeft) {
            playerCamera.DoTilt(-cameraTilt);
        }

        if (hasRight) {
            playerCamera.DoTilt(cameraTilt);
        }
    }

    void ContinueWallRun() {

        // Disable physics and stop vertical movement
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        playerMovement.wallRunning = true;

        // Calculate wall normal and forward vectors
        Vector3 wallNormal = hasRight ? rightHit.normal: leftHit.normal;

        // Curved surface wall run
        if (!(hasLeft && InputController.Instance.GetWalkDirection().x > 0) 
        && !(hasRight && InputController.Instance.GetWalkDirection().x < 0)) {
            rb.AddForce(-wallNormal * 100f, ForceMode.Force);
        }
    }

    void StopWallRun() {
        rb.useGravity = true;
        playerMovement.wallRunning = false;

        headBob.enabled = true;

        playerCamera.DoFieldOfView(60f);
        playerCamera.DoTilt(0f);
    }

    void WallJump() {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = hasRight ? rightHit.normal: leftHit.normal;
        Vector3 jumpForce = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(jumpForce, ForceMode.Impulse);
    }
}