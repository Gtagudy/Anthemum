using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EndturnState : StateTurnBase
{
	public override void EnterState(TurnManager turnManager, CombatSceneSO combatSceneSO)
	{
		turnManager.uiManager.PopTurnOrderUI();
		if (turnManager.EntitiesTurn.isPlayer)
		{
			turnManager.EntitiesTurn.GetMovesDisplay().SetActive(false);
			//turnManager.EntitiesTurn.ToggleDecisions().SetActive(false);
		}
		turnManager.EntitiesTurn.EndTurnTick();
		if(turnManager.queue.Count > 0 )
		{
			turnManager.ChangeState(turnManager.StartTurnState);
		} else
		{
			turnManager.ChangeState(turnManager.PostTurnState);
		}
	}

	public override void UpdateState(TurnManager turnManager)
	{
	}
}