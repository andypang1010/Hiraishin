using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactables
{
    public Interactables[] interactables;

    public override void OnInteract()
    {
        if (isActivated) {
            foreach (Interactables interactable in interactables) {
                interactable.isActivated = true;
            }

            isActivated = false;
        }
    }
}
