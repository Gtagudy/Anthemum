using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class MoveDisplayer : MonoBehaviour
{
	public UnityEvent displayMoves;

	private bool isShowing = false;

	private void Start()
	{
		//displayMoves.AddListener(GameObject.FindAnyObjectByType<UIManager>().GetComponent<UIManager>().DisplayMoves());
	}
	private void OnClick(GameObject go)
	{
		displayMoves.Invoke();
	}
}