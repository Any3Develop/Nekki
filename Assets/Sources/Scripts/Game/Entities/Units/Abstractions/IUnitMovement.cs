using UnityEngine;

namespace Nekki.Game.Entities.Units.Abstractions
{
    public interface IUnitMovement
    {
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        
        bool Enabled { get; }
        float RemainingDistance { get; }

        void Enable(bool value);
        void SetSteering(Transform target);
        void MoveAuto(Vector3 worldPoint);
        void MoveRelative(Vector3 worldPoint);
        void Move(Vector3 worldPoint);
        void Stop();
    }
}