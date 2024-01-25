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

    
    void Awake()
    {
        Instance = this; 
    }

    public IEnumerator StartBattle(PlayerController player, CombatUnit enemy)
    {
        player.gameObject.transform.position = enemy.transform.position + new Vector3(5,0,0);
        Rigidbody2D playerRb = player.gameObject.GetComponent<Rigidbody2D>();
        playerRb.velocity = Vector3.zero;
        Debug.Log("Game manager called OverworldbattleManager:");
        overlay = Instantiate(UIOverlayPrefab, UIOverlayPrefab.transform);
        actionMenuText = GameObject.Find("ActionMenuText").GetComponent<TextMeshProUGUI>();
        playerUnit = player;
        enemyUnit = enemy;
        playerHUD = GameObject.Find("PlayerStatus").GetComponent<StatusHUD>();
        enemyHUD = GameObject.Find("EnemyStatus").GetComponent<StatusHUD>();
        attackButton = GameObject.Find("AttackButton").GetComponent<Button>();
        attackButton.onClick.AddListener(OnAttackButton);
        skillMenuButton = GameObject.Find("SkillButton").GetComponent<Button>();
        skillMenuButton.onClick.AddListener(OnSkillButton);
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        actionMenuText.text = "Battle start!";
        yield return new WaitForSeconds(2.0f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
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
                //EndBattle();
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
        if (state == BattleState.PLAYERTURN)
        {
            if (playerUnit.skills[skillIndex].type == SkillType.HEAL)
            {
                playerUnit.ChangeHealth(playerUnit.skills[skillIndex].baseValue);
                actionMenuText.text = "Player healed for " + playerUnit.skills[skillIndex].baseValue + " hp!";
                playerHUD.SetHp(playerUnit.currentHp);
                playerUnit.currentMp = Mathf.Clamp(playerUnit.currentMp - playerUnit.skills[skillIndex].mpCost, 0, playerUnit.maxMp);
                playerHUD.SetMp(playerUnit.currentMp);
            }
            else if (playerUnit.skills[skillIndex].type == SkillType.DAMAGE)
            {
                enemyUnit.ChangeHealth(-playerUnit.skills[skillIndex].baseValue);
                enemyHUD.SetHp(enemyUnit.currentHp);
                actionMenuText.text = "Player dealt " + playerUnit.skills[skillIndex].baseValue + " damage to " + enemyUnit.unitName + "!";
                playerUnit.currentMp = Mathf.Clamp(playerUnit.currentMp - playerUnit.skills[skillIndex].mpCost, 0, playerUnit.maxMp);
                playerHUD.SetMp(playerUnit.currentMp);
            }
            else
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
            }
            else
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
}
