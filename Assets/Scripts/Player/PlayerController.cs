using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public float cameraExplosionForce;
    public Volume volume;
    ChromaticAberration chromaticAberration;
    GameObject cam;

    void Start()
    {

        cam = Camera.main.transform.parent.gameObject;
        if (!volume.profile.TryGet(out chromaticAberration)) {
            Debug.LogWarning("No Chromatic Aberration component found on Global Volume");
        }
        else {
            chromaticAberration.intensity.value = 0f;
        }
    }

    public void Decapacitate() {
        chromaticAberration.intensity.value = 1f;

        // Remove HeadBob and MoveCamera
        cam.GetComponent<HeadBob>().enabled = false;
        cam.GetComponent<MoveCamera>().enabled = false;

        // Add rigidbody and collider to camera
        Rigidbody camRB = Camera.main.gameObject.AddComponent<Rigidbody>();
        camRB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        camRB.AddForce(cameraExplosionForce * Camera.main.transform.up, ForceMode.Impulse);

        Camera.main.gameObject.AddComponent<SphereCollider>();

        // Destroy player gameObject
        Destroy(gameObject);
    }

    public void Shot() {
        // TODO
    }
}
