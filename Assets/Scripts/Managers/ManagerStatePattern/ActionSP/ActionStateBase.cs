using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class ActionStateBase
{
	public abstract void EnterState(ActionManager actionManager);

	public abstract void UpdateState(ActionManager actionManager);

	public abstract void HandleButtonPress(ActionManager actionManager);

	public abstract void HandleStepBack(ActionManager actionManager);
}