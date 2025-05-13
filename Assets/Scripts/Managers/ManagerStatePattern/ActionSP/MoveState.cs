using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


public class MoveState : ActionStateBase
{
	bool isMoving = false;
	
	private List<GridMapPoint> debugPath;

	private List<Vector3> pathVectorList;
	private int currentNode;
	ActionManager actionManager;

	int x;
	int y;

	Ray ray;

	GridMapPoint[] targetingPoints;

	public override void EnterState(ActionManager actionManager)
	{
		this.actionManager = actionManager;


		ReadyToMove(actionManager, actionManager.turnManager.GetCombatEntity());
		isMoving = true;

		HighlightAllMovables(actionManager.turnManager.EntitiesTurn.movementPoints);
	}

	private void HighlightAllMovables(int movementPoints)
	{
		
		var start = actionManager.gridManager.GetCurrentEntityCoords(actionManager.turnManager.EntitiesTurn);
		var visited = new HashSet<GridMapPoint>{ start };
		var frontier = new Queue<(GridMapPoint cell, int dist)>();
		frontier.Enqueue((start, 0));

		while (frontier.Count > 0)
		{
			var (cell, dist) = frontier.Dequeue();
			// highlight if not the origin
			if (dist > 0)
			{ 
				var highlight = cell.transform.GetChild(0).gameObject;
				highlight.SetActive(true);
			}
			

			if (dist == movementPoints) 
				continue;  // don’t go beyond your movement budget

			// enqueue each neighbor
			foreach (var neigh in new[]{cell.Up, cell.Right, cell.Down, cell.Left})
			{
				if (neigh != null && neigh.availablePoint && !visited.Contains(neigh))
				{
					visited.Add(neigh);
					frontier.Enqueue((neigh, dist + 1));
				}
			}
		}
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
		actionManager.ChangeState(actionManager.targetState);
	}

	public override void UpdateState(ActionManager actionManager)
	{
		if (Input.GetMouseButtonDown(0) && isMoving)
		{
			var ray = actionManager.camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out var hit) &&
			    hit.collider.CompareTag("Grid") &&
			    hit.collider.GetComponent<GridMapPoint>().availablePoint)
			{
				// Draw the ray & point
				Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);
				Debug.DrawRay(hit.point, Vector3.up * 0.5f, Color.green, 1f);

				// Convert to local grid coords using RoundToInt
				GridManager gm = actionManager.gridManager;
				float rawX = (hit.point.x - gm.gridStart.x) / gm.gridPointSize;
				float rawY = (hit.point.z - gm.gridStart.z) / gm.gridPointSize;
				int cellX = Mathf.RoundToInt(rawX);
				int cellY = Mathf.RoundToInt(rawY);
				
				Vector3 destination = gm.GetWorldPosition(cellX, cellY, true);
				Transform destin = actionManager.gridManager.gridPoints[cellX, cellY].transform;
				
				Debug.Log($"Click→raw({rawX:F2},{rawY:F2})→cell({cellX},{cellY})");

				// Run A* and draw its world-space path
				CombatEntity entity = actionManager.turnManager.GetCombatEntity();
				
				var path = gm.StartPath.FindPath(
					entity.GetGridPositionX(),
					entity.GetGridPositionY(),
					cellX, cellY
				);
				if (path != null)
				{
					for (int i = 0; i < path.Count - 1; i++)
					{
						var a = gm.GetWorldPosition(path[i].pos_x, path[i].pos_y, true);
						var b = gm.GetWorldPosition(path[i+1].pos_x, path[i+1].pos_y, true);
						Debug.DrawLine(a, b, Color.black, 1f);
					}
				}
				
				debugPath = path;
				
				// Finally update the grid and entity
				gm.UpdateGridPoint(cellX, cellY, entity);
				entity.UpdateGridPosition(cellX, cellY);
				entity.UpdatePosition(destin);

				isMoving = false;
			}
		}

	}
	public void ReadyToMove(ActionManager actionManager, CombatEntity combatEntity)
	{
		isMoving = !isMoving;

		if (isMoving && !combatEntity.hasMoved)
		{
			//actionManager.gridManager.MoveEntity(combatEntity,isMoving,actionManager.camera);
			isMoving = false;
			combatEntity.hasMoved = true;
		}

	}

	public void MoveProperly(Vector3 targetPoint)
	{
		currentNode = 0;
		pathVectorList = actionManager.gridManager.StartPath.FindPath(targetPoint, targetPoint, pathVectorList);

		if(pathVectorList != null && pathVectorList.Count > 1)
		{
			pathVectorList.RemoveAt(0);
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
}