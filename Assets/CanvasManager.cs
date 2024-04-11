using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public TMP_Text kunaiCount;
    public TMP_Text bulletTimeCD;
    public TMP_Text bulletTimeDuration;

    public PlayerThrow playerThrow;
    public PlayerBulletTime playerBulletTime;

    // Update is called once per frame
    void Update()
    {
        kunaiCount.text = "Kunai Count: " + playerThrow.kunaiRemaining;
        bulletTimeCD.text = "Bullet Time CD: " + Math.Max(0, Math.Round(playerBulletTime.cooldownCounter, 1));
        bulletTimeDuration.text = "Bullet Time Duration: " + Math.Round(playerBulletTime.durationCounter, 1);
    }
}
