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

		this.actionManager = actionManager;
	}

	private void HighlightAllMovables(int movementPoints)
	{
		x = actionManager.turnManager.EntitiesTurn.GetGridPositionX();
		y = actionManager.turnManager.EntitiesTurn.GetGridPositionY();
		targetingPoints = new GridMapPoint[4];

		GridMapPoint home = actionManager.gridManager.gridPoints[x, y];

		targetingPoints[0] = home.Up;
		targetingPoints[1] = home.Right;
		targetingPoints[2] = home.Down;
		targetingPoints[3] = home.Left;

		for (int i = 0; i < 4; i++)
		{
			targetingPoints[i].transform.GetChild(0).gameObject.SetActive(true);
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
			Vector3 mousePos = Input.mousePosition;
			ray = actionManager.camera.ScreenPointToRay(mousePos);



			if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.tag == "Grid" && hit.collider.gameObject.GetComponent<GridMapPoint>().availablePoint == true)
			{

				CombatEntity thisEntity = actionManager.turnManager.GetCombatEntity();


				Vector3 hitSpot = hit.point;
				int gridx = Mathf.RoundToInt(Mathf.FloorToInt((hitSpot.x - actionManager.gridManager.gridStart.x)) / actionManager.gridManager.gridPointSize);
				int gridy = Mathf.RoundToInt(Mathf.FloorToInt((hitSpot.z - actionManager.gridManager.gridStart.z)) / actionManager.gridManager.gridPointSize);

				List<GridMapPoint> path = actionManager.gridManager.StartPath.FindPath(thisEntity.GetGridPositionX(), thisEntity.GetGridPositionY(), gridx, gridy);

				if(path != null)
				{
					for(int i = 0; i < path.Count - 1; i++)
					{
						Debug.DrawLine(new Vector3(path[i].pos_x, path[i].pos_y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].pos_x, path[i + 1].pos_y), Color.red);
					}
				}

				Debug.Log("--------------");
				Debug.Log(thisEntity.GetGridPositionX() + " " + thisEntity.GetGridPositionY());

				Debug.Log("--------------");

				Debug.Log(gridx + " " + gridy);
				Debug.Log("--------------");

				actionManager.gridManager.UpdateGridPoint(gridx, gridy, actionManager.turnManager.EntitiesTurn);

				thisEntity.UpdatePosition(hit.transform);
				thisEntity.UpdateGridPosition(gridx, gridy);
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

	public void OnDrawGizmos()
	{
		Gizmos.DrawRay(ray);
	}
}