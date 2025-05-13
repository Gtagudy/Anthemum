using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    TurnManager turnManager;
    EntityManager entityManager;
    GridManager gridManager;
    GameManager gameManager;
    UIManager uiManager;

    public List<CombatEntity> players;
    public List<CombatEntity> enemies;

	public void StartCombat(CombatSceneSO combatSceneSO)
	{
        if(gameManager.gameState != GameState.Combat)
        {
            gameManager.gameState = GameState.Combat;
        }
        Debug.Log("We now starting combat!");
        
        players = new List<CombatEntity>();
        enemies = new List<CombatEntity>();

        for(int i = 0; i < combatSceneSO.GetPlayers().Length; i++)
        {
				//players.Add(combatSceneSO.GetPlayers()[i].GetComponent<CombatEntity>());
		        CombatEntity player = combatSceneSO.GetPlayers()[i].GetComponent<CombatEntity>();
                Instantiate(player);
                players.Add(player);
            
        }
        for(int i = 0; i < combatSceneSO.GetEnemies().Length; i++)
        {
            CombatEntity enemy = combatSceneSO.GetEnemies()[i].GetComponent<CombatEntity>();
            Instantiate(enemy);
            players.Add(enemy);
        }

        gridManager.CreateGridMap(combatSceneSO.GetGrid());
        entityManager.NotifyOfAll(players, enemies);
        turnManager.QueueEntities(combatSceneSO);
        gridManager.SetEntitiesToGrid(players, enemies);
	}

    public void EndCombat(bool hasWon)
    {
        uiManager.ClearHistory();
        uiManager.ClearQueue();
        uiManager.ClearMoveList();
        uiManager.ClearTargeting();
        turnManager.EmptyEntities();
        gridManager.DestroyGrid();
        uiManager.HideCombatUI();

        gameManager.gameState = GameState.World;
    }

	// Start is called before the first frame update
	void Awake()
    {
        turnManager = GetComponent<TurnManager>();
        //actionManager = GetComponent<ActionManager>();
        entityManager = GetComponent<EntityManager>();
        gridManager = GetComponent<GridManager>();
        gameManager = GetComponent<GameManager>();
        uiManager = GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveFromCombat(CombatEntity dead)
    {
        if(dead.isPlayer)
        {
            players.Remove(dead);
            entityManager.Players.Remove(dead);
        } else
        {
            enemies.Remove(dead);
            entityManager.Enemies.Remove(dead);
        }
    }
}
