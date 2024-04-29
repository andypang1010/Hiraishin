using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerLocomotionState
{
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

        
        if (jumpInput) {
            controller.jumpBufferCounter = data.jumpBuffer;
        }

        else {
            controller.jumpBufferCounter -= Time.deltaTime * Time.timeScale;
        }

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

    }

    public override void Exit() {
        base.Exit();

    }

}
