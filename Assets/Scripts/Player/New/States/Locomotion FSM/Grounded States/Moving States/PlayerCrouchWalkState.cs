using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchWalkState : PlayerMovingState
{
    public PlayerCrouchWalkState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerData playerData) : base(playerController, playerStateMachine, playerData)
    {
    }

    public override void Enter() {
        base.Enter();
        // Debug.Log("Crouch Walk State");

        // Shrink to crouch size
        controller.transform.localScale = new Vector3(controller.transform.localScale.x, data.crouchScale, controller.transform.localScale.z);
        
        // Apply downward force so doesn't float
        controller.rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        SpeedControl(data.crouchSpeed);

        if (walkInput.sqrMagnitude < 0.1f) {
            stateMachine.ChangeState(controller.CrouchIdleState);
        }

        else if (!crouchInput) {
            controller.transform.localScale = new Vector3(controller.transform.localScale.x, controller.defaultScale, controller.transform.localScale.z);
            stateMachine.ChangeState(controller.WalkState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        Move(data.crouchSpeed);
    }

    public override void Exit() {
        base.Exit();
    }

}
