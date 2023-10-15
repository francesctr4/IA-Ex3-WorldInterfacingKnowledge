using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Camera position parameters

    public struct Waypoint
    {
        public Transform wp; // Array of waypoint positions
        public float teleportInterval; // Time interval between teleports in seconds
    }

    public Transform[] waypoints;
    public float[] teleportIntervals;

    public Waypoint[] wps;

    private int currentWaypoint = 0;
    private float lastTeleportTime;

    private void Start()
    {
        wps = new Waypoint[waypoints.Length];

        for (int i = 0; i < waypoints.Length; i++)
        {
            wps[i].wp = waypoints[i];
            wps[i].teleportInterval = teleportIntervals[i];
        }

        SetWaypoints();
        TeleportToWaypoint(currentWaypoint);
    }

    private void SetWaypoints()
    {
        if (wps.Length < 2)
        {
            Debug.LogError("You need at least 2 waypoints for camera teleportation.");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        // Check if it's time to teleport to the next waypoint
        if (Time.time - lastTeleportTime >= wps[currentWaypoint].teleportInterval)
        {
            // Update the last teleport time
            lastTeleportTime = Time.time;

            // Switch to the next waypoint

            //currentWaypoint = (currentWaypoint + 1) % wps.Length;

            if (currentWaypoint < wps.Length - 1)
            {
                currentWaypoint++;
            }
            
            TeleportToWaypoint(currentWaypoint);
        }
    }

    private void TeleportToWaypoint(int waypointIndex)
    {
        // Change the position and the rotation of the camera to the next waypoint
        transform.position = wps[waypointIndex].wp.position;
        transform.rotation = wps[waypointIndex].wp.rotation;
    }

}
