using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public enum Debuff
{
	None,
	Speeddown,
	AttackDown,
	DefenseDown,
	MaxHealthDown,
	DotBleed,
	DotBurn,
	Curse
}
public enum Buff
{
	None,
	SpeedUp,
	AttackUp,
	DefenseUp,
	MaxHealthUp,
	HPDot,
	Risk
}

public abstract class StatusEffectBase
{
	public Buff Buff { get; private set; }
	public Debuff Debuff { get; private set; }

	public int statusStacks { get; set; }
	public int statusDuration { get; set; }
	public int effectValue { get; private set; }

	public bool isBuff;

	public StatusEffectBase(Buff buff, Debuff debuff, int statusStacks, int statusDuration, int effectValue)
	{
		Buff = buff;
		Debuff = debuff;
		this.statusStacks = statusStacks;
		this.statusDuration = statusDuration;
		this.effectValue = effectValue;
	}
	public StatusEffectBase(Buff buff, Debuff debuff, int statusStacks, int effectValue)
	{
		Buff = buff;
		Debuff = debuff;
		this.statusStacks = statusStacks;
		this.effectValue = effectValue;
	}
	public virtual void OnTurnStart(CombatEntity entity)
	{
	}

	public virtual void OnActive(CombatEntity entity)
	{

	}

	public virtual void OnTurnEnd(CombatEntity entity)
	{
		statusStacks -= 1;
	}

	public void AddStacks(int stack)
	{
		statusStacks += stack;
	}

	public void ClearStacks()
	{
		statusStacks = 0;
	}
	public void DurationDecay()
	{
		statusDuration -= 1;

		if(statusDuration <= 0)
		{
			StackDecay();
		}
	}
	public void StackDecay()
	{
		statusStacks -= 1;
	}
	public void WorsenStack()
	{
		statusStacks -= 1;
	}
	public int FinalValue()
	{
		return effectValue * statusStacks;
	}
	
}
