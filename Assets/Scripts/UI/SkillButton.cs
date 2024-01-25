using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    public int skillIndex;
    public OverworldBattleManager battleManager;

    private void Awake()
    {
        battleManager = GameObject.Find("OverworldBattleManager").GetComponent<OverworldBattleManager>();
    }

    public IEnumerator OnClickCoroutine()
    {
        StartCoroutine(battleManager.PlayerSkill(skillIndex));
        yield return new WaitForSeconds(0.25f);
        Destroy(transform.parent.gameObject);
    }

    public void OnClick()
    {
        //StartCoroutine(OnClickCoroutine());
        battleManager.PlayerDidClickSkill(skillIndex);
        Destroy(transform.parent.gameObject);
    }
}
