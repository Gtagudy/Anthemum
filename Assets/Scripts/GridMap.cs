using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GridMap : MonoBehaviour
{
	[SerializeField] GameObject gridSO;
	internal void CreateGrid(int x, int y)
	{
		for (int i = 0; i < x; i++)
		{
			for(int j = 0; j < y; j++)
			{
				Instantiate(gridSO, gridSO.transform);
			}
		}
	}
}