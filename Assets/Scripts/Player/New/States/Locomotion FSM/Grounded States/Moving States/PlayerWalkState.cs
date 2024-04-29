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
        // Debug.Log("Walk State");
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (walkInput.sqrMagnitude < 0.1f) {
            stateMachine.ChangeState(controller.IdleState);
        }

        else if (jumpInput
        && controller.coyoteTimeCounter > 0f 
        && controller.jumpBufferCounter > 0f) {

            controller.AirState.moveSpeed = data.walkSpeed;
            stateMachine.ChangeState(controller.JumpState);
        }

        else if (crouchInput) {
            stateMachine.ChangeState(controller.CrouchWalkState);
        }

        else if (sprintInput) {
            stateMachine.ChangeState(controller.SprintState);
        }

        else {
            SpeedControl(data.walkSpeed);
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
