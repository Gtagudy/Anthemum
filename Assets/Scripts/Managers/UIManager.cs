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
    TurnManager turnManager;
    public IntGameEvent changeHealth;
	public UnityAction<int> intReact;
    public GameEventListener gameEventListener;

    public UnityEvent displayMoves;

    IntGameEvent updateHealth;
    IntListener listenForHealth;
	/*
    The UI Manage is a manager made along with the GameManager. The UI will even begin at Title,
    working throughout the game in both the World and the Combat
     */

	[SerializeField] GameObject GamePanel;
    [SerializeField] GameObject CommandPanel;
    [SerializeField] GameObject MoveListDisplay;
	[SerializeField] GameObject enemyTargetDisplay;
	[SerializeField] GameObject playerTargetDisplay;
    [SerializeField] Button EndTurn;


	[SerializeField] public TextMeshProUGUI TurnStateMachine;
	[SerializeField] public TextMeshProUGUI ActionStateMachine;
    [SerializeField] TextMeshProUGUI EntityTurn;
    [SerializeField] Button AbilityButton;
    [SerializeField] Button EntityButton;
    [SerializeField] List<Button> MoveListClick;

    //[SerializeField] Scrollbar[] healthBars;
    [SerializeField] TextMeshProUGUI playerHealth;
    [SerializeField] TextMeshProUGUI enemyHealth;

    CombatEntity combatEntity;
    private bool movesCreated = false;
    private bool playerGenerated = false;
    private bool enemyGenerated = false;

	internal void CreateHealthBars(CombatEntity entity)
	{
        entity.GetHealthBar().maxValue = entity.entity.GetMaxHealth();
        entity.GetHealthBar().value = entity.entity.GetHealth();
        //updateHealth.RegisterListener(listenForHealth);
	}
	public void DisplayMoves()
	{
		
        //dequeue.GetMovesDisplay().SetActive(true);
        //CommandPanel.SetActive(false);

        //dequeue.get
     
		/*MoveListDisplay.SetActive(false);
		CommandPanel.SetActive(true);*/
	}

    public void HideMoves()
    {
		combatEntity = turnManager.GetCombatEntity();

		combatEntity.GetMovesDisplay().SetActive(true);
		//MoveListDisplay.SetActive(false);
		//CommandPanel.SetActive(true);
	}

	internal void WhoseTurn(CombatEntity playerTurn)
	{
        //EntityTurn.text = playerTurn.GetEntitySO().entityName;
        if(playerTurn.entity.isPlayer)
        {
            CreateMoves(playerTurn);
        }
	}
	private void CreateMoves(CombatEntity playerTurn)
	{
        for (int i = 0; i < playerTurn.entity.GetAbilities().Count; i++)
        {
            if(!playerTurn.movesCreated)
            {
                movesCreated = true;
                AbilityButton.GetComponent<AbilityButton>().UpdateAbility(playerTurn.entity.GetAbilities()[i]);
                AbilityButton.GetComponentInChildren<TextMeshProUGUI>().text = playerTurn.entity.GetAbilities()[i].name;
                MoveListClick.Add(AbilityButton);
                Instantiate(AbilityButton.gameObject, playerTurn.GetMovesDisplay().transform);

				/*
                 *  Button button = Instantiate(ButtonWithID, playerTurn.GetMovesDisplay().transform);
                GeneralSelectionButton GSB = button.GetComponent<GeneralSelectionButton>();

                button.name = sO.name;
                GSB.buttonID = sO.name;
                button.GetComponentInChildren<TextMeshProUGUI>().text = sO.name;
                movesCreated = true;
                MoveListClick.Add(ButtonWithID);

                string buttonName = button.name;
                Debug.Log(buttonName);
                button.onClick.AddListener(() =>
                {
                    actionManager.OnButtonPressed();
                });
                 */
			}
		}
        playerTurn.movesCreated = true;
	}

	void Awake()
    {
        //MoveListDisplay.RegisterListener(gameEventListener);
        //displayMoves.AddListener(DisplayMoves());
        turnManager = GetComponent<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	internal void LetPlayerTarget(AbilityButton button, CombatEntity[] enemies, CombatEntity[] players)
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

	internal void UpdateHealth(CombatEntity entity)
	{
        //updateHealth.Raise();
		entity.GetHealthBar().value = entity.entity.GetHealth();
	}

	internal void PlayTitle(TextMeshProUGUI titleText, Button start, Button quit)
	{
        titleText.CrossFadeAlpha(100, 5, true);
	}

	internal void LetPlayerTarget(AbilityButton button, GridMapPoint occupiedSpace)
	{
		if (button.GetAbility().AbilityEffectType == AbilityEffectType.Damage)
		{
			enemyTargetDisplay.SetActive(true);
			playerTargetDisplay.SetActive(false);
			if (!enemyGenerated)
			{
				enemyGenerated = true;
				EntityButton.GetComponent<EntityButton>().UpdateEntity(occupiedSpace.gridSO.entityHere);
				EntityButton.GetComponentInChildren<TextMeshProUGUI>().text = occupiedSpace.gridSO.entityName;
				Instantiate(EntityButton.gameObject, enemyTargetDisplay.transform);
			}
		}
		else if (button.GetAbility().AbilityEffectType == AbilityEffectType.Health)
		{
			playerTargetDisplay.SetActive(true);
			enemyTargetDisplay.SetActive(false);
			if (!playerGenerated)
			{
				playerGenerated = true;
				if (button.GetAbility().target == Targeting.Self)
				{
					EntityButton.GetComponent<EntityButton>().UpdateEntity(occupiedSpace.gridSO.entityHere);
					EntityButton.GetComponentInChildren<TextMeshProUGUI>().text = occupiedSpace.gridSO.entityName;
					Instantiate(EntityButton.gameObject, playerTargetDisplay.transform);
				}
			}
		}
	}
}
