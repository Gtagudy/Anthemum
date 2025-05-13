using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StatusEffectFactory
{
	public static StatusEffectBase CreateBuff(Buff type, int stacks, int baseValue)
	{
		switch (type)
		{
			case Buff.AttackUp:
				return new AttackUp(Buff.AttackUp, Debuff.None, stacks, baseValue);
			default:
				throw new ArgumentException("Unknown status effect type: " + type);
		}
	}

	internal static StatusEffectBase CreateDebuff(Debuff type, int stacks, int baseValue)
	{
		throw new NotImplementedException();
	}
}