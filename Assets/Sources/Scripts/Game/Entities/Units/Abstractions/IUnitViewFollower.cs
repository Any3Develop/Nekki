using UnityEngine;

namespace Nekki.Game.Entities.Units.Abstractions
{
    public interface IUnitViewFollower
    {
        bool Enabled { get; }

        void SetSteering(Transform value);
        void Enable(bool value);
    }
}