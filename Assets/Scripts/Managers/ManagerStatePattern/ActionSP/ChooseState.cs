using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChooseState : ActionStateBase
{
	int x;
	int y;

	GridMapPoint[] targetingPoints;

	public override void EnterState(ActionManager actionManager)
	{
		
	}

	public override void HandleButtonPress(ActionManager actionManager, AbilityButton button)
	{
		if (button != null)
		{

			x = actionManager.turnManager.EntitiesTurn.GetGridPositionX();
			y = actionManager.turnManager.EntitiesTurn.GetGridPositionY();
			/*targetingPoints = new GridMapPoint[4];

			GridMapPoint Here = actionManager.gridManager.gridPoints[x, y];
			targetingPoints[0] = Here.Up;
			targetingPoints[1] = Here.Right;
			targetingPoints[2] = Here.Down;
			targetingPoints[3] = Here.Left;

			for (int i = 0; i < 4; i++)
			{
				targetingPoints[i].transform.GetChild(1).gameObject.SetActive(true);
			}*/
			//Debug.Log(button.GetComponent<AbilityButton>().GetTargeting().ToString());
			actionManager.chosenAbility = button.GetAbility();
			actionManager.entityManager.GetTargets(button);
			//actionManager.entityManager.GetTargets(button, targetingPoints);
			//actionManager.UpdateHighlightedPoints(targetingPoints);
			actionManager.ChangeState(actionManager.targetState);
			//}
		}
	}

	public override void HandleButtonPress(ActionManager actionManager, string b)
	{

		switch(b)
		{
			case "Choose":
				actionManager.turnManager.uiManager.DisplayMoves();
				actionManager.ChangeState(actionManager.decideState);

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
		
	}

	public override void UpdateState(ActionManager actionManager)
	{
		
	}
}