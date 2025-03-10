using Nekki.Game.Entities.Abstractions;
using Nekki.Game.Entities.Player.Data;

namespace Nekki.Game.Entities.Player.Abstractions
{
    public interface IPlayerViewModel : IRuntimeEntity
    {
        PlayerConfig Config { get; }
        new IPlayerView View { get; }
        IPlayerMovement Movement { get; }
        IPlayerAnimator Animator { get; }
    }
}