using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;
    public PlayerThrow playerThrow;
    public PlayerBulletTime playerBulletTime;

    [Header("Texts")]
    public TMP_Text playerSpeed;
    public TMP_Text kunaiCount;
    public TMP_Text bulletTimeCD;
    public TMP_Text bulletTimeDuration;

    [Header("Settings")]
    public bool isDebug;


    void Update()
    {
        kunaiCount.text = "Kunai Count: " + playerThrow.kunaiRemaining;

        playerSpeed.gameObject.SetActive(isDebug);
        bulletTimeCD.gameObject.SetActive(isDebug);
        bulletTimeDuration.gameObject.SetActive(isDebug);

        if (isDebug) {
            playerSpeed.text = "Player Speed: " + Math.Round(playerMovement.GetMoveVelocity().magnitude, 1);
            bulletTimeCD.text = "Bullet Time CD: " + Math.Max(0, Math.Round(playerBulletTime.cooldownCounter, 1));
            bulletTimeDuration.text = "Bullet Time Duration: " + Math.Round(playerBulletTime.durationCounter, 1);
        }
    }
}
