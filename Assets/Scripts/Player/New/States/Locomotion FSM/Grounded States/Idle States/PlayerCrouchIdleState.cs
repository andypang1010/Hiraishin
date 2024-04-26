using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchIdleState : PlayerGroundedState
{
    public PlayerCrouchIdleState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerData playerData)
     : base(playerController, playerStateMachine, playerData)
    {
    }

    public override void Enter() {
        base.DoChecks();

        // Shrink to crouch size
        playerController.transform.localScale = new Vector3(playerController.transform.localScale.x, playerData.crouchScale, playerController.transform.localScale.z);
        
        // Apply downward force so doesn't float
        playerController.rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (walkInput.sqrMagnitude > 0.1f) {
            playerStateMachine.ChangeState(playerController.CrouchWalkState);
        }

        else if (!crouchInput) {
            playerController.transform.localScale = new Vector3(playerController.transform.localScale.x, playerController.defaultScale, playerController.transform.localScale.z);
            playerStateMachine.ChangeState(playerController.IdleState);
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
