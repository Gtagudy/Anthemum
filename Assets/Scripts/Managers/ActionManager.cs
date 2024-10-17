using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    System.Random random = new System.Random();
	int tempNum = 0;
	UIManager uiManager;
	TurnManager turnManager;

	bool chosenMove = false;
	//public event Action clicked;
	internal void ResolveEnemy(EntitySO dequeue)
	{
		//random.Next(1, 3);

		AbilitySO abilitySO = dequeue.getAbility(tempNum);
		if (abilitySO != null)
		{
			PauseAMoment();
			ConfirmAbility(dequeue, abilitySO);
		}
		Debug.Log("Just a debug here teehee");
	}

	private void ConfirmAbility(EntitySO dequeue, AbilitySO abilitySO)
	{
		PauseAMoment();
		if (abilitySO.target != Targeting.Self)
		{

		}
		else
		{
			if (abilitySO.AbilityEffectType == AbilityEffectType.Health)
			{
				//dequeue.ChangeHealth(-abilitySO.damage);
				
			}

		}
	}

	internal void ResolvePlayer(EntitySO dequeue)
	{
		
		if(chosenMove)
		{

			turnManager.TurnEnd(dequeue);
		}
		Debug.Log("Its the players turn GRAAAAHG");
	}

	// Start is called before the first frame update
	void Awake()
    {
        uiManager = GetComponent<UIManager>();
		turnManager = GetComponent<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	

	IEnumerator PauseAMoment()
	{
		yield return new WaitForSeconds(100);
	}
}
