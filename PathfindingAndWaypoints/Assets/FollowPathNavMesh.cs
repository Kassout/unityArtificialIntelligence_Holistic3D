using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathNavMesh : MonoBehaviour
{
    public GameObject wpManager;

    private GameObject[] wps;

    private UnityEngine.AI.NavMeshAgent agent;
    
    // Start is called before the first frame update
    void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void GoToHeli()
    {
        agent.SetDestination(wps[4].transform.position);
        //g.AStar(currentNode, wps[4]);
        //currentWP = 0;
    }

    public void GoToRuin()
    {
        agent.SetDestination(wps[0].transform.position);
        //g.AStar(currentNode, wps[0]);
        //currentWP = 0;
    }

    public void GoToTanks()
    {
        agent.SetDestination(wps[8].transform.position);
        //g.AStar(currentNode, wps[8]);
        //currentWP = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }
}
