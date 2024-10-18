using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
	[SerializeField] AbilitySO AbilitySO;

	private void Start()
	{}

	public void UpdateAbility(AbilitySO abilitySO)
	{
		this.AbilitySO = abilitySO;
	}
	public AbilitySO GetAbility()
	{
		return this.AbilitySO;
	}
	public Targeting GetTargeting()
	{
		return AbilitySO.target;
	}
	public AbilityEffectType GetEffectType()
	{
		return AbilitySO.AbilityEffectType;
	}
}

