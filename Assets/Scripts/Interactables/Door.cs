using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Door : Interactables
{
    public bool isOpen;
    public Vector3 openValue;
    public float openDuration;

    public override void OnInteract()
    {
        if (!isActivated) return;

        isOpen = !isOpen;

        if (isOpen) {
            transform.DOMove(transform.position + openValue, openDuration);
            isActivated = false;
        }

        GameObject[] kunaisOnDoor = GetComponentsInChildren<Kunai>().Select(kunai => kunai.gameObject).ToArray();

        if (kunaisOnDoor.Length > 0) {
            foreach (GameObject kunai in kunaisOnDoor)
            PlayerTeleport.teleportables.Remove(kunai);
            GameObject.Find("PLAYER").GetComponent<PlayerThrow>().AddKunaiCount(kunaisOnDoor.Length);
        }

        foreach (GameObject kunai in kunaisOnDoor) {
            Destroy(kunai);
        }
    }
}
