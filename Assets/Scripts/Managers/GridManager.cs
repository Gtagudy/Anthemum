using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField] public GridMapPoint[,] gridPoints;

    //[SerializeField] public GameObject gridMap;
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
		turnManager = GetComponent<TurnManager>();
	}
	internal void CreateGridMap(int[,] grid)
	{

		gridLength = grid.GetLength(0);
		gridWidth  = grid.GetLength(1);
		gridPoints = new GridMapPoint[gridLength, gridWidth];

		for (int i = 0; i < gridLength; i++)
		for (int j = 0; j < gridWidth;  j++)
		{
			// 1) Compute the exact WORLD‐space center of this cell
			Vector3 worldPos = gridParent.transform.position
			                   + new Vector3(
				                   i * gridPointSize + gridPointSize * 0.5f,
				                   0,
				                   j * gridPointSize + gridPointSize * 0.5f
			                   );

			// 2) Instantiate at that worldPos (no need to adjust afterwards)
			GameObject go = Instantiate(
				gridPoint,    // your tile prefab
				worldPos,     // exact world center
				Quaternion.identity,
				gridParent.transform    // parent container
			);

			go.transform.localScale = Vector3.one;  // ensure scale=1

			// 3) Add & configure your GridMapPoint
			var mp = go.GetComponent<GridMapPoint>();
			mp.name           = $"[{i},{j}]";
			mp.AssignGridSO();
			mp.availablePoint = true;
			mp.UpdatePosition(i, j);

			gridPoints[i, j] = mp;
		}
		for (int i = 0; i < gridLength; i++)
		{
			for (int j = 0; j < gridWidth; j++)
			{
				var cell = gridPoints[i, j];

				// Cardinal neighbors
				cell.Up    = (i + 1 < gridLength ) ? gridPoints[i + 1, j] : null;
				cell.Down  = (i - 1 >= 0           ) ? gridPoints[i - 1, j] : null;
				cell.Right = (j + 1 < gridWidth  ) ? gridPoints[i, j + 1] : null;
				cell.Left  = (j - 1 >= 0           ) ? gridPoints[i, j - 1] : null;
			}
		}
	}
	public IEnumerable<GridMapPoint> GetCellsInShape(
		GridMapPoint origin, 
		Targeting type,
		int range,
		Vector2Int size
	) {
		var results = new List<GridMapPoint>();
		int ox = origin.pos_x, oy = origin.pos_y;

		switch(type)
		{
			case Targeting.Self:
				results.Add(origin);
				break;

			case Targeting.Single:
				// all within range, but you’ll filter entity vs empty later
				for (int i = ox - range; i <= ox + range; i++)
				for (int j = oy - range; j <= oy + range; j++)
					if (InBounds(i, j))
						results.Add(gridPoints[i,j]);
				break;

			case Targeting.AOE:
				// a rectangle of size.x × size.y centered on target cell
				int halfW = size.x/2, halfH = size.y/2;
				for (int i = ox - halfW; i <= ox + halfW; i++)
				for (int j = oy - halfH; j <= oy + halfH; j++)
					if (InBounds(i, j) &&
					    Mathf.Abs(i-ox) + Mathf.Abs(j-oy) <= range) // optional radius cap
						results.Add(gridPoints[i,j]);
				break;

			case Targeting.Line:
				// a straight line in the entity’s facing (you’ll need a facing dir)
				// for now, just a horizontal line both ways:
				for (int i = ox - range; i <= ox + range; i++)
					if (InBounds(i, oy)) results.Add(gridPoints[i,oy]);
				break;

			case Targeting.Cone:
				// simple 90° cone in +Y direction
				for (int d = 1; d <= range; d++)
				for (int w = -d; w <= d; w++)
					if (InBounds(ox + w, oy + d))
						results.Add(gridPoints[ox + w, oy + d]);
				break;
		}
		return results;
	}

	public bool InBounds(int i, int j)
		=> i >= 0 && i < gridLength
		          && j >= 0 && j < gridWidth;
	private void OnDrawGizmos() {
		if (gridPoints == null) return;
		for (int x = 0; x < gridLength; x++) {
			for (int y = 0; y < gridWidth; y++) {
				var cell = gridPoints[x,y];
				Vector3 c = GetWorldPosition(x,y, true);
				Gizmos.color = cell.availablePoint ? Color.white : Color.red;
				Gizmos.DrawWireCube(c, Vector3.one * (gridPointSize * 0.9f));

				// Neighborhood stuff
				/*Gizmos.color = Color.cyan;
				foreach (var n in new[]{cell.Up, cell.UpRight, cell.Right, cell.DownRight,
					         cell.Down, cell.DownLeft, cell.Left, cell.UpLeft})
					if (n != null)
						Gizmos.DrawLine(c, GetWorldPosition(n.pos_x, n.pos_y, true));*/
			}
		}
	}
	public Vector3 GetWorldPosition(int x, int y, bool elevation = false)
	{
		float yOff = elevation ? gridPoints[x,y].elevation : 0f;
        // add half-cell to center
        return new Vector3(
            gridStart.x + x * gridPointSize + gridPointSize * 0.5f,
            yOff,
            gridStart.z + y * gridPointSize + gridPointSize * 0.5f
        );
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
	internal void SetEntitiesToGrid(List<CombatEntity> players, List<CombatEntity> enemies)
	{
		for(int i = 0; i < players.Count; i++)
		{
			GridMapPoint filledMapPoint = UseEntityCoords(players, i);
			
			//GameObject makePlayer = Instantiate(players[i].gameObject, gridParent.transform.position + new Vector3(filledMapPoint.pos_x, 0, filledMapPoint.pos_y), Quaternion.identity, gridParent.transform);
			
			filledMapPoint.UpdateEntitySO(players[i]);
			players[i].UpdatePosition(filledMapPoint.GetComponent<Transform>().transform);
			//players[i].UpdatePosition();
			players[i].transform.SetParent(turnManager.combatManger.combatRoot.transform);
			filledMapPoint.availablePoint = false;
		}
		for (int i = 0; i < enemies.Count; i++)
		{
			GridMapPoint filledMapPoint = UseEntityCoords(enemies, i);

			filledMapPoint.UpdateEntitySO(enemies[i]);
			enemies[i].UpdatePosition(filledMapPoint.GetComponent<Transform>().transform);
			players[i].transform.SetParent(turnManager.combatManger.combatRoot.transform);
		
			filledMapPoint.availablePoint = false;

		}
	}

	private GridMapPoint UseEntityCoords(List<CombatEntity> entities, int i)
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
		//this.camera = camera;
	}

	public void UpdateGridPoint(int x, int y, CombatEntity combatEntity)
	{
		gridPoints[combatEntity.GetGridPositionX(), combatEntity.GetGridPositionY()].gridSO.UpdateEntitySO(null);
		gridPoints[combatEntity.GetGridPositionX(), combatEntity.GetGridPositionY()].availablePoint = true;

		gridPoints[x, y].gridSO.UpdateEntitySO(combatEntity);
		gridPoints[x, y].availablePoint = false;
	}
	
	public void SpawnSceneObjects(CombatSceneSO.GridObjectSpawn[] spawns)
	{
		foreach (var s in spawns)
		{
			GridMapPoint cell = gridPoints[s.x, s.y];
			if (cell == null) continue;

			// 1) Instantiate the prefab at the tile’s position
			var go = Instantiate(
				s.so.prefab,
				cell.transform.position,
				Quaternion.identity,
				gridParent.transform
			);

			
			// 2) Grab the existing component instead of adding it
			var obj = go.GetComponent<GridObject>();
			if (obj == null)
			{
				Debug.LogError($"Prefab {s.so.prefab.name} is missing GridObject!");
				continue;
			}

			// 3) Assign its data and link it into the grid
			obj.data           = s.so;
			cell.gridObject    = obj;
			cell.availablePoint = false;
		}
	}


	internal void DestroyGrid()
	{

		for (int i = 0; i < gridLength - 1; i++)
		{
			for (int j = 0; j < gridWidth - 1; j++)
			{
				GridMapPoint thisGrid = gridPoints[i, j];
				if (thisGrid != null)
				{
					if (i < gridLength)
						thisGrid.Up = null; ;

					if (i < gridLength && j < gridWidth)
						thisGrid.UpRight = null;

					if (j < gridWidth)
						thisGrid.Right = null;

					if (i > 0 && j < gridWidth)
						thisGrid.DownRight = null;

					if (i > 0)
						thisGrid.Down = null;

					if (i > 0 && j > 0)
						thisGrid.DownLeft = null;

					if (j > 0)
						thisGrid.Left = null;

					if (j > 0 && i < gridLength)
						thisGrid.UpLeft = null;
				}
			}
		}
		for (int i = 0; i < gridLength; i++)
		{
			for (int j = 0; j < gridWidth; j++)
			{
				gridPoints[i, j].GetComponent<GridMapPoint>().gridSO = null;
				Destroy(gridPoints[i, j].GetComponent<GridMapPoint>().gridSO);

				Destroy(gridPoints[i, j].gameObject);
			}
		}

		Array.Clear(gridPoints, 0, gridPoints.Length);
	}
	
}
