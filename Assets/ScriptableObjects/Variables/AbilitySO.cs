using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObject/Abilities")]
public class AbilitySO : ScriptableObject
{
	public string AbilityName;
	public Sprite icon;
	public string description;

	[Header("resource")]
	public int stamina;
	public int mana;

	[Header("Cooldown")]
	public int turnCooldown;

	[Header("Ability Stats")]
	public int damage;

	public int knockback;

	public int AOE;

	[Header("Targeting")]
	public Targeting target;
	public int range;
	public Vector2Int size = Vector2Int.zero;

	public AbilityEffectType AbilityEffectType;

	public Buff buff;

	public Debuff debuff;

	public int statusEffectCount;
	
	[Header("QTE")]
	public float qteStartDelay        = 0.5f;
	public float qteWindow            = 1.0f;
	public float qteSuccessMultiplier = 1.5f;   
}