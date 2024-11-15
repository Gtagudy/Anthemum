using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MoveState : ActionStateBase
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
		throw new NotImplementedException();
	}

	public override void HandleButtonPress(ActionManager actionManager, EntityButton buttonID)
	{
		throw new NotImplementedException();
	}

	public override void UpdateState(ActionManager actionManager)
	{
		throw new NotImplementedException();
	}
}