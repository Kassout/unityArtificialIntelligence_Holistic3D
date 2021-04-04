using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPath : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject[] waypoints;
    int currentWP = 0;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(waypoints[currentWP].transform.position);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 1)
        {
            currentWP++;
            if (currentWP >= waypoints.Length) currentWP = 0;
            agent.SetDestination(waypoints[currentWP].transform.position);
        }
        
        anim.SetFloat("velocity", agent.velocity.magnitude);
    }
}
