using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField] GridMapPoint[,] gridPoints;

    [SerializeField] public GameObject gridMap;
    [SerializeField] public GameObject gridParent;

    [SerializeField] GameObject gridPoint;

    [SerializeField] int offset;

    int gridLength;

    int gridWidth;
	[SerializeField] float gridPointSize = 1.0f;

	GridManager gridManager;

	private void Awake()
	{
		gridManager = GetComponent<GridManager>();
	}


	internal void CreateGridMap(int[,] grid)
	{
        gridLength = grid.GetLength(0);
        gridWidth = grid.GetLength(1);

        gridPoints = new GridMapPoint[gridLength, gridWidth];

        for (int i = 0; i < gridLength; i++)
        {
            for(int j = 0; j < gridWidth; j++)
            {
				Vector3 pos = new Vector3(i * gridPointSize, 0, j * gridPointSize);


				GameObject newGridSpot = Instantiate(gridMap, gridParent.transform.position + pos, Quaternion.identity);
				newGridSpot.name = $"X: {i}, Y: {j}";
                gridPoints[i, j] = newGridSpot.AddComponent<GridMapPoint>();
                gridPoints[i, j].GetComponent<GridMapPoint>().name = $"X: {i}, Y: {j}";
                gridPoints[i, j].AssignGridSO();

				gridPoints[i, j].UpdatePosition(i, j);

                /*

                newGridSpot.transform.position = new Vector3(i, 0 + offset, j);*/
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
			
			filledMapPoint.UpdateEntitySO(players[i].GetEntitySO());
			players[i].UpdatePosition(filledMapPoint.GetComponent<Transform>().transform);
		}
		for (int i = 0; i < enemies.Length; i++)
		{
			GridMapPoint filledMapPoint = UseEntityCoords(enemies, i);

			filledMapPoint.UpdateEntitySO(enemies[i].GetEntitySO());
			enemies[i].UpdatePosition(filledMapPoint.GetComponent<Transform>().transform);
		}
	}

	private GridMapPoint UseEntityCoords(CombatEntity[] entities, int i)
	{
		return gridPoints[(int)entities[i].GetGridPositionX(),
						(int)entities[i].GetGridPositionY()];
	}
}
