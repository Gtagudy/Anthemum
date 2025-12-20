using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EntityManager : MonoBehaviour
{
    ActionManager actionManager;
    UIManager uiManager;
	GameManager gameManager;
	TurnManager turnManager;
	GridManager gridManager;

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
	
	/// <summary>
	/// Returns the alive player whose world‐space position
	/// is closest to `source`. Returns null if no players remain.
	/// </summary>
	public CombatEntity FindClosestPlayer(CombatEntity source)
	{
		CombatEntity best = null;
		float bestSq = float.MaxValue;
		Vector3 srcPos = source.transform.position;

		foreach (var p in Players)
		{
			// skip any dead/out‐of‐combat
			if (p == null) continue;

			float sq = (p.transform.position - srcPos).sqrMagnitude;
			if (sq < bestSq)
			{
				bestSq = sq;
				best   = p;
			}
		}
		return best;
	}

	internal void GetPlayers(AbilitySO abilitySO, CombatEntity dequeue)
	{
		int chosenPlayer = random.Next(Players.Count);
		uiManager.AddToHistory(abilitySO, dequeue, Players[chosenPlayer]);
		Players[chosenPlayer].Entity.ChangeHealth(abilitySO.damage);
		uiManager.UpdateHealth(Players[chosenPlayer]);
		CheckForDeath(Players[chosenPlayer]);

	}

	public void CheckForDeath(CombatEntity dequeue)
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
		StartCoroutine( ExecuteAbilityRoutine(chosenAbility, entity) );
		
		/*
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
		
		if (chosenAbility.knockback > 0 && entity != null)
		{
			var dir = GetCardinalDirection(
				turnManager.EntitiesTurn.GetGridPositionX(), turnManager.EntitiesTurn.GetGridPositionY(),
				entity.GetGridPositionX(), entity.GetGridPositionY()
			);
			actionManager.StartCoroutine(
				ShoveRoutine(entity, chosenAbility.knockback, dir)
			);
		}
		*/
		
	}
	private IEnumerator ExecuteAbilityRoutine(
		AbilitySO abi,
		CombatEntity target
	) {
		var caster = turnManager.EntitiesTurn;

		bool qteSuccess = false;

		// 1) Create the UnityEvent and wire up the listener
		var successEvent = new UnityEvent();
		successEvent.AddListener(() => qteSuccess = true);

		// 2) (optional) a fail event if you want to hook something on fail
		var failEvent = new UnityEvent();

		// 3) Now pass those into RunQTE
		yield return StartCoroutine(
			QTEManager.Instance.RunQTE(
				abi.qteStartDelay,
				abi.qteWindow,
				successEvent,
				failEvent
			)
		);

		// 2) Compute final damage (with any QTE multiplier)
		int baseDamage = abi.damage 
		                 + caster.Entity.Stats.attack 
		                 - target.Entity.Stats.defense;
		int finalDamage = Mathf.Max(0,
			Mathf.RoundToInt(baseDamage * (qteSuccess ? abi.qteSuccessMultiplier : 1f))
		);

		// 3) Apply damage & update UI/history/death
		target.Entity.ChangeHealth(finalDamage);
		uiManager.UpdateHealth(target);

		if (abi.AbilityEffectType == AbilityEffectType.Damage)
		{
			uiManager.AddToHistory(abi, caster, target);
			CheckForDeath(target);
		}
		else
		{
			HandleAbilityStatus(abi, target);
		}

		// 4) Knockback
		if (abi.knockback > 0 && target.Entity.GetHealth() > 0 && target.gameObject.activeInHierarchy)
		{
			var dir = GetCardinalDirection(
				caster.GetGridPositionX(), caster.GetGridPositionY(),
				target.GetGridPositionX(), target.GetGridPositionY()
			);
			float rawKb = abi.knockback * (qteSuccess ? abi.qteSuccessMultiplier : 1f);

		// convert to int
			int finalKnockback = Mathf.RoundToInt(rawKb);

		// now call your shove coroutine
			yield return StartCoroutine(
				ShoveRoutine(target, finalKnockback, dir)
			);
		}

		// 5) Signal that this action (and turn‐step) is complete
		//actionManager.OnActionComplete();
	}
	public Vector2Int GetCardinalDirection(int sx, int sy, int tx, int ty)
    {
        int dx = tx - sx, dy = ty - sy;
        // pick the dominant axis
        if (Mathf.Abs(dx) > Mathf.Abs(dy)) dy = 0;
        else                             dx = 0;
        return new Vector2Int(Mathf.Clamp(dx, -1, 1),
                              Mathf.Clamp(dy, -1, 1));
    }

    public IEnumerator ShoveRoutine(CombatEntity victim, int tiles, Vector2Int dir)
    {
        int startX = victim.GetGridPositionX();
        int startY = victim.GetGridPositionY();

        for (int i = 1; i <= tiles; i++)
        {
            int nx = startX + dir.x * i;
            int ny = startY + dir.y * i;

            // 1) Bounds check
            if (!gridManager.InBounds(nx, ny))
                break;

            var cell = gridManager.GetPointUsingCoords(nx, ny);

            // 2) Collision?
            if (!cell.availablePoint)
            {
                // a) Scenery?
                if (cell.gridObject != null)
                {
                    cell.gridObject.ApplyDamage(1);
                }
                // b) Another entity?
                else if (cell.gridSO.entityHere != null)
                {
                    // you could chain‐push or just stop
                }
                break;
            }

            // 3) Move occupancy & update victim’s grid coords
            gridManager.UpdateGridPoint(nx, ny, victim);
            victim.UpdateGridPosition(nx, ny);

            // 4) Animate the GameObject to that world position
            yield return AnimateMove(victim.gameObject, cell.transform.position);
        }

        // 5) Once done, signal end‐of‐action
        //actionManager.OnActionComplete();
    }

    public IEnumerator AnimateMove(GameObject go, Vector3 dest)
    {
        float elapsed = 0f, duration = 0.1f;
        Vector3 origin = go.transform.position;
        while (elapsed < duration)
        {
            go.transform.position =
                Vector3.Lerp(origin, dest, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        go.transform.position = dest;
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
		gridManager = GetComponent<GridManager>();
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
