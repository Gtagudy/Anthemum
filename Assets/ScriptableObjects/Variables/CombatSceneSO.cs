using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatScene", menuName = "ScriptableObject/CombatScene")]
public class CombatSceneSO : ScriptableObject
{
	[SerializeField] public CombatEntity[] Players;
	[SerializeField] public CombatEntity[] Enemies;

	[SerializeField] int rows = 0;
	[SerializeField] int columns = 0;

	[SerializeField] public int[,] grid;

	public int turnCount;

	public CombatEntity[] GetPlayers()
	{
		return Players;
	}

	public CombatEntity[] GetEnemies()
	{
		return Enemies;
	}

	public int[,] GetGrid()
	{
		grid = new int[rows, columns];
		return grid;
	}
}