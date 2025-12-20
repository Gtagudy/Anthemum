using System.Collections.Generic;

public interface ITargetingStrategy
{
    /// <summary>
    /// All cells (tiles) this ability can legally target from the caster’s position.
    /// </summary>
    IEnumerable<GridMapPoint> GetValidCells(CombatEntity caster, AbilitySO ability);

    /// <summary>
    /// True if clicking this cell counts as a valid final target.
    /// </summary>
    bool IsValidTarget(CombatEntity caster, AbilitySO ability, GridMapPoint cell);
}