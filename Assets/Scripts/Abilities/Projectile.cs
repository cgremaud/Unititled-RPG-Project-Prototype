using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damageDealt;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemyCollider = collision.GetComponentInParent<Enemy>();
        if (enemyCollider != null )
        {
            enemyCollider.ChangeHealth(-damageDealt);
            StatusHUD enemyHUD = GameObject.Find("EnemyStatus").GetComponent<StatusHUD>();
            enemyHUD.SetHp(enemyCollider.currentHp);
        }
        Destroy(gameObject);
    }
}
