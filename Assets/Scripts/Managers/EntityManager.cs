using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EntityManager : MonoBehaviour
{
    ActionManager actionManager;
    UIManager uiManager;
    EntitySO[] Players;
    EntitySO[] Enemies;

	public void CheckEntity(EntitySO dequeue)
	{
		if(dequeue != null)
        {
            if (dequeue.isPlayer == true)
            {

                Debug.Log("Welcome player!");
                actionManager.ResolvePlayer(dequeue);

            }
            else
            {
                Debug.Log("Ohh your the enemy!!");
                actionManager.ResolveEnemy(dequeue);
            }
        }
	}

	internal void GetTargets(AbilityButton button)
	{
        uiManager.LetPlayerTarget(button, Enemies, Players);
	}

	internal void HandleAbility(AbilitySO chosenAbility, EntitySO entity)
	{
        entity.ChangeHealth(chosenAbility.damage);
		uiManager.UpdateHealth(entity);
	}

	internal void NotifyOfAll(EntitySO[] players, EntitySO[] enemies)
	{
        Players = players;
        Enemies = enemies;
		/*for (int i = 0; i < players.Length; i++) 
        {
            this.Players[i] = players[i];
        }
		for (int i = 0; i < players.Length; i++)
		{
			this.Enemies[i] = enemies[i];
		}*/
	}

	internal Queue ReqeueuEntities(Queue queue)
	{
		EntitySO[] tempOrder = new EntitySO[Players.Length + Enemies.Length];

		Debug.Log("The game is " + tempOrder.Length + " entities long");


		int i = 0;

		foreach (EntitySO entity in Players)
		{
			if (entity != null)
			{
				Debug.Log("Welcome " + entity.name + "to combat!");
				tempOrder[i] = entity;
				i++;
			}
		}
		foreach (EntitySO entity in Enemies)
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
			if (entity != null)
			{
				Debug.Log("Well well, get QUEUED" + entity.name);
				queue.Enqueue(entity);
			}
		}
		return queue;
	}



	// Start is called before the first frame update
	void Awake()
    {
        actionManager = GetComponent<ActionManager>();
        uiManager = GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
