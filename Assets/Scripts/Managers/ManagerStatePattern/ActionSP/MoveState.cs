using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


public class MoveState : ActionStateBase
{
	private bool isMoving;
	private ActionManager actionManager;
	private CombatEntity thisEntity;
	private GridManager gm;
	private List<GridMapPoint> debugPath;

	private float moveDuration = 0.4f;
	
	private List<GridMapPoint> queuedPath;  // NEW

	/// <summary>
	/// Call this before switching into MoveState to tell it which path to take.
	/// </summary>
	public void QueuePath(List<GridMapPoint> path)
	{
		queuedPath = path;
	}
	
	public override void EnterState(ActionManager am)
	{
		this.actionManager = am;
		thisEntity         = am.turnManager.GetCombatEntity();
		gm                 = am.gridManager;

		// If an AI path was queued, run it immediately:
		if (queuedPath != null)
		{
			// Start the animation, then clear our queue
			am.StartCoroutine(MoveAlongPath(queuedPath));
			queuedPath = null;
			return;
		}

		// Otherwise, it’s the player’s turn to click:
		ClearAllHighlights();
		HighlightReachable(thisEntity.GetCurrentStamina());
		isMoving = true;
	}
	private void HighlightReachable(int movementPoints)
	{
		var start = gm.GetCurrentEntityCoords(thisEntity);
		var visited = new HashSet<GridMapPoint> { start };
		var queue   = new Queue<(GridMapPoint cell, int dist)>();
		queue.Enqueue((start, 0));

		while (queue.Count > 0)
		{
			var (cell, dist) = queue.Dequeue();
			if (dist > 0)
				cell.transform.GetChild(0).gameObject.SetActive(true);

			if (dist == movementPoints) continue;

			foreach (var n in new[]{cell.Up, cell.Right, cell.Down, cell.Left})
			{
				if (n != null && n.availablePoint && visited.Add(n))
					queue.Enqueue((n, dist + 1));
			}
		}
	}
	
	private void OnDrawGizmosSelected()
	{
		if (debugPath == null) return;
		var gm = actionManager?.gridManager;
		if (gm == null) return;

		Gizmos.color = Color.yellow;
		for (int i = 0; i < debugPath.Count; i++)
		{
			var p = debugPath[i];
			var pos = gm.GetWorldPosition(p.pos_x, p.pos_y, true);
			Gizmos.DrawSphere(pos, gm.gridPointSize * 0.2f);
			if (i > 0)
			{
				var prev = debugPath[i-1];
				var pPos = gm.GetWorldPosition(prev.pos_x, prev.pos_y, true);
				Gizmos.DrawLine(pPos, pos);
			}
		}
	}

	private void ClearAllHighlights()
	{
		foreach (var point in actionManager.gridManager.gridPoints)
		{
			if (point == null) continue;
			point.transform.GetChild(0).gameObject.SetActive(false);
		}
	}

	public override void UpdateState(ActionManager actionManager)
	{
		if (isMoving && Input.GetMouseButtonDown(0))
		{
			Ray ray = actionManager.camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out var hit) && hit.collider.CompareTag("Grid"))
			{
				// 1) Get the exact GridMapPoint you clicked
				var cell = hit.collider.GetComponent<GridMapPoint>();
				if (cell != null && cell.availablePoint)
				{
					int x = cell.pos_x;
					int y = cell.pos_y;

					// 2) Build & clamp your path
					var rawPath = gm.StartPath.FindPath(
						thisEntity.GetGridPositionX(),
						thisEntity.GetGridPositionY(),
						x, y
					);
					if (rawPath == null) return;

					// Trim to movement budget
					int maxSteps = thisEntity.GetCurrentStamina();
					var path = rawPath.Count - 1 > maxSteps
						? rawPath.GetRange(0, maxSteps + 1)
						: rawPath;

					// Debug draw
					debugPath = path;
					DrawDebugPath(path);

					// 2) Start the animation coroutine (will clear highlights when done)
					isMoving = false;
					actionManager.StartCoroutine(MoveAlongPath(path));
				}
			}
		}
	}

	// -------------------------------
	// 3) Animate movement along each tile
	private IEnumerator MoveAlongPath(List<GridMapPoint> path)
	{
		ClearAllHighlights();

		for (int i = 1; i < path.Count; i++)
		{
			var cell   = path[i];
			Vector3 targetWorld = gm.GetWorldPosition(cell.pos_x, cell.pos_y, true);

			// 1) update grid & entity coords
			gm.UpdateGridPoint(cell.pos_x, cell.pos_y, thisEntity);
			thisEntity.UpdateGridPosition(cell.pos_x, cell.pos_y);

			// 2) animate
			float elapsed = 0f;
			Vector3 origin = thisEntity.transform.position;
			while (elapsed < moveDuration)
			{
				thisEntity.transform.position =
					Vector3.Lerp(origin, targetWorld, elapsed / moveDuration);
				elapsed += Time.deltaTime;
				yield return null;
			}
			thisEntity.transform.position = targetWorld;
		}

		// At this point, we've moved exactly one tile.
		// Now we need to resume the enemy’s turn logic if it’s still the same enemy.

		// Determine who just moved:
		var mover = thisEntity;  // thisEntity was set in EnterState

		if (!mover.isPlayer)
		{
			// We’re still in the same enemy’s turn—so restart their EnemyTurnRoutine
			actionManager.StartCoroutine(
				actionManager.EnemyTurnRoutine(mover)
			);
		}
		else
		{
			// If it was a player’s moveState, just go back to make a decision
			actionManager.ChangeState(actionManager.decideState);
		}
	}

	// Optional: draw the debug path in Scene view
	private void DrawDebugPath(List<GridMapPoint> path)
	{
		for (int i = 0; i < path.Count - 1; i++)
		{
			Vector3 a = gm.GetWorldPosition(
				path[i].pos_x, path[i].pos_y, true);
			Vector3 b = gm.GetWorldPosition(
				path[i + 1].pos_x, path[i + 1].pos_y, true);
			Debug.DrawLine(a, b, Color.yellow, 1f);
		}
	}
	public override void HandleButtonPress(ActionManager actionManager, AbilityButton buttonID)
	{
		ClearAllHighlights();
		actionManager.ChangeState(actionManager.chooseState);
	}

	public override void HandleButtonPress(ActionManager actionManager, string b)
	{
		ClearAllHighlights();
		actionManager.ChangeState(actionManager.decideState);
	}

	public override void HandleButtonPress(ActionManager actionManager, EntityButton buttonID)
	{
		ClearAllHighlights();
		actionManager.ChangeState(actionManager.targetState);
	}

}
