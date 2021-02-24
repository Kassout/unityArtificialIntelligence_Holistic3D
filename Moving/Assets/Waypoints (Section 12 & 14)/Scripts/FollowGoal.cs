using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class FollowGoal : MonoBehaviour
{
    public Transform goal;

    private float speed = 2.0f;
    private float accuracy = 1.0f;
    private float rotSpeed = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 lookAtGoal = new Vector3(goal.position.x, transform.position.y, goal.position.z);

        Vector3 direction = lookAtGoal - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
            Time.deltaTime * rotSpeed);
        
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
