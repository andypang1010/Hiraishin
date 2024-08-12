using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Button : Interactables
{
    public Interactables[] interactables;
    public Vector3 pressValue;
    public float moveDuration;

    public override void OnInteract()
    {
        transform.DOMove(transform.position + pressValue, moveDuration);
        if (isActivated) {
            foreach (Interactables interactable in interactables) {
                interactable.isActivated = true;
                interactable.OnInteract();
            }

            isActivated = false;
        }
    }
}
