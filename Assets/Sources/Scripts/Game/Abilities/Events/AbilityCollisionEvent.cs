using Nekki.Game.Abilities.Abstractions;
using UnityEngine;

namespace Nekki.Game.Abilities.Events
{
    public readonly struct AbilityCollisionEvent
    {
        public IAbilityView View { get; }
        public GameObject GameObject { get; }

        public AbilityCollisionEvent(IAbilityView view, GameObject gameObject) 
            => (View, GameObject) = (view, gameObject);
    }
}