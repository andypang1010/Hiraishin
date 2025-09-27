using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("References")]
    public PlayerController playerController;
    public PlayerMovement playerMovement;
    public PlayerThrow playerThrow;
    public PlayerBulletTime playerBulletTime;

    [Header("UI")]
    public GameObject kunaiHUD;
    public GameObject bulletTimeHUD;
    public TMP_Text kunaiCount;


    void Update()
    {
        if (playerController.isDead) {
            kunaiHUD.SetActive(false);
            bulletTimeHUD.SetActive(false);
            return;
        }
        
        kunaiHUD.SetActive(playerThrow.kunaiAvailable);
        bulletTimeHUD.SetActive(playerBulletTime.enabled);
        kunaiCount.text = " × " + playerThrow.kunaiRemaining;
    }
}
