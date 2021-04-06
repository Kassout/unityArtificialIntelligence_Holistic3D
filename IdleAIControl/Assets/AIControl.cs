using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour {

	private GameObject[] goalLocations;
	private NavMeshAgent agent;
	private Animator anim;
	private float speedMult;
	private float detectionRadius = 20;
	private float fleeRadius = 10;
	
	void ResetAgent()
	{
		speedMult = Random.Range(0.1f, 1.5f);
		agent.speed = 2 * speedMult;
		agent.angularSpeed = 120;
		anim.SetFloat("speedMult", speedMult);
		anim.SetTrigger("isWalking");
		agent.ResetPath();
	}

	public void DetectNewObstacle(Vector3 position)
	{
		if (Vector3.Distance(position, transform.position) < detectionRadius)
		{
			Vector3 fleeDirection = (transform.position - position).normalized;
			Vector3 newGoal = transform.position + fleeDirection * fleeRadius;

			NavMeshPath path = new NavMeshPath();
			agent.CalculatePath(newGoal, path);

			if (path.status != NavMeshPathStatus.PathInvalid)
			{
				agent.SetDestination(path.corners[path.corners.Length - 1]);
				anim.SetTrigger("isRunning");
				agent.speed = 10;
				agent.angularSpeed = 500;
			}
		}
	}

	// Use this for initialization
	void Start () {
		goalLocations = GameObject.FindGameObjectsWithTag("Goal");
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(goalLocations[Random.Range(0,goalLocations.Length)].transform.position);
		
		anim = GetComponent<Animator>();
		anim.SetFloat("wOffset", Random.Range(0,1));
		ResetAgent();
	}
	
	bool hasArrivedAtDestination = false;
	void PickLocation()
	{
		ResetAgent();
		agent.SetDestination(goalLocations[Random.Range(0,goalLocations.Length)].transform.position);
		hasArrivedAtDestination = false;
	}
	void Update () {
		if(agent.remainingDistance < 1 && hasArrivedAtDestination != true)
		{
			hasArrivedAtDestination = true;
			anim.SetTrigger("isIdle");
			agent.ResetPath();
			//pick new location after somewhere between 5 and 10 seconds.
			Invoke("PickLocation",Random.Range(5f,10f));
		} 
	}
}
