using System.Collections.Generic;
using System.Linq;

public class SingleTargetStrategy : ITargetingStrategy
{
    public IEnumerable<GridMapPoint> GetValidCells(CombatEntity caster, AbilitySO abi)
    {
        GridMapPoint origin = caster.GetCurrentGridCell();
        return caster.combatManager.gridManager
            .GetCellsInShape(origin, abi.target, abi.range, abi.size);
    }

    public bool IsValidTarget(CombatEntity caster, AbilitySO abi, GridMapPoint cell)
    {
        // must have an opposing entity there
        return cell.gridSO.entityHere != null
               && caster.Entity.isPlayer != cell.gridSO.entityHere.Entity.isPlayer;
    }
}