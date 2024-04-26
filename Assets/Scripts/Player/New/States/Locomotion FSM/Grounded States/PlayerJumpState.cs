using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerGroundedState
{
    public PlayerJumpState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerData playerData)
     : base(playerController, playerStateMachine, playerData)
    {
    }

    public override void Enter() {
        base.Enter();
        Debug.Log("Jump State");

        // Reset jump buffer to prevent jumping again
        controller.jumpBufferCounter = 0f;

        // Resets y-velocity to have consistent jump height
        controller.rb.velocity = new Vector3(controller.rb.velocity.x, 0, controller.rb.velocity.z);
        controller.rb.AddForce(controller.transform.up * data.jumpForce, ForceMode.Impulse);

        stateMachine.ChangeState(controller.AirState);
        return;
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public override void Exit() {
        base.Exit();
    }
}
