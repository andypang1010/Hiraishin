using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerBulletTime : MonoBehaviour
{
    [Header("References")]
    public Slider bulletTimeSlider;
    public Image bulletTimeFill;
    public Color cooldownColor;
    public Color readyColor;
    [Header("Settings")]
    public float dilutedTimeScale;
    public float bulletTimeDuration;
    public float bulletTimeCD;
    [HideInInspector] public float durationCounter { get; private set; }
    [HideInInspector] public float cooldownCounter { get; private set; }

    public bool inBulletTime {get; private set; }
    float defaultTimeScale;
    float defaultDeltaTime;
    bool startCooldown;
    public Volume volume;
    ColorAdjustments colorAdjustments;

    void Start() {
        defaultTimeScale = Time.timeScale;
        defaultDeltaTime = Time.fixedDeltaTime;

        if (!volume.profile.TryGet(out colorAdjustments)) {
            Debug.LogWarning("No Lens Distortion component found on Global Volume");
        }

        startCooldown = true;

        bulletTimeSlider.maxValue = bulletTimeDuration;
        bulletTimeSlider.value = bulletTimeDuration;
    }

    void Update()
    {
        // When toggles bullet time key, cooldown is complete, and not in bullet time mode
        if (InputController.Instance.GetBulletTimeDown()) {
            if (cooldownCounter <= 0 && !inBulletTime) {
                inBulletTime = true;
                startCooldown = false;

                colorAdjustments.active = true;

                // Initialize duration counter
                durationCounter = 0;

                bulletTimeSlider.maxValue = bulletTimeDuration;
            }

            else if (inBulletTime)
            {
                StartCoolDown();
            }
        }

        else {

            // Decrement CD countdown
            cooldownCounter -= Time.unscaledDeltaTime;
        }

        // In bullet time mode and duration is still within max duration
        if (inBulletTime && durationCounter < bulletTimeDuration) {

            durationCounter += Time.unscaledDeltaTime;

            bulletTimeSlider.value = bulletTimeDuration - durationCounter;

            // Slow down time
            Time.timeScale = dilutedTimeScale;
            Time.fixedDeltaTime = defaultDeltaTime * dilutedTimeScale;
        }

        else {
            // Set time to regular scale
            Time.timeScale = defaultTimeScale;
            Time.fixedDeltaTime = defaultDeltaTime;

            if (cooldownCounter > 0) {
                bulletTimeSlider.value = bulletTimeCD - cooldownCounter;
                bulletTimeFill.color = cooldownColor;
            }

            else {
                bulletTimeFill.color = readyColor;
            }

            StartCoolDown();
        }
    }

    private void StartCoolDown()
    {
        if (!startCooldown)
        {
            inBulletTime = false;
            startCooldown = true;

            colorAdjustments.active = false;

            // Cooldown time is dependent on how long the ability is used
            cooldownCounter = bulletTimeCD * (durationCounter / bulletTimeDuration);

            bulletTimeSlider.maxValue = bulletTimeCD;

        }
    }
}
