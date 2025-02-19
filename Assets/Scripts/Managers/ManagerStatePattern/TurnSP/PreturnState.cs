using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PreturnState : StateTurnBase
{
	public override void EnterState(TurnManager turnManager, CombatSceneSO combatSceneSO)
	{
		//turnManager.EntitiesTurn.EvaluateStatus();
		turnManager.entityManager.ReqeueuEntities(turnManager.queue);
		
		turnManager.uiManager.CreateTurnOrder(turnManager.queue);

		Queue queue = turnManager.queue;
		if (queue.Count > 0)
		{
			Debug.Log("Oho look whose turn it is " + queue.Peek());
			turnManager.ChangeState(turnManager.StartTurnState);
		}
	}

	public override void UpdateState(TurnManager turnManager)
	{
	}
} 