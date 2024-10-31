using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, Health.IHealthListener
{
    enum State
    {
        Idle,
        Follow,
        Attack,
        Die
    }
    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    AudioSource audioSource;

    State state;

    float currentStateTime;
    public float timeForNextState = 2;
    void Start()
    {
        animator = GetComponent<Animator>();   
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        state = State.Idle;
        currentStateTime = timeForNextState;
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                currentStateTime -= Time.deltaTime;
                if(currentStateTime < 0)
                {
                    float distance = (player.transform.position - transform.position).magnitude;
                    //거리가 1.5보다 작으면 공격 아니면 따라오기
                    if (distance < 1.5f)
                    {
                        StartAttack();
                    }
                    else
                    {
                        StartFollow();
                    }
                }
                break;            
            case State.Follow:
                //남은 거리가 1보다 작거나 갈 수 있는 길이 없을 때
                if( agent.remainingDistance<1.0f || !agent.hasPath)
                {
                    StartIdle();
                }
                break;            
            case State.Attack:
                currentStateTime -= Time.deltaTime;
                if (currentStateTime < 0)
                {
                    StartIdle();
                }
                break;
        }
    }

    void StartIdle()
    {
        audioSource.Stop();
        state = State.Idle;
        currentStateTime = timeForNextState;
        agent.isStopped = true;
        animator.SetTrigger("Idle");
    }
    void StartFollow()
    {
        audioSource.Play();
        state = State.Follow;
        agent.destination = player.transform.position;
        agent.isStopped = false;
        animator.SetTrigger("Run");
    }
    void StartAttack()
    {
        state = State.Attack;
        currentStateTime = timeForNextState;
        animator.SetTrigger("Attack");
    }

    public void OnDie()
    {
        state = State.Die;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Invoke("DestroyThis", 2);
    }

    void DestroyThis()
    {
        GameManager.Instance.EnemyDie();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Health>().Damage(1);
        }
    }
}
