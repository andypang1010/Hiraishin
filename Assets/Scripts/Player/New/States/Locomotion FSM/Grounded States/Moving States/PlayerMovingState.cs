using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingState : PlayerGroundedState
{
    protected Vector3 moveDirection;
    float horizontalInput, verticalInput;

    public PlayerMovingState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerData playerData) : base(playerController, playerStateMachine, playerData)
    {
    }

    public override void Enter() {
        base.Enter();
        
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


    
    protected void Move(float moveSpeed) {
        moveDirection = (controller.transform.right * horizontalInput + controller.transform.forward * verticalInput).normalized;

        // Apply force perpendicular to slope's normal if on slope
        if (OnSlope) {
            controller.rb.AddForce(20 * moveSpeed * GetSlopeMoveDirection(), ForceMode.Force);

            // Apply downward force to keep player on slope
            if (controller.rb.velocity.y > 0) {
                controller.rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        // Move in direction
        else {
            controller.rb.AddForce(10 * moveSpeed * moveDirection, ForceMode.Force);
        }
    }


    protected void SpeedControl(float maxSpeed) {

        // Prevents player from exceeding move speed on slopes
        if (OnSlope && !jumpInput && controller.rb.velocity.magnitude > maxSpeed) {
            controller.rb.velocity = controller.rb.velocity.normalized * maxSpeed;
        }

        else {
            Vector3 rawVelocity = new Vector3(controller.rb.velocity.x, 0, controller.rb.velocity.z);

            // Clamp x and z axis velocity
            if (rawVelocity.magnitude > maxSpeed) {
                Vector3 clampedVelocity = rawVelocity.normalized * maxSpeed;
                controller.rb.velocity = new Vector3(clampedVelocity.x, controller.rb.velocity.y, clampedVelocity.z);
            }
        }
    }

    Vector3 GetSlopeMoveDirection() {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    void StepClimb() {
        Debug.DrawRay(controller.stepRayLower.position, controller.stepRayLower.forward * 0.15f, Color.red);
        Debug.DrawRay(controller.stepRayUpper.position, controller.stepRayUpper.forward * 0.2f, Color.green);

        if (Physics.Raycast(controller.stepRayLower.position, controller.stepRayLower.forward, out _, 0.15f)
        && !Physics.Raycast(controller.stepRayUpper.position, controller.stepRayUpper.forward, out _, 0.2f)) {
            controller.rb.position += new Vector3(0f , data.stepSmoothing, 0f);
        }
    }
}
