using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerData playerData;
    public InputController inputController;

    [Header("Player Settings")]
    public Rigidbody rb;
    public Transform stepRayLower, stepRayUpper;
    public Transform throwPoint;
    public float defaultScale;
    public float coyoteTimeCounter;
    public float jumpBufferCounter;

    [Header("Locomotion FSM")]
    public PlayerStateMachine locomotionStateMachine;
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchWalkState CrouchWalkState { get; private set; }
    public PlayerSprintState SprintState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }

    [Header("Action FSM")]
    public PlayerStateMachine actionStateMachine;

    void Awake() {
        locomotionStateMachine = new PlayerStateMachine();
        actionStateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, locomotionStateMachine, playerData);
        WalkState = new PlayerWalkState(this, locomotionStateMachine, playerData);
        CrouchIdleState = new PlayerCrouchIdleState(this, locomotionStateMachine, playerData);
        CrouchWalkState = new PlayerCrouchWalkState(this, locomotionStateMachine, playerData);
        SprintState = new PlayerSprintState(this, locomotionStateMachine, playerData);
        JumpState = new PlayerJumpState(this, locomotionStateMachine, playerData);
        AirState = new PlayerAirState(this, locomotionStateMachine, playerData);
    }

    void Start()
    {
        locomotionStateMachine.Initialize(IdleState);
        actionStateMachine.Initialize(null);

        inputController = GetComponent<InputController>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        stepRayUpper.transform.position = stepRayLower.transform.position + playerData.stepHeight * Vector3.up;
        defaultScale = transform.localScale.y;
    }

    void Update()
    {
        locomotionStateMachine.CurrentState.LogicUpdate();
        // actionStateMachine.CurrentState.LogicUpdate();
    }

    void FixedUpdate() {
        locomotionStateMachine.CurrentState.PhysicsUpdate();
        // actionStateMachine.CurrentState.PhysicsUpdate();
    }

    public Vector3 GetMoveVelocity() {
        return new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
}
