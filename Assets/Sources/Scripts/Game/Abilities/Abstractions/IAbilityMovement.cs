using UnityEngine;

namespace Nekki.Game.Abilities.Abstractions
{
    public interface IAbilityMovement
    {
        /// <summary>
        /// Apply fluid builder and start movement.
        /// </summary>
        /// <param name="speed">linear speed</param>
        /// <param name="rangeLimit">if need to interrupt when it meets limit.</param>
        void Apply(float speed, float? rangeLimit = null);
        
        /// <summary>
        /// Stop and reset all the settings of the movement.
        /// </summary>
        void Reset();

        /// <summary>
        /// If it has built before: will pause or unpause the movement.
        /// </summary>
        void Enable(bool value);

        IAbilityMovement SetPosition(Vector3 value);
        IAbilityMovement SetRotation(Quaternion value);
        IAbilityMovement SetSteering(Transform value);
        IAbilityMovement SetDirection(Vector3 value);
        IAbilityMovement SetDestination(Vector3 value);
    }
}