using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent; 
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.transform.position;
    }

    void Update()
    {
        if (agent.remainingDistance < 1f)
        {
            agent.destination = player.transform.position;
        }
    }
}
