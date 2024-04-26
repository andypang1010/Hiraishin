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
        Debug.Log("Sprint State");
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        SpeedControl(data.sprintSpeed);

        if (walkInput.sqrMagnitude < 0.1f) {
            stateMachine.ChangeState(controller.IdleState);
            return;
        }

        else if (jumpInput
        && controller.coyoteTimeCounter > 0f 
        && controller.jumpBufferCounter > 0f) {

            stateMachine.ChangeState(controller.JumpState);
            controller.AirState.moveSpeed = data.sprintSpeed;
            return;
        }

        else if (!sprintInput) {
            stateMachine.ChangeState(controller.WalkState);
            return;
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
