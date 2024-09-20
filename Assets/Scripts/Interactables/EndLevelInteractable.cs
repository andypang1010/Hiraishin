using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelInteractable : Interactables
{
    public bool destroy;
    public override void OnInteract()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        if (destroy) {
            Destroy(gameObject);
        }
    }
}
