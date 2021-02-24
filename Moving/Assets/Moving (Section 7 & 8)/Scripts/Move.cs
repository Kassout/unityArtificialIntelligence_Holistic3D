using System;
using UnityEngine;

public class Move : MonoBehaviour {

    public Vector3 goal = new Vector3(5, 0, 4);
    public float speed = 0.1f;

    private void Start()
    {
    }

    // Use a lateUpdate to compute character movement so after you finished computed every physics computation that happens in your scene
    private void LateUpdate() {
        // use normalized vector to get a vector cut in a multitude of magnitude 1 vectors
        // then multiply by speed to adjust the time it needs to the character to reach the goal
        // finally, multiply by Time.deltaTime to ensure you call for the translation at the exact same time for every frame
        transform.Translate(goal.normalized * speed * Time.deltaTime);
    }
}
