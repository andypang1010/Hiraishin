using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : Interactables
{
    public bool isOpen;

    public override void OnInteract()
    {
        isOpen = !isOpen;

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

    protected override void Update() {
        base.Update();
        gameObject.SetActive(!isOpen);
    }
}
