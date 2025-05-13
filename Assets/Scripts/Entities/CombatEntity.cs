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
	[Header("Data Template (assigned at spawn)")]
	[SerializeField] private string entityName;
	[SerializeField] private EntitySO entity;        // your ScriptableObject template
	[SerializeField] private Slider healthSlider;     // UI slider for health

	[Header("Runtime State")]
	private int currentHealth;
	public int movementPoints;
	public int actionPoints;
	public bool hasMoved;
	
	private Dictionary<Buff, StatusEffectBase> buffs = new();
	private Dictionary<Debuff, StatusEffectBase> debuffs = new();

	public string EntityName => entityName;
	public EntitySO Entity => entity;
	[SerializeField] public Sprite Sprite;

    public IntGameEvent healthChange;
	
    [SerializeField] Slider Health;

	[SerializeField] public Dictionary<Buff, StatusEffectBase> Buffs = new Dictionary<Buff, StatusEffectBase>();
	[SerializeField] public Dictionary<Debuff, StatusEffectBase> Debuffs = new Dictionary<Debuff, StatusEffectBase>();

	[SerializeField] GameObject myTurn;
    [SerializeField] GameObject decisionsUI;
	[SerializeField] GameObject MoveListDisplay;
	[SerializeField] GameObject enemyTargetDisplay;
	[SerializeField] GameObject playerTargetDisplay;
	[SerializeField] public GameObject effectCollection;
	private bool isMyTurn = false;
    public bool movesCreated = false;

    public bool isPlayer = false;

    [SerializeField] int x;
    [SerializeField] int y;


	// Start is called before the first frame update
	void Awake()
    {
        isPlayer = entity.isPlayer;
        Sprite = GetComponent<SpriteRenderer>().sprite;
	}

	public void InitializeFromData(EntitySO entitySO)
	{
			// Copy name
			entityName = entity.entityName;

			// Health
			currentHealth = entity.GetMaxHealth();
			if (healthSlider != null)
			{
				healthSlider.maxValue   = entity.GetMaxHealth();
				healthSlider.value      = currentHealth;
			}

			// Movement & action
			movementPoints = entity.Stats.originalMovementPoints;;
			actionPoints   = entity.Stats.originalStamina;

			// Reset turn flags
			hasMoved = false;

			// Load sprite if you store one on the SO
			var sr = GetComponent<SpriteRenderer>();
			if (sr != null && Sprite != null)
				sr.sprite = Sprite;
	}
	
    // Update is called once per frame
    void Update()
    {
       /* if(isMyTurn)
        {
            myTurn.SetActive(true);
        }
        else
        {
            myTurn.SetActive(false);
        }*/
    }

    internal Slider GetHealthBar()
    {
        return Health;
    }
    internal GameObject ToggleDecisions()
    {
        return decisionsUI;
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
    
    public Sprite GetSprite()
    {
        return Sprite;
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
        this.transform.position = transform.position;// + (transform.up / 2);
	}
    internal void UpdateGridPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void AddBuff(Buff type, int stacks, int baseValue)
    {
	    AddOrStackStatusEffect(type, stacks, baseValue, buffs, StatusEffectFactory.CreateBuff);
	    buffs[type].OnActive(this);
	    Debug.Log($"Applied {type} with effective value: {buffs[type].FinalValue()}");
    }

    public void AddDebuff(Debuff type, int stacks, int baseValue)
    {
	    AddOrStackStatusEffect(type, stacks, baseValue, debuffs, StatusEffectFactory.CreateDebuff);
	    // debuffs[type].OnActive(this);
	    Debug.Log($"Applied {type} with effective value: {debuffs[type].FinalValue()}");
    }
    private delegate StatusEffectBase StatusEffectFactoryDelegate<T>(T type, int stacks, int baseValue);

    private void AddOrStackStatusEffect<T>(T type, int stacks, int baseValue, Dictionary<T, StatusEffectBase> collection,
	    StatusEffectFactoryDelegate<T> createStatusEffect) where T : Enum
    {
	    if (collection.TryGetValue(type, out var existingEffect))
	    {
		    existingEffect.AddStacks(stacks);
	    }
	    else
	    {
		    var newEffect = createStatusEffect(type, stacks, baseValue);
		    collection.Add(type, newEffect);
	    }
    }

}
