using UnityEngine;
using UnityEngine.AI;

public class NPCBaseFSM : StateMachineBehaviour
{
    public GameObject NPC;
    public NavMeshAgent agent;
    public GameObject opponent;
    public float speed = 2.0f;
    public float rotSpeed = 1.0f;
    public float accuracy = 3.0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPC = animator.gameObject;
        opponent = NPC.GetComponent<TankAI>().GetPlayer();
        agent = NPC.GetComponent<NavMeshAgent>();
    }
}
