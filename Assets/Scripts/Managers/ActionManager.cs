using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class ActionManager : MonoBehaviour
{
    System.Random random = new System.Random();
	int tempNum = 0;
	UIManager uiManager;
	TurnManager turnManager;
	EntityManager entityManager;

	bool chosenMove = false;
	AbilitySO chosenAbility;

	//public event Action clicked;
	internal void ResolveEnemy(CombatEntity dequeue)
	{
		PauseAMoment();
		AbilitySO abilitySO = dequeue.GetEntitySO().getAbility(random.Next(0,1));
		if (abilitySO != null)
		{
			ConfirmAbility(dequeue, abilitySO);
		}
		Debug.Log("Just a debug here teehee");
	}

	private void ConfirmAbility(CombatEntity dequeue, AbilitySO abilitySO)
	{
		PauseAMoment();
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
		turnManager.TurnEnd();
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	

	IEnumerator PauseAMoment()
	{
		yield return new WaitForSeconds(500);
	}
}
