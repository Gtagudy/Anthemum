using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    Queue queue = new Queue();
    EntityManager entityManager;
    EntitySO[] tempOrder;

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
			NextTurn((EntitySO)queue.Dequeue());
        }
    }

    void TurnStart(EntitySO playerTurn)
    {
        Debug.Log("Now, it seems it is " + playerTurn + " turn");
        Debug.Log("Heres your health" + playerTurn.GetHealth());
        uiManager.WhoseTurn(playerTurn);
        entityManager.CheckEntity((EntitySO)playerTurn);
    }

    public void TurnEnd(EntitySO playerTurn)
    {
        Debug.Log("Alright, turn is over for" + playerTurn);
        Debug.Log("Here is your new health " + playerTurn + ": " + playerTurn.GetHealth());

        queue = entityManager.ReqeueuEntities(queue);
    }

    void NextTurn(EntitySO playerTurn)
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

	public void QueueEntities(EntitySO[] players, EntitySO[] enemies)
	{
        EntitySO[] tempOrder = new EntitySO[players.Length + enemies.Length];

        Debug.Log("The game is " + tempOrder.Length + " entities long");


        int i = 0;

        foreach (EntitySO entity in players)
        {
            if (entity != null)
            {
                Debug.Log("Welcome " + entity.name + "to combat!");
                tempOrder[i] = entity;
                i++;
            }
        }
		foreach (EntitySO entity in enemies)
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
        entity.GetSpeed())
        .ToArray();
        foreach (EntitySO entity in tempOrder) 
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
