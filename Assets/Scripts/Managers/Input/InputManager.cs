using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, PlayerInputs.IGameplayActions
{
    public static InputManager Instance { get; private set;}

    public Vector2 WalkDirection { get; private set; }
    public Vector2 LookDirection { get; private set; }
    public bool CrouchDown { get; private set; }
    public bool CrouchUp { get; private set; }
    public bool CrouchHold { get; private set; }
    public bool Sprint { get; private set; }
    public bool Jump { get; private set; }
    public bool TagDown { get; private set; }
    public bool TagUp { get; private set; }
    public bool TagHold { get; private set; }
    public bool PickUp { get; private set; }
    public bool Throw { get; private set; }
    public bool BulletTime { get; private set; }
    public bool Teleport { get; private set; }
    public bool Attack { get; private set; }

    void Awake() {
        if (Instance != null) {
            Debug.LogWarning("More than one InputController in scene");
        }

        Instance = this;
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        WalkDirection = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {

        LookDirection = context.ReadValue<Vector2>();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        CrouchDown = context.started && context.performed;
        // print("Crouch Down: " + CrouchDown);
        CrouchHold = context.performed;
        // print("Crouch Hold: " + CrouchHold);
        CrouchUp = context.canceled;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        Sprint = context.performed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump = context.performed;
    }

    public void OnTag(InputAction.CallbackContext context)
    {
        TagDown = context.started;
        TagHold = context.performed;
        TagUp = context.canceled;
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        PickUp = context.started;
    }

    public void OnBulletTime(InputAction.CallbackContext context)
    {
        BulletTime = context.started;
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        Throw = context.started;
    }

    public void OnTeleport(InputAction.CallbackContext context)
    {
        Teleport = context.performed;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Attack = context.started;
    }
}
