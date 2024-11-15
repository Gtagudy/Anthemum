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

	public override void HandleButtonPress(ActionManager actionManager)
	{
		CombatEntity combatEntity = actionManager.turnManager.GetCombatEntity();
		Debug.Log(combatEntity.GetEntitySO().name + " is the mf whos moves should show");
		combatEntity.GetMovesDisplay().SetActive(true);
		actionManager.ChangeState(actionManager.chooseState);
	}

	public override void HandleStepBack(ActionManager actionManager)
	{
		actionManager.ChangeState(actionManager.moveState);
	}

	public override void UpdateState(ActionManager actionManager)
	{
	}
}