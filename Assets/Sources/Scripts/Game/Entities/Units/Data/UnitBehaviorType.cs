using Unity.Behavior;

namespace Nekki.Game.Entities.Units.Data
{
    [BlackboardEnum]
    public enum UnitBehaviorType
    {
        Static = 0,
        Utility,
        Creep,
        Commander,
        Boss,
    }
}