using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState
{
    protected PlayerController controller;
    protected PlayerStateMachine stateMachine;
    protected PlayerData data;

    public PlayerBaseState(PlayerController controller, PlayerStateMachine stateMachine, PlayerData data) {
        this.controller = controller;
        this.stateMachine = stateMachine;
        this.data = data;
    }

    public virtual void Enter() {}
    public virtual void LogicUpdate() {}
    public virtual void PhysicsUpdate() {}
    public virtual void Exit() {}
}
