using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public TMP_Text playerSpeed;
    public TMP_Text kunaiCount;
    public TMP_Text throwReady;
    public TMP_Text bulletTimeCD;
    public TMP_Text bulletTimeDuration;

    public PlayerMovement playerMovement;
    public PlayerThrow playerThrow;
    public PlayerBulletTime playerBulletTime;

    // Update is called once per frame
    void Update()
    {
        Vector3 playerVelocity = new Vector3(playerMovement.GetComponent<Rigidbody>().velocity.x, 0, playerMovement.GetComponent<Rigidbody>().velocity.z);
        playerSpeed.text = "Player Speed: " + Math.Round(playerVelocity.magnitude, 1);
        kunaiCount.text = "Kunai Count: " + playerThrow.kunaiRemaining;
        throwReady.text = "Throw Ready: " + playerThrow.readyToThrow.ToString();
        bulletTimeCD.text = "Bullet Time CD: " + Math.Max(0, Math.Round(playerBulletTime.cooldownCounter, 1));
        bulletTimeDuration.text = "Bullet Time Duration: " + Math.Round(playerBulletTime.durationCounter, 1);
    }
}
