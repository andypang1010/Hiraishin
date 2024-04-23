using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public GameObject player;
    [Range(0, 1f)] public float idleAmplitude;
    [Range(0, 30)] public float idleFrequency;
    [Range(0, 1f)] public float walkAmplitude;
    [Range(0, 30)] public float walkFrequency;
    [Range(0, 2f)] public float sprintAmplitude;
    [Range(0, 30)] public float sprintFrequency;
    [Range(0, 1f)] public float crouchAmplitude;
    [Range(0, 30)] public float crouchFrequency;
    public float stablizedOffset;

    PlayerMovement playerMovement;
    Transform playerCamera, cameraHolder;
    Vector3 startPosition;
    float amplitude;
    float frequency;

    void Start() {
        cameraHolder = transform;
        playerCamera = cameraHolder.GetChild(0);

        startPosition = playerCamera.localPosition;

        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        PlayerMovement.MovementState movementState = playerMovement.GetMovementState();

        if (playerMovement.GetMoveVelocity().magnitude <= 0.3f) {
            amplitude = idleAmplitude;
            frequency = idleFrequency;
        }
        else {
            // Change amplitude and frequency depending on movement state
            switch(movementState) {
                case PlayerMovement.MovementState.SPRINT:
                    amplitude = sprintAmplitude;
                    frequency = sprintFrequency;
                    break;
                case PlayerMovement.MovementState.CROUCH: 
                    amplitude = crouchAmplitude;
                    frequency = crouchFrequency;
                    break;
                case PlayerMovement.MovementState.WALK:
                default:
                    amplitude = walkAmplitude;
                    frequency = walkFrequency;
                    break;
            }
        }
    // }

    // private void FixedUpdate() {
        PlayMotion();
        ResetPosition();
        playerCamera.LookAt(StablizedTarget());
    }

    void PlayMotion() {
        // Head bob only if is grounded
        if (!playerMovement.isGrounded()) return;

        // Offset camera position by head bob
        playerCamera.localPosition += FootStepMotion() * Time.deltaTime;
    }

    Vector3 FootStepMotion() {
        Vector3 pos = Vector3.zero;

        // Oscillate using sine and cosine curves
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude / 2;

        return pos;
    }

    void ResetPosition() {
        if (playerCamera.localPosition == startPosition) return;

        // Slowly move back to start position
        playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, startPosition, 1 * Time.deltaTime);
    }

    Vector3 StablizedTarget() {

        // Focuses at a position in front
        Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y + cameraHolder.localPosition.y, player.transform.position.z);
        pos += playerCamera.forward * stablizedOffset;

        return pos;
    }
}
