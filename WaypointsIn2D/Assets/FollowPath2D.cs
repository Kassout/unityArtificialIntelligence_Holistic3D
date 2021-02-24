using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath2D : MonoBehaviour {

	Transform goal;
	float speed = 5.0f;
	float accuracy = 1.0f;
	float rotSpeed = 2.0f;
	public GameObject wpManager;
	GameObject[] wps;
	GameObject currentNode;
	int currentWP = 0;
	Graph g;

	// Use this for initialization
	void Start () {
        //NOTE: ----------------------
        //For 2D best to have the z values of the waypoints the same as for
        //the tank -------------------
  		wps = wpManager.GetComponent<WPManager>().waypoints;
		g = wpManager.GetComponent<WPManager>().graph;
		currentNode = wps[7];
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

	public void GoToStatue()
	{
		g.AStar(currentNode, wps[11]);
		currentWP = 0;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		if(g.getPathLength() == 0 || currentWP == g.getPathLength())
			return;

		//the node we are closest to at this moment
		currentNode = g.getPathPoint(currentWP);


        //if we are close enough to the current waypoint move to next
        //work out the distance in the x,y plane
        Vector2 currentPathPoint = new Vector2(g.getPathPoint(currentWP).transform.position.x,
                                            g.getPathPoint(currentWP).transform.position.y);

        Vector2 tankPos = new Vector2(transform.position.x, transform.position.y);
        if (Vector3.Distance(
			currentPathPoint,
			tankPos) < accuracy)
		{
		  	currentWP++;
		}
			
		//if we are not at the end of the path
		if(currentWP < g.getPathLength())
		{
			goal = g.getPathPoint(currentWP).transform;
			Vector3 lookAtGoal = goal.position;
			Vector3 direction = lookAtGoal - this.transform.position;
            //In Unity 2D the default forward movement vector for a sprite 
            //(who's icon is facing upwards like the tank in this example) 
            //is transform.up (the y axis)
            transform.transform.up = Vector3.Slerp(transform.transform.up, direction,rotSpeed * Time.deltaTime);
            this.transform.Translate(0, speed * Time.deltaTime, 0);
        }
		
	
	}
}
