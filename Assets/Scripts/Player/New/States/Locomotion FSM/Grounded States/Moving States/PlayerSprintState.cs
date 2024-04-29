using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerMovingState
{
    public PlayerSprintState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerData playerData) : base(playerController, playerStateMachine, playerData)
    {
    }

    public override void Enter() {
        base.Enter();
        // Debug.Log("Sprint State");
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (walkInput.sqrMagnitude < 0.1f) {
            stateMachine.ChangeState(controller.IdleState);
        }

        else if (jumpInput
        && controller.coyoteTimeCounter > 0f 
        && controller.jumpBufferCounter > 0f) {

            controller.AirState.moveSpeed = data.sprintSpeed;
            stateMachine.ChangeState(controller.JumpState);
        }

        else if (!sprintInput) {
            stateMachine.ChangeState(controller.WalkState);
        }

        else {  
            SpeedControl(data.sprintSpeed);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        Move(data.sprintSpeed);
    }

    public override void Exit() {
        base.Exit();
    }

}
