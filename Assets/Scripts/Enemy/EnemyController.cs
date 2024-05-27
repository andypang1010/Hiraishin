using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EzySlice;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum States {
        PATROL,
        QUESTION,
        CHASE
    }
    [Header("References")]
    public GameObject player;
    
    [Header("Sinking")]
    public float startSinkTime;
    public float sinkSpeed;
    public float destroyTime;

    Animator animator;
    float velX, velZ;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Slice(Transform slicePlane, float attackForce) {
        SlicedHull hull = gameObject.Slice(slicePlane.position, slicePlane.up, gameObject.GetComponent<Renderer>().material);

        if (hull != null) {

            // Create upper and lower hulls
            GameObject upperHull = hull.CreateUpperHull(gameObject);
            GameObject lowerHull = hull.CreateLowerHull(gameObject);
            SetupComponent(upperHull, attackForce);
            SetupComponent(lowerHull, attackForce);
            
            // Automatically collects all kunais on the target
            player.GetComponent<PlayerThrow>().AddKunaiCount(gameObject.GetComponentsInChildren<Kunai>().Length);

            // Remove target from teleportables list
            PlayerTeleport.teleportables.Remove(gameObject);
            Destroy(gameObject);
        }

    }

    void SetupComponent(GameObject slicedObject, float attackForce) {

        // Adding rigidBody and meshCollider to slicedObject
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(attackForce, slicedObject.transform.position, 1);
        // StartCoroutine(Sink(slicedObject));
    }

    // IEnumerator Sink(GameObject target)
    // {
    //     yield return new WaitForSeconds(startSinkTime);
    //     target.GetComponent<MeshCollider>().enabled = false;
    //     target.GetComponent<Rigidbody>().isKinematic = true;

    //     // Destroy all kunais on the target
    //     foreach (Kunai kunai in target.GetComponentsInChildren<Kunai>()) {
    //         Destroy(kunai.gameObject);
    //     }
    //     player.GetComponent<PlayerThrow>().AddKunaiCount(target.GetComponentsInChildren<Kunai>().Length);

    //     // Start sinking
    //     float time = 0;
    //     while (time < destroyTime)
    //     {
    //         target.transform.position = new Vector3(
    //             target.transform.position.x, 
    //             target.transform.position.y - sinkSpeed, 
    //             target.transform.position.z
    //         );

    //         time += Time.fixedDeltaTime;
    //         yield return null;
    //     }

    //     Destroy(target);
    // }
}
