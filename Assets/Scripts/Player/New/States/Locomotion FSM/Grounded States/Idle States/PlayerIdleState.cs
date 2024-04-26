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
        Debug.Log("Entered Idle State");
        base.DoChecks();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (walkInput.sqrMagnitude > 0.1f) {
            playerStateMachine.ChangeState(playerController.WalkState);
        }

        else if (jumpInput) {
            playerStateMachine.ChangeState(playerController.JumpState);
        }

        else if (crouchInput) {
            playerStateMachine.ChangeState(playerController.CrouchIdleState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void DoChecks() {
        base.DoChecks();
    }
}
