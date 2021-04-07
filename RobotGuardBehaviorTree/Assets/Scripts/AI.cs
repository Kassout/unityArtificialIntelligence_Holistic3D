using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Panda;

public class AI : MonoBehaviour
{
    public Transform player;
    public Transform bulletSpawn;
    public Slider healthBar;   
    public GameObject bulletPrefab;

    NavMeshAgent agent;
    public Vector3 destination; // The movement destination.
    public Vector3 target;      // The position to aim to.
    float health = 100.0f;
    float rotSpeed = 5.0f;

    float visibleRange = 80.0f;
    float shotRange = 40.0f;
    
    Vector3 lastAttackingPos;
    bool angry = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = shotRange - 5; //for a little buffer
        InvokeRepeating("UpdateHealth",5,0.5f);
    }

    void Update()
    {
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(transform.position);
        healthBar.value = (int)health;
        healthBar.transform.position = healthBarPos + new Vector3(0,60,0);
    }

    void UpdateHealth()
    {
       if(health < 100)
        health ++;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "bullet")
        {
            health -= 10;
        }
    }

    [Task]
    public void PickRandomDestination()
    {
        Vector3 dest = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
        agent.SetDestination(dest);
        Task.current.Fail();
    }
    
    [Task]
    public void PickDestination(float x, float z)
    {
        Vector3 dest = new Vector3(x, 0, z);
        agent.SetDestination(dest);
        Task.current.Succeed();
    }

    [Task]
    public void MoveToDestination()
    {
        if (Task.isInspected)
        {
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);
        }

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public void TargetPlayer()
    {
        target = player.transform.position;
        Task.current.Succeed();
    }

    [Task]
    bool Turn(float angle)
    {
        var p = transform.position + Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
        target = p;
        return true;
    }

    [Task]
    public void LookAtTarget()
    {
        Vector3 direction = target - transform.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);
        
        if (Task.isInspected)
        {
            Task.current.debugInfo = string.Format("angle={0}", Vector3.Angle(transform.forward, direction));
        }

        if (Vector3.Angle(transform.forward, direction) < 5.0f)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public bool Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 2000);
        return true;
    }

    [Task]
    bool SeePlayer()
    {
        Vector3 distance = player.transform.position - transform.position;

        RaycastHit hit;
        bool seeWall = false;
        
        Debug.DrawRay(transform.position, distance, Color.red);

        if (Physics.Raycast(transform.position, distance, out hit))
        {
            if (hit.collider.gameObject.CompareTag("wall"))
            {
                seeWall = true;
            }
        }

        if (Task.isInspected)
        {
            Task.current.debugInfo = string.Format("wall={0}", seeWall);
        }

        if (distance.magnitude < visibleRange && !seeWall)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [Task]
    public bool IsHealthLessThan( float health )
    {
        if( Task.isInspected )
            Task.current.debugInfo = string.Format("health={0}", this.health);

        return this.health < health;
    }
    [Task]
    public bool InDanger(float minDist)
    {
        Vector3 distance = player.transform.position - transform.position;
        return (distance.magnitude < minDist);
    }
    
    [Task]
    public void TargetAttackPos()
    {
        target = lastAttackingPos;
        Task.current.Succeed();
    }

    [Task]
    public void TakeCover()
    {
        Vector3 awayFromPlayer = this.transform.position - player.transform.position;
        
        //increased the flee range to the agent
        //has further to come back
        Vector3 dest = this.transform.position + awayFromPlayer * 5;
        agent.SetDestination(dest);
        
        //remember where we were before fleeing
        //and get angry
        lastAttackingPos = this.transform.position;
        angry = true;
        //don't be angry after 30 seconds
        //make sure this is longer than it takes for
        //health to be restored
        Invoke("CoolDown",30);
        
        Task.current.Succeed();
    }

    [Task]
    public bool Explode()
    {
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
        return true;
    }

    [Task]
    public void SetTargetDestination()
    {
        agent.SetDestination(target);
        Task.current.Succeed();
    }

    [Task]
    bool ShotLinedUp()
    {
        Vector3 distance = target - transform.position;
        if (distance.magnitude < shotRange && Vector3.Angle(transform.forward, distance) < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    [Task]
    public bool IsAngry()
    {
        if( Task.isInspected )
            Task.current.debugInfo = string.Format("angry={0}", angry);

        return angry;
    }
    
    void CoolDown()
    {
        angry = false;
    }
}

