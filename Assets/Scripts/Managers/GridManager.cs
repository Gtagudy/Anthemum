using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    [SerializeField] public GridMap gridMap;


	internal void CreateGridMap(Tuple<int, int> grid)
	{
        int x = grid.Item1;
        int y = grid.Item2;
        gridMap.CreateGrid(x, y);
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
