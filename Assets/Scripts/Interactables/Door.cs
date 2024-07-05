using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactables
{
    public bool isOpen;

    public override void OnInteract()
    {
        isOpen = !isOpen;
    }

    void Update() {
        gameObject.SetActive(!isOpen);
    }
}
