using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    public int frameRate;

    void Awake() {
        if (Instance != null) {
            Debug.LogWarning("More than one GameManager in scene");
        }

        Instance = this;
    }
    
    void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Kunai"), LayerMask.NameToLayer("Kunai"));
    }

    void Update() {
        Application.targetFrameRate = frameRate;
    }
}
