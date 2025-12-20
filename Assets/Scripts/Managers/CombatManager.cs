using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    TurnManager turnManager;
    EntityManager entityManager;
    public GridManager gridManager;
    GameManager gameManager;
    UIManager uiManager;

    public List<CombatEntity> players;
    public List<CombatEntity> enemies;

    [SerializeField] public GameObject combatRoot;
    
	public void StartCombat(CombatSceneSO combatSceneSO)
	{
        if(gameManager.gameState != GameState.Combat)
        {
            gameManager.gameState = GameState.Combat;
        }
        Debug.Log("We now starting combat!");
        
        players = new List<CombatEntity>();
        enemies = new List<CombatEntity>();

        foreach (GameObject playerPrefab in combatSceneSO.GetPlayers())
        {
            // 1) Instantiate the prefab itself (creates the visible GameObject)
            GameObject go = Instantiate(playerPrefab, combatRoot.transform);

            // 2) Grab the CombatEntity component from *that* instance
            CombatEntity inst = go.GetComponent<CombatEntity>();

            // 3) (Re)initialize its data if needed
            inst.InitializeFromData();

            // 4) Add the *instance* to your list
            players.Add(inst);
        }

        // Spawn enemies (same as above)
        foreach (var enemyPrefab in combatSceneSO.GetEnemies())
        {
            GameObject go = Instantiate(enemyPrefab, combatRoot.transform);
            CombatEntity inst = go.GetComponent<CombatEntity>();
            inst.InitializeFromData();
            enemies.Add(inst);
        }

        gridManager.CreateGridMap(combatSceneSO.GetGrid());
        gridManager.SpawnSceneObjects(combatSceneSO.objectsToSpawn);
        entityManager.NotifyOfAll(players, enemies);
        turnManager.ShareCombatScene(combatSceneSO);
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
