using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObject/Stats")]
public class StatSO : ScriptableObject
{
	[Header("Stats")]
	public int health;
	public int originalHealth;

	public int attack;
	public int originalAttack;

	public int magic;
	public int originalMagic;
	
	public int defense;
	public int originalDefense;

	public int stamina;
	public int originalStamina;

	public int movementPoints;
	public int originalMovementPoints;
	
	public int mana;
	public int originalMana;
	
	public int experience;
	public int level;
	public int experienceToNextLevel;
	public int gold;

	public int manaRegen;
	public int staminaRegen;

	[SerializeField] public int speed;
}