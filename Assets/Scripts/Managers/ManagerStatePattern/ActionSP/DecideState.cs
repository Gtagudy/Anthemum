using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DecideState : ActionStateBase
{
	public override void EnterState(ActionManager actionManager)
	{

	}

	public override void HandleButtonPress(ActionManager actionManager, AbilityButton buttonID)
	{
		throw new NotImplementedException();
	}

	public override void HandleButtonPress(ActionManager actionManager, string b)
	{
		switch(b)
		{
			case "Move":

				actionManager.ChangeState(actionManager.moveState);

				break;

			case "Choose":
				CombatEntity combatEntity = actionManager.turnManager.GetCombatEntity();
				Debug.Log(combatEntity.entity.name + " is the mf whos moves should show");
				actionManager.turnManager.uiManager.DisplayMoves();
				//combatEntity.GetMovesDisplay().SetActive(true);
				actionManager.ChangeState(actionManager.chooseState);

				break;
			case "Skip":
				actionManager.turnManager.uiManager.AddToHistory("Skipped turn", actionManager.turnManager.GetCombatEntity());
				actionManager.turnManager.ChangeState(actionManager.turnManager.EndTurnState);
				break;
		}

	}

	public override void HandleButtonPress(ActionManager actionManager, EntityButton buttonID)
	{
		
	}

	public override void UpdateState(ActionManager actionManager)
	{
	}
}