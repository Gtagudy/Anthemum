using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObject/Entities")]

public class EntitySO : IScriptableObject
{
	[SerializeField] List<AbilitySO> Ability;

	//[SerializeField] List<AbilitySO> Buff;

	//[SerializeField] List<AbilitySO> Debuff;

	

	[SerializeField] public StatSO Stats;
	//[SerializeField] Scrollbar Health;

	public bool isPlayer = false;

	public bool isAlive = true;
	private void OnEnable()
	{
		Stats.health = Stats.originalHealth;
		Stats.attack = Stats.originalAttack;
		Stats.defense = Stats.originalDefense;
		isAlive = true;
	}

	public int GetSpeed()
	{
		return Stats.speed;
	}
	public AbilitySO getAbility(int tempNum)
	{
		return Ability[tempNum];
	}
	public List<AbilitySO> GetAbilities()
	{
		return Ability;
	}
	public int GetHealth()
	{
		return Stats.health;
	}
	public int GetMaxHealth()
	{
		return Stats.originalHealth;
	}
	public Transform GetTransform()
	{
		return this.GetComponent<Transform>();
	}
	public int ChangeHealth(int v)
	{
		Stats.health -= v;
		if(GetHealth() > GetMaxHealth())
		{
			Stats.health = GetHealth();
		}
		if(Stats.health <= 0)
		{
			isAlive = false;

			return 0;
		}
		return Stats.health;
	}

}
