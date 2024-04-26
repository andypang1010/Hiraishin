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

    PlayerController playerController;
    Transform playerCamera, cameraHolder;
    Vector3 startPosition;
    float amplitude;
    float frequency;

    void Start() {
        cameraHolder = transform;
        playerCamera = cameraHolder.GetChild(0);

        startPosition = playerCamera.localPosition;

        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        // PlayerMovement.MovementState movementState = playercon.GetMovementState();

        if (playerController.GetMoveVelocity().magnitude <= 0.3f) {
            amplitude = idleAmplitude;
            frequency = idleFrequency;
        }

        else if (playerController.locomotionStateMachine.CurrentState is PlayerSprintState) {
            amplitude = sprintAmplitude;
            frequency = sprintFrequency;
        }

        else if (playerController.locomotionStateMachine.CurrentState is PlayerCrouchWalkState) {
            amplitude = crouchAmplitude;
            frequency = crouchFrequency;
        }

        else if (playerController.locomotionStateMachine.CurrentState is PlayerWalkState) {
            amplitude = walkAmplitude;
            frequency = walkFrequency;
        }


    // }

    // private void FixedUpdate() {
        PlayMotion();
        ResetPosition();
        playerCamera.LookAt(StablizedTarget());
    }

    void PlayMotion() {
        // Head bob only if is grounded
        if (playerController.locomotionStateMachine.CurrentState as PlayerGroundedState == null) return;

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
