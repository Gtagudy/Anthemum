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

	public string description;

	public Sprite icon;

	public int actionPoints;

	public int cooldown;

	public int damage;

	public int AOE;

	public Targeting target;

	public AbilityEffectType AbilityEffectType;

	public Buff buff;

	public Debuff debuff;

	public int statusEffectCount;
}