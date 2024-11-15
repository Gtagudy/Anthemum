using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CombatEntity : MonoBehaviour
{
    public IntGameEvent healthChange;

    [SerializeField] EntitySO entity;
	
    [SerializeField] Slider Health;
    
    [SerializeField] GameObject myTurn;
	[SerializeField] GameObject MoveListDisplay;
	[SerializeField] GameObject enemyTargetDisplay;
	[SerializeField] GameObject playerTargetDisplay;
	private bool isMyTurn = false;
    public bool movesCreated = false;

    public bool hasMoved = false;

    public bool isPlayer = false;

    [SerializeField] int x;
    [SerializeField] int y;
	[SerializeField] public float movementPoints;

	// Start is called before the first frame update
	void Awake()
    {
        isPlayer = entity.isPlayer;
	}

    // Update is called once per frame
    void Update()
    {
        if(isMyTurn)
        {
            myTurn.SetActive(true);
        }
        else
        {
            myTurn.SetActive(false);
        }
    }

    internal Slider GetHealthBar()
    {
        return Health;
    }
    internal GameObject GetMovesDisplay()
    {
        return MoveListDisplay;
    }
	internal GameObject GetEnemyDisplay()
	{
		return enemyTargetDisplay;
	}
	internal GameObject GetPlayerDisplay()
	{
		return playerTargetDisplay;
	}
	public EntitySO GetEntitySO()
    {
        return entity;
    }

	internal int GetGridPositionX()
	{
        return x;
	}
    internal int GetGridPositionY()
	{
        return y;
	}

	internal void UpdatePosition(Transform transform)
	{
		this.transform.position = transform.position + (transform.up / 2);
	}
    internal void UpdateGridPosition()
    {

    }
}
