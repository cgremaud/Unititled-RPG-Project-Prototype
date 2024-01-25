using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CombatUnit
{
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
