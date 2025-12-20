using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
	private void Start()
	{}
	[SerializeField] private Image iconImage;
	[SerializeField] private Image cooldownOverlay;
	[SerializeField] private TextMeshProUGUI  costText;

	private CombatEntity owner;
	private AbilitySO    abi;
	private ActionManager actionManager;

	public void Setup(CombatEntity owner, AbilitySO ability, ActionManager AM)
	{
		actionManager = AM;
		
		this.owner = owner;
		abi   = ability;
		iconImage.sprite = ability.icon;
	}
	
	void Update()
	{
		// 1) Cooldown overlay (turn‐based)
		if (owner.TryGetCooldown(abi, out int cd) && cd > 0)
		{
			cooldownOverlay.fillAmount = (float)cd / abi.turnCooldown;
			costText.text              = $"{cd}T";
		}
		else
		{
			cooldownOverlay.fillAmount = 0f;
			// 2) Show both costs, skipping zeros
			string m = abi.mana    > 0 ? $"M:{abi.mana}"    : "";
			string s = abi.stamina > 0 ? $"S:{abi.stamina}" : "";
			costText.text = $"{m}{(m!="" && s!=""? " ": "")}{s}";
		}

		// 3) Gray out if you can’t use both
		iconImage.color = owner.CanUse(abi) ? Color.white : Color.gray;
	}

	public void OnClick()
	{
		if (!owner.StartAbilityUse(abi))
			return;
		
		actionManager.ConfirmAbility(this);
	}
	public void UpdateAbility(AbilitySO abilitySO)
	{
		this.abi = abilitySO;
	}
	public AbilitySO GetAbility()
	{
		return this.abi;
	}
	public Targeting GetTargeting()
	{
		return abi.target;
	}
	public AbilityEffectType GetEffectType()
	{
		return abi.AbilityEffectType;
	}
}

