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

	public void StartCombat(CombatSceneSO combatSceneSO)
	{
        if(gameManager.gameState != GameState.Combat)
        {
            gameManager.gameState = GameState.Combat;
        }
        Debug.Log("We now starting combat!");

        combatSceneSO.Players[0] = gameManager.MainCharacter;
        

        combatSceneSO.Enemies[0] = gameManager.MainEnemy;


        gridManager.CreateGridMap(combatSceneSO.GetGrid());
        entityManager.NotifyOfAll(combatSceneSO.GetPlayers(), combatSceneSO.GetEnemies());
        turnManager.QueueEntities(combatSceneSO);
        gridManager.SetEntitiesToGrid(combatSceneSO.GetPlayers(), combatSceneSO.GetEnemies());
	}

	// Start is called before the first frame update
	void Awake()
    {
        turnManager = GetComponent<TurnManager>();
        //actionManager = GetComponent<ActionManager>();
        entityManager = GetComponent<EntityManager>();
        gridManager = GetComponent<GridManager>();
        gameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
