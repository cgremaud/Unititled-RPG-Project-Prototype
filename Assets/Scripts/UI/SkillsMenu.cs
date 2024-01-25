using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsMenu : MonoBehaviour
{
    public Button skillButtonPrefab;
    public Vector2 buttonStartPosition;
    //public Canvas UIOverlayPrefab;

    private void Awake()
    {
        buttonStartPosition = new Vector2(-190, 130);
        //UIOverlayPrefab = GameObject.FindObjectOfType<Canvas>();
    }

    public void DisplayUnitSkills(CombatUnit unit)
    {
        for (int i = 0; i < unit.skills.Count; i++)
        {
            Button button = Instantiate(skillButtonPrefab, buttonStartPosition, skillButtonPrefab.transform.rotation );
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = unit.skills[i].abilityName;
            button.transform.SetParent(gameObject.transform, false);
            buttonStartPosition.y -= 50;
            SkillButton skillButton = button.GetComponentInChildren<SkillButton>();
            skillButton.skillIndex = i;
            //this doesn't work
            //button.onClick.AddListener(unit.UseSkill);
           
        }
    }

    /*public void OnSkillButtonClick(int skillIndex)
    {
        StartCoroutine(BattleSystem.instance.PlayerSkill(skillIndex));
    }*/
}
