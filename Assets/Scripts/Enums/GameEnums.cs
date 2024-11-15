using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameEnums 
{
    /*public AbilityEffectType abilityEffectType;

    public Targeting targeting;

    public Turns turns;

    public GameState gameState;*/
}
public enum AbilityEffectType
{
    Damage,
    Health,
    Buff,
    Debuff,
    Stun
}    

public enum Targeting
{
    Single,
    Multi,
    Self,
    AOE
}

public enum Turns
{
    Preturn,
    TurnStart,
    PlayerTurn,
    EnemyTurn,
    TurnEnd,
    PostTurn
}
public enum GameState
{
    Title,
    World,
    Cutscene,
    Combat,
    Pause
}

public enum CombatState
{
    Choose,
    Target,
    Item,
    Move,
    Cutscene
}