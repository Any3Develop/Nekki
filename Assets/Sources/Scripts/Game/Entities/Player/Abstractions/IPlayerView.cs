using Nekki.Game.Entities.Abstractions;

namespace Nekki.Game.Entities.Player.Abstractions
{
    public interface IPlayerView : IRuntimeEntityView
    {
        IPlayerViewModel ViewModel { get; }
        void Shake();
    }
}