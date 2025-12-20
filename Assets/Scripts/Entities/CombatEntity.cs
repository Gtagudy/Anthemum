using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	private StatSO stats;
	
	[SerializeField] private Slider healthSlider;     // UI slider for health

	[Header("Runtime State")]
	private int currentHealth;
	private int movementPoints;
	private int currentStamina;
	private int currentMana;
	public bool hasMoved;
	
	private Dictionary<Buff, StatusEffectBase> buffs = new();
	private Dictionary<Debuff, StatusEffectBase> debuffs = new();

	public string EntityName => entityName;
	public EntitySO Entity
	{
		get => entity;
		set => entity = value;
	}

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

    public CombatManager combatManager;
    
// maps each AbilitySO to its remaining cooldown _in turns_
    private Dictionary<AbilitySO,int> cooldowns = new();
    
    public GridMapPoint GetCurrentGridCell()
    {
	    return combatManager.gridManager.GetPointUsingCoords(x, y);
    }
    
	// Start is called before the first frame update
	void Awake()
    {
        isPlayer = entity.isPlayer;
        Sprite = GetComponent<SpriteRenderer>().sprite;
        combatManager = FindFirstObjectByType<CombatManager>();
	}
	/// <summary>
	/// Initialize from data grabs everything from the entity and sets it up with 'this' monobehavior
	/// </summary>
	public void InitializeFromData()
	{
		stats = entity.Stats;
		
		entityName = entity.entityName;

		currentHealth = entity.GetMaxHealth();
		if (healthSlider != null)
		{
			healthSlider.maxValue   = entity.GetMaxHealth();
			healthSlider.value      = currentHealth;
		}

		movementPoints = entity.Stats.originalMovementPoints;;
		currentStamina   = entity.Stats.originalStamina;

		hasMoved = false;

		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		if (sr != null && Sprite != null)
			sr.sprite = Sprite;
	}

    /// <summary>
    /// Can we afford the ability and is it off cooldown?
    /// Takes in AbilitySO
    /// </summary>
    public bool CanUse(AbilitySO abi)
    {
	    bool hasMana    = currentMana    >= abi.mana;
	    bool hasStamina = currentStamina >= abi.stamina;
	    bool offCd      = !cooldowns.ContainsKey(abi) || cooldowns[abi] == 0;
	    return hasMana && hasStamina && offCd;
    }

    /// <summary>
    /// Deducts cost & starts the turn‐based cooldown.
    /// </summary>
    public bool StartAbilityUse(AbilitySO abi)
    {
	    if (!CanUse(abi)) return false;
	    currentMana    -= abi.mana;
	    currentStamina -= abi.mana;
	    cooldowns[abi]  = abi.turnCooldown;
	    return true;
    }
    /// <summary>
    /// Ticks down the cooldowns and regens resources. Called every 'Start Turn'
    /// </summary>
    public void EndTurnTick()
    {
	    // Tick cooldowns
	    var keys = cooldowns.Keys.ToList();
	    foreach (var a in keys)
		    if (cooldowns[a] > 0) cooldowns[a]--;

	    // Regen both resources
	    currentMana    = Mathf.Min(stats.mana,    currentMana   + stats.manaRegen);
	    currentStamina = Mathf.Min(stats.stamina, currentStamina + stats.staminaRegen);
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

    public bool TryGetCooldown(AbilitySO ability, out int cooldown)
    {
	    return cooldowns.TryGetValue(ability, out cooldown);
    }

    public int GetCurrentStamina()
    {
	    return currentStamina;
    }

    public int GetMovementPoints()
    {
	    return movementPoints;
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
