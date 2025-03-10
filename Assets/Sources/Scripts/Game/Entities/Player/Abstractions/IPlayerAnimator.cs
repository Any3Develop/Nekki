using System;
using UnityEngine;

namespace Nekki.Game.Entities.Player.Abstractions
{
    public interface IPlayerAnimator : IDisposable
    {
        IPlayerViewModel ViewModel { get; }
        bool Enabled { get; }

        void Enable(bool value);
        void Attack(int type);
        void Move(Vector3 dir);
    }
}