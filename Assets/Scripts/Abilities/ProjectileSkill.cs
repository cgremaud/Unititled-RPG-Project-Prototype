using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : Skill
{
    public GameObject projectilePrefab; //this can carry the "deal damage on collision" code right? 
    public int projectileCount;
    public float thrust = 100f;
    public float yRange = 1.0f;
    public float projectileDelay = 0.3f;
    // Start is called before the first frame update

    private void Awake()
    {
        //Launch should be called in OverWorldBattleManager after skill is instantiated
        //LaunchProjectiles();
    }

    public virtual void LaunchProjectiles(CombatUnit target)
    {

        PlayerController player = GameObject.Find("PlayerCharacter").GetComponent<PlayerController>();
        Vector2 heading = target.transform.position - player.transform.position;
        GameObject projectile = Instantiate(projectilePrefab, player.transform.position, Quaternion.identity);
        Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
        projectileRB.AddForce(heading * thrust);
        //Debug.Log("In the coroutine after the yield");

        //the below did not work
        /* for (int i = 0; i < projectileCount; i++)
         {
             Debug.Log("hitting the place where I start the coroutine");
             StartCoroutine(SpawnProjectile(target));
         }*/
        /*Destroy(gameObject);*/

    }

    /* public IEnumerator SpawnProjectile(CombatUnit target)
     {
         Debug.Log("In the coroutine before the yield");
         yield return new WaitForSeconds(projectileDelay);
         PlayerController player = GameObject.Find("PlayerCharacter").GetComponent<PlayerController>();
         Vector2 heading = target.transform.position - player.transform.position;
         GameObject projectile = Instantiate(projectilePrefab, player.transform.position, Quaternion.identity);
         Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
         projectileRB.AddForce(heading * thrust);
         Debug.Log("In the coroutine after the yield");


     }*/

    //Question: can/should I do a version of the LaunchProjectiles w/o a target argument?


}
