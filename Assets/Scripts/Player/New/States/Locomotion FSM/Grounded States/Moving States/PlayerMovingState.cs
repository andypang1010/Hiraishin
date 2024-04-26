using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingState : PlayerGroundedState
{
    protected Vector3 moveDirection;
    float horizontalInput, verticalInput;
    RaycastHit slopeHit;

    public PlayerMovingState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerData playerData) : base(playerController, playerStateMachine, playerData)
    {
    }

    public override void Enter() {
        base.DoChecks();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        
        horizontalInput = walkInput.x;
        verticalInput = walkInput.y;
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        StepClimb();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    
    protected void Move(float moveSpeed) {
        moveDirection = (playerController.transform.right * horizontalInput + playerController.transform.forward * verticalInput).normalized;

        // Apply force perpendicular to slope's normal if on slope
        if (OnSlope) {
            playerController.rb.AddForce(20 * moveSpeed * GetSlopeMoveDirection(), ForceMode.Force);

            // Apply downward force to keep player on slope
            if (playerController.rb.velocity.y > 0) {
                playerController.rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        // Move in direction
        else {
            playerController.rb.AddForce(10 * moveSpeed * moveDirection, ForceMode.Force);
        }
    }


    protected void SpeedControl(float maxSpeed) {

        // Prevents player from exceeding move speed on slopes
        if (OnSlope && !jumpInput && playerController.rb.velocity.magnitude > maxSpeed) {
            playerController.rb.velocity = playerController.rb.velocity.normalized * maxSpeed;
        }

        else {
            Vector3 rawVelocity = new Vector3(playerController.rb.velocity.x, 0, playerController.rb.velocity.z);

            // Clamp x and z axis velocity
            if (rawVelocity.magnitude > maxSpeed) {
                Vector3 clampedVelocity = rawVelocity.normalized * maxSpeed;
                playerController.rb.velocity = new Vector3(clampedVelocity.x, playerController.rb.velocity.y, clampedVelocity.z);
            }
        }
    }

    Vector3 GetSlopeMoveDirection() {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    void StepClimb() {
        Debug.DrawRay(playerController.stepRayLower.position, playerController.stepRayLower.forward * 0.15f, Color.red);
        Debug.DrawRay(playerController.stepRayUpper.position, playerController.stepRayUpper.forward * 0.2f, Color.green);

        if (Physics.Raycast(playerController.stepRayLower.position, playerController.stepRayLower.forward, out _, 0.15f)
        && !Physics.Raycast(playerController.stepRayUpper.position, playerController.stepRayUpper.forward, out _, 0.2f)) {
            playerController.rb.position += new Vector3(0f , playerData.stepSmoothing, 0f);
        }
    }

    bool OnSlope
    {
        get
        {
            if (Physics.Raycast(playerController.transform.position, Vector3.down, out slopeHit, playerData.playerHeight * 0.5f + 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

                return angle < playerData.maxSlopeAngle && angle != 0;
            }

            return false;
        }
    }
}
