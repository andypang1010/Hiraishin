using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
    Vector3 moveDirection;
    public float moveSpeed;
    public PlayerAirState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerData playerData) : base(playerController, playerStateMachine, playerData)
    {
    }

    public override void Enter() {
        base.Enter();
        Debug.Log("Air State");
        Debug.Log(moveSpeed);

        controller.rb.drag = 0;
        controller.rb.useGravity = true;
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        moveDirection = (controller.transform.right * controller.inputController.GetMoveDirection().x + controller.transform.forward * controller.inputController.GetMoveDirection().y).normalized;

        SpeedControl(data.walkSpeed);

        controller.coyoteTimeCounter -= Time.deltaTime * Time.timeScale;

        if (Physics.Raycast(controller.transform.position, Vector3.down, data.playerHeight * 0.5f + 0.1f)) {
            stateMachine.ChangeState(controller.IdleState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        controller.rb.AddForce(10 * data.airMultiplier * moveSpeed * moveDirection, ForceMode.Force);
    }

    public override void Exit() {
        base.Exit();
    }

    protected void SpeedControl(float maxSpeed) {
        Vector3 rawVelocity = new Vector3(controller.rb.velocity.x, 0, controller.rb.velocity.z);

        // Clamp x and z axis velocity
        if (rawVelocity.magnitude > maxSpeed) {
            Vector3 clampedVelocity = rawVelocity.normalized * maxSpeed;
            controller.rb.velocity = new Vector3(clampedVelocity.x, controller.rb.velocity.y, clampedVelocity.z);
        }
    }


}
