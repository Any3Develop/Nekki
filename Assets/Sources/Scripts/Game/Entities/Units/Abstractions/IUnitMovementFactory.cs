namespace Nekki.Game.Entities.Units.Abstractions
{
    public interface IUnitMovementFactory
    {
        IUnitMovement Create(params object[] args);
    }
}