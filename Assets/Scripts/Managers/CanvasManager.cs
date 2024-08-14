using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public TMP_Text playerSpeed;
    public TMP_Text kunaiCount;
    public TMP_Text bulletTimeCD;
    public TMP_Text bulletTimeDuration;
    public TMP_Text attackReady;

    public PlayerMovement playerMovement;
    public PlayerThrow playerThrow;
    public PlayerTag playerTag;
    public PlayerBulletTime playerBulletTime;
    public PlayerAttack playerAttack;

    // Update is called once per frame
    void Update()
    {
        playerSpeed.text = "Player Speed: " + Math.Round(playerMovement.GetMoveVelocity().magnitude, 1);
        kunaiCount.text = "Kunai Count: " + playerThrow.kunaiRemaining;
        bulletTimeCD.text = "Bullet Time CD: " + Math.Max(0, Math.Round(playerBulletTime.cooldownCounter, 1));
        bulletTimeDuration.text = "Bullet Time Duration: " + Math.Round(playerBulletTime.durationCounter, 1);
    }
}
