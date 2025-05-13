using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToFight : MonoBehaviour
{
    GameManager gameManager;

	[SerializeField] CombatSceneSO gameScene;

	private void Start()
	{
		gameManager = FindFirstObjectByType<GameManager>();
	}

	public void LeadToCombatStart(CombatSceneSO game)
	{
		gameManager.StartCombat(game);
	}
}
