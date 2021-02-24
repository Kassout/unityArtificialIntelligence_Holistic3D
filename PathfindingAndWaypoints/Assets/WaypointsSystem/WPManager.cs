using UnityEngine;

[System.Serializable]
public struct Link
{
    public enum direction {UNI, BI}
    public GameObject node1;
    public GameObject node2;
    public direction dir;
}
public class WPManager : MonoBehaviour
{
    public GameObject[] waypoints;
    public Link[] links;
    public Graph graph = new Graph();

    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Length > 0)
        {
            foreach (var wp in waypoints)
            {
                graph.AddNode(wp);
            }

            foreach (var l in links) 
            {
                graph.AddEdge(l.node1, l.node2);
                if (l.dir == Link.direction.BI)
                {
                    graph.AddEdge(l.node2, l.node1);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        graph.debugDraw();
    }
}
