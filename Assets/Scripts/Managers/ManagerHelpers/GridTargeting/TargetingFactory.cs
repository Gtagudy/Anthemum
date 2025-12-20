using System;

public static class TargetingFactory
{
    public static ITargetingStrategy Create(Targeting type)
    {
        switch(type)
        {
            case Targeting.Self:   return new SelfTargetStrategy();
            case Targeting.Single: return new SingleTargetStrategy();
            // case TargetingType.AoE:    return new AoETargetStrategy();
            // …
            default: throw new NotImplementedException();
        }
    }
}