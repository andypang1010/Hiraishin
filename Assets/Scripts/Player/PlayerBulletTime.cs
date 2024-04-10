using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletTime : MonoBehaviour
{
    public KeyCode bulletTimeKey;
    public bool inBulletTime;
    public float dilutedTimeScale;

    float defaultTimeScale;
    float defaultDeltaTime;

    void Start() {
        defaultTimeScale = Time.timeScale;
        defaultDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(bulletTimeKey)) {
            inBulletTime = !inBulletTime;
        }

        if (inBulletTime) {

            // Slow down time
            Time.timeScale = dilutedTimeScale;
            Time.fixedDeltaTime = defaultDeltaTime * dilutedTimeScale;
        }
        else {
            inBulletTime = false;

            // Set time to regular scale
            Time.timeScale = defaultTimeScale;
            Time.fixedDeltaTime = defaultDeltaTime;
        }
    }
}
