using System;
using TMPro;
using UnityEngine;

public class GridMapPoint : MonoBehaviour
{
	public int pos_x;
	public int pos_y;

	public int gValue;
	public int hValue;
	public int fValue;

	public GridMapPoint previousNode;
	[SerializeField] public GridSO gridSO;
	public bool availablePoint;
	internal float elevation;
	private TextMeshProUGUI textMeshProUGUI;
	int debugRotate = 1;

	[SerializeField] public GameObject Targeting;
	[SerializeField] public GameObject Dangerous;

	[SerializeField] public GridMapPoint Up;
	[SerializeField] public GridMapPoint UpRight;
	[SerializeField] public GridMapPoint Right;
	[SerializeField] public GridMapPoint DownRight;
	[SerializeField] public GridMapPoint Down;
	[SerializeField] public GridMapPoint DownLeft;
	[SerializeField] public GridMapPoint Left;
	[SerializeField] public GridMapPoint UpLeft;
	public void UpdateFValue()
	{
		fValue = gValue + hValue;
	}

	public GridMapPoint(int xPos, int yPos)
	{
		pos_x = xPos;
		pos_y = yPos;
		availablePoint = true;
	}
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
		pos_x = i;
		pos_y = j;
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
	public void UpdateEntitySO(CombatEntity SO)
	{
		gridSO.UpdateEntitySO(SO);
	}
}