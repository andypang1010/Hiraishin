using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    protected Vector2 walkInput;
    protected bool crouchInput, sprintInput, jumpInput;

    public PlayerGroundedState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerData playerData)
     : base(playerController, playerStateMachine, playerData)
    {
    }

    public override void Enter() {
        base.DoChecks();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        
        playerController.rb.drag = playerData.groundDrag;

        // Handle Grounded States Inputs
        walkInput = playerController.inputController.GetWalkDirection();
        crouchInput = playerController.inputController.GetCrouchHold();
        sprintInput = playerController.inputController.GetSprintHold();
        jumpInput = playerController.inputController.GetJumpDown();
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
