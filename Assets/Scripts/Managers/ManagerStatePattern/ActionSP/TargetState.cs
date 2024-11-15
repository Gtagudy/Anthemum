using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.EventSystems.EventTrigger;


public class TargetState : ActionStateBase
{
	public override void EnterState(ActionManager actionManager)
	{

	}

	public override void HandleButtonPress(ActionManager actionManager, AbilityButton buttonID)
	{
		actionManager.ChangeState(actionManager.chooseState);
	}

	public override void HandleButtonPress(ActionManager actionManager, string b)
	{
		actionManager.ChangeState(actionManager.decideState);

	}

	public override void HandleButtonPress(ActionManager actionManager, EntityButton buttonID)
	{
		actionManager.entityManager.HandleAbility(actionManager.chosenAbility,buttonID.GetEntity());
		//chosenMove = true;
		actionManager.ResolvePlayer(buttonID.GetEntity());
		actionManager.ChangeState(actionManager.chooseState);

	}

	public override void UpdateState(ActionManager actionManager)
	{
	}
}