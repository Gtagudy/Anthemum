using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObject/Entities")]

public class EntitySO : IScriptableObject
{
	[SerializeField] List<AbilitySO> Ability;

	[SerializeField] StatSO Stats;

	public bool isPlayer = false;

	public int GetSpeed()
	{
		return Stats.speed;
	}
	public AbilitySO getAbility(int tempNum)
	{
		return Ability[tempNum];
	}

}
