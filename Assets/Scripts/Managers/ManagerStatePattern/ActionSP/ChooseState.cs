using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChooseState : ActionStateBase
{
	public override void EnterState(ActionManager actionManager)
	{
		
	}

	public override void HandleButtonPress(ActionManager actionManager)
	{
		/*if (button != null)
		{
			//Debug.Log(button.GetComponent<AbilityButton>().GetTargeting().ToString());
			chosenAbility = button.GetAbility();
			entityManager.GetTargets(button);
			//}
		}*/
	}

	public override void HandleStepBack(ActionManager actionManager)
	{
		actionManager.ChangeState(actionManager.decideState);
	}

	public override void UpdateState(ActionManager actionManager)
	{
		throw new NotImplementedException();
	}
}