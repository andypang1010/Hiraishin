using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    public KeyCode sprintKey, crouchKey, jumpKey;

    [Header("Movement")]
    public float sprintSpeed;
    public float walkSpeed;
    public float groundDrag;
    public Transform orientation;
    public MovementState movementState;
    public enum MovementState {
        WALK,
        SPRINT,
        CROUCH,
        AIR
    }
    private float moveSpeed;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    bool grounded;

    [Header("Jump")]
    public float jumpForce;
    public float airMultiplier;

    [Header("Crouch")]
    public float crouchSpeed;
    public float crouchScale;
    float defaultScale;

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

        GetInput();
        ClampSpeed();
        SetDrag();
        HandleMovementState();
    }

    void FixedUpdate() {
        Move();
    }

    void GetInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKeyDown(jumpKey) && grounded) {
            Jump();
        }

        if (Input.GetKeyDown(crouchKey) && grounded) {
            
            // Handle crouch size
            transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        
        if (Input.GetKeyUp(crouchKey) || !grounded) {
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
        moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;
        if (grounded) {
            // print("Grounded: " + (10 * moveSpeed * moveDirection).magnitude);
            rb.AddForce(10 * moveSpeed * moveDirection, ForceMode.Force);
        }
        else {
            // print("Not grounded: " + (10 * moveSpeed * moveDirection * airMultiplier).magnitude);
            rb.AddForce(10 * moveSpeed * moveDirection * airMultiplier, ForceMode.Force);
        }
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

    void ClampSpeed() {
        Vector3 rawVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (rawVelocity.magnitude > moveSpeed) {
            Vector3 clampedVelocity = rawVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(clampedVelocity.x, rb.velocity.y, clampedVelocity.z);
        }
    }

    void Jump() {

        // Resets y-velocity to have consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public Vector3 GetMoveVelocity() {
        return new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
    
    public bool isGrounded() {
        return grounded;
    }
}
