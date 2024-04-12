using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletTime : MonoBehaviour
{
    [Header("Settings")]
    public float dilutedTimeScale;
    public float bulletTimeDuration;
    public float bulletTimeCD;
    [HideInInspector] public float durationCounter;
    [HideInInspector] public float cooldownCounter;

    bool inBulletTime;
    float defaultTimeScale;
    float defaultDeltaTime;
    bool startCooldown;
    InputController inputController;

    void Start() {
        defaultTimeScale = Time.timeScale;
        defaultDeltaTime = Time.fixedDeltaTime;

        inputController = GetComponent<InputController>();

        startCooldown = true;
    }

    void Update()
    {
        // When toggles bullet time key, cooldown is complete, and not in bullet time mode
        if (inputController.GetBulletTimeDown()) {
            if (cooldownCounter < 0 && !inBulletTime) {
                inBulletTime = true;
                startCooldown = false;

                // Initialize duration counter
                durationCounter = 0;
            }

            else if (inBulletTime)
            {
                StartCoolDown();
            }
        }

        else {

            // Decrement CD countdown
            cooldownCounter -= Time.deltaTime / Time.timeScale;
        }

        // In bullet time mode and duration is still within max duration
        if (inBulletTime && durationCounter < bulletTimeDuration) {
            durationCounter += Time.deltaTime / Time.timeScale;

            // Slow down time
            Time.timeScale = dilutedTimeScale;
            Time.fixedDeltaTime = defaultDeltaTime * dilutedTimeScale;
        }

        else {
            // Set time to regular scale
            Time.timeScale = defaultTimeScale;
            Time.fixedDeltaTime = defaultDeltaTime;

            StartCoolDown();
        }
    }

    private void StartCoolDown()
    {
        if (!startCooldown)
        {
            inBulletTime = false;
            startCooldown = true;

            // Cooldown time is dependent on how long the ability is used
            cooldownCounter = bulletTimeCD * (durationCounter / bulletTimeDuration);
        }
    }
}
