using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;

    private float speed;
    private bool turning = false;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
        anim = GetComponent<Animator>();
        anim.SetFloat("swimSpeed", speed);
        anim.SetFloat("swimOffset", Random.Range(0.0f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        // Determine the bounding box of the manager cube
        Bounds b = new Bounds(myManager.transform.position, myManager.swimLimits * 2);
        
        // If fish is outside the bounds the cube then start turning around
        RaycastHit hit = new RaycastHit();
        Vector3 direction = Vector3.zero;
        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        } else if (Physics.Raycast(transform.position, transform.forward * 50, out hit)) 
        {
            turning = true;
            direction = Vector3.Reflect(transform.forward, hit.normal);
            Debug.DrawRay(transform.position, transform.forward * 50, Color.red);
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            // Turn towards the centre of the manager cube
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
            {
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            }

            if (Random.Range(0, 100) < 20)
            {
                ApplyRules();
            }
            
            anim.SetFloat("swimSpeed", speed);
        }
        
        transform.Translate(0,0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.allFish;
        
        Vector3 vCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach (var go in gos)
        {
            if (go != gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, transform.position);
                if (nDistance <= myManager.neighbourDistance)
                {
                    vCentre += go.transform.position;
                    groupSize++;

                    if (nDistance < 1.0f)
                    {
                        vAvoid += (transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed += anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vCentre = vCentre / groupSize + (myManager.goalPos - transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vCentre + vAvoid) - transform.position;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
