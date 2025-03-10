using System;

namespace Nekki.Game.Entities.Player.Abstractions
{
    public interface IPlayerMovement : IDisposable
    {
        bool Enabled { get; }
        
        void Enable(bool value);
    }
}