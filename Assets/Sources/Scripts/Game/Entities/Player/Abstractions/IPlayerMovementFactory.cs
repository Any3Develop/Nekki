namespace Nekki.Game.Entities.Player.Abstractions
{
    public interface IPlayerMovementFactory
    {
        IPlayerMovement Create(bool is2D, params object[] args);
    }
}