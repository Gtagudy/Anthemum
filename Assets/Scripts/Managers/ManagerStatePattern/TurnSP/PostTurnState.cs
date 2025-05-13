using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PostTurnState : StateTurnBase
{
	public override void EnterState(TurnManager turnManager, CombatSceneSO combatSceneSO)
	{
		if(turnManager.gameIsOver)
		{
			Debug.WriteLine("GAMEOVER");
		} else
		{
			turnManager.ChangeState(turnManager.PreturnState);
		}
	}

	public override void UpdateState(TurnManager turnManager)
	{
	}
}