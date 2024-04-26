using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerMovingState
{
    public PlayerWalkState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerData playerData)
     : base(playerController, playerStateMachine, playerData)
    {
    }

    public override void Enter() {
        base.Enter();
        Debug.Log("Walk State");
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        SpeedControl(data.walkSpeed);

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

        else if (crouchInput) {
            stateMachine.ChangeState(controller.CrouchWalkState);
            return;
        }

        else if (sprintInput) {
            stateMachine.ChangeState(controller.SprintState);
            return;
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        Move(data.walkSpeed);
    }

    public override void Exit() {
        base.Exit();
    }

}
