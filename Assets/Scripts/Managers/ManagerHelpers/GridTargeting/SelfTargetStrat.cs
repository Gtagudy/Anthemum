using System.Collections.Generic;

public class SelfTargetStrategy : ITargetingStrategy
{
    public IEnumerable<GridMapPoint> GetValidCells(CombatEntity caster, AbilitySO abi)
    {
        return new[]{ caster.GetCurrentGridCell() };
    }
    public bool IsValidTarget(CombatEntity caster, AbilitySO abi, GridMapPoint cell)
    {
        return cell.gridSO.entityHere == caster;
    }
}