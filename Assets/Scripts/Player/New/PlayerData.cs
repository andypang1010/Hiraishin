using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject {

    [Header("Player Settings")]
    public float playerHeight;

[Header("Movement")]
    public float sprintSpeed;
    public float walkSpeed;
    public float groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float airMultiplier;
    public float coyoteTime;
    public float jumpBuffer;

    [Header("Crouch")]
    public float crouchSpeed;
    public float crouchScale;

    [Header("Slope Check")]
    public float maxSlopeAngle;

    [Header("Step Check")]
    public float stepHeight;
    public float stepSmoothing;
}
