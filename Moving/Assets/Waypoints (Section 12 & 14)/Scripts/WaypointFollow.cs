using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class WaypointFollow : MonoBehaviour
{
    //private GameObject[] waypoints;
    public UnityStandardAssets.Utility.WaypointCircuit circuit;
    private int currentWP = 0;

    public float speed = 1.0f;
    private float accuracy = 1.0f; 
    public float rotSpeed = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        //waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (circuit.Waypoints.Length == 0)
        {
            return;
        }

        Vector3 lookAtGoal = new Vector3(circuit.Waypoints[currentWP].transform.position.x, transform.position.y, circuit.Waypoints[currentWP].transform.position.z);
        Vector3 direction = lookAtGoal - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);

        // To loop waypoints like a circuit
        if (direction.magnitude < accuracy)
        {
            currentWP++;
            if (currentWP >= circuit.Waypoints.Length)
            {
                currentWP = 0;
            }
        }
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
