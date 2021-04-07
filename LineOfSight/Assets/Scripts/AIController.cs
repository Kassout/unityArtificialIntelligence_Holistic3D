using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Transform player;

    private Animator anim;
    
    private float rotationSpeed = 2.0f;
    private float speed = 2.0f;
    private float visDist = 20.0f;
    private float visAngle = 30.0f;
    private float shootDist = 5.0f;

    private string state = "IDLE";

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        if (direction.magnitude < visDist && angle < visAngle)
        {
            direction.y = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                Time.deltaTime * rotationSpeed);

            if (direction.magnitude > shootDist)
            {
                if (state != "RUNNING")
                {
                    state = "RUNNING";
                    anim.SetTrigger("isRunning");
                }
            }
            else
            {
                if (state != "SHOOTING")
                {
                    state = "SHOOTING";
                    anim.SetTrigger("isShooting");
                }
            }
        }
        else if (state != "IDLE")
        {
            state = "IDLE";
            anim.SetTrigger("isIdle");
        }

        if (state == "RUNNING")
        {
            transform.Translate(0, 0, Time.deltaTime * speed);
        }
    }
}
