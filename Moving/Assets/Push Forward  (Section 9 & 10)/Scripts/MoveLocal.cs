using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLocalWithAnimation : MonoBehaviour
{
    public Transform goal;

    public float speed = 0.5f;
    private float accuracy = 1.0f;
    public float rotSpeed = 0.4f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // As LookAt method don't ignore the difference of height between your gameObject and the gameObject you want to look at
        // it will rotation your gameObject over the Z and X axis
        // To avoid this, just set the position you want to look at, at the same height of the one of your character
        Vector3 lookAtGoal = new Vector3(goal.position.x, transform.position.y, goal.position.z);
        
        // Use Quaternion.Slerp() method to time stepping the rotation of the game object to reach to goal right angles for the computed direction
        Vector3 direction = lookAtGoal - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);
        
        transform.LookAt(lookAtGoal);
        // Use lookAtGoal to set the goal of your zombie to reach a position ignoring the height of the gameObject you want to reach to
        // if (Vector3.Distance(transform.position, lookAtGoal) > accuracy)
        // {
        //     transform.Translate(0, 0, speed * Time.deltaTime);
        // }
    }
}
