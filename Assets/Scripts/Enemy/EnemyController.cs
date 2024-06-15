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
    // List<Rigidbody> ragdollRBs;
    
    [Header("Sinking")]
    public float startSinkTime;
    public float sinkSpeed;
    public float destroyTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        DeactivateRagdoll();
    }

    public void Die() {

        ActivateRagdoll();
        PlayerTeleport.teleportables.Remove(gameObject);
    }

    void ActivateRagdoll() {
        foreach (Rigidbody rb in new List<Rigidbody>(transform.GetComponentsInChildren<Rigidbody>())) {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        foreach (var component in GetComponents<Component>()) {
            if (component.GetType() == typeof(EnemyController)
            || component.GetType() == typeof(Transform)) {
                continue;
            }

            else {
                Destroy(component);
            }
        }
    }

    void DeactivateRagdoll() {
        foreach (Rigidbody rb in new List<Rigidbody>(transform.GetComponentsInChildren<Rigidbody>())) {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void Update() {
        if (TryGetComponent(out NavMeshAgent agent)
        && !agent.isOnNavMesh) {
            ActivateRagdoll();
        }
    }
}
