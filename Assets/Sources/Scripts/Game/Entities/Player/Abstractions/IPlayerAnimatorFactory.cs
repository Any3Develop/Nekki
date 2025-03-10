namespace Nekki.Game.Entities.Player.Abstractions
{
    public interface IPlayerAnimatorFactory
    {
        IPlayerAnimator Create(params object[] args);
    }
}