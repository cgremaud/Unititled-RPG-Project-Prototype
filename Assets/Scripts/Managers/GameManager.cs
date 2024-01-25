using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { NEUTRAL, BATTTLE, DIALOG }
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public OverworldBattleManager battleManager;
    //should probably abstract out into PlayerParty class
    public List<CombatUnit> playerParty;
    public GameState gameState;
    private void Awake()
    {
        Instance = this;
        gameState = GameState.NEUTRAL;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBattle(PlayerController player, Enemy enemy)
    {
        gameState = GameState.BATTTLE;
        StartCoroutine(OverworldBattleManager.Instance.StartBattle(player, enemy));
    }
}
