using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlock : Interactables
{

    [Header("Unlock Abilities")]
    public bool unlockTeleport;
    public bool unlockTag;
    public bool unlockAttack;
    public bool unlockBulletTime;

    [Header("Interact")]
    public Interactables[] interactables;
    
    protected GameObject player;

    protected override void Start() {
        base.Start();
        player = GameObject.Find("PLAYER");
    }

    public override void OnInteract()
    {
        if (!isActivated)
        {
            return;
        }

        if (unlockAttack)
        {
            player.GetComponent<PlayerAttack>().enabled = true;
        }

        if (unlockTeleport)
        {
            player.GetComponent<PlayerTeleport>().enabled = true;
            player.GetComponent<PlayerThrow>().kunaiAvailable = true;
        }

        if (unlockTag)
        {
            player.GetComponent<PlayerTag>().enabled = true;
        }

        if (unlockBulletTime)
        {
            player.GetComponent<PlayerBulletTime>().enabled = true;
        }

        foreach (Interactables interactable in interactables)
        {
            interactable.isActivated = true;
        }

        HideAbility();
    }

    private void HideAbility()
    {
        isActivated = false;

        if (TryGetComponent(out Collider collider)) {
            collider.enabled = false;
        }

        foreach (Collider childCollider in GetComponentsInChildren<Collider>(true)) {
            childCollider.enabled = false;
        }

        
        if (TryGetComponent(out MeshRenderer meshRenderer)) {
            meshRenderer.enabled = false;
        }

        foreach (MeshRenderer childMesh in GetComponentsInChildren<MeshRenderer>(true)) {
            childMesh.enabled = false;
        }
    }
}
