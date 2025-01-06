using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GridPoint", menuName = "ScriptableObject/GridPoint")]

public class GridSO : IScriptableObject
{
	[SerializeField] public CombatEntity entityHere;
	[SerializeField] bool obstructionHere = false;
	[SerializeField] public bool IsEntityHere = false;

	[SerializeField] GameObject GameObject;

	[SerializeField] int[,] gridPosition;
	[SerializeField] int gridX;
	[SerializeField] int gridY;


	void Start()
	{
	}

	public void UpdatePosition(int x, int y)
	{
		int[,] newPoint = new int[x,y]; 

		this.gridPosition = newPoint;

		gridX = x;
		gridY = y;
	}

	public void UpdateEntitySO(CombatEntity? SO)
	{
		entityHere = SO;
		obstructionHere = true;
		IsEntityHere = true;
	}
}
