using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//will eventually move enum declaration here instead of BattleSystem
//public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, SUSPEND }
public class OverworldBattleManager : MonoBehaviour
{
    public static OverworldBattleManager Instance { get; private set; }
    public BattleState state;
    
    public PlayerController playerUnit;
    public CombatUnit enemyUnit;

    public StatusHUD playerHUD;
    public StatusHUD enemyHUD;

    public TextMeshProUGUI actionMenuText;
    public Canvas UIOverlayPrefab;
    public Canvas overlay;
    public GameObject skillMenuHUDPrefab;
    public Button attackButton;
    public Button skillMenuButton;

    public Animator playerAnimator;
    public Animator enemyAnimator;

    public CinemachineVirtualCamera battleCamera;

    
    void Awake()
    {
        Instance = this; 
    }

    public IEnumerator StartBattle(PlayerController player, CombatUnit enemy)
    {
        //set the camera follow position to the first child of enemy which is the battle focus point. Refactor to make this live on the enemy party.
        battleCamera.Follow = enemy.gameObject.transform.GetChild(0);
        //Set player position to be opposite the enemy. Refactor to set relative to the battle focus point.
        player.gameObject.transform.position = enemy.transform.position + new Vector3(5,0,0);
        //get the hard bodies.
        Rigidbody2D playerRb = player.gameObject.GetComponent<Rigidbody2D>();
        playerRb.velocity = Vector3.zero;
        //Debug.Log("Game manager called OverworldbattleManager:");

        //set player and enemy  units. Will be a list in the future.
        playerUnit = player;
        enemyUnit = enemy;

        //set up battle UI. TODO abstract this to new SetupBattleUI method
        //todo refactor to use getcomponentinchildren of overlay rather than gameobject.find
        overlay = Instantiate(UIOverlayPrefab, UIOverlayPrefab.transform);
        actionMenuText = GameObject.Find("ActionMenuText").GetComponent<TextMeshProUGUI>();
        playerHUD = GameObject.Find("PlayerStatus").GetComponent<StatusHUD>();
        enemyHUD = GameObject.Find("EnemyStatus").GetComponent<StatusHUD>();
        attackButton = GameObject.Find("AttackButton").GetComponent<Button>();
        attackButton.onClick.AddListener(OnAttackButton);
        skillMenuButton = GameObject.Find("SkillButton").GetComponent<Button>();
        skillMenuButton.onClick.AddListener(OnSkillButton);
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        actionMenuText.text = "Battle start!";

        //display start text and wait til moving to player turn. 
        yield return new WaitForSeconds(2.0f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        //TODO: add handling for player parties
        actionMenuText.text = "Your turn";

    }

    public  void OnAttackButton()
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
            //need to instantiate as a child of overlay, the canvas element.
            GameObject skillsMenuGO = Instantiate(skillMenuHUDPrefab, skillMenuHUDPrefab.transform.position, skillMenuHUDPrefab.transform.rotation);
            skillsMenuGO.transform.SetParent(overlay.transform, false);
            SkillsMenu skillsMenuHUD = skillsMenuGO.GetComponent<SkillsMenu>();
            skillsMenuHUD.DisplayUnitSkills(playerUnit);

        }
        else
        {
            return;
        }
    }

    public IEnumerator PlayerAttack()
    {
        if (state == BattleState.PLAYERTURN)
        {
            if (playerUnit.Attack(enemyUnit))
            {
                enemyHUD.SetHp(enemyUnit.currentHp);
                actionMenuText.text = playerUnit.unitName + " attacks for " + playerUnit.damageDealt + " damage!";
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
                StartCoroutine(EndBattle());
            }

            else
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
        Skill selectedSkill = playerUnit.skills[skillIndex];
        //todo move this to player and call here as playerUnit.UseSkill(skillIndex)
        if (state == BattleState.PLAYERTURN)
        {
            if (playerUnit.currentMp < selectedSkill.mpCost)
            {
                actionMenuText.text = "not enough mp!";
                yield return new WaitForSeconds(3.0f);
                
            }
            if (selectedSkill.type == SkillType.HEAL)
            {
                //TODO: refactor to instantiate skill prefab
                playerUnit.ChangeHealth(playerUnit.skills[skillIndex].baseValue);
                actionMenuText.text = "Player healed for " + playerUnit.skills[skillIndex].baseValue + " hp!";
                playerHUD.SetHp(playerUnit.currentHp);
                playerUnit.currentMp = Mathf.Clamp(playerUnit.currentMp - playerUnit.skills[skillIndex].mpCost, 0, playerUnit.maxMp);
                playerHUD.SetMp(playerUnit.currentMp);
            }
            else if (selectedSkill.type == SkillType.DAMAGE)
            {
                //todo: base value * player's magic dmg modifier
                enemyUnit.ChangeHealth(-selectedSkill.baseValue);
                enemyHUD.SetHp(enemyUnit.currentHp);
                actionMenuText.text = "Player dealt " + playerUnit.skills[skillIndex].baseValue + " damage to " + enemyUnit.unitName + "!";
                playerUnit.currentMp = Mathf.Clamp(playerUnit.currentMp - playerUnit.skills[skillIndex].mpCost, 0, playerUnit.maxMp);
                playerHUD.SetMp(playerUnit.currentMp);
            }
            else if (selectedSkill.type == SkillType.PROJECTILE)
            {
                ProjectileSkill skillObj = (ProjectileSkill)playerUnit.skills[skillIndex];
                skillObj.LaunchProjectiles(enemyUnit);
                playerUnit.currentMp = Mathf.Clamp(playerUnit.currentMp - playerUnit.skills[skillIndex].mpCost, 0, playerUnit.maxMp);
                playerHUD.SetMp(playerUnit.currentMp);
                Debug.Log(playerUnit.currentMp);
            }
            
            else
            {
                actionMenuText.text = "Miss!";
            }
            yield return new WaitForSeconds(3.0f);
            enemyHUD.SetHp(enemyUnit.currentHp);
            if (enemyUnit.currentHp == 0)
            {
                state = BattleState.WON;
                //end battle player wins
                StartCoroutine(EndBattle());
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
            }
            else
            {
                actionMenuText.text = enemyUnit.unitName + " Missed!";
            }
            yield return new WaitForSeconds(3.0f);
            if (playerUnit.currentHp == 0)
            {
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
        }

    }

    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            
            actionMenuText.text = enemyUnit.unitName + " was defeated!";
            yield return new WaitForSeconds(3.0f);
            Destroy(enemyUnit.gameObject);
            Destroy(overlay.gameObject);
            
            GameManager.Instance.gameState = GameState.NEUTRAL;
            battleCamera.Follow = playerUnit.gameObject.transform;
        }
        else
        {
            actionMenuText.text = "You were defeated!";
            yield return new WaitForSeconds(3.0f);
            /*playerUnit = null;
            enemyUnit = null;*/
        }
    }
}
