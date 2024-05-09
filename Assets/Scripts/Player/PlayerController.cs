using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float cameraExplosionForce;
    GameObject cam;

    void Start()
    {
        cam = Camera.main.transform.parent.gameObject;
    }

    public void Decapacitate() {

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
        
    }
}
