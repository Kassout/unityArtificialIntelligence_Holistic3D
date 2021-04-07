using UnityEngine;

public class TankAI : MonoBehaviour
{
    private Animator anim;
    public GameObject player;
    public GameObject bullet;
    public GameObject turret;

    public GameObject GetPlayer()
    {
        return player;
    }

    private void Fire()
    {
        GameObject b = Instantiate(bullet, turret.transform.position, turret.transform.rotation);
        b.GetComponent<Rigidbody>().AddForce(turret.transform.forward * 500);
    }

    public void StartFiring()
    {
        InvokeRepeating("Fire", 0.5f, 0.5f);
    }

    public void StopFiring()
    {
        CancelInvoke("Fire");
    }

    // Use this for initialization
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        anim.SetFloat("distance", Vector3.Distance(transform.position, player.transform.position));
    }
}
