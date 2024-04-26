using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    protected Vector2 walkInput;
    protected bool crouchInput, sprintInput, jumpInput;

    protected RaycastHit slopeHit;

    public PlayerGroundedState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerData playerData)
     : base(playerController, playerStateMachine, playerData)
    {
    }

    public override void Enter() {
        base.Enter();
        
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        controller.coyoteTimeCounter = data.coyoteTime;

        controller.rb.drag = data.groundDrag;

        // Handle Grounded States Inputs
        walkInput = controller.inputController.GetMoveDirection();
        crouchInput = controller.inputController.GetCrouchHold();
        sprintInput = controller.inputController.GetSprintHold();
        jumpInput = controller.inputController.GetJumpDown();

        if (jumpInput) {
            controller.jumpBufferCounter = data.jumpBuffer;
        }

        else {
            controller.jumpBufferCounter -= Time.deltaTime * Time.timeScale;
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        controller.rb.useGravity = !OnSlope;
    }

    public override void Exit() {
        base.Exit();
    }

    protected bool OnSlope
    {
        get
        {
            if (Physics.Raycast(controller.transform.position, Vector3.down, out slopeHit, data.playerHeight * 0.5f + 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

                return angle < data.maxSlopeAngle && angle != 0;
            }

            return false;
        }
    }
}
