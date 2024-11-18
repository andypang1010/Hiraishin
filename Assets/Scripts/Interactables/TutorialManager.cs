using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public AbilityUnlock ability;
    public GameObject tutorialMessage;

    private void Start() {
        if (tutorialMessage != null) {
            tutorialMessage.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.transform.root.gameObject.Equals(GameObject.Find("PLAYER"))) {
            if (ability == null || (ability != null && !ability.isActivated))
            {
                tutorialMessage.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.transform.root.gameObject.Equals(GameObject.Find("PLAYER"))) {
            tutorialMessage.SetActive(false);
        }
    }

}