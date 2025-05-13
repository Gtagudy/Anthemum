using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StartTurnState : StateTurnBase
{
	CombatEntity EntitiesTurn;
	public override void EnterState(TurnManager turnManager, CombatSceneSO combatSceneSO)
	{
		
		EntitiesTurn = (CombatEntity)turnManager.queue[0];
		turnManager.queue.RemoveAt(0);
		turnManager.EntitiesTurn = EntitiesTurn;
		turnManager.uiManager.UpdateCamera();
		/*if(playerTurn.GetEntitySO().isPlayer)
        {
            playerTurn.GetMovesDisplay().SetActive(true);
        }*/
		Debug.Log("Now, it seems it is " + EntitiesTurn.name + " turn");
		Debug.Log("Heres your health" + EntitiesTurn.Entity.GetHealth());
		EntitiesTurn.hasMoved = false;
		//turnManager.camera.m_LookAt = EntitiesTurn.transform;
		//turnManager.camera.m_Follow = EntitiesTurn.transform;
		turnManager.uiManager.WhoseTurn(EntitiesTurn);
		//turnManager.entityManager.CheckEntity((CombatEntity)EntitiesTurn);

		turnManager.uiManager.ChangeEntityPanel(EntitiesTurn);

		if(EntitiesTurn.isPlayer)
		{
			turnManager.ChangeState(turnManager.PlayerturnState);
		} else
		{
			turnManager.ChangeState(turnManager.EnemyTurnState);
		}
	}

	public override void UpdateState(TurnManager turnManager)
	{
		
	}

}