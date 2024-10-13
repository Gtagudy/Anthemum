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
	// Start is called before the first frame update
	void Awake()
    {
        entityManager = GetComponent<EntityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(queue.Count > 0)
        {
            Debug.Log("Oho look whose turn it is " + queue.Peek());
			NextTurn(queue.Dequeue());
        }
    }

    void TurnStart(object playerTurn)
    {
        Debug.Log("Now, it seems it is " + playerTurn + " turn");
        entityManager.CheckEntity((EntitySO)playerTurn);
        TurnEnd(playerTurn);
    }

    void TurnEnd(object playerTurn)
    {

    }

    void NextTurn(object playerTurn)
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
            }
        }
	}
}
