using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToGoal : MonoBehaviour
{
    public float speed = 2.0f;
    public float accuracy = 0.01f;
    public Transform goal;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(goal.position);
        Vector3 direction = goal.position - transform.position;
        Debug.DrawRay(transform.position, direction, Color.red); 
        if (direction.magnitude > accuracy)
        {
            // Use space.world to translate the character relatively to the world coordinates of your scene, not relatively to your goal coordinate
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
    }
}
