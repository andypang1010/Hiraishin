using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputController
{
    static KeyCode sprintKey = KeyCode.LeftShift;
    static KeyCode crouchKey = KeyCode.LeftControl;
    static KeyCode altCrouchKey = KeyCode.LeftCommand;
    static KeyCode jumpKey = KeyCode.Space;
    static KeyCode tagKey = KeyCode.T;
    static KeyCode pickupKey = KeyCode.E;
    static KeyCode bulletTimeKey = KeyCode.Q;
    static KeyCode throwKey = KeyCode.Mouse0;
    static KeyCode attackKey = KeyCode.Mouse1;
    static KeyCode teleportKey = KeyCode.Mouse2;

    public static Vector2 GetWalkDirection() {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public static Vector2 GetLookDirection() {
        return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    }

    public static bool GetSprint() {
        return Input.GetKey(sprintKey);
    }

    public static bool GetCrouchDown() {
        return Input.GetKeyDown(crouchKey) ^ Input.GetKeyDown(altCrouchKey);
    }

    public static bool GetCrouchHold() {
        return Input.GetKey(crouchKey) ^ Input.GetKey(altCrouchKey);
    }

    public static bool GetCrouchUp() {
        return Input.GetKeyUp(crouchKey) ^ Input.GetKeyUp(altCrouchKey);
    }

    public static bool GetJumpDown() {
        return Input.GetKeyDown(jumpKey);
    }

    public static bool GetTagDown() {
        return Input.GetKeyDown(tagKey);
    }

    public static bool GetTagHold() {
        return Input.GetKey(tagKey);
    }

    public static bool GetTagUp() {
        return Input.GetKeyUp(tagKey);
    }

    public static bool GetPickupDown() {
        return Input.GetKeyDown(pickupKey);
    }

    public static bool GetThrowDown() {
        return Input.GetKeyDown(throwKey);
    }

    public static bool GetBulletTimeDown() {
        return Input.GetKeyDown(bulletTimeKey);
    }

    public static bool GetTeleportDown() {
        return Input.GetKeyDown(teleportKey);
    }

    public static bool GetAttackDown() {
        return Input.GetKeyDown(attackKey);
    }
}
