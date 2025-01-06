using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinding : MonoBehaviour 
{
	[SerializeField] GridMapPoint[,] gridPoints;

	private const int moveForward = 10;
	private const int moveDiagonal = 14;

	private List<GridMapPoint> openList;
	private List<GridMapPoint> closeList;

	GridManager gridManager;

	public static Pathfinding pathfinding { get; private set; }

	void Start()
	{
		gridManager = GetComponent<GridManager>();
	}

	public Pathfinding(GridMapPoint[,] gridPoints)
	{
		gridPoints = gridManager.gridPoints;
		pathfinding = this;
	}

	public List<GridMapPoint> FindPath(int startX, int startY, int endX, int endY)
	{
		GridMapPoint startNode = gridManager.GetPointUsingCoords(startX, startY);
		GridMapPoint endNode = gridManager.GetPointUsingCoords(endX, endY);

		openList = new List<GridMapPoint>() { startNode };
		closeList = new List<GridMapPoint>();

		for (int x = 0; x < gridManager.gridLength; x++)
		{
			for(int y = 0; y < gridManager.gridWidth; y++)
			{
				GridMapPoint point = gridManager.GetPointUsingCoords(x, y);
				point.gValue = int.MaxValue;
				point.UpdateFValue();
				point.previousNode = null;
			}
		}

		startNode.gValue = 0;
		startNode.hValue = CalculateDistance(startNode, endNode);

		while(openList.Count > 0)
		{
			GridMapPoint currentNode = GetLowestFCostNode(openList);
			if(currentNode == endNode)
			{
				return CalculatePath(endNode);
			}

			openList.Remove(currentNode);
			closeList.Add(currentNode);

			foreach (GridMapPoint neighboringNode in GetNeigbourList(currentNode))
			{
				if (closeList.Contains(neighboringNode)) continue;
				if(!neighboringNode.availablePoint)
				{
					closeList.Add(neighboringNode);
					continue;
				}

				int costToCheck = currentNode.gValue + CalculateDistance(currentNode, neighboringNode);

				if(costToCheck < neighboringNode.gValue)
				{
					neighboringNode.previousNode = currentNode;
					neighboringNode.gValue = costToCheck;
					neighboringNode.hValue = CalculateDistance(neighboringNode, endNode);
					neighboringNode.UpdateFValue();

					if(!openList.Contains(neighboringNode))
					{
						openList.Add(neighboringNode);
					}
				}
			}
		}

		return null;
	}

	private List<GridMapPoint> GetNeigbourList(GridMapPoint startNode)
	{
		List<GridMapPoint> neighbors = new List<GridMapPoint>();

		if(startNode.pos_x - 1 >= 0)
		{
			neighbors.Add(gridManager.GetPointUsingCoords(startNode.pos_x - 1, startNode.pos_y));

			if(startNode.pos_y - 1 >= 0) neighbors.Add(gridManager.GetPointUsingCoords(startNode.pos_x - 1, startNode.pos_y - 1));

			if(startNode.pos_y + 1 < gridManager.gridWidth) neighbors.Add(gridManager.GetPointUsingCoords(startNode.pos_x + 1, startNode.pos_y + 1));
		}
		if (startNode.pos_x + 1 < gridManager.gridLength)
		{
			neighbors.Add(gridManager.GetPointUsingCoords(startNode.pos_x + 1, startNode.pos_y));

			if (startNode.pos_y - 1 >= 0) neighbors.Add(gridManager.GetPointUsingCoords(startNode.pos_x + 1, startNode.pos_y - 1));

			if (startNode.pos_y + 1 < gridManager.gridLength) neighbors.Add(gridManager.GetPointUsingCoords(startNode.pos_x + 1, startNode.pos_y + 1));
		}
		if(startNode.pos_y - 1 >= 0) neighbors.Add(gridManager.GetPointUsingCoords(startNode.pos_x, startNode.pos_y - 1));

		if(startNode.pos_y + 1 < gridManager.gridWidth) neighbors.Add(gridManager.GetPointUsingCoords(startNode.pos_x, startNode.pos_y + 1));

		return neighbors;
	}

	private List<GridMapPoint> CalculatePath(GridMapPoint endNode)
	{
		List<GridMapPoint > path = new List<GridMapPoint>();
		path.Add(endNode);
		GridMapPoint currentNode = endNode;
		while (currentNode != null)
		{
			path.Add(currentNode);
			currentNode = currentNode.previousNode;
		}
		path.Reverse();

		return path;
	}

	private int CalculateDistance(GridMapPoint a, GridMapPoint b)
	{
		int xDistancce = Mathf.Abs(a.pos_x  - b.pos_x);
		int yDistance = Mathf.Abs(a.pos_y - b.pos_y);
		int remainding = Mathf.Abs(xDistancce - yDistance);
		return moveDiagonal * Mathf.Min(xDistancce, yDistance) + moveForward * remainding;
	}

	private GridMapPoint GetLowestFCostNode(List<GridMapPoint> list)
	{
		GridMapPoint lowestCost = list[0];
		for(int i = 1; i < list.Count; i++)
		{
			if (list[i].fValue < lowestCost.fValue)
			{
				lowestCost = list[i];
			}
		}
		return lowestCost;
	}

	internal List<Vector3> FindPath(Vector3 targetPoint1, Vector3 targetPoint2, List<Vector3> pathVectorList)
	{
		GridMapPoint A = gridManager.GetPointUsingCoords((int)targetPoint1.x, (int)targetPoint1.y);
		GridMapPoint B = gridManager.GetPointUsingCoords((int)targetPoint2.x, (int)targetPoint2.y);

		List<GridMapPoint> path = FindPath(A.pos_x, A.pos_y, B.pos_x, B.pos_y);

		if(path == null){
			return null;
		} else
		{
			List<Vector3> result = new List<Vector3>();
			foreach (GridMapPoint p in path)
			{
				pathVectorList.Add(new Vector3(p.pos_x, p.pos_y) * gridManager.gridPointSize + Vector3.one * gridManager.gridPointSize * 0.5f);
			}
			return pathVectorList;
		}
	}

}