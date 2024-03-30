using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, SUSPEND }

public class BattleSystem : MonoBehaviour
{
    //public static BattleSystem instance;
    public BattleState state;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public PlayerController playerUnit;
    public Enemy enemyUnit;

    public GameObject enemySpawnPoint;
    public GameObject playerSpawnPoint;


    public TextMeshProUGUI actionMenuText;

    public StatusHUD playerHUD;
    public StatusHUD enemyHUD;

    public Canvas UIOverlay;

    public GameObject skillMenuHUDPrefab;


    /*private void Awake()
    {
        instance = this;
    }*/
    // Start is called before the first frame update
    void Start()
    {
        
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerSpawnPoint.transform.position, playerPrefab.transform.rotation);
        playerUnit = playerGO.GetComponent<PlayerController>();
        Animator playerAnimator = playerGO.GetComponent<Animator>();
        playerAnimator.SetFloat("Look X", 1.0f);
        GameObject enemyGO = Instantiate(enemyPrefab, enemySpawnPoint.transform.position, enemyPrefab.transform.rotation);
        enemyUnit = enemyGO.GetComponent<Enemy>();

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        actionMenuText.text = "Battle Start!";
        yield return new WaitForSeconds(3.0f);        
        PlayerTurn();
        state = BattleState.PLAYERTURN;
    }

    void PlayerTurn ()
    {
        actionMenuText.text = "Your turn";
  
    }

    public IEnumerator PlayerAttack()
    {
        if (state == BattleState.PLAYERTURN)
        {
            if (playerUnit.Attack(enemyUnit))
            {
                enemyHUD.SetHp(enemyUnit.currentHp);
                actionMenuText.text = playerUnit.name + " attacks for " + playerUnit.damageDealt + " damage!";
            }
            else
            {
                actionMenuText.text = playerUnit.unitName + " Missed!";
            }
            yield return new WaitForSeconds(3.0f);
            if (enemyUnit.currentHp == 0)
            {
                state = BattleState.WON;
                //end battle player wins
                //EndBattle();
            } else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }

    }

    public void PlayerDidClickSkill(int skillIndex)
    {
        StartCoroutine(PlayerSkill(skillIndex));
    }
    public IEnumerator PlayerSkill(int skillIndex)
    {
        if (state == BattleState.PLAYERTURN)
        {
            if (playerUnit.skills[skillIndex].type == SkillType.HEAL)
            {
                playerUnit.ChangeHealth(playerUnit.skills[skillIndex].baseValue);
                actionMenuText.text = "Player healed for " + playerUnit.skills[skillIndex].baseValue + " hp!";
                playerHUD.SetHp(playerUnit.currentHp);
                playerUnit.currentMp = Mathf.Clamp(playerUnit.currentMp - playerUnit.skills[skillIndex].mpCost, 0, playerUnit.maxMp);
                playerHUD.SetMp(playerUnit.currentMp);
            } else if (playerUnit.skills[skillIndex].type == SkillType.DAMAGE)
            {
                enemyUnit.ChangeHealth(-playerUnit.skills[skillIndex].baseValue);
                enemyHUD.SetHp(enemyUnit.currentHp);
                actionMenuText.text = "Player dealt " + playerUnit.skills[skillIndex].baseValue + " damage to " + enemyUnit.unitName + "!";
                playerUnit.currentMp = Mathf.Clamp(playerUnit.currentMp - playerUnit.skills[skillIndex].mpCost, 0, playerUnit.maxMp);
                playerHUD.SetMp(playerUnit.currentMp);
            } else
            {
                actionMenuText.text = "Miss!";
            }
            Debug.Log("Hit line 119");
            yield return new WaitForSeconds(3.0f);
            if (enemyUnit.currentHp == 0)
            {
                state = BattleState.WON;
                //end battle player wins
                //EndBattle();
            }
            else
            { 
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }

        }
        

    }

  

    IEnumerator EnemyTurn()
    {
        if (state == BattleState.ENEMYTURN)
        {
            if (enemyUnit.Attack(playerUnit))
            {
                //
                playerHUD.SetHp(playerUnit.currentHp);
                actionMenuText.text = enemyUnit.unitName + " attacks for " + enemyUnit.damageDealt + " damage!";
            } else
            {
                actionMenuText.text = enemyUnit.unitName + " Missed!";
            }
            yield return new WaitForSeconds(3.0f);
            if (playerUnit.currentHp == 0)
            {
                state = BattleState.LOST;
                //EndBattle();
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }
        
    }

    public void OnAttackButton()
    {
        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerAttack());
        } else
        {
            return;
        }
        state = BattleState.SUSPEND;
        
    }

    public void OnSkillButton()
    {
        
        if (state == BattleState.PLAYERTURN)
        {
            //need to instantiate as a child of battle overlay, the canvas element.
            GameObject skillsMenuGO = Instantiate(skillMenuHUDPrefab, skillMenuHUDPrefab.transform.position, skillMenuHUDPrefab.transform.rotation);
            skillsMenuGO.transform.SetParent(UIOverlay.transform, false);
            SkillsMenu skillsMenuHUD = skillsMenuGO.GetComponent<SkillsMenu>();
            skillsMenuHUD.DisplayUnitSkills(playerUnit);

        }
        else
        {
            return;
        }
        //state = BattleState.SUSPEND;
    }

}
