using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerBaseState CurrentState { get; private set; }

    public void Initialize(PlayerBaseState initialState) {
        CurrentState = initialState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerBaseState newState) {
        Debug.Log(CurrentState + " -> " + newState);
        
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();

    }
}
