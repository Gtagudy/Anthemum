using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EnemyTurnState : StateTurnBase
{
	public override void EnterState(TurnManager turnManager, CombatSceneSO combatSceneSO)
	{
		turnManager.actionManager.ResolveEnemy(turnManager.EntitiesTurn);

		turnManager.ChangeState(turnManager.EndTurnState);
	}

	public override void UpdateState(TurnManager turnManager)
	{
		
	}
}