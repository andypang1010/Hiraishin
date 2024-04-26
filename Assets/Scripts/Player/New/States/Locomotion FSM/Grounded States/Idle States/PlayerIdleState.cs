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
        Debug.Log("Idle State");
        
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (walkInput.sqrMagnitude > 0.1f) {
            stateMachine.ChangeState(controller.WalkState);
            return;
        }

        else if (jumpInput
        && controller.coyoteTimeCounter > 0f 
        && controller.jumpBufferCounter > 0f) {
            
            stateMachine.ChangeState(controller.JumpState);
            controller.AirState.moveSpeed = 0;
            return;
        }

        else if (crouchInput) {
            stateMachine.ChangeState(controller.CrouchIdleState);
            return;
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public override void Exit() {
        base.Exit();
    }

}
