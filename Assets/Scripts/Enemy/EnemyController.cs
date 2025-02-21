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

    public bool isDead;

    void Start()
    {
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
            rb.mass = 0.01f;
        }

        foreach (var component in GetComponents<Component>()) {
            if (component.GetType() == typeof(EnemyController)
            || component.GetType() == typeof(Transform)) {
                continue;
            }

            else if (component.GetType() == typeof(Animator)) {
                (component as Animator).enabled = false;
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
        && !agent.isOnNavMesh
        
        || isDead) {
            ActivateRagdoll();
        }
    }
}
