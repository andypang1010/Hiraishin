using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerMovingState
{
    public PlayerSprintState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerData playerData) : base(playerController, playerStateMachine, playerData)
    {
    }

    public override void Enter() {
        base.DoChecks();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (walkInput.sqrMagnitude < 0.1f) {
            playerStateMachine.ChangeState(playerController.IdleState);
        }

        else if (jumpInput) {
            playerStateMachine.ChangeState(playerController.JumpState);
        }

        else if (!sprintInput) {
            playerStateMachine.ChangeState(playerController.WalkState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        Move(playerData.sprintSpeed);
        SpeedControl(playerData.sprintSpeed);
    }

    public override void Exit() {
        base.Exit();
    }

    public override void DoChecks() {
        base.DoChecks();
    }
}
