using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public List<CombatEntity> queue = new List<CombatEntity>();
    CombatEntity[] tempOrder;

    public CombatEntity EntitiesTurn;

    public EntityManager entityManager;
    public UIManager uiManager;
    public ActionManager actionManager;
    public GameManager gameManager;
    public CombatManager combatManger;

    public bool gameIsOver = false;

    StateTurnBase stateTurn;
    public PreturnState PreturnState = new();
	public StartTurnState StartTurnState = new();
	public PlayerturnState PlayerturnState = new();
	public EnemyTurnState EnemyTurnState = new();
    public PostTurnState PostTurnState = new();
    public EndturnState EndTurnState = new();

     public CinemachineVirtualCamera camera;

    CombatSceneSO gameScene;

    int totalTurn;

    public UnityEvent endTurn;

    // Start is called before the first frame update
    void Awake()
    {
        entityManager = GetComponent<EntityManager>();
        uiManager = GetComponent<UIManager>();
        actionManager = GetComponent<ActionManager>();
        combatManger = GetComponent<CombatManager>();
        camera = CinemachineVirtualCamera.FindFirstObjectByType<CinemachineVirtualCamera>();
    }

	private void Start()
	{
		stateTurn = PreturnState;
        gameManager = GetComponent<GameManager>();
	}

	public void ChangeState(StateTurnBase newState)
    {
        stateTurn = newState;
		//uiManager.TurnStateMachine.text = newState.ToString();

		newState.EnterState(this, gameScene);
    }


	// Update is called once per frame
	void FixedUpdate()
    {
        stateTurn.UpdateState(this);
    }

    public CombatEntity GetCombatEntity()
    {
        return EntitiesTurn;
    }
    /*
    void TurnStart(CombatEntity playerTurn)
    {
        EntitiesTurn = playerTurn;

        if(playerTurn.GetEntitySO().isPlayer)
        {
            playerTurn.GetMovesDisplay().SetActive(true);
        }
		/*Debug.Log("Now, it seems it is " + playerTurn + " turn");
        Debug.Log("Heres your health" + EntitiesTurn.GetEntitySO().GetHealth());
		EntitiesTurn.hasMoved = false;
        camera.m_LookAt = EntitiesTurn.transform;
        camera.m_Follow = EntitiesTurn.transform;
        uiManager.WhoseTurn(EntitiesTurn);
        entityManager.CheckEntity((CombatEntity)EntitiesTurn);
    }

    public void TurnEnd()
    {
        if (queue.Count <= 0 && !gameIsOver)
        {
            queue = entityManager.ReqeueuEntities(queue);
        }
        else if (EntitiesTurn == null || !gameIsOver)
        {
            Debug.Log("Oho look whose turn it is " + queue.Peek());
            NextTurn((CombatEntity)queue.Dequeue());
        }

    }

    void NextTurn(CombatEntity playerTurn)
    {
        if (entityManager != null)
        {
            Debug.Log("Lets start the next turn!");
            TurnStart(playerTurn);
        }
    }

	private void Reset()
	{
		
	}*/

	public void QueueEntities(CombatSceneSO combatSceneSO)
	{
        /*gameScene = combatSceneSO;

		tempOrder = new CombatEntity[combatSceneSO.Players.Length + combatSceneSO.Enemies.Length];

        Debug.Log("The game is " + tempOrder.Length + " entities long");


        int i = 0;

        foreach (CombatEntity entity in combatSceneSO.Players)
        {
            if (entity != null)
            {
                Debug.Log("Welcome " + entity.name + "to combat!");
                tempOrder[i] = entity;
                i++;
            }
        }
		foreach (CombatEntity entity in combatSceneSO.GetEnemies())
		{
			if (entity != null)
			{
				Debug.Log("Welcome " + entity.name + "to combat!");
				tempOrder[i] = entity;
				i++;
			}
		}
		tempOrder = tempOrder.OrderByDescending(
        (entity) => 
        entity.GetEntitySO()
        .GetSpeed())
        .ToArray();
        foreach (CombatEntity entity in tempOrder) 
        {
            if(entity != null)
            {
                Debug.Log("Well well, get QUEUED" + entity.name);
                queue.Enqueue(entity);
                uiManager.CreateHealthBars(entity);
            }
        }*/
        
        gameIsOver = false;
        gameScene = combatSceneSO;
        ChangeState(PreturnState);
        //stateTurn.EnterState(this, gameScene);
	}

	internal void EmptyEntities()
	{
        queue.Clear();
	}

	internal void DropEntity(CombatEntity dequeue)
	{
        queue.Remove(dequeue);
        combatManger.RemoveFromCombat(dequeue);
        if(dequeue.isPlayer && combatManger.players.Count < 1 || !dequeue.isPlayer && combatManger.enemies.Count < 1)
        {
            combatManger.EndCombat(!dequeue.isPlayer);
            gameIsOver = true;
        }
	}
}
