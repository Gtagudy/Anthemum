using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AttackUp : StatusEffectBase
{
	public AttackUp(Buff buff, Debuff debuff, int statusStacks, int effectValue) : base(buff, debuff, statusStacks, effectValue)
	{
		
	}

	public override void OnTurnStart(CombatEntity entity)
	{
		base.OnTurnStart(entity);
	}
	public override void OnActive(CombatEntity entity)
	{
		int finalBuff = FinalValue();

		entity.Entity.Stats.attack += finalBuff;
		base.OnActive(entity);
	}
	public override void OnTurnEnd(CombatEntity entity)
	{
		int finalBuff = FinalValue();
		entity.Entity.Stats.attack -= finalBuff;
		base.OnTurnEnd(entity);
	}
}