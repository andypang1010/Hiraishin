using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;
    public PlayerThrow playerThrow;
    public PlayerBulletTime playerBulletTime;

    [Header("Texts")]
    public TMP_Text playerSpeed;
    public Image kunai;
    public TMP_Text kunaiCount;
    public TMP_Text bulletTimeCD;
    public TMP_Text bulletTimeDuration;

    [Header("Settings")]
    public bool isDebug;


    void Update()
    {
        if (playerThrow.kunaiAvailable) {
            kunai.gameObject.SetActive(true);
        }

        else {
            kunai.gameObject.SetActive(false);
        }

        kunaiCount.text = " × " + playerThrow.kunaiRemaining;

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
