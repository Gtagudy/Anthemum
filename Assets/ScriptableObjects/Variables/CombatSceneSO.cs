using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatScene", menuName = "ScriptableObject/CombatScene")]
public class CombatSceneSO : ScriptableObject
{
	[SerializeField] public GameObject[] Players;
	[SerializeField] public GameObject[] Enemies;

	[SerializeField] int rows = 0;
	[SerializeField] int columns = 0;

	[SerializeField] public int[,] grid;

	public int turnCount;
	
	[Header("Scene Objects")]
	public GridObjectSpawn[] objectsToSpawn;

	[System.Serializable]
	public struct GridObjectSpawn {
		public GridObjectSO so;
		public int x, y;
	}
	
	
	public GameObject[] GetPlayers()
	{
		return Players;
	}

	public GameObject[] GetEnemies()
	{
		return Enemies;
	}

	public int[,] GetGrid()
	{
		grid = new int[rows, columns];
		return grid;
	}
}