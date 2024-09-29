using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("References")]
    public EnemyData data;
    public Transform eyeTransform;
    GameObject player;

    [Header("Detection")]
    public bool playerSeen;
    public float PlayerDistance { get; private set; }
    public Vector3 PlayerSeenLocation { get; private set; }

    void Start()
    {
        playerSeen = false;
        PlayerDistance = float.MaxValue;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player.GetComponent<PlayerController>().isDead) {
            playerSeen = false;
            return;
        }

        Vector3 playerDirection = player.transform.position - eyeTransform.position;
        PlayerDistance = playerDirection.magnitude;

        // Check if player is within radius
        if (PlayerDistance <= data.lookRadius

        // Check if player's within the angle on XZ plane
        && Vector3.Angle(eyeTransform.forward, (playerDirection - Vector3.up * playerDirection.y).normalized) <= data.lookAngle

        // Check if player is not obstructed by obstacles
        && Physics.Raycast(eyeTransform.position, playerDirection.normalized, out RaycastHit hit, data.lookRadius)
        && hit.transform.gameObject == player

        // Check if player's y value is within height
        && playerDirection.y > -data.lookHeight * 0.5f
        && playerDirection.y < data.lookHeight * 0.5f) {

            playerSeen = true;
        }

        if (playerSeen) {
            PlayerSeenLocation = player.transform.position;
        }
    }

    public bool WithinAttackRadius() {
        print("IsPlayerSeen: " + playerSeen);
        print("Player is within attack reach: " + (PlayerDistance < data.attackReach * 1.2f));

        return playerSeen && (PlayerDistance < data.attackReach * 1.2f);
    }

    void OnDrawGizmos() {
        void DrawArc(float startAngle, float endAngle, 
        Vector3 position, Quaternion orientation, float radius, 
        Color color, bool drawChord = false, bool drawSector = false, 
        int arcSegments = 32)
        {

            float arcSpan = Mathf.DeltaAngle(startAngle, endAngle);
        
            // angle step is calculated by dividing the arc span by number of approximation segments
            float angleStep = (arcSpan / arcSegments) * Mathf.Deg2Rad;
            float stepOffset = startAngle * Mathf.Deg2Rad;
        
            // stepStart, stepEnd, lineStart and lineEnd variables are declared outside of the following for loop
            float stepStart = 0.0f;
            float stepEnd = 0.0f;
            Vector3 lineStart = Vector3.zero;
            Vector3 lineEnd = Vector3.zero;
        
            // arcStart and arcEnd need to be stored to be able to draw segment chord
            Vector3 arcStart = Vector3.zero;
            Vector3 arcEnd = Vector3.zero;
        
            // arcOrigin represents an origin of a circle which defines the arc
            Vector3 arcOrigin = position;
        
            for (int i = 0; i < arcSegments; i++)
            {
                // Calculate approximation segment start and end, and offset them by start angle
                stepStart = angleStep * i + stepOffset;
                stepEnd = angleStep * (i + 1) + stepOffset;
        
                lineStart.x = Mathf.Cos(stepStart);
                lineStart.y = 0.0f;
                lineStart.z = Mathf.Sin(stepStart);
        
                lineEnd.x = Mathf.Cos(stepEnd);
                lineEnd.y = 0.0f;
                lineEnd.z = Mathf.Sin(stepEnd);
        
                // Results are multiplied so they match the desired radius
                lineStart *= radius;
                lineEnd *= radius;
        
                // Results are multiplied by the orientation quaternion to rotate them 
                // since this operation is not commutative, result needs to be
                // reassigned, instead of using multiplication assignment operator (*=)
                lineStart = orientation * lineStart;
                lineEnd = orientation * lineEnd;
        
                // Results are offset by the desired position/origin 
                lineStart += position;
                lineEnd += position;
        
                // If this is the first iteration, set the chordStart
                if (i == 0)
                {
                    arcStart = lineStart;
                }
        
                // If this is the last iteration, set the chordEnd
                if(i == arcSegments - 1)
                {
                    arcEnd = lineEnd;
                }
        
                Debug.DrawLine(lineStart, lineEnd, color);
            }
        
            if (drawChord)
            {
                Debug.DrawLine(arcStart, arcEnd, color);
            }
            if (drawSector)
            {
                Debug.DrawLine(arcStart, arcOrigin, color);
                Debug.DrawLine(arcEnd, arcOrigin, color);
            }
        }

        DrawArc(-data.lookAngle, data.lookAngle, eyeTransform.position + data.lookHeight * Vector3.up * 0.5f, Quaternion.LookRotation(-eyeTransform.right), data.lookRadius, Color.red, false, true);
        DrawArc(-data.lookAngle, data.lookAngle, eyeTransform.position + data.lookHeight * Vector3.down * 0.5f, Quaternion.LookRotation(-eyeTransform.right), data.lookRadius, Color.red, false, true);
        // DrawArc(-data.lookAngle, data.lookAngle, eyeTransform.position, Quaternion.LookRotation(-eyeTransform.right), data.lookRadius, Color.red, false, true);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyeTransform.position + data.lookHeight * Vector3.up * 0.5f, eyeTransform.position + data.lookHeight * Vector3.down * 0.5f);
    
    }
}
