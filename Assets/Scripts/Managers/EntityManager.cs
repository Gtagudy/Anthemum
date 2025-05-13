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
	GameManager gameManager;
	TurnManager turnManager;

	public List<CombatEntity> Players;
	public List<CombatEntity> Enemies;

	IntGameEvent updateHealth;
	System.Random random = new System.Random();

	public void CheckEntity(CombatEntity dequeue)
	{
		if(dequeue != null)
        {
            if (dequeue.Entity.isPlayer == true)
            {

                Debug.Log("Welcome player!");
                //actionManager.ResolvePlayer(dequeue);

            }
            else
            {
                Debug.Log("Ohh your the enemy!!");
                actionManager.ResolveEnemy(dequeue);
            }
        }
	}

	internal void GetPlayers(AbilitySO abilitySO, CombatEntity dequeue)
	{
		int chosenPlayer = random.Next(Players.Count);
		uiManager.AddToHistory(abilitySO, dequeue, Players[chosenPlayer]);
		Players[chosenPlayer].Entity.ChangeHealth(abilitySO.damage);
		uiManager.UpdateHealth(Players[chosenPlayer]);
		CheckForDeath(Players[chosenPlayer]);

	}

	private void CheckForDeath(CombatEntity dequeue)
	{
		if(dequeue.isPlayer && dequeue.Entity.GetHealth() <= 0)
		{			
			turnManager.DropEntity(dequeue);
		} else if (dequeue.Entity.GetHealth() <= 0)
		{
			turnManager.DropEntity(dequeue);
		}
	}

	internal void GetTargets(AbilityButton button)
	{
        uiManager.LetPlayerTarget(button, Enemies, Players);
	}

	internal void HandleAbility(AbilitySO chosenAbility, CombatEntity entity)
	{
		entity.Entity.ChangeHealth(chosenAbility.damage
			+ turnManager.EntitiesTurn.Entity.Stats.attack
			- entity.Entity.Stats.defense);
		uiManager.UpdateHealth(entity);

		if(chosenAbility.AbilityEffectType == AbilityEffectType.Damage)
		{
			uiManager.AddToHistory(chosenAbility, turnManager.EntitiesTurn, entity);
			CheckForDeath(entity);
		}
		else
		{
			HandleAbilityStatus(chosenAbility, entity);
		}
	}
	internal void HandleAbilityStatus(AbilitySO abilitySO, CombatEntity dequeue)
	{
		uiManager.AddToHistory(abilitySO, turnManager.EntitiesTurn, dequeue);

		if (abilitySO.AbilityEffectType == AbilityEffectType.Buff 
			|| abilitySO.AbilityEffectType == AbilityEffectType.DamageBuff)
		{
			dequeue.AddBuff(abilitySO.buff, 1, abilitySO.statusEffectCount);
		} 
		else if(abilitySO.AbilityEffectType == AbilityEffectType.Debuff
			|| abilitySO.AbilityEffectType == AbilityEffectType.DamageDebuff) 
		{
			dequeue.AddDebuff(abilitySO.debuff, 1, abilitySO.statusEffectCount);
		} else if(abilitySO.AbilityEffectType == AbilityEffectType.DamageSyphon)
		{
			turnManager.EntitiesTurn.AddBuff(abilitySO.buff, 1, abilitySO.statusEffectCount);
			turnManager.EntitiesTurn.AddDebuff(abilitySO.debuff, 1, abilitySO.statusEffectCount);
		}
	}

	internal void NotifyOfAll(List<CombatEntity> players, List<CombatEntity> enemies)
	{
		Players = new List<CombatEntity>(players.Count);
		Enemies = new List<CombatEntity>(enemies.Count);

		for(int i = 0; i < players.Count; i++)
		{
			//Players[i] = players[i];
			Players.Add(players[i]);
		}

		for (int i = 0; i < enemies.Count; i++)
		{
			//Enemies[i] = enemies[i];

			Enemies.Add(enemies[i]);

		}

		/*for (int i = 0; i < players.Length; i++) 
        {
            this.Players[i] = players[i];
        }
		for (int i = 0; i < players.Length; i++)
		{
			this.Enemies[i] = enemies[i];
		}*/
	}

	public List<CombatEntity> ReqeueuEntities(List<CombatEntity> queue)
	{
		CombatEntity[] tempOrder = new CombatEntity[Players.Count + Enemies.Count];

		Debug.Log("The game is " + tempOrder.Length + " entities long");


		int i = 0;

		foreach (CombatEntity entity in Players)
		{
			if (entity != null)
			{
				Debug.Log("Welcome " + entity.name + "to combat!");
				tempOrder[i] = entity;
				i++;
			}
		}
		foreach (CombatEntity entity in Enemies)
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
		entity.Entity.GetSpeed())
		.ToArray();
		foreach (CombatEntity entity in tempOrder)
		{
			if (entity != null)
			{
				Debug.Log("Well well, get QUEUED" + entity.name);
				queue.Add(entity);
			}
		}
		return queue;
	}



	// Start is called before the first frame update
	void Awake()
    {
        actionManager = GetComponent<ActionManager>();
        uiManager = GetComponent<UIManager>();
		turnManager = GetComponent<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	internal void GetTargets(AbilityButton button, GridMapPoint[] targetingPoints)
	{
		if(button.name == "NUUUKE")
		{
			uiManager.LetPlayerTarget(button, Enemies, Players);
		}
		GridMapPoint occupiedSpace;
		for(int i = 0; i < targetingPoints.Length; i++)
		{
			if (targetingPoints[i] != null && targetingPoints[i].gridSO.IsEntityHere)
			{
				occupiedSpace = targetingPoints[i];
				uiManager.LetPlayerTarget(button, occupiedSpace);
			}
		}
	}

}
