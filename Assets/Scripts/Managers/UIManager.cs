using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject entireUI;
    [SerializeField] bool entireUIOn = false;
    [Header("Managers")]
    TurnManager turnManager;
    [Header("Listeners/Events")]
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

    public Queue turnOrderUI = new Queue();
   // public Queue historyUI = new Queue();

    public LinkedList<GameObject> historyUI = new LinkedList<GameObject>();

    [Header("UI Elements")]
	//[SerializeField] GameObject GamePanel;
    //[SerializeField] GameObject CommandPanel;
    [SerializeField] GameObject MoveListDisplay;
	[SerializeField] GameObject enemyTargetDisplay;
	[SerializeField] GameObject playerTargetDisplay;
	[SerializeField] GameObject turnOrderDisplay;
	[SerializeField] GameObject uiIcon;
	[SerializeField] GameObject GameHistoryDisplay;
	[SerializeField] Button EndTurn;

    GameObject oldIcon;

	[SerializeField] GameObject historySegment;

	[SerializeField] public TextMeshProUGUI TurnStateMachine;
	[SerializeField] public TextMeshProUGUI ActionStateMachine;
    [SerializeField] TextMeshProUGUI EntityTurn;
    [SerializeField] Button AbilityButton;
    [SerializeField] Button EntityButton;
    [SerializeField] List<GameObject> MoveListClick;
    [SerializeField] List<GameObject> enemyPlayerTarget;

    //[SerializeField] Scrollbar[] healthBars;
    [SerializeField] TextMeshProUGUI playerHealth;
    [SerializeField] TextMeshProUGUI enemyHealth;

    [SerializeField] Image CurrentTurn;

    [Header("Entities Panel")]
    [SerializeField] Image CurrentESprite;
    [SerializeField] TextMeshProUGUI CurrentEName;
    [SerializeField] Slider CurrentEHealth;
    [SerializeField] Slider CurrentEActionPoints;
    [SerializeField] Slider CurrentEMovement;

    [SerializeField] private CinemachineVirtualCamera camera;
    
    private bool MovesCreated = false;

    CombatEntity combatEntity;
    private bool movesShown = false;
    private bool playerGenerated = false;
    private bool enemyGenerated = false;

	int historyNum = 0;

	private void Start()
	{
		
	}

	public void UpdateCamera()
	{
		camera.Follow = turnManager.GetCombatEntity().transform;
		camera.LookAt = turnManager.GetCombatEntity().transform;
	}

	internal void CreateHealthBars(CombatEntity entity)
	{



        //entity.GetHealthBar().maxValue = entity.entity.GetMaxHealth();
        //entity.GetHealthBar().value = entity.entity.GetHealth();
        //updateHealth.RegisterListener(listenForHealth);
	}
	public void DisplayMoves()
	{
        if(!movesShown)
        {
            MoveListDisplay.SetActive(true);
            movesShown = true;
        } 
        else
        {
            MoveListDisplay.SetActive(false);
            movesShown = false;
        }
		
        //dequeue.GetMovesDisplay().SetActive(true);
        //CommandPanel.SetActive(false);

        //dequeue.get
     
		/*MoveListDisplay.SetActive(false);
		CommandPanel.SetActive(true);*/
	}

    public void HideMoves()
    {
		combatEntity = turnManager.GetCombatEntity();

		//combatEntity.GetMovesDisplay().SetActive(true);
		//MoveListDisplay.SetActive(false);
		//CommandPanel.SetActive(true);
	}

	internal void WhoseTurn(CombatEntity playerTurn)
	{
        ShowCombatUI();
        CurrentTurn.sprite = playerTurn.Sprite;;
        //EntityTurn.text = playerTurn.GetEntitySO().entityName;
        if(playerTurn.Entity.isPlayer)
        {
            CreateMoves(playerTurn);
        }
	}
	private void CreateMoves(CombatEntity playerTurn)
	{
        ClearMoveList();
        for (int i = 0; i < playerTurn.Entity.GetAbilities().Count; i++)
        {
            //if(!playerTurn.movesCreated)
            //{
                //movesCreated = true;
                AbilityButton.GetComponent<AbilityButton>().UpdateAbility(playerTurn.Entity.GetAbilities()[i]);
                AbilityButton.GetComponentInChildren<TextMeshProUGUI>().text = playerTurn.Entity.GetAbilities()[i].name;

                GameObject newButton = Instantiate(AbilityButton.gameObject, MoveListDisplay.transform);

                MoveListClick.Add(newButton);
                //Instantiate(AbilityButton.gameObject, playerTurn.GetMovesDisplay().transform);

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
			//}
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

	internal void LetPlayerTarget(AbilityButton button, List<CombatEntity> enemies, List<CombatEntity> players)
	{
        
        if(button.GetAbility().AbilityEffectType == AbilityEffectType.Damage)
        {
            enemyTargetDisplay.SetActive(true);
            playerTargetDisplay.SetActive(false);
            if(!enemyGenerated)
            {
                enemyGenerated = true;
		        for (int i = 0; i < enemies.Count; i++)
		        {
                    EntityButton.GetComponent<EntityButton>().UpdateEntity(enemies[i]);
                    EntityButton.GetComponentInChildren<TextMeshProUGUI>().text = enemies[i].EntityName;
			        GameObject target = Instantiate(EntityButton.gameObject, enemyTargetDisplay.transform);
                    enemyPlayerTarget.Add(target);
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
				    for (int i = 0; i < players.Count; i++)
				    {
					    EntityButton.GetComponent<EntityButton>().UpdateEntity(players[i]);
					    EntityButton.GetComponentInChildren<TextMeshProUGUI>().text = players[i].EntityName;
						GameObject target = Instantiate(EntityButton.gameObject, playerTargetDisplay.transform);
						enemyPlayerTarget.Add(target);
					}
				}
            }
        }
        else if(button.GetAbility().AbilityEffectType == AbilityEffectType.Buff)
        {
            if (!playerGenerated)
            {
                playerTargetDisplay.SetActive(true);
                enemyTargetDisplay.SetActive(false);
                if (button.GetAbility().target == Targeting.Self)
                {
                    EntityButton.GetComponent<EntityButton>().UpdateEntity(turnManager.EntitiesTurn);
                    EntityButton.GetComponentInChildren<TextMeshProUGUI>().text = turnManager.EntitiesTurn.EntityName;
					GameObject target = Instantiate(EntityButton.gameObject, playerTargetDisplay.transform);
					enemyPlayerTarget.Add(target);

				}
			}
        }
	}

	internal void UpdateHealth(CombatEntity entity)
	{
        //updateHealth.Raise();
		entity.GetHealthBar().value = entity.Entity.GetHealth();
        if(entity.isPlayer)
        {
            CurrentEHealth.value = entity.Entity.GetHealth();
        }
	}

	internal void PlayTitle(TextMeshProUGUI titleText, Button start, Button quit)
	{
        titleText.CrossFadeAlpha(100, 5, true);
	}
    //deprecated
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

	internal void CreateTurnOrder(List<CombatEntity> queue)
	{
        GameObject newTurnIcon;

        if(queue != null)
        {
            foreach (CombatEntity item in queue)
            {
				newTurnIcon = Instantiate(uiIcon, turnOrderDisplay.transform);
                newTurnIcon.GetComponent<Image>().sprite = item.Sprite;
                newTurnIcon.transform.localScale = Vector3.one;
                turnOrderUI.Enqueue(newTurnIcon);
            }
        }
	}
    internal void PopTurnOrderUI()
    {
        GameObject oldIcon = (GameObject)turnOrderUI.Dequeue();
        Destroy(oldIcon);
    }

	internal void AddToHistory(AbilitySO chosenAbility, CombatEntity CurrentTurn, CombatEntity Target)
	{
        
        if(historyNum >= 6)
        {
			GameObject oldIcon = (GameObject)historyUI.First.Value;
			Destroy(oldIcon);
            historyUI.RemoveFirst();
		}

        GameObject historyText;

        historyText = Instantiate(historySegment, GameHistoryDisplay.transform);

        if(chosenAbility.AbilityEffectType == AbilityEffectType.Damage)
        {
            historyText.GetComponent<TextMeshProUGUI>().text = CurrentTurn.EntityName + " performed " + chosenAbility.AbilityName
                + " for " + (chosenAbility.damage + CurrentTurn.Entity.Stats.attack - Target.Entity.Stats.defense) + "(" + 
                chosenAbility.damage + ")+(" + CurrentTurn.Entity.Stats.attack + ")-(" + Target.Entity.Stats.defense + ") damage to " + Target.EntityName;
        }
        else if(chosenAbility.AbilityEffectType == AbilityEffectType.Health)
        {
			historyText.GetComponent<TextMeshProUGUI>().text = CurrentTurn.EntityName + " used " + chosenAbility.AbilityName
                + " and healed themselves for " + chosenAbility.damage;
		}
		else if (chosenAbility.AbilityEffectType == AbilityEffectType.Buff)
        {
            historyText.GetComponent<TextMeshProUGUI>().text = CurrentTurn.EntityName + " performed " + chosenAbility.AbilityName
                + " and buffed " + chosenAbility.buff + " for " + chosenAbility.statusEffectCount + " points!";
        }
        historyText.transform.localScale = Vector3.one;
        historyUI.AddLast(historyText);
        historyNum = historyUI.Count;
	}
	internal void AddToHistory(string chosenAbility, CombatEntity entity)
    {
		if (historyNum >= 6)
		{
			GameObject oldIcon = (GameObject)historyUI.First.Value;
			Destroy(oldIcon);
			historyUI.RemoveFirst();
		}

		GameObject historyText;

		historyText = Instantiate(historySegment, GameHistoryDisplay.transform);

		historyText.GetComponent<TextMeshProUGUI>().text = entity.name + " performed " + chosenAbility;
		historyText.transform.localScale = Vector3.one;
		historyUI.AddLast(historyText);
		historyNum = historyUI.Count;

	}

	internal void ChangeEntityPanel(CombatEntity entitiesTurn)
	{
        CurrentESprite.sprite = entitiesTurn.Sprite;
        CurrentEName.text = entitiesTurn.EntityName;
        CurrentEHealth.minValue = 0;
        CurrentEHealth.value = entitiesTurn.Entity.GetHealth();
        CurrentEHealth.maxValue = entitiesTurn.Entity.GetMaxHealth();

        CurrentEActionPoints.value = entitiesTurn.actionPoints;;
        CurrentEActionPoints.maxValue = entitiesTurn.actionPoints;

        CurrentEMovement.value = entitiesTurn.movementPoints;
        CurrentEMovement.maxValue = entitiesTurn.movementPoints;
	}

    internal void ShowCombatUI()
    {
        entireUI.SetActive(true);
    }

    internal void HideCombatUI()
    {
        entireUI.SetActive(false);
    }

	internal void ClearHistory()
	{
        historyNum = 0;

        int historyCount = historyUI.Count;
		for(int i =  0; i < historyCount; i++)
        {
			GameObject oldIcon = (GameObject)historyUI.Last.Value;
			Destroy(oldIcon);
		}
	}

	internal void ClearQueue()
	{
		for (int i = 0; i < turnOrderUI.Count; i++)
		{
			GameObject oldIcon = (GameObject)turnOrderUI.Dequeue();
			Destroy(oldIcon);
		}
        playerGenerated = false;
        enemyGenerated = false;
	}

    internal void ClearMoveList()
    {

		int moveCount = MoveListClick.Count;
		for (int i = moveCount - 1; i >= 0; i--)
		{
			Destroy(MoveListClick[i]);
			//MoveListClick[i].SetActive(false);
            MoveListClick.RemoveAt(i);
		}
	}
	internal void ClearTargeting()
	{

		int moveCount = enemyPlayerTarget.Count;
		for (int i = moveCount - 1; i >= 0; i--)
		{
            //GameObject oldIcon = enemyPlayerTarget[i];
            //Destroy(oldIcon);
            //oldIcon.SetActive(false);
            Destroy(enemyPlayerTarget[i]);
			enemyPlayerTarget.RemoveAt(i);
		}
	}

	internal void StartTurnEffects(CombatEntity player)
	{
		for(int i = 0; i > player.Buffs.Count; i++)
        {
            /*GameObject effect = Instantiate(StatusEffectF)

								newTurnIcon = Instantiate(uiIcon, turnOrderDisplay.transform);*/

		}
	}
}
