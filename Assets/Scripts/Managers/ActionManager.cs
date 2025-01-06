using System;
using System.Collections;
using System.Collections.Generic;
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
		uiManager.ActionStateMachine.text = newState.ToString();
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

	//public event Action clicked;
	internal void ResolveEnemy(CombatEntity dequeue)
	{
		PauseAMoment();
		AbilitySO abilitySO = dequeue.entity.getAbility(random.Next(0,1));
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
		}
		else if(abilitySO.target == Targeting.Single)
		{
			if(abilitySO.AbilityEffectType == AbilityEffectType.Damage)
			{
				entityManager.GetPlayers(abilitySO, dequeue);
			}
		}
	}
	public void ConfirmAbility(AbilityButton button)
	{
		if (button != null) 
		{
			//Debug.Log(button.GetComponent<AbilityButton>().GetTargeting().ToString());
			chosenAbility = button.GetAbility();
			entityManager.GetTargets(button);
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
}
