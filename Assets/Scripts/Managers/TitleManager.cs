using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
	GameManager gameManager;

	[SerializeField] public TextMeshProUGUI TitleText;
	[SerializeField] public Button StartBTN;
	[SerializeField] public Button QuitBTN;
	[SerializeField] public Canvas TitleUI;

	private void Start()
	{
		gameManager = GetComponent<GameManager>();
	}

	public void MoveToWorld()
	{
		gameManager.gameState = GameState.World;
		TitleUI.gameObject.SetActive(false);
		//TitleUI.GetComponent<TMP_EditorPanelUI>
	}
}