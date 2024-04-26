using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState
{
    protected PlayerController playerController;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerData playerData;

    protected float startTime;

    public PlayerBaseState(PlayerController playerController, PlayerStateMachine playerStateMachine, PlayerData playerData) {
        this.playerController = playerController;
        this.playerStateMachine = playerStateMachine;
        this.playerData = playerData;
    }

    public virtual void Enter() {
        DoChecks();
        startTime = Time.time;
    }

    public virtual void LogicUpdate() {}

    public virtual void PhysicsUpdate() {
        DoChecks();
    }

    public virtual void Exit() {}

    public virtual void DoChecks() {}
}
