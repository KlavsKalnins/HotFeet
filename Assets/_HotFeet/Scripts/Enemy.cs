using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;

    [SerializeField] private float range;
    private bool isGameActive = true;
    public static int enemyCount;
    public static Action<int> OnEnemyChange;

    private void OnEnable()
    {
        enemyCount++;
        OnEnemyChange?.Invoke(enemyCount);
        GameManager.OnGameState += StopChasing;
        
    }

    private void OnDisable()
    {
        enemyCount--;
        OnEnemyChange?.Invoke(enemyCount);
        GameManager.OnGameState -= StopChasing;
        
    }

    void Update()
    {
        if (Player.instance == null)
        {
            target = GameManager.instance.player.transform;
            return;
        }
        if (target == null)
            target = GameManager.instance.player.transform;
        if (Vector3.Distance(transform.position, Player.instance.transform.position) < range)
                agent.SetDestination(target.position);
    }

    private void StopChasing(bool status)
    {
        if (!status)
        {
            isGameActive = false;
            agent.SetDestination(transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,range);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GeneratedMesh"))
        {
            gameObject.SetActive(false);
        }
    }
}
