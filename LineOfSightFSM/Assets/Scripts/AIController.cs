using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject player;

    private Animator anim;
    public GameObject GetPlayer()
    {
        return player;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("distance", Vector3.Distance(transform.position, player.transform.position));
        anim.SetFloat("angle", Vector3.Angle(player.transform.position - transform.position, transform.forward));
    }
}
