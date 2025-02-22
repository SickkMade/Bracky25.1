using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float WanderRadius = 30f;
    [SerializeField] float ChaseMusicRadius = 5f;
    private NavMeshAgent agent;
    private EnemyState eState = EnemyState.Wander;
    Vector3 targetPos;
    bool Wandering => Mathf.Abs(transform.position.x - targetPos.x) + Mathf.Abs(transform.position.z - targetPos.z) > 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = transform.position;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Wandering && eState == EnemyState.Wander)
        {
            if (PlayerManager.Instance.playerData.IsInTunnels)
            {
                eState = EnemyState.Chase;
                agent.SetDestination(PlayerManager.Instance.playerData.position);
            }
            else
            {
                eState = EnemyState.Wander;
                targetPos = GetRandomPoint();
                agent.SetDestination(targetPos);
            }
        }
        if (eState == EnemyState.Chase)
        {
            agent.SetDestination(PlayerManager.Instance.playerData.position);
            targetPos = PlayerManager.Instance.playerData.position;
            if (agent.remainingDistance < ChaseMusicRadius)
            {
                Debug.Log("Chase?");
                AudioManager.Instance.ChangeToChase();
            }
            if (!PlayerManager.Instance.playerData.IsInTunnels)
            {
                targetPos = GetRandomPoint();
                agent.SetDestination(targetPos);
                eState = EnemyState.Wander;
            }
            
        }
    }

    public Vector3 GetRandomPoint()
    {
        Vector3 randomDir = UnityEngine.Random.insideUnitSphere * WanderRadius;
        randomDir.y = 0;
        Vector3 randomPoint = transform.position + randomDir;
        NavMeshHit hit;
        Vector3 finalPosition = transform.position;
        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, 1))
        {
            finalPosition = hit.position;
        }

        Debug.Log("Enemy moving to " + finalPosition);
        return finalPosition;
    }
}

public enum EnemyState
{
    Wander,
    Chase
}

