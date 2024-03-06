using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : Skill
{
    public GameObject projectilePrefab; 
    public int projectileCount;
    public int projectilesFired = 0;
    public float thrust = 100f;
    public float yRange = 1.0f;
    public float projectileDelay = 0.3f;
    CombatUnit m_target;
    PlayerController m_playerController;
    Vector2 heading;


    private void Update()
    {
        /*Debug.Log("update was called on projectile skill");*/
        if (projectilesFired >= projectileCount)
        {
            CancelInvoke();
            Destroy(gameObject);
        }
    }

    public virtual void LaunchProjectiles(CombatUnit target, PlayerController player)
    {
        
        m_target = target;
        m_playerController = player;
        heading = m_target.transform.position - m_playerController.transform.position;
        //testing launching in random direction
        //heading = Random.insideUnitCircle.normalized;
        InvokeRepeating("SpawnProjectile", 0.1f, projectileDelay);

    }

    public void SpawnProjectile()
    {
        //heading = Random.insideUnitCircle.normalized;
        GameObject projectile = Instantiate(projectilePrefab, m_playerController.transform.position, Quaternion.identity);
        Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
        projectileRB.AddForce(heading * thrust);
        projectilesFired++;
    }


    //Question: can/should I do a version of the LaunchProjectiles w/o a target argument?


}
