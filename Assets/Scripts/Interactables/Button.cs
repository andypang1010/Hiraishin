using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Button : Interactables
{
    public Interactables[] interactables;
    public Vector3 pressValue;
    public float moveDuration;
    public float startBounceTime;
    public bool canBounceBack;
    private bool isPressed;

    public override void OnInteract()
    {
        if (!isActivated)
        {
            return;
        }

        if (!isPressed) {
            Press();

            if (canBounceBack) {
                Invoke(nameof(Release), startBounceTime);
            }
        }
    }

    private void ActivateConnectedInteractables()
    {
        foreach (Interactables interactable in interactables)
        {
            interactable.isActivated = true;
            interactable.OnInteract();
        }
    }

    private void Press() {
        isPressed = true;
        transform.DOMove(transform.position + pressValue, moveDuration);

        isActivated = false;

        ActivateConnectedInteractables();
    }

    private void Release() {
        isPressed = false;
        transform.DOMove(transform.position - pressValue, moveDuration);

        isActivated = true;
    }
}
