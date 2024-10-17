using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] Button ButtonExample;
    [SerializeField] List<Button> MoveListClick;

    //[SerializeField] Scrollbar[] healthBars;
    [SerializeField] TextMeshProUGUI playerHealth;
    [SerializeField] TextMeshProUGUI enemyHealth;

	internal void CreateHealthBars(EntitySO entity)
	{
        if(entity != null)
        {
            if(entity.isPlayer)
            {
                playerHealth.text = entity.GetHealth().ToString();
            } else
            {
                enemyHealth.text = entity.GetHealth().ToString();
            }
        }
        /*entity.GetTransform();
        Instantiate(healthBars[0]);
        healthBars[]*/
	}

	public void DisplayMoves(EntitySO dequeue)
	{

        MoveListDisplay.SetActive(true);
        CommandPanel.SetActive(false);
     
		/*MoveListDisplay.SetActive(false);
		CommandPanel.SetActive(true);*/
	}

    public void HideMoves()
    {
        MoveListDisplay.SetActive(false);
        CommandPanel.SetActive(true);
    }

	internal void WhoseTurn(EntitySO playerTurn)
	{
        EntityTurn.text = playerTurn.entityName;
        if(playerTurn.isPlayer)
        {
            CreateMoves(playerTurn);
        }
	}

	private void CreateMoves(EntitySO playerTurn)
	{
		for (int i = 0; i < playerTurn.GetAbilities().Count; i++)
		{
			ButtonExample.GetComponent<AbilityButton>().UpdateAbility(playerTurn.GetAbilities()[i]);
			ButtonExample.GetComponentInChildren<TextMeshProUGUI>().text = playerTurn.GetAbilities()[i].name;
			MoveListClick.Add(ButtonExample);
			float offset = i * 0.2f;
			Vector2 position = MoveListDisplay.transform.position + transform.right * offset;
			Instantiate(ButtonExample.gameObject, MoveListDisplay.transform);
		}
	}

	void Awake()
    {
        //MoveListDisplay.RegisterListener(gameEventListener);
        //displayMoves.AddListener(DisplayMoves());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
