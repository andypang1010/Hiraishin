using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public new bool enabled;
    public GameObject player;
    [Range(0, 1f)] public float walkAmplitude;
    [Range(0, 30)] public float walkFrequency;
    [Range(0, 2f)] public float sprintAmplitude;
    [Range(0, 30)] public float sprintFrequency;
    [Range(0, 1f)] public float crouchAmplitude;
    [Range(0, 30)] public float crouchFrequency;
    public float toggleSpeed;
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

        if (!enabled) return;
            PlayMotion();
            ResetPosition();
            playerCamera.LookAt(StablizedTarget());
    }

    void PlayMotion() {
        float moveSpeed = new Vector2(
            Math.Abs(playerMovement.GetMoveVelocity().x), 
            Math.Abs(playerMovement.GetMoveVelocity().z)).magnitude;

        // Head bob only if reaches toggle speed and is grounded
        if (moveSpeed < toggleSpeed) return;
        if (!playerMovement.isGrounded()) return;

        // Offset camera position by head bob
        playerCamera.localPosition += FootStepMotion() * Time.deltaTime;
    }

    Vector3 FootStepMotion() {
        Vector3 pos = Vector3.zero;

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
