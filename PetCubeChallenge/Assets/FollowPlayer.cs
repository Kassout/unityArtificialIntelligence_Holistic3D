using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    public float speed = 2.0f;
    public float rotSpeed = 1.0f;

    private float accuracy = 5.00f;

    // Update is called once per frame
    void Update()
    {
        //Vector3 lookAtGoal = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(player);
        Vector3 direction = player.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);
        if (direction.magnitude > accuracy)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
    }
}
