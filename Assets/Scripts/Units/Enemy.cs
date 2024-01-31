using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : CombatUnit
{
    public Transform goal;

    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision triggered");
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            GameManager.Instance.StartBattle(player, gameObject.GetComponent<Enemy>());
            Debug.Log("called game manager");
        }
    }
}
