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

	[SerializeField] StatSO Stats;
	//[SerializeField] Scrollbar Health;

	public bool isPlayer = false;
	private void OnEnable()
	{
		
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
	public Transform GetTransform()
	{
		return this.GetComponent<Transform>();
	}
	public void ChangeHealth(int v)
	{
		Stats.health -= v;
	}
}
