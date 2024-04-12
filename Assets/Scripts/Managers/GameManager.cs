using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int frameRate;
    void Start()
    {
        Application.targetFrameRate = frameRate;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Kunai"), LayerMask.NameToLayer("Kunai"));
    }
}
