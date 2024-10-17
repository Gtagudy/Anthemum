using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public IntGameEvent changeHealth;
	public UnityAction<int> intReact;
    public GameEventListener gameEventListener;

    public UnityEvent displayMoves;

    

	/*
    The UI Manage is a manager made along with the GameManager. The UI will even begin at Title,
    working throughout the game in both the World and the Combat
     */

	[SerializeField] GameObject GamePanel;
    [SerializeField] GameObject CommandPanel;
    [SerializeField] GameObject MoveListDisplay;

    [SerializeField] TextMeshProUGUI StateMachine;
    [SerializeField] TextMeshProUGUI EntityTurn;
    [SerializeField] Button MoveExample;
    [SerializeField] List<Button> MoveListClick;

    [SerializeField] Scrollbar[] healthBars;

	internal void CreateHealthBars(EntitySO entity)
	{
        /*entity.GetTransform();
        Instantiate(healthBars[0]);
        healthBars[]*/
	}

	internal void DisplayMoves(EntitySO dequeue)
	{
        for(int i = 0; i < dequeue.GetAbilities().Count; i++)
        {
            MoveListClick[i].GetComponentInChildren<TextMeshProUGUI>().text = dequeue.GetAbilities()[i].name;
        }
	}

	internal void WhoseTurn(EntitySO playerTurn)
	{
        EntityTurn.text = playerTurn.entityName;
	}

	void Awake()
    {
        MoveListDisplay.RegisterListener(gameEventListener);
        //displayMoves.AddListener(DisplayMoves());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
