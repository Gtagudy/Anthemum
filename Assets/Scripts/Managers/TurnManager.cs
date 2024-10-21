using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    Queue queue = new Queue();
    EntityManager entityManager;
    CombatEntity[] tempOrder;

    CombatEntity CurrentTurn;

    UIManager uiManager;    

	// Start is called before the first frame update
	void Awake()
    {
        entityManager = GetComponent<EntityManager>();
        uiManager = GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(queue.Count > 0)
        {
            Debug.Log("Oho look whose turn it is " + queue.Peek());
			NextTurn((CombatEntity)queue.Dequeue());
        }
    }

    public CombatEntity GetCombatEntity()
    {
        return CurrentTurn;
    }
    void TurnStart(CombatEntity playerTurn)
    {
        CurrentTurn = playerTurn;
        if(playerTurn.GetEntitySO().isPlayer)
        {
            playerTurn.GetMovesDisplay().SetActive(true);
        }
        Debug.Log("Now, it seems it is " + playerTurn + " turn");
        Debug.Log("Heres your health" + playerTurn.GetEntitySO().GetHealth());
        uiManager.WhoseTurn(playerTurn);
        entityManager.CheckEntity((CombatEntity)playerTurn);
    }

    public void TurnEnd(CombatEntity playerTurn)
    {
        if (playerTurn.GetEntitySO().isPlayer)
        {
            playerTurn.GetMovesDisplay().SetActive(false);
        }
		Debug.Log("Alright, turn is over for" + playerTurn);
        Debug.Log("Here is your new health " + playerTurn + ": " + playerTurn.GetEntitySO().GetHealth());

        queue = entityManager.ReqeueuEntities(queue);
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
		
	}

	public void QueueEntities(CombatEntity[] players, CombatEntity[] enemies)
	{
		tempOrder = new CombatEntity[players.Length + enemies.Length];

        Debug.Log("The game is " + tempOrder.Length + " entities long");


        int i = 0;

        foreach (CombatEntity entity in players)
        {
            if (entity != null)
            {
                Debug.Log("Welcome " + entity.name + "to combat!");
                tempOrder[i] = entity;
                i++;
            }
        }
		foreach (CombatEntity entity in enemies)
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
                //uiManager.CreateHealthBars(entity);
            }
        }
	}
}
