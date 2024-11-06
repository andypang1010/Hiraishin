using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using Unity.AI.Navigation;
using UnityEngine;

public class Door : Interactables
{
    [Header("Navmesh")]
    public GameObject floor;
    public NavMeshSurface surface;
    
    [Header("Settings")]
    public bool isSingleUse;
    public bool isAutoClose;
    public Vector3 openValue;
    public float openDuration;
    public float startCloseTime;
    private bool isUsed;
    private bool isOpen;

    public override void OnInteract()
    {
        if (!isActivated || isUsed) return;

        if (isSingleUse) {
            isUsed = true;
        }

        if (!isOpen) {
            Open();

            if (isAutoClose) {
                Invoke(nameof(Close), startCloseTime);
            }
        }

        else {
            Close();
        }

        ReturnKunais();
    }

    private void ReturnKunais()
    {
        GameObject[] kunaisOnDoor = GetComponentsInChildren<Kunai>().Select(kunai => kunai.gameObject).ToArray();

        if (kunaisOnDoor.Length > 0)
        {
            foreach (GameObject kunai in kunaisOnDoor)
                PlayerTeleport.teleportables.Remove(kunai);
            GameObject.Find("PLAYER").GetComponent<PlayerThrow>().AddKunaiCount(kunaisOnDoor.Length);
        }

        foreach (GameObject kunai in kunaisOnDoor)
        {
            Destroy(kunai);
        }
    }

    private void Open() {
        transform.DOMove(transform.position + openValue, openDuration);
        isActivated = false;
        isOpen = true;

        RebuildNavmesh("Default");
    }

    private void Close() {
        transform.DOMove(transform.position - openValue, openDuration);
        isActivated = true;
        isOpen = false;

        RebuildNavmesh("Unwalkable");
    }

    private void RebuildNavmesh(string layer) {
        if (floor != null) {
            floor.layer = LayerMask.NameToLayer(layer);
            surface.BuildNavMesh();
        }
    }
}
