using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class StateTurnBase
{
	public abstract void EnterState(TurnManager turnManager, CombatSceneSO combatSceneSO);

	public abstract void UpdateState(TurnManager turnManager);
}