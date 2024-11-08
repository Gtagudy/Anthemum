using System;
using TMPro;
using UnityEngine;

public class GridMapPoint : MonoBehaviour
{
	[SerializeField] public GridSO gridSO;
	internal bool availablePoint;
	internal float elevation;
	private TextMeshProUGUI textMeshProUGUI;
	int debugRotate = 1;

	private void OnMouseDown()
	{
		GetComponent<Transform>().rotation = Quaternion.Euler(0, debugRotate, debugRotate);
		debugRotate++;
	}
	private void Awake()
	{

		//this.textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
	}

	internal void UpdatePosition(int i, int j)
	{
		gridSO.UpdatePosition(i, j);
		//textMeshProUGUI = Instantiate(textMeshProUGUI);
	}

	private GridSO GetGridSO()
	{
		return gridSO;
	}

	internal void AssignGridSO()
	{
		gridSO = ScriptableObject.CreateInstance<GridSO>();

	}
	public void UpdateEntitySO(EntitySO SO)
	{
		gridSO.UpdateEntitySO(SO);
	}
}