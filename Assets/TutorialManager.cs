using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialMessage;

    private void Start() {
        if (tutorialMessage != null) {
            tutorialMessage.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.root.gameObject.Equals(GameObject.Find("PLAYER"))) {
            tutorialMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.transform.root.gameObject.Equals(GameObject.Find("PLAYER"))) {
            tutorialMessage.SetActive(false);
        }
    }

}