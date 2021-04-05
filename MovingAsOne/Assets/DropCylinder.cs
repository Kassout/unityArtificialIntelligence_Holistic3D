using UnityEngine;

public class DropCylinder : MonoBehaviour
{
    public GameObject obstacle;
    private GameObject[] agents;
    
    // Start is called before the first frame update
    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("Agent");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Instantiate(obstacle, hitInfo.point, obstacle.transform.rotation);
                foreach (var a in agents)
                {
                    a.GetComponent<AIControl>().DetectNewObstacle(hitInfo.point);
                }
            }
        }        
    }
}
