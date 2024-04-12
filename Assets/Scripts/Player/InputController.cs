using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode altCrouchKey = KeyCode.LeftCommand;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode throwKey = KeyCode.Mouse0;
    public KeyCode bulletTimeKey = KeyCode.Q;
    public KeyCode teleportKey = KeyCode.Mouse2;
    public KeyCode attackKey = KeyCode.Mouse1;

    public Vector2 GetWalkDirection() {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public Vector2 GetLookDirection() {
        return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    }

    public bool GetSprint() {
        return Input.GetKey(sprintKey);
    }

    public bool GetCrouch() {
        return Input.GetKey(crouchKey) ^ Input.GetKey(altCrouchKey);
    }

    public bool GetJump() {
        return Input.GetKeyDown(jumpKey);
    }

    public bool GetPickup() {
        return Input.GetKeyDown(pickupKey);
    }

    public bool GetThrow() {
        return Input.GetKeyDown(throwKey);
    }

    public bool GetBulletTime() {
        return Input.GetKeyDown(bulletTimeKey);
    }

    public bool GetTeleport() {
        return Input.GetKeyDown(teleportKey);
    }

    public bool GetAttack() {
        return Input.GetKeyDown(attackKey);
    }
}
