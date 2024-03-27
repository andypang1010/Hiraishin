using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public Transform orientation;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    bool grounded;

    [Header("Jump")]
    public float jumpForce;
    public float airMultiplier;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Check if is grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);

        GetInput();
        ClampSpeed();
        SetDrag();
    }

    void FixedUpdate() {
        Move();
    }

    void GetInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            Jump();
        }
    }

    void Move() {
        moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;
        if (grounded) {
            print("Grounded: " + (10 * moveSpeed * moveDirection).magnitude);
            rb.AddForce(10 * moveSpeed * moveDirection, ForceMode.Force);
        }
        else {
            print("Not grounded: " + (10 * moveSpeed * moveDirection * airMultiplier).magnitude);
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
}
