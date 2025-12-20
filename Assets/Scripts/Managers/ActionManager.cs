using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static System.Collections.Specialized.BitVector32;
using static UnityEngine.EventSystems.EventTrigger;

public class ActionManager : MonoBehaviour
{
    System.Random random = new System.Random();
	int tempNum = 0;
	UIManager uiManager;
	public TurnManager turnManager;
	public EntityManager entityManager;
	public GridManager gridManager;

	public UnityEvent endTurn;

	[SerializeField] bool chosenMove = false;

	bool isMoving = false;

	public AbilitySO chosenAbility;
	public Camera camera;

	public ActionStateBase actionState;
	public ChooseState chooseState = new();
	public MoveState moveState = new();
	public TargetState targetState = new();
	public DecideState decideState = new();


	public UnityAction action;

	public GridMapPoint[] gridPoints;

	// expose these in the Inspector to fine‐tune pacing:
	[SerializeField] private float thinkDelay   = 0.5f;
	[SerializeField] private float actionDelay  = 0.3f;
	
	private void Start()
	{
		if(endTurn == null)
		{
			endTurn = new UnityEvent();	
		}

		actionState = decideState;

		actionState.EnterState(this);
	}
	
	public void ChangeState(ActionStateBase newState)
	{
		//uiManager.ActionStateMachine.text = newState.ToString();
		actionState = newState;

		actionState.EnterState(this);
	}
    void Update()
    {
		actionState.UpdateState(this);
    }

	public void OnButtonPressed(string ButtonID)
	{
		actionState.HandleButtonPress(this, ButtonID);
	}
	public void OnButtonEPressed(EntityButton button)
	{
		actionState.HandleButtonPress(this, button);
	}
	public void OnButtonAPressed(AbilityButton button)
	{
		actionState.HandleButtonPress(this, button);
	}
	
	/// <summary>
	/// The full enemy‐turn sequence.
	/// </summary>
public IEnumerator EnemyTurnRoutine(CombatEntity enemy)
{
    // 0) Brief “think” pause
    yield return new WaitForSeconds(thinkDelay);

    // 1) Choose an ability
    AbilitySO abi = enemy.Entity.getAbility(
        random.Next(0, enemy.Entity.GetAbilities().Count)
    );

    // 2) Find the closest player
    CombatEntity target = entityManager.FindClosestPlayer(enemy);
    if (target == null)
    {
        // Nothing left to attack
        turnManager.ChangeState(turnManager.EndTurnState);
        yield break;
    }

    // 3) Gather grid coordinates
    int sx = enemy.GetGridPositionX();
    int sy = enemy.GetGridPositionY();
    int tx = target.GetGridPositionX();
    int ty = target.GetGridPositionY();

    // 4) Compute Manhattan distance for “in range?”
    int manhattanDist = Mathf.Abs(tx - sx) + Mathf.Abs(ty - sy);

    // 5) If the target is outside ability range, try to move one cardinal step
    if (manhattanDist > abi.range)
    {
        // Pick the cardinal direction (dx, dy) toward target
        int dx = tx - sx;
        int dy = ty - sy;

        // Zero out the smaller axis so we only move one cardinal step
        if (Mathf.Abs(dx) > Mathf.Abs(dy))
            dy = 0;
        else
            dx = 0;

        int stepX = Mathf.Clamp(dx, -1, 1);
        int stepY = Mathf.Clamp(dy, -1, 1);

        int newX = sx + stepX;
        int newY = sy + stepY;

        // 5a) Check bounds
        if (!gridManager.InBounds(newX, newY))
        {
            // Out of bounds → can’t step, so end turn
            turnManager.ChangeState(turnManager.EndTurnState);
            yield break;
        }

        // 5b) See if that neighbor cell is free or is the target’s own tile
        var cell = gridManager.GetPointUsingCoords(newX, newY);
        bool isTargetCell = (newX == tx && newY == ty);

        if (cell.availablePoint)
        {
            // Cardial neighbor is free → move there via MoveState
            var pathToOne = new List<GridMapPoint>
            {
                gridManager.GetPointUsingCoords(sx, sy),
                cell
            };
            moveState.QueuePath(pathToOne);
            ChangeState(moveState);

            // Stop now—don’t fall into attack
            yield break;
        }
        else if (isTargetCell)
        {
            // The player’s tile is occupied by the player itself,
            // so this counts as “we can’t move there,” but that also
            // means the player is adjacent—so we’ll attack below.
        }
        else
        {
            // Neighbor is blocked by scenery or another enemy. Let’s try A* for one tile:
            List<GridMapPoint> fullPath = gridManager.StartPath.FindPath(sx, sy, tx, ty);
            if (fullPath != null && fullPath.Count >= 2)
            {
                // Use the next cell in the A* route, if it’s free
                GridMapPoint next = fullPath[1];
                if (next.availablePoint)
                {
                    var pathToOne = new List<GridMapPoint>
                    {
                        fullPath[0], // current
                        next
                    };
                    moveState.QueuePath(pathToOne);
                    ChangeState(moveState);
                    yield break;
                }
            }

            // If we get here, both the cardinal step and the A* step were blocked.
            // No legal move → end turn
            turnManager.ChangeState(turnManager.EndTurnState);
            yield break;
        }
    }

    // 6) If we reach here, manhattanDist <= abi.range → attack sequence

    // 6a) Run QTE
    bool qteSuccess = false;
    var onSuccess = new UnityEngine.Events.UnityEvent();
    onSuccess.AddListener(() => qteSuccess = true);
    var onFail = new UnityEngine.Events.UnityEvent();

    yield return StartCoroutine(
        QTEManager.Instance.RunQTE(
            abi.qteStartDelay,
            abi.qteWindow,
            onSuccess,
            onFail
        )
    );

    // 6b) Compute & apply damage
    int baseDamage = abi.damage
                   + enemy.Entity.Stats.attack
                   - target.Entity.Stats.defense;
    int finalDamage = Mathf.Max(0,
        Mathf.RoundToInt(baseDamage * (qteSuccess ? abi.qteSuccessMultiplier : 1f))
    );
    target.Entity.ChangeHealth(finalDamage);
    uiManager.UpdateHealth(target);
    uiManager.AddToHistory(abi, enemy, target);

    // 6c) Check for death
    entityManager.CheckForDeath(target);

    // 6d) If target still alive & has knockback, shove them
    if (abi.knockback > 0 && target.Entity.GetHealth() > 0)
    {
        var dir = entityManager.GetCardinalDirection(
            enemy.GetGridPositionX(), enemy.GetGridPositionY(),
            target.GetGridPositionX(), target.GetGridPositionY()
        );
        float rawKb = abi.knockback * (qteSuccess ? abi.qteSuccessMultiplier : 1f);
        int kb = Mathf.RoundToInt(rawKb);
        yield return StartCoroutine(
            entityManager.ShoveRoutine(target, kb, dir)
        );
    }

    // 7) End this enemy’s turn
    yield return new WaitForSeconds(actionDelay);
    turnManager.ChangeState(turnManager.EndTurnState);
}


// A helper coroutine that does the QTE → damage → knockback in one shot
private IEnumerator RunEnemyAttackSequence(
    CombatEntity enemy,
    CombatEntity target,
    AbilitySO abi
)
{
    // A) Run QTE prompt
    bool qteSuccess = false;
    var onSuccess = new UnityEngine.Events.UnityEvent();
    onSuccess.AddListener(() => qteSuccess = true);
    var onFail = new UnityEngine.Events.UnityEvent();

    yield return StartCoroutine(
        QTEManager.Instance.RunQTE(
            abi.qteStartDelay,
            abi.qteWindow,
            onSuccess,
            onFail
        )
    );

    // B) Compute and apply damage (with QTE multiplier)
    int baseDamage = abi.damage
                   + enemy.Entity.Stats.attack
                   - target.Entity.Stats.defense;
    int finalDamage = Mathf.Max(0,
        Mathf.RoundToInt(baseDamage * (qteSuccess ? abi.qteSuccessMultiplier : 1f))
    );
    target.Entity.ChangeHealth(finalDamage);
    uiManager.UpdateHealth(target);
    uiManager.AddToHistory(abi, enemy, target);

    // C) Check for death
    entityManager.CheckForDeath(target);

    // D) If still alive and knockback > 0, shove them
    if (abi.knockback > 0 && target.Entity.GetHealth() > 0)
    {
        var dir = entityManager.GetCardinalDirection(
            enemy.GetGridPositionX(), enemy.GetGridPositionY(),
            target.GetGridPositionX(), target.GetGridPositionY()
        );
        float rawKb = abi.knockback * (qteSuccess ? abi.qteSuccessMultiplier : 1f);
        int kb = Mathf.RoundToInt(rawKb);

        yield return StartCoroutine(
            entityManager.ShoveRoutine(target, kb, dir)
        );
    }
	}

	/// <summary>
	/// From a list of candidate GridMapPoints, returns the one whose
	/// grid coordinates are closest (Euclidean) to the source entity.
	/// </summary>
	private GridMapPoint PickClosestNeighbor(
		CombatEntity source,
		List<GridMapPoint> candidates
	) {
		int sx = source.GetGridPositionX();
		int sy = source.GetGridPositionY();

		GridMapPoint best     = null;
		float        bestDist = float.MaxValue;

		foreach (var c in candidates)
		{
			// squared distance in grid‐space
			float dx = c.pos_x - sx;
			float dy = c.pos_y - sy;
			float d2 = dx*dx + dy*dy;

			if (d2 < bestDist)
			{
				bestDist = d2;
				best     = c;
			}
		}
		return best;
	}
	//public event Action clicked;
	internal void ResolveEnemy(CombatEntity dequeue)
	{
		PauseAMoment();
		AbilitySO abilitySO = dequeue.Entity.getAbility(random.Next(0,dequeue.Entity.GetAbilities().Count));
		if (abilitySO != null)
		{

			ConfirmAbility(dequeue, abilitySO);
		}
		Debug.Log("Just a debug here teehee");
	}

	private void ConfirmAbility(CombatEntity dequeue, AbilitySO abilitySO)
	{
		PauseAMoment();
		Debug.Log("---------------");
		Debug.Log("GRAAAAGH");
		Debug.Log("---------------");

		if (abilitySO.target == Targeting.Self)
		{
			if(abilitySO.AbilityEffectType == AbilityEffectType.Health)
			{
				entityManager.HandleAbility(abilitySO, dequeue);
			}
			else if(abilitySO.AbilityEffectType == AbilityEffectType.Buff || abilitySO.AbilityEffectType == AbilityEffectType.Debuff)
			{
				entityManager.HandleAbilityStatus(abilitySO, dequeue);
			}
		}
		else if(abilitySO.target == Targeting.Single)
		{
			if(abilitySO.AbilityEffectType == AbilityEffectType.Damage)
			{
				entityManager.GetPlayers(abilitySO, dequeue);
			} 
			else if(abilitySO.AbilityEffectType == AbilityEffectType.DamageDebuff)
			{
				entityManager.HandleAbility(abilitySO, dequeue);
				entityManager.HandleAbilityStatus(abilitySO, turnManager.EntitiesTurn);
			}
		}
	}
	public void ConfirmAbility(AbilityButton button)
	{
		if (button != null) 
		{
			//Debug.Log(button.GetComponent<AbilityButton>().GetTargeting().ToString());
			chosenAbility = button.GetAbility();
			ChangeState(targetState);
			//entityManager.GetTargets(button);
				//}
		}
	}

	public void FinalizeAbility(EntityButton entity)
	{
		entityManager.HandleAbility(chosenAbility, entity.GetEntity());
		chosenMove = true;
		ResolvePlayer(entity.GetEntity());
	}

	internal void ResolvePlayer(CombatEntity dequeue)
	{
		
		/*if(chosenMove)
		{
			chosenMove = false;
			turnManager.TurnEnd(dequeue);
		}
		Debug.Log("Its the players turn GRAAAAHG");*/
	}

	

	// Start is called before the first frame update
	void Awake()
    {
        uiManager = GetComponent<UIManager>();
		turnManager = GetComponent<TurnManager>();
		entityManager = GetComponent<EntityManager>();
		gridManager = GetComponent<GridManager>();

		camera = Camera.main;
    }

    // Update is called once per frame

	

	IEnumerator PauseAMoment()
	{
		yield return new WaitForSeconds(500);
	}

	internal void UpdateHighlightedPoints(GridMapPoint[] targetingPoints)
	{
		gridPoints = targetingPoints;
	}
	public void EmptyHighlightPoints(int p)
	{
		for(int i = 0; i < gridPoints.Length; i++)
		{
			gridPoints[i].transform.GetChild(p).gameObject.SetActive(true);
		}
	}

	public void ConfirmAbility(CombatEntity gridSoEntityHere)
	{
		entityManager.HandleAbility(chosenAbility, gridSoEntityHere);
	}
}
