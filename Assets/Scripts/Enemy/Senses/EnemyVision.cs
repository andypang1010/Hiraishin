using System.Collections;
using System.Collections.Generic;
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
        PlayerDistance = int.MaxValue;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null) {
            playerSeen = false;
            return;
        }

        Vector3 playerDirection = player.transform.position - eyeTransform.position;
        PlayerDistance = playerDirection.magnitude;

        // Check if player is within viewing distance, angle, and not obstructed
        if (PlayerDistance <= data.lookRadius
        && Vector3.Angle(transform.forward, playerDirection.normalized) <= data.lookAngle / 2
        && Physics.Raycast(eyeTransform.position, playerDirection.normalized, out RaycastHit hit, data.lookRadius)
        && hit.transform.gameObject == player) {

            playerSeen = true;
        }

        if (playerSeen) {
            PlayerSeenLocation = player.transform.position;
        }
    }

    void OnDrawGizmos() {
        void DrawArc(float startAngle, float endAngle, 
        Vector3 position, Quaternion orientation, float radius, 
        Color color, bool drawChord = false, bool drawSector = false, 
        int arcSegments = 32)
        {

            float arcSpan = Mathf.DeltaAngle(startAngle, endAngle);
        
            // Since Mathf.DeltaAngle returns a signed angle of the shortest path between two angles, it 
            // is necessary to offset it by 360.0 degrees to get a positive value
            if (arcSpan <= 0)
            {
                arcSpan += 360.0f;
            }
        
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

        DrawArc(-data.lookAngle / 2, data.lookAngle / 2, eyeTransform.position, Quaternion.LookRotation(-transform.right), data.lookRadius, Color.red, false, true);
    }

    public bool WithinAttackRadius() {
        return playerSeen && PlayerDistance <= data.attackReach * 1.1f;
    }
}
