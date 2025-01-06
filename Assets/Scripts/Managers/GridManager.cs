using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField] public GridMapPoint[,] gridPoints;

    [SerializeField] public GameObject gridMap;
    [SerializeField] public GameObject gridParent;

    [SerializeField] GameObject gridPoint;

    [SerializeField] int offset;
	[SerializeField] public float gridPointSize = 1.0f;
	
	public Pathfinding StartPath;

    public int gridLength;
    public int gridWidth;

	public Vector3 gridStart;

	public TurnManager turnManager;

	CombatEntity combatEntity;
	Camera camera;

	public GridManager(int length, int width)
	{
		gridLength = length;
		gridWidth = width;

		gridPoints = new GridMapPoint[length, width];
	}

	private void Awake()
	{

	}
	internal void CreateGridMap(int[,] grid)
	{

		gridLength = grid.GetLength(0);
		gridWidth = grid.GetLength(1);

		gridPoints = new GridMapPoint[gridLength, gridWidth];

		for (int i = 0; i < gridLength; i++)
		{
			for (int j = 0; j < gridWidth; j++)
			{
				Vector3 pos = new Vector3(i * gridPointSize, 0, j * gridPointSize);


				GameObject newGridSpot = Instantiate(gridMap, gridParent.transform.position + pos, Quaternion.identity, gridParent.transform);
				newGridSpot.name = $"X: {i}, Y: {j}";
				gridPoints[i, j] = newGridSpot.AddComponent<GridMapPoint>();
				gridPoints[i, j].GetComponent<GridMapPoint>().name = $"X: {i}, Y: {j}";
				gridPoints[i, j].AssignGridSO();

				gridPoints[i, j].UpdatePosition(i, j);

				/*

                newGridSpot.transform.position = new Vector3(i, 0 + offset, j);*/
			}
		}
		for (int i = 0; i < gridLength - 1; i++)
		{
			for (int j = 0; j < gridWidth - 1; j++)
			{
				GridMapPoint thisGrid = gridPoints[i, j];
				if (thisGrid != null)
				{
					if(i < gridLength)
						thisGrid.Up = gridPoints[i + 1, j];

					if (i < gridLength && j < gridWidth)
						thisGrid.UpRight = gridPoints[i + 1, j + 1];

					if (j < gridWidth)
						thisGrid.Right = gridPoints[i, j + 1];

					if (i > 0 && j < gridWidth)
						thisGrid.DownRight = gridPoints[i - 1, j + 1];

					if (i > 0)
						thisGrid.Down = gridPoints[i - 1, j];

					if (i > 0 && j > 0)
						thisGrid.DownLeft = gridPoints[i - 1, j - 1];

					if (j > 0)
						thisGrid.Left = gridPoints[i, j - 1];

					if (j > 0 && i < gridLength)
						thisGrid.UpLeft = gridPoints[i + 1, j - 1];
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (gridPoints == null)
		{
			for (int y = 0; y < gridLength; y++)
			{
				for (int x = 0; x < gridWidth; x++)
				{
					Vector3 pos = GetWorldPosition(x, y);
					Gizmos.DrawCube(pos, Vector3.one / 4);
				}
			}

		}
		else
		{
			for (int y = 0; y < gridWidth; y++)
			{
				for (int x = 0; x < gridLength; x++)
				{
					Vector3 pos = GetWorldPosition(x, y, true);
					Gizmos.color = gridPoints[x, y].availablePoint ? Color.white : Color.red;
					Gizmos.DrawCube(pos, Vector3.one / 4);
				}
			}
		}
	}
	public Vector3 GetWorldPosition(int x, int y, bool elevation = false)
	{
		return new Vector3(transform.position.x + (x * gridPointSize),
			elevation == true ? gridPoints[x, y].elevation : 0f,
			transform.position.z + (y * gridPointSize));
	}

	// Start is called before the first frame update
	void Start()
    {
        gridStart = gridParent.transform.position;
		StartPath = GetComponent<Pathfinding>();
		turnManager = GetComponent<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
		
	}
	internal void SetEntitiesToGrid(CombatEntity[] players, CombatEntity[] enemies)
	{
		for(int i = 0; i < players.Length; i++)
		{
			GridMapPoint filledMapPoint = UseEntityCoords(players, i);
			
			filledMapPoint.UpdateEntitySO(players[i]);
			players[i].UpdatePosition(filledMapPoint.GetComponent<Transform>().transform);
		}
		for (int i = 0; i < enemies.Length; i++)
		{
			GridMapPoint filledMapPoint = UseEntityCoords(enemies, i);

			filledMapPoint.UpdateEntitySO(enemies[i]);
			enemies[i].UpdatePosition(filledMapPoint.GetComponent<Transform>().transform);
		}
	}

	private GridMapPoint UseEntityCoords(CombatEntity[] entities, int i)
	{
		return gridPoints[(int)entities[i].GetGridPositionX(),
						(int)entities[i].GetGridPositionY()];
	}

	public GridMapPoint GetPointUsingCoords(int x, int y)
	{
		return gridPoints[x,y];
	}

	public GridMapPoint GetCurrentEntityCoords(CombatEntity entity)
	{
		return gridPoints[entity.GetGridPositionX(), entity.GetGridPositionY()];
	}

	internal void MoveEntity(CombatEntity combatEntity, bool isMoving, Camera camera)
	{
		this.combatEntity = combatEntity;
		this.camera = camera;
	}

	public void UpdateGridPoint(int x, int y, CombatEntity combatEntity)
	{

		gridPoints[combatEntity.GetGridPositionX(), combatEntity.GetGridPositionY()].gridSO.UpdateEntitySO(null);

		gridPoints[x, y].gridSO.UpdateEntitySO(combatEntity);
	}
}
