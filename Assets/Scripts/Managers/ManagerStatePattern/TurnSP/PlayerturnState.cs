using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

public class PlayerturnState : StateTurnBase
{
	public override void EnterState(TurnManager turnManager, CombatSceneSO combatSceneSO)
	{
		//turnManager.GetCombatEntity().ToggleDecisions().gameObject.SetActive(true);

		CombatEntity player = turnManager.EntitiesTurn;

		turnManager.uiManager.StartTurnEffects(player);
		foreach(StatusEffectBase statusEffectBase in player.Buffs.Values)
		{
			statusEffectBase.DurationDecay();
		}
	}

	public override void UpdateState(TurnManager turnManager)
	{
		
	}

	public void EndMyTurn()
	{

	}
	public void RemoveStatusEffect()
	{
		List<StatusEffectBase> list = new List<StatusEffectBase>();

		foreach (StatusEffectBase effect in list)
		{
			//turn.Remove(effect)
		};
	}
}