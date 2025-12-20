using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class TargetState : ActionStateBase
{
	private ITargetingStrategy strat;
	private HashSet<GridMapPoint> validCells;

	public override void EnterState(ActionManager am)
	{
		var abi = am.chosenAbility;
		strat      = TargetingFactory.Create(abi.target);
		validCells = new HashSet<GridMapPoint>(
			strat.GetValidCells(am.turnManager.EntitiesTurn, abi)
		);
		foreach (var c in validCells) c.ShowTargetHighlight(true);
	}

	public override void UpdateState(ActionManager am)
	{
		if (!Input.GetMouseButtonDown(0)) return;
		var hit = Physics.RaycastAll(am.camera.ScreenPointToRay(Input.mousePosition))
			.FirstOrDefault(h => h.collider.CompareTag("Grid"));
		if (hit.collider == null) return;

		var cell = hit.collider.GetComponent<GridMapPoint>();
		if (!validCells.Contains(cell)) return;
		if (!strat.IsValidTarget(am.turnManager.EntitiesTurn, am.chosenAbility, cell))
			return;

		// Confirm!
		am.ConfirmAbility(cell.gridSO.entityHere);  
		
		//am.entityManager.HandleAbility(am.chosenAbility, buttonID.GetEntity());

		ClearHighlights();
		am.ChangeState(am.decideState);
	}

	private void ClearHighlights()
	{
		foreach (var c in validCells) c.ShowTargetHighlight(false);
		validCells.Clear();
	}

	public override void HandleButtonPress(ActionManager actionManager, AbilityButton buttonID)
	{
		actionManager.ChangeState(actionManager.chooseState);
	}

	public override void HandleButtonPress(ActionManager actionManager, string b)
	{
		actionManager.EmptyHighlightPoints(0);
		actionManager.ChangeState(actionManager.decideState);

	}

	public override void HandleButtonPress(ActionManager actionManager, EntityButton buttonID)
	{
		actionManager.entityManager.HandleAbility(actionManager.chosenAbility, buttonID.GetEntity());
		//chosenMove = true;
		//actionManager.ResolvePlayer(buttonID.GetEntity());
		actionManager.EmptyHighlightPoints(1);
		actionManager.turnManager.uiManager.DisplayMoves();

		actionManager.ChangeState(actionManager.decideState);
	}

}