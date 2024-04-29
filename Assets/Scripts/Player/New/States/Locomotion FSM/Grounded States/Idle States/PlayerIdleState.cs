using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerData playerData)
     : base(playerController, playerStateMachine, playerData)
    {
    }

    public override void Enter() {
        base.Enter();
        
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (walkInput.sqrMagnitude > 0.1f) {
            stateMachine.ChangeState(controller.WalkState);
        }

        else if (jumpInput
        && controller.coyoteTimeCounter > 0f 
        && controller.jumpBufferCounter > 0f) {
            
            controller.AirState.moveSpeed = 0;
            stateMachine.ChangeState(controller.JumpState);
        }

        else if (crouchInput) {
            stateMachine.ChangeState(controller.CrouchIdleState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public override void Exit() {
        base.Exit();
    }

}
