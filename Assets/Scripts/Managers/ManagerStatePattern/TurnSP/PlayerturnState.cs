using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

public class PlayerturnState : StateTurnBase
{
	public override void EnterState(TurnManager turnManager, CombatSceneSO combatSceneSO)
	{
		turnManager.GetCombatEntity().ToggleDecisions().gameObject.SetActive(true);

	}

	public override void UpdateState(TurnManager turnManager)
	{
		
	}

	public void EndMyTurn()
	{

	}
}