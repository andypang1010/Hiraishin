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
        base.Enter();
        // Debug.Log("Crouch Idle State");

        // Shrink to crouch size
        controller.transform.localScale = new Vector3(controller.transform.localScale.x, data.crouchScale, controller.transform.localScale.z);
        
        // Apply downward force so doesn't float
        controller.rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (walkInput.sqrMagnitude > 0.1f) {
            stateMachine.ChangeState(controller.CrouchWalkState);
        }

        else if (!crouchInput) {
            controller.transform.localScale = new Vector3(controller.transform.localScale.x, controller.defaultScale, controller.transform.localScale.z);
            stateMachine.ChangeState(controller.IdleState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public override void Exit() {
        base.Exit();
    }

}
