using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    TurnManager turnManager;
    EntityManager entityManager;
    GridManager gridManager;

	public void StartCombat(CombatSceneSO combatSceneSO)
	{
        Debug.Log("We now starting combat!");

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
