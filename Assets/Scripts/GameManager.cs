using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int frameRate;
    void Start()
    {
        Application.targetFrameRate = frameRate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
