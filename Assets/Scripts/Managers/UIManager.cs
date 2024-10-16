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
	[SerializeField] GameObject enemyTargetDisplay;
	[SerializeField] GameObject playerTargetDisplay;


	[SerializeField] TextMeshProUGUI StateMachine;
    [SerializeField] TextMeshProUGUI EntityTurn;
    [SerializeField] Button AbilityButton;
    [SerializeField] Button EntityButton;
    [SerializeField] List<Button> MoveListClick;

    //[SerializeField] Scrollbar[] healthBars;
    [SerializeField] TextMeshProUGUI playerHealth;
    [SerializeField] TextMeshProUGUI enemyHealth;

    private bool movesCreated = false;
    private bool playerGenerated = false;
    private bool enemyGenerated = false;

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
        if(!movesCreated)
        {
		    for (int i = 0; i < playerTurn.GetAbilities().Count; i++)
		    {
                movesCreated = true;
			    AbilityButton.GetComponent<AbilityButton>().UpdateAbility(playerTurn.GetAbilities()[i]);
			    AbilityButton.GetComponentInChildren<TextMeshProUGUI>().text = playerTurn.GetAbilities()[i].name;
			    MoveListClick.Add(AbilityButton);
			    Instantiate(AbilityButton.gameObject, MoveListDisplay.transform);
		    }
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

	internal void LetPlayerTarget(AbilityButton button, EntitySO[] enemies, EntitySO[] players)
	{
        
        if(button.GetAbility().AbilityEffectType == AbilityEffectType.Damage)
        {
            enemyTargetDisplay.SetActive(true);
            playerTargetDisplay.SetActive(false);
            if(!enemyGenerated)
            {
                enemyGenerated = true;
		        for (int i = 0; i < enemies.Length; i++)
		        {
                    EntityButton.GetComponent<EntityButton>().UpdateEntity(enemies[i]);
                    EntityButton.GetComponentInChildren<TextMeshProUGUI>().text = enemies[i].name;
			        Instantiate(EntityButton.gameObject, enemyTargetDisplay.transform);
		        }
            }
        }
        else if(button.GetAbility().AbilityEffectType == AbilityEffectType.Health)
        {
            playerTargetDisplay.SetActive(true);
            enemyTargetDisplay.SetActive(false);
            if(!playerGenerated)
            {
                playerGenerated = true;
                if(button.GetAbility().target == Targeting.Self)
                {
				    for (int i = 0; i < enemies.Length; i++)
				    {
					    EntityButton.GetComponent<EntityButton>().UpdateEntity(players[i]);
					    EntityButton.GetComponentInChildren<TextMeshProUGUI>().text = players[i].name;
					    Instantiate(EntityButton.gameObject, playerTargetDisplay.transform);
				    }
			    }
            }
        }
	}

	internal void UpdateHealth(EntitySO entity)
	{
		if(entity.isPlayer)
        {
            playerHealth.text = entity.GetHealth().ToString();
        }
        else
        {
            enemyHealth.text = entity.GetHealth().ToString();
        }
	}
}
