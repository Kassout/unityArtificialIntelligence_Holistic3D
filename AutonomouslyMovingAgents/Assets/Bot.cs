using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class Bot : MonoBehaviour
{
    private NavMeshAgent agent;

    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - transform.position;
        agent.SetDestination(transform.position - fleeVector);
    }

    void Pursue()
    {
        Vector3 targetDir = target.transform.position - transform.position;

        float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDir));

        if ((toTarget > 90 && relativeHeading < 20) || target.GetComponent<Drive>().currentSpeed < 0.01f)
        {
            Seek(target.transform.position);
            return;
        }
        
        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<Drive>().currentSpeed);
        Seek(target.transform.position + target.transform.forward * lookAhead * 5);
    }

    void Evade()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        
        float lookAhead = targetDir.magnitude / (agent.speed + target.GetComponent<Drive>().currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead * 5);
    }

    private Vector3 wanderTarget = Vector3.zero;
    
    void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = gameObject.transform.InverseTransformVector(targetLocal);
        
        Seek(targetWorld);
    }

    void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 10; //10 is hide range

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        Seek(chosenSpot);
    }
    
    void CleverHide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = World.Instance.GetHidingSpots()[0];

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 10; //10 is hide range

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = World.Instance.GetHidingSpots()[i];
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 100.0f;
        hideCol.Raycast(backRay, out info, distance);
        
        Seek(info.point + chosenDir.normalized * 5);
    }

    bool CanSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayToTarget, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject.CompareTag("cop"))
            {
                return true;
            } 
        }

        return false;
    }

    bool TargetCanSeeMe()
    {
        Vector3 toAgent = transform.position - target.transform.position;
        float lookingAngle = Vector3.Angle(target.transform.forward, toAgent);

        if (lookingAngle < 60)
        {
            return true;
        }

        return false;
    }

    private bool coolDown = false;

    void BehaviourCoolDown()
    {
        coolDown = false;
    }

    bool TargetInRange()
    {
        if (Vector3.Distance(target.transform.position, transform.position) < 10)
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!coolDown)
        {
            if (!TargetInRange())
            {
                Wander();
            }
            else if (CanSeeTarget() && TargetCanSeeMe())
            {
                CleverHide();
                coolDown = true;
                Invoke("BehaviourCoolDown", 5);
            }
            else
            {
                Pursue();
            }
        }
    }
}
