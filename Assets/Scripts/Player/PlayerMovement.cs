using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum MovementState {
        WALK,
        SPRINT,
        CROUCH,
        AIR
    }

    [Header("Input")]
    public KeyCode sprintKey;
    public KeyCode crouchKey;
    public KeyCode jumpKey;

    [Header("Movement")]
    public float sprintSpeed;
    public float walkSpeed;
    public float groundDrag;
    [HideInInspector] public MovementState movementState;
    private float moveSpeed;

    [Header("Jump")]
    public float jumpForce;
    public float airMultiplier;
    public float coyoteTime;
    public float jumpBuffer;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    [Header("Crouch")]
    public float crouchSpeed;
    public float crouchScale;
    private float defaultScale;

    [Header("Ground Check")]
    public float playerHeight;
    // public LayerMask groundLayer;
    bool grounded;

    [Header("Slope Check")]
    public float maxSlopeAngle;
    RaycastHit slopeHit;
    bool exitingSlope;

    float horizontalInput, verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        defaultScale = transform.localScale.y;
    }

    void Update()
    {
        // Check if is grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);
        exitingSlope = !grounded;

        GetInput();
        SpeedControl();
        SetDrag();
        HandleMovementState();
    }

    void FixedUpdate() {
        Move();
    }

    void GetInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Coyote time check
        if (grounded) {
            coyoteTimeCounter = coyoteTime;
        }
        else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump buffer check
        if (Input.GetKeyDown(jumpKey)) {
            jumpBufferCounter = jumpBuffer;
        }
        else {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        if (grounded) {
            if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f) {
                Jump();

                // Reset jump buffer to prevent jumping again
                jumpBufferCounter = 0f;
            }

            if (Input.GetKeyDown(crouchKey)) {
                
                // Shrink to crouch size
                transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
                
                // Apply downward force so doesn't float
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            }
        }

        if (Input.GetKeyUp(crouchKey) || !grounded) {
            // Revert back to normal scale
            transform.localScale = new Vector3(transform.localScale.x, defaultScale, transform.localScale.z);
        }
    }

    void HandleMovementState() {
        if (!grounded) {
            movementState = MovementState.AIR;
        }

        else {
            if (Input.GetKey(crouchKey)) {
                movementState = MovementState.CROUCH;
                moveSpeed = crouchSpeed;
            }

            else if (Input.GetKey(sprintKey)) {
                movementState = MovementState.SPRINT;
                moveSpeed = sprintSpeed;
            }


            else {
                movementState = MovementState.WALK;
                moveSpeed = walkSpeed;
            }
        }
    }

    void Move() {
        moveDirection = (transform.right * horizontalInput + transform.forward * verticalInput).normalized;

        // Apply force perpendicular to slope's normal if on slope
        if (OnSlope() && !exitingSlope) {
            rb.AddForce(20 * moveSpeed * GetSlopeMoveDirection(), ForceMode.Force);

            // Apply downward force to keep player on slope
            if (rb.velocity.y > 0) {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        // Move in direction
        else if (grounded) {
            rb.AddForce(10 * moveSpeed * moveDirection, ForceMode.Force);
        }

        // Move in direction but slower in air
        else if (!grounded) {
            rb.AddForce(10 * moveSpeed * moveDirection * airMultiplier, ForceMode.Force);
        }

        // Disable gravity while on slope to avoid slipping
        rb.useGravity = !OnSlope();
    }

    
    void SetDrag()
    {
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    void SpeedControl() {

        // Prevents player from exceeding move speed on slopes
        if (OnSlope() && !exitingSlope && rb.velocity.magnitude > moveSpeed) {
            rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        else {
            Vector3 rawVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            // Clamp x and z axis velocity
            if (rawVelocity.magnitude > moveSpeed) {
                Vector3 clampedVelocity = rawVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(clampedVelocity.x, rb.velocity.y, clampedVelocity.z);
            }
        }
    }

    void Jump() {
        exitingSlope = true;

        // Resets y-velocity to have consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)) {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    Vector3 GetSlopeMoveDirection() {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public Vector3 GetMoveVelocity() {
        return new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
    
    public bool isGrounded() {
        return grounded;
    }

    public MovementState GetMovementState() {
        return movementState;
    }
}
