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

	public override void HandleButtonPress(ActionManager actionManager, AbilityButton button)
	{
		if (button != null)
		{
			//Debug.Log(button.GetComponent<AbilityButton>().GetTargeting().ToString());
			actionManager.chosenAbility = button.GetAbility();
			actionManager.entityManager.GetTargets(button);
			actionManager.ChangeState(actionManager.targetState);
			//}
		}
	}

	public override void HandleButtonPress(ActionManager actionManager, string b)
	{

		switch(b)
		{
			case "Punch":

				break;
			case "Rest":

				break;
			case "Enrage":

				break;
			case "Stun":

				break;
		}

		/*if (button != null)
		{
			//Debug.Log(button.GetComponent<AbilityButton>().GetTargeting().ToString());
			chosenAbility = button.GetAbility();
			entityManager.GetTargets(button);
			//}
		}*/
	}

	public override void HandleButtonPress(ActionManager actionManager, EntityButton buttonID)
	{
		throw new NotImplementedException();
	}

	public override void UpdateState(ActionManager actionManager)
	{
		
	}
}