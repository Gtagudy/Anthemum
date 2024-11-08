using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    TurnManager turnManager;
    EntityManager entityManager;
    GridManager gridManager;

	public void StartCombat(CombatEntity[] players, CombatEntity[] enemies, int[,] grid)
	{
        Debug.Log("We now starting combat!");

        gridManager.CreateGridMap(grid);
        entityManager.NotifyOfAll(players, enemies);
        turnManager.QueueEntities(players, enemies);
        gridManager.SetEntitiesToGrid(players, enemies);
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
