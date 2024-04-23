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

    [Header("Slope Check")]
    public float maxSlopeAngle;
    RaycastHit slopeHit;
    float playerHeight = 2;
    bool grounded;
    bool exitingSlope;

    [Header("Step Check")]
    public GameObject rayLower;
    public GameObject rayUpper;
    public float stepHeight;
    public float stepSmoothing;

    Rigidbody rb;
    Vector3 moveDirection;
    float horizontalInput, verticalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;


        rayUpper.transform.position = rayLower.transform.position + stepHeight * Vector3.up;
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
        StepClimb();
    }

    void GetInput() {
        Vector2 movement = InputController.Instance.GetWalkDirection();
        horizontalInput = movement.x;
        verticalInput = movement.y;

        // Coyote time check
        if (grounded) {
            coyoteTimeCounter = coyoteTime;
        }
        else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump buffer check
        if (InputController.Instance.GetJumpDown()) {
            jumpBufferCounter = jumpBuffer;
        }
        else {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        if (grounded) {
            if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f
            && movementState != MovementState.CROUCH) {
                Jump();

                // Reset jump buffer to prevent jumping again
                jumpBufferCounter = 0f;
            }

            if (InputController.Instance.GetCrouchDown()) {
                
                // Shrink to crouch size
                transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
                
                // Apply downward force so doesn't float
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            }
        }

        if (InputController.Instance.GetCrouchUp() || !grounded) {
            transform.localScale = new Vector3(transform.localScale.x, defaultScale, transform.localScale.z);
        }
    }

    void HandleMovementState() {
        if (!grounded) {
            movementState = MovementState.AIR;
        }

        else {
            if (InputController.Instance.GetCrouchHold() && grounded) {
                movementState = MovementState.CROUCH;
                moveSpeed = crouchSpeed;
            }

            else if (InputController.Instance.GetSprint()) {
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

    void StepClimb() {
        Debug.DrawRay(rayLower.transform.position, rayLower.transform.forward * 0.1f, Color.red);
        Debug.DrawRay(rayUpper.transform.position, rayUpper.transform.forward * 0.2f, Color.green);

        if (Physics.Raycast(rayLower.transform.position, rayLower.transform.forward, out _, 0.15f)
        && !Physics.Raycast(rayUpper.transform.position, rayUpper.transform.forward, out _, 0.2f)) {
            rb.position += new Vector3(0f , stepSmoothing, 0f);
        }
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
