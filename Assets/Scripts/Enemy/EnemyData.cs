using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Data/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    [Header("Listen Settings")]
    public float listenRadius;
    public float movementThreshold;

    [Header("Vision Settings")]
    public float lookRadius;
    [Range(0, 180)] public float lookAngle;

    [Header("Patrol Settings")]
    public float patrolSpeed;
    public float minTargetDistance;

    [Header("Search Settings")]
    public float searchSpeed;
    public float searchDuration;

    [Header("Chase Settings")]
    public float chaseSpeed;

    [Header("Evade Settings")]
    public float evadeSpeed;
    public float startEvadeDistance;
    public float evadeSafeDistance;

    [Header("Alert Settings")]
    public float alertRadius;

    [Header("Attack Settings")]
    public float attackCD;
    public float attackReach; 
}