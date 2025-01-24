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

    [Header("UI")]
    public GameObject kunaiHUD;
    public GameObject bulletTimeHUD;
    public TMP_Text kunaiCount;


    void Update()
    {
        kunaiHUD.SetActive(playerThrow.kunaiAvailable);
        bulletTimeHUD.SetActive(playerBulletTime.enabled);

        kunaiCount.text = " × " + playerThrow.kunaiRemaining;
    }
}
