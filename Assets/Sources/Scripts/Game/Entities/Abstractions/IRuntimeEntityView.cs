namespace Nekki.Game.Entities.Abstractions
{
    public interface IRuntimeEntityView
    {
        IRuntimeEntity Entity { get; }
        IEntityViewMapper Mapper { get; }
    }
}