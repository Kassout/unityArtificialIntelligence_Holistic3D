using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    private Transform goal;
    private float speed = 5.0f;
    private float accuracy = 1.0f;
    private float rotSpeed = 2.0f;

    public GameObject wpManager;

    private GameObject[] wps;
    private GameObject currentNode;
    private int currentWP = 0;
    private Graph g;
    
    // Start is called before the first frame update
    void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        currentNode = wps[0];
    }

    public void GoToHeli()
    {
        g.AStar(currentNode, wps[4]);
        currentWP = 0;
    }

    public void GoToRuin()
    {
        g.AStar(currentNode, wps[0]);
        currentWP = 0;
    }

    public void GoToTanks()
    {
        g.AStar(currentNode, wps[8]);
        currentWP = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (g.getPathLength() == 0 || currentWP == g.getPathLength())
        {
            return;
        }
        
        // the node we are closest to at this moment
        currentNode = g.getPathPoint(currentWP);
        
        // if we are close enough to the current waypoint move to next
        if (Vector3.Distance(g.getPathPoint(currentWP).transform.position, transform.position) < accuracy)
        {
            currentWP++;
        }
        
        // if we are not at the end of the path
        if (currentWP < g.getPathLength())
        {
            goal = g.getPathPoint(currentWP).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x, transform.position.y, goal.position.z);
            Vector3 direction = lookAtGoal - transform.position;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                Time.deltaTime * rotSpeed);
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
}
