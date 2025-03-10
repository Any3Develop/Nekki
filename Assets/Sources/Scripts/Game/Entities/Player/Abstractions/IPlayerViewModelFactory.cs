using Nekki.Game.Entities.Player.Data;

namespace Nekki.Game.Entities.Player.Abstractions
{
    public interface IPlayerViewModelFactory
    {
        IPlayerViewModel Create(PlayerData playerData);
    }
}