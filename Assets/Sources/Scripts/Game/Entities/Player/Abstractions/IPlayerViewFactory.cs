using UnityEngine;

namespace Nekki.Game.Entities.Player.Abstractions
{
    public interface IPlayerViewFactory
    {
        IPlayerView Create(IPlayerViewModel viewModel, Transform root);
    }
}