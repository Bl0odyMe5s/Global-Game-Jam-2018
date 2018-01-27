using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single point on a camera rail.
/// </summary>
public class CameraRailPointScript : MonoBehaviour {

    public float fov;
    public float timeToNeighbour;
    public Vector3 nextNeighbourPosition;
    public bool linear;
    public float distanceToNextNeighbour;
    public float speedInNode;

    private Vector3[] positions = new Vector3[4];
    

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.grey;
        Gizmos.DrawSphere(transform.position, 0.4f);
        if (nextNeighbourPosition != new Vector3())
        {
            Gizmos.color = CameraRailScript.railColour;
            Gizmos.DrawLine(transform.position, nextNeighbourPosition);
        }
        Gizmos.color = CameraRailScript.railDirectionColour;
        Gizmos.DrawLine(transform.position, transform.forward * 3 + transform.position);

        positions = new Vector3[4];
        positions[0] = transform.forward * 3 + transform.position + (0.8f * transform.right + 0.45f * transform.up) * fov / 15.5f;
        positions[1] = transform.forward * 3 + transform.position - (0.8f * transform.right + 0.45f * transform.up) * fov / 15.5f;
        positions[2] = transform.forward * 3 + transform.position + (0.8f * transform.right - 0.45f * transform.up) * fov / 15.5f;
        positions[3] = transform.forward * 3 + transform.position - (0.8f * transform.right - 0.45f * transform.up) * fov / 15.5f;

        Gizmos.DrawLine(transform.position, positions[0]);
        Gizmos.DrawLine(transform.position, positions[1]);
        Gizmos.DrawLine(transform.position, positions[2]);
        Gizmos.DrawLine(transform.position, positions[3]);

        Gizmos.DrawLine(positions[0], positions[3]);
        Gizmos.DrawLine(positions[1], positions[2]);
        Gizmos.DrawLine(positions[0], positions[2]);
        Gizmos.DrawLine(positions[3], positions[1]);
    }
    
}
